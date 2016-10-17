using System;
using System.Collections.Generic;
using System.Threading;

namespace _8th_Circle_Server
{
    public class CombatHandler
    {
        private Queue<CombatMob> mCombatQueue;
        private World mWorld;
        private Random mRand;
        private object mQueueLock;
        private Thread mSpinWorkThread;

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
                    lock (combatHandler.GetCombatLock())
                    {
                        Queue<CombatMob> combatQueue = combatHandler.GetCombatQueue();

                        if (combatQueue.Count > 0)
                        {
                            Thread combatThread = new Thread(() => combatTask(combatHandler, combatQueue.Dequeue()));
                            combatThread.Start();
                        }
                    }
                }// catch
            }// while
        }// spinWork
        
        public void enQueueCombat(CombatMob mob)
        {
            lock (mQueueLock)
            {
                mCombatQueue.Enqueue(mob);
            }

            mSpinWorkThread.Interrupt();
        }// enQueueCombat

        public static void combatTask(CombatHandler combatHandler, CombatMob attacker)
        {
            List<CombatMob> combatList = attacker.GetCombatList();

            while (combatList.Count > 0)
            {  
                if (attacker.GetPrimaryTarget() == null)
                    attacker.SetPrimaryTarget(combatList[0]);

                CombatMob target = attacker.GetPrimaryTarget();
                combatHandler.attack(attacker, target);
                combatHandler.checkDeath(attacker, target);

                for (int i = 0; i < combatList.Count; ++i)
                {
                    target = combatList[i];
                    combatHandler.attack(target, attacker);
                    combatHandler.checkDeath(target, attacker);
                }

                attacker.safeWrite(attacker.playerString());
                target.safeWrite(target.playerString());

                Thread.Sleep(4000);
            }// while (combatList.Count > 0)
        }// combatTask

        public void executeSpell(CombatMob attacker, CombatMob target, Action spell)
        {
            bool isCrit = false;

            if (target == null && attacker.GetCombatList().Count > 0)
                target = attacker.GetCombatList()[0];

            if (target == null)
                return;

            double spellRoll = mRand.NextDouble();

            if (spellRoll >= .95)
                isCrit = true;

            bool isHit = true;
            attacker[STAT.CURRENTMANA] -= spell.mManaCost;

            if (!isHit)
                processMiss(attacker, target, spell);
            else if (spell.mDamType == DamageType.HEALING)
                processHeal(attacker, target, spell, isCrit);
            else
                processAbilityHit(attacker, target, spell, isCrit);

            attacker.ModifyActionTimer(spell.mUseTime);
        }// executeSpell

        public void abilityAttack(CombatMob attacker, CombatMob target, Action ability)
        {
            bool isCrit = false;
            bool isHit = false;

            if (target == null)
                target = attacker.GetPrimaryTarget();
            if (attacker.GetPrimaryTarget() == null && attacker.GetCombatList().Count > 0)
                target = attacker.GetCombatList()[0];
            if (target == null)
                return;

            double hitChance = ((attacker[STAT.BASEHIT] + attacker[STAT.HITMOD] + ability.mHitBonus) - (target[STAT.BASEEVADE] + target[STAT.EVADEMOD]));
            double attackRoll = mRand.NextDouble();

            if (attackRoll >= .95)
                isCrit = true;
            else if (attackRoll >= (1 - (hitChance / 100)))
                isHit = true;

            if (!ability.mEvadable)
                isHit = true;

            if (!isHit)
                processMiss(attacker, target, ability);
            else
                processAbilityHit(attacker, target, ability, isCrit);

            attacker.ModifyActionTimer(ability.mUseTime);
        }// abilityAttack

        public void processAbilityHit(CombatMob attacker, CombatMob target, Action ability, bool isCrit)
        {
            String damageString = String.Empty;
            int maxHp = target[STAT.BASEMAXHP] + target[STAT.MAXHPMOD];
            double damage = 0;

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

            attacker.safeWrite(damageString);

            checkDeath(attacker, target);
        }// processAbilityHit

        public void processHeal(CombatMob attacker, CombatMob target, Action ability, bool isCrit)
        {
            String healString = String.Empty;
            int maxHp = target[STAT.BASEMAXHP] + target[STAT.MAXHPMOD];
            double healAmount = 0;

            if (ability.mDamScaling == DamageScaling.PERLEVEL)
            {
                int level = attacker[STAT.LEVEL];
                healAmount = mRand.Next(level * ability.mBaseMinDamage, level * ability.mBaseMaxDamage) + ability.mDamageBonus;
            }// if

            if (isCrit)
                healAmount *= 1.5 + 1;

            target[STAT.CURRENTHP] += (int)healAmount;

            if (target[STAT.CURRENTHP] > (target[STAT.BASEMAXHP] + target[STAT.MAXHPMOD]))
                target[STAT.CURRENTHP] = (target[STAT.BASEMAXHP] + target[STAT.MAXHPMOD]);

            if (!isCrit)
                healString += "your " + ability.GetName() + " heals " + target.GetName() + " for " + (int)healAmount + " hp";
            else
                healString += "your " + ability.GetName() + " critically heals " + target.GetName() + " for " + (int)healAmount + " hp";
            
            attacker.safeWrite(healString);

            if (attacker != target)
                target.safeWrite(attacker.GetName() + "'s " + ability.GetName() + " heals you for " + (int)healAmount + " hp");
        }// processHeal

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

            if (isHit) 
                processHit(attacker, target, weapon, isCrit);
            else
                processMiss(attacker, target, null);
        }// attack

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

        private void processHit(CombatMob attacker, CombatMob target, Equipment weapon, bool isCrit)
        {
            double damage = 0;

            if (weapon != null)
                damage = mRand.Next(weapon.GetMinDam(), weapon.GetMaxDam()) + attacker[STAT.BASEDAMBONUSMOD] + attacker[STAT.DAMBONUSMOD];
            else
                damage = mRand.Next(attacker[STAT.BASEMINDAM], attacker[STAT.BASEMAXDAM]) + attacker[STAT.BASEDAMBONUSMOD] + attacker[STAT.DAMBONUSMOD];

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

            attacker.safeWrite(damageString);

            damageString = String.Empty;

            if (isCrit)
                damageString += attacker.GetName() + " critically hits you for " + (int)damage + " damage";
            else
                damageString += attacker.GetName() + " hits you for " + (int)damage + " damage";

            target.safeWrite(damageString);
        }// processHit

        private void processMiss(CombatMob attacker, CombatMob target, Action ability)
        {
            String attackString = "attack";

            if (ability != null)
                attackString = ability.GetName();

            attacker.safeWrite("your " + attackString + " misses the " + target.GetName());
            target.safeWrite(attacker.GetName() + "'s " + attackString + " misses you");
        }// processMiss

        private bool checkDeath(CombatMob attacker, CombatMob target)
        {
            bool ret = false;
            String clientString = String.Empty;

            if (target[STAT.CURRENTHP] <= 0)
            {
                endCombat(target);

                String attackerString = "you have slain the " + target.GetName();
                String receiversString = attacker.GetName() + " has slain " + target.GetName();
                Utils.broadcast(attacker.GetCurrentRoom(), attacker, receiversString, attackerString);

                target.slain(attacker, ref clientString);
                target.safeWrite(clientString);

                ret = true;
            }// if

            return ret;

        }// checkDeath

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
