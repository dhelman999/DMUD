using System;
using System.Collections.Generic;
using System.Threading;

namespace _8th_Circle_Server
{
    // Combat handler, like all other handlers uses a thread to process combats.  Implements the logic to attack, counterattack, use
    // abilities, spells, keep track of targets, who is alive or dead, who is in the combat and who isn't when to end combat, calculating
    // damage, hit chances, and everything else combat related.
    public class CombatHandler
    {
        // autoattack rounds are divided into 4 second round times
        internal const int ROUNDTIME = 4000;

        private World mWorld;

        // Holds who the current mobs that are in combat
        private Queue<CombatMob> mCombatQueue;
        
        // Generates random numbers for combat
        private Random mRand;

        // Main thread to do process combat
        private Thread mSpinWorkThread;

        // Primitive thread safety, needs to be much better
        private object mQueueLock;

        public CombatHandler(World world)
        {
            mCombatQueue = new Queue<CombatMob>();
            mQueueLock = new object();
            mWorld = world;
            mRand = new Random();
        }// Constructor

        public void start()
        {
            mSpinWorkThread = new Thread(() => spinWork(this));
            mSpinWorkThread.Start();
        }// start

        // Main thread to process combat
        public static void spinWork(CombatHandler combatHandler)
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }// try
                catch
                {
                    // At least this is sort of thread safe!
                    lock (combatHandler.GetCombatLock())
                    {
                        Queue<CombatMob> combatQueue = combatHandler.GetCombatQueue();

                        // Combat spawns a new thread, the thread attacks and counter attacks while commands are processed
                        // by the combatants, when the combat ends, the threads ends.  This allows mobs to enter combat
                        // whenever they wish and not be reliant on the gameticks of the areas.
                        if (combatQueue.Count > 0)
                        {
                            Thread combatThread = new Thread(() => combatTask(combatHandler, combatQueue.Dequeue()));
                            combatThread.Start();
                        }
                    }
                }// catch
            }// while (true)
        }// spinWork
        
        // Request to be in combat
        public void enQueueCombat(CombatMob mob)
        {
            lock (mQueueLock)
            {
                mCombatQueue.Enqueue(mob);
            }

            mSpinWorkThread.Interrupt();
        }// enQueueCombat

        // The main autoattack and counter autoattack combat task
        public static void combatTask(CombatHandler combatHandler, CombatMob attacker)
        {
            List<CombatMob> combatList = attacker.GetCombatList();

            while (combatList.Count > 0)
            {  
                // If your primary target is killed, or no longer exists in combat, switch to the next available target
                if (attacker.GetPrimaryTarget() == null)
                    attacker.SetPrimaryTarget(combatList[0]);

                // Attack your primary target
                CombatMob target = attacker.GetPrimaryTarget();
                combatHandler.attack(attacker, target);
                combatHandler.checkDeath(attacker, target);

                // Have all other members of the combat attack, there is a flaw in this logic if many people start combat at different
                // times and with differnt opponents
                for (int i = 0; i < combatList.Count; ++i)
                {
                    target = combatList[i];
                    combatHandler.attack(target, attacker);
                    combatHandler.checkDeath(target, attacker);
                }

                attacker.safeWrite(attacker.playerString());
                target.safeWrite(target.playerString());

                // Sleep before the next round begins
                Thread.Sleep(ROUNDTIME);
            }// while (combatList.Count > 0)
        }// combatTask

        // Casts damaging and healing spells
        public void executeSpell(CombatMob attacker, CombatMob target, Action spell)
        {
            bool isCrit = false;

            // If your target is killed get the next one
            if (target == null && attacker.GetCombatList().Count > 0)
                target = attacker.GetCombatList()[0];

            // Can't cast anything if there are no targets
            if (target == null)
                return;

            double spellRoll = mRand.NextDouble();

            // Crits are on a d20 system, a 20 is a crit.
            if (spellRoll >= .95)
                isCrit = true;

            bool isHit = true;
            attacker[STAT.CURRENTMANA] -= spell.mManaCost;

            // Process hits, misses, and heals
            if (!isHit)
                processMiss(attacker, target, spell);
            else if (spell.mDamType == DamageType.HEALING)
                processHeal(attacker, target, spell, isCrit);
            else
                processAbilityHit(attacker, target, spell, isCrit);

            // Spells have a cooldown
            attacker.ModifyActionTimer(spell.mUseTime);
        }// executeSpell

        // Attack using an ability
        public void abilityAttack(CombatMob attacker, CombatMob target, Action ability)
        {
            bool isCrit = false;
            bool isHit = false;

            // Get the primary target, if it is dead go to the next one, if none don't need to be in combat.
            if (target == null)
                target = attacker.GetPrimaryTarget();
            if (attacker.GetPrimaryTarget() == null && attacker.GetCombatList().Count > 0)
                target = attacker.GetCombatList()[0];
            if (target == null)
                return;

            double hitChance = ((attacker[STAT.BASEHIT] + attacker[STAT.HITMOD] + ability.mHitBonus) - (target[STAT.BASEEVADE] + target[STAT.EVADEMOD]));
            double attackRoll = mRand.NextDouble();

            // 20's are a crit
            if (attackRoll >= .95)
                isCrit = true;
            else if (attackRoll >= (1 - (hitChance / 100)))
                isHit = true;

            // Some abilities cannot miss
            if (!ability.mEvadable)
                isHit = true;

            // Process hits and misses
            if (!isHit)
                processMiss(attacker, target, ability);
            else
                processAbilityHit(attacker, target, ability, isCrit);

            // Abilities have cooldowns
            attacker.ModifyActionTimer(ability.mUseTime);
        }// abilityAttack

        // An ability just hit a mob
        public void processAbilityHit(CombatMob attacker, CombatMob target, Action ability, bool isCrit)
        {
            String damageString = String.Empty;
            int maxHp = target[STAT.BASEMAXHP] + target[STAT.MAXHPMOD];
            double damage = 0;

            // Abilities have differnt scaling, and have requirements like a weapon must be present to backstab
            if (ability.mDamScaling == DamageScaling.PERLEVEL)
            {
                int level = attacker[STAT.LEVEL];
                damage = mRand.Next(level * ability.mBaseMinDamage, level * ability.mBaseMaxDamage) + ability.mDamageBonus;
            }// if
            else if (ability.mDamScaling == DamageScaling.DAMAGEMULTPERLEVEL && ability.mWeaponRequired)
            {
                int level = attacker[STAT.LEVEL];
                Equipment weapon = (Equipment)(attacker[EQSlot.PRIMARY]);
                damage = mRand.Next(weapon.GetMinDam(), weapon.GetMaxDam()) * 2;
                damage += mRand.Next(level * ability.mBaseMinDamage, level * ability.mBaseMaxDamage) +
                          ability.mDamageBonus + attacker[STAT.DAMBONUSMOD] + attacker[STAT.BASEDAMBONUSMOD];
            }// else if

            // Crits do 50% more damage
            if (isCrit)
                damage *= 1.5 + 1;

            damage *= (1 - (target[ability.mDamType] / 100));

            if (damage == 0)
                damage = 1;

            target[STAT.CURRENTHP] -= (int)damage;

            if (!isCrit)
                damageString += "your " + ability.GetName() + " " + damageToString(maxHp, damage) +
                                " the " + target.GetName() + " for " + (int)damage + " damage";
            else
                damageString += "your critical " + ability.GetName() + " " + damageToString(maxHp, damage) +
                                " the " + target.GetName() + " for " + (int)damage + " damage";

            // Show the client how much damage they took
            attacker.safeWrite(damageString);

            checkDeath(attacker, target);
        }// processAbilityHit

        // A mob just got healed
        public void processHeal(CombatMob attacker, CombatMob target, Action ability, bool isCrit)
        {
            String healString = String.Empty;
            int maxHp = target[STAT.BASEMAXHP] + target[STAT.MAXHPMOD];
            double healAmount = 0;

            // Heals can also scale differently
            if (ability.mDamScaling == DamageScaling.PERLEVEL)
            {
                int level = attacker[STAT.LEVEL];
                healAmount = mRand.Next(level * ability.mBaseMinDamage, level * ability.mBaseMaxDamage) + ability.mDamageBonus;
            }// if

            // Heals can crit for 50% more
            if (isCrit)
                healAmount *= 1.5 + 1;

            target[STAT.CURRENTHP] += (int)healAmount;

            // You can't be healed for more than your max hp + modifiers
            if (target[STAT.CURRENTHP] > (target[STAT.BASEMAXHP] + target[STAT.MAXHPMOD]))
                target[STAT.CURRENTHP] = (target[STAT.BASEMAXHP] + target[STAT.MAXHPMOD]);

            if (!isCrit)
                healString += "your " + ability.GetName() + " heals " + target.GetName() + " for " + (int)healAmount + " hp";
            else
                healString += "your " + ability.GetName() + " critically heals " + target.GetName() + " for " + (int)healAmount + " hp";
            
            // Show the healing strings
            attacker.safeWrite(healString);

            if (attacker != target)
                target.safeWrite(attacker.GetName() + "'s " + ability.GetName() + " heals you for " + (int)healAmount + " hp");
        }// processHeal

        // The autoattack function
        public void attack(CombatMob attacker, CombatMob target)
        {
            double hitChance = hitChance = ((attacker[STAT.BASEHIT] + attacker[STAT.HITMOD]) - (target[STAT.BASEEVADE] + target[STAT.EVADEMOD]));
            bool isCrit = false;
            bool isHit = false;
            double attackRoll = mRand.NextDouble();
            Console.WriteLine("attack roll " + attackRoll);

            if (attackRoll >= .95)
            {
                isCrit = true;
                isHit = true;
            }
            else if (attackRoll >= (1 - (hitChance / 100)))
                isHit = true;

            Equipment weapon = (Equipment)(attacker[EQSlot.PRIMARY]);

            // Process hits and misses
            if (isHit) 
                processHit(attacker, target, weapon, isCrit);
            else
                processMiss(attacker, target, null);
        }// attack

        // Show the good old MUD style damage strings
        private String damageToString(int maxHp, double damage)
        {
            double dam = damage / maxHp;

            if (dam <= .01)
                return "barely scratches";
            else if (dam <= .02)
                return "scratches";
            else if (dam <= .03)
                return "hits";
            else if (dam <= .04)
                return "wounds";
            else if (dam <= .05)
                return "injures";
            else if (dam <= .07)
                return "thrashes";
            else if (dam <= .08)
                return "wrecks";
            else if (dam <= .1)
                return "maims";
            else if (dam <= .15)
                return "DECIMATES";
            else if (dam <= .2)
                return "MASSACRES";
            else if (dam <= .25)
                return "EVICERATES";
            else if (dam <= .3)
                return "DISEMBOWELS";
            else if (dam < 1)
                return "*OBLITERATES*";
            else
                return "***ANNIHILATES***";
        }// damageToSring

        // Process an autoattack hit
        private void processHit(CombatMob attacker, CombatMob target, Equipment weapon, bool isCrit)
        {
            double damage = 0;

            // You can attack wtih or without a weapon
            if (weapon != null)
                damage = mRand.Next(weapon.GetMinDam(), weapon.GetMaxDam()) + attacker[STAT.BASEDAMBONUSMOD] + attacker[STAT.DAMBONUSMOD];
            else
                damage = mRand.Next(attacker[STAT.BASEMINDAM], attacker[STAT.BASEMAXDAM]) + attacker[STAT.BASEDAMBONUSMOD] + attacker[STAT.DAMBONUSMOD];

            // Crits do 50% more damage
            if (isCrit)
                damage *= 1.5;

            if(weapon != null)
                damage *= (1 - (target[weapon.GetDamType()] / 100));
            else
                damage *= (1 - (target[DamageType.PHYSICAL] / 100));

            if ((int)damage == 0)
                damage = 1;

            target[STAT.CURRENTHP] -= (int)damage;
            String damageString = String.Empty;

            int maxHp = target[STAT.BASEMAXHP] + target[STAT.MAXHPMOD];

            if (!isCrit)
                damageString += "your attack " + damageToString(maxHp, damage) + " the " + target.GetName() + " for " + (int)damage + " damage";
            else
                damageString += "your critical hit " + damageToString(maxHp, damage) + " the " + target.GetName() + " for " + (int)damage + " damage";

            // Show the attacker how much damage he did
            attacker.safeWrite(damageString);

            damageString = String.Empty;

            if (isCrit)
                damageString += attacker.GetName() + " critically hits you for " + (int)damage + " damage";
            else
                damageString += attacker.GetName() + " hits you for " + (int)damage + " damage";

            // Show the target how much damage he took
            target.safeWrite(damageString);
        }// processHit

        // Autoattack just missed
        private void processMiss(CombatMob attacker, CombatMob target, Action ability)
        {
            String attackString = "attack";

            if (ability != null)
                attackString = ability.GetName();

            attacker.safeWrite("your " + attackString + " misses the " + target.GetName());
            target.safeWrite(attacker.GetName() + "'s " + attackString + " misses you");
        }// processMiss

        // Checks death, destroys the mob and sends out appropriate death strings to the client.
        private bool checkDeath(CombatMob attacker, CombatMob target)
        {
            bool ret = false;
            String clientString = String.Empty;

            if (target[STAT.CURRENTHP] <= 0)
            {
                endCombat(target);

                String attackerString = "you have slain the " + target.GetName();
                String receiversString = attacker.GetName() + " has slain " + target.GetName();
                Utils.Broadcast(attacker.GetCurrentRoom(), attacker, receiversString, attackerString);

                target.slain(attacker, ref clientString);
                target.safeWrite(clientString);

                ret = true;
            }// if

            return ret;

        }// checkDeath

        // Ends the combat for the target and all attackers that only have that target in their combat.
        public void endCombat(CombatMob target)
        {
            CombatMob attacker = null;

            for (int i = 0; i < target.GetCombatList().Count; ++i)
            {
                attacker = target.GetCombatList()[i];
                attacker.GetCombatList().Remove(target);

                if (attacker.GetPrimaryTarget() == target)
                    attacker.SetPrimaryTarget(null);

                if (attacker.GetCombatList().Count == 0)
                    Utils.UnsetFlag(ref attacker.mFlags, MobFlags.INCOMBAT);
            }

            target.GetCombatList().Clear();
            target.SetPrimaryTarget(null);
            Utils.UnsetFlag(ref target.mFlags, MobFlags.INCOMBAT);
        }// endCombat

        // Accessors
        public Queue<CombatMob> GetCombatQueue() { return mCombatQueue; }
        public object GetCombatLock() { return mQueueLock; }

    }// Class CombatHandler

}// Namespace _8th_Circle_Server
