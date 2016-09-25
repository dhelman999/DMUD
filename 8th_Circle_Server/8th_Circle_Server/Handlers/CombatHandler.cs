using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;

namespace _8th_Circle_Server
{
    public class CombatHandler
    {
        // Debug
        internal const bool DEBUG = false;
        internal int SEED = 0;

        // Member Variables
        public Queue<CombatMob> mCombatQueue;
        public World mWorld;
        public Random mRand;

        // TODO
        // Is this the best way to not spawn new combat threads
        // when someone issues another attack command?
        public static List<CombatMob> sCurrentCombats;

        private object mQueueLock;
        private Thread mSpinWorkThread;

        public CombatHandler(World world)
        {
            mCombatQueue = new Queue<CombatMob>();
            mQueueLock = new object();
            mWorld = world;
            mRand = new Random();
            sCurrentCombats = new List<CombatMob>();
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
                    combatHandler.processCombat();
                }// catch
            }// while
        }// spinWork
        
        public void enQueueCombat(CombatMob mob)
        {
            lock (mQueueLock)
            {
                mCombatQueue.Enqueue(mob);
            }// lock

            mSpinWorkThread.Interrupt();
        }// enQueueEvent

        private void processCombat()
        {
            while (mCombatQueue.Count > 0)
            {
                CombatMob mob = mCombatQueue.Dequeue();
                sCurrentCombats.Add(mob);
                Thread combatThread = new Thread(() => combatTask(this, mob));
                combatThread.Start();
            }// while    
        }// processCombat

        public static void combatTask(CombatHandler ch, CombatMob attacker)
        {
            List<CombatMob> combatList = attacker.mStats.mCombatList;

            while (combatList.Count > 0)
            {     
                if (attacker.mStats.mPrimaryTarget == null)
                    attacker.mStats.mPrimaryTarget = combatList[0];

                CombatMob target = attacker.mStats.mPrimaryTarget;
                ch.attack(attacker, target);
                ch.checkDeath(attacker, target);

                for (int i = 0; i < combatList.Count; ++i)
                {
                    target = combatList[i];
                    ch.attack(target, attacker);
                    ch.checkDeath(target, attacker);
                }

                if (attacker.mResType == ResType.PLAYER)
                    attacker.mClientHandler.safeWrite((attacker).playerString());

                Thread.Sleep(4000);
            }// while(pl.mFlagList.Contains(MobFlags.FLAG_INCOMBAT))
        }// combatTask

        public void executeSpell(CombatMob attacker, CombatMob target, Action spell)
        {
            bool isCrit = false;

            if (target == null && attacker.mStats.mCombatList.Count > 0)
                target = attacker.mStats.mCombatList[0];

            if (target == null)
                return;

            double spellRoll = mRand.NextDouble();

            if (spellRoll >= .95)
                isCrit = true;

            bool isHit = true;
            attacker.mStats.mCurrentMana -= spell.mManaCost;

            if (!isHit)
                processMiss(attacker, target, spell);
            else if (spell.mDamType == DamageType.HEALING)
                processHeal(attacker, target, spell, isCrit);
            else
                processAbilityHit(attacker, target, spell, isCrit);

            attacker.mActionTimer += spell.mUseTime;
        }// executeSpell

        public void abilityAttack(CombatMob attacker, CombatMob target, Action ability)
        {
            bool isCrit = false;
            bool isHit = false;

            if (target == null)
                target = attacker.mStats.mPrimaryTarget;
            if (attacker.mStats.mPrimaryTarget == null && attacker.mStats.mCombatList.Count > 0)
                target = attacker.mStats.mCombatList[0];
            if (target == null)
                return;

            double hitChance = ((attacker.mStats.mBaseHit + attacker.mStats.mHitMod + ability.mHitBonus) -
                               (target.mStats.mBaseEvade + target.mStats.mEvadeMod));
            
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

            attacker.mActionTimer += ability.mUseTime;
        }// abilityAttack

        public void processAbilityHit(CombatMob attacker, CombatMob target, Action ability, bool isCrit)
        {
            string damageString = string.Empty;
            int maxHp = target.mStats.mBaseMaxHp + target.mStats.mMaxHpMod;
            double damage = 0;

            if (ability.mDamScaling == DamageScaling.PERLEVEL)
            {
                int level = attacker.mStats.mLevel;
                damage = mRand.Next(level * ability.mBaseMinDamage, level * ability.mBaseMaxDamage) + ability.mDamageBonus;
            }// if
            else if (ability.mDamScaling == DamageScaling.DAMAGEMULTPERLEVEL && ability.mWeaponRequired)
            {
                int level = attacker.mStats.mLevel;
                Equipment weapon = (Equipment)(attacker.mEQList[(int)EQSlot.PRIMARY]);
                damage = mRand.Next(weapon.mMinDam, weapon.mMaxDam) * 2;
                damage += mRand.Next(level * ability.mBaseMinDamage, level * ability.mBaseMaxDamage) +
                          ability.mDamageBonus + attacker.mStats.mDamBonusMod + attacker.mStats.mBaseDamBonus;
            }// else if

            if (isCrit)
                damage *= 1.5 + 1;

            damage *= (1 - (target.mResistances[(int)ability.mDamType] / 100));

            if (damage == 0)
                damage = 1;

            target.mStats.mCurrentHp -= (int)damage;

            if (!isCrit)
                damageString += "your " + ability.mName + " " + damageToString(maxHp, damage) +
                                " the " + target.mName + " for " + (int)damage + " damage";
            else
                damageString += "your critical " + ability.mName + " " + damageToString(maxHp, damage) +
                                " the " + target.mName + " for " + (int)damage + " damage";

            if (attacker.mResType == ResType.PLAYER)
                ((CombatMob)attacker).mClientHandler.safeWrite(damageString);

            checkDeath(attacker, target);
        }// processAbilityHit

        public void processHeal(CombatMob attacker, CombatMob target, Action ability, bool isCrit)
        {
            string healString = string.Empty;
            int maxHp = target.mStats.mBaseMaxHp + target.mStats.mMaxHpMod;
            double healAmount = 0;

            if (ability.mDamScaling == DamageScaling.PERLEVEL)
            {
                int level = attacker.mStats.mLevel;
                healAmount = mRand.Next(level * ability.mBaseMinDamage, level * ability.mBaseMaxDamage) + ability.mDamageBonus;
            }// if

            if (isCrit)
                healAmount *= 1.5 + 1;

            target.mStats.mCurrentHp += (int)healAmount;

            if (target.mStats.mCurrentHp > (target.mStats.mBaseMaxHp + target.mStats.mMaxHpMod))
                target.mStats.mCurrentHp = (target.mStats.mBaseMaxHp + target.mStats.mMaxHpMod);

            if (!isCrit)
                healString += "your " + ability.mName + " heals " + target.mName + " for " + (int)healAmount + " hp";
            else
                healString += "your " + ability.mName + "critically heals " + target.mName + " for " + (int)healAmount + " hp";
            
            if (attacker.mResType == ResType.PLAYER)
                ((CombatMob)attacker).mClientHandler.safeWrite(healString);

            if (attacker != target && target.mResType == ResType.PLAYER)
                ((CombatMob)target).mClientHandler.safeWrite(attacker.mName + "'s " + ability.mName + " heals you for " + (int)healAmount + " hp");
        }// processHeal

        public void attack(CombatMob attacker, CombatMob target)
        {
            double hitChance = hitChance = ((attacker.mStats.mBaseHit + attacker.mStats.mHitMod) - (target.mStats.mBaseEvade + target.mStats.mEvadeMod));
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

            Equipment weapon = (Equipment)(attacker.mEQList[(int)EQSlot.PRIMARY]);

            if (isHit) 
                processHit(attacker, target, weapon, isCrit);
            else
                processMiss(attacker, target, null);
        }// attack

        private string damageToString(int maxHp, double damage)
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
                damage = mRand.Next(weapon.mMinDam, weapon.mMaxDam) + attacker.mStats.mBaseDamBonus + attacker.mStats.mDamBonusMod;
            else
                damage = mRand.Next(attacker.mStats.mBaseMinDam, attacker.mStats.mBaseMaxDam) + attacker.mStats.mBaseDamBonus + attacker.mStats.mDamBonusMod;

            if (isCrit)
                damage *= 1.5;

            if(weapon != null)
                damage *= (1 - (target.mResistances[(int)weapon.mDamType] / 100));
            else
                damage *= (1 - (target.mResistances[(int)DamageType.PHYSICAL] / 100));

            if ((int)damage == 0)
                damage = 1;

            target.mStats.mCurrentHp -= (int)damage;
            string damageString = string.Empty;

            if (attacker.mResType == ResType.PLAYER)
            {
                int maxHp = target.mStats.mBaseMaxHp + target.mStats.mMaxHpMod;

                if (!isCrit)
                    damageString += "your attack " + damageToString(maxHp, damage) + " the " + target.mName + " for " + (int)damage + " damage";
                else
                    damageString += "your critical hit " + damageToString(maxHp, damage) + " the " + target.mName + " for " + (int)damage + " damage";

                ((CombatMob)attacker).mClientHandler.safeWrite(damageString);
            }// if
            if (target.mResType == ResType.PLAYER)
            {
                damageString = string.Empty;

                if (isCrit)
                    damageString += attacker.mName + " critically hits you for " + (int)damage + " damage";
                else
                    damageString += attacker.mName + " hits you for " + (int)damage + " damage";

                ((CombatMob)target).mClientHandler.safeWrite(damageString);
            }// if
        }// processHit

        private void processMiss(CombatMob attacker, CombatMob target, Action ability)
        {
            string attackString = "attack";

            if (ability != null)
                attackString = ability.mName;

            if (attacker.mResType == ResType.PLAYER)
                    ((CombatMob)attacker).mClientHandler.safeWrite("your " + attackString + " misses the " + target.mName);
            if (target.mResType == ResType.PLAYER)
                    ((CombatMob)target).mClientHandler.safeWrite(attacker.mName + "'s " + attackString + " misses you");
        }// processMiss

        private bool checkDeath(CombatMob attacker, CombatMob target)
        {
            if (target.mStats.mCurrentHp <= 0)
            {
                CombatMob cm = null;

                for (int i = 0; i < target.mStats.mCombatList.Count; ++i)
                {
                    cm = target.mStats.mCombatList[i];
                    cm.mStats.mCombatList.Remove(target);

                    if (cm.mStats.mCombatList.Count == 0)
                    {
                        cm.mFlagList.Remove(MobFlags.FLAG_INCOMBAT);
                        sCurrentCombats.Remove(cm);
                    }
                }

                target.mStats.mCombatList.Clear();
                target.mFlagList.Remove(MobFlags.FLAG_INCOMBAT);
                sCurrentCombats.Remove(target);

                if (attacker.mResType == ResType.PLAYER)
                {
                    ((CombatMob)attacker).mClientHandler.safeWrite("you have slain the " + target.mName);
                    target.slain(attacker);
                }
                if(target.mResType == ResType.PLAYER)
                    ((CombatMob)target).mClientHandler.safeWrite(target.slain(attacker));

                if (attacker.mStats.mPrimaryTarget == target)
                    attacker.mStats.mPrimaryTarget = null;

                return true;
            }// if

            return false;

        }// checkDeath

    }// Class CombatHandler

}// Namespace _8th_Circle_Server
