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
        public Queue mCombatQueue;
        public World mWorld;
        public Random mRand;
        private object mQueueLock;
        private Thread mSpinWorkThread;

        public CombatHandler(World world)
        {
            mCombatQueue = new Queue();
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
                CombatMob mob = (CombatMob)mCombatQueue.Dequeue();
                Thread combatThread = new Thread(() => combatTask(this, mob));
                combatThread.Start();
            }// while
                
        }// processCombat

        public static void combatTask(CombatHandler ch, CombatMob attacker)
        {
            ArrayList combatList = attacker.mStats.mCombatList;

            while (combatList.Count > 0)
            {
                CombatMob target = (CombatMob)combatList[0];
                ch.attack(attacker, target, false);

                // Make this a generic sequence
                if (ch.checkDeath(attacker, target))
                {
                    if (combatList.Count == 0)
                    {
                        attacker.mFlagList.Remove(MobFlags.FLAG_INCOMBAT);
                        target.mStats.mCombatList.Clear();
                        return;
                    }// if
                }// if

                for (int i = 0; i < combatList.Count; ++i)
                {
                    target = (CombatMob)combatList[i];
                    ch.attack(target, attacker, false);

                    if (ch.checkDeath(target, attacker))
                    {
                        for (int j = 0; j < combatList.Count; ++j)
                        {
                            target = (CombatMob)combatList[j];
                            target.mFlagList.Remove(MobFlags.FLAG_INCOMBAT);  
                        }
                        attacker.mStats.mCombatList.Clear();
                        attacker.mFlagList.Remove(MobFlags.FLAG_INCOMBAT);
                        if (attacker.mResType == ResType.PLAYER)
                            ((CombatMob)attacker).mClientHandler.safeWrite(((CombatMob)attacker).playerString());
                        return;
                    }// if
                }// for

                if (attacker.mResType == ResType.PLAYER)
                    ((CombatMob)attacker).mClientHandler.safeWrite(((CombatMob)attacker).playerString());
                Thread.Sleep(4000);
            }// while(pl.mFlagList.Contains(MobFlags.FLAG_INCOMBAT))
        }// combatTask

        public void abilityAttack(CombatMob attacker, CombatMob target, Action action)
        {
            if (target == null && attacker.mStats.mCombatList.Count > 0)
                target = (CombatMob)attacker.mStats.mCombatList[0];

            if (target == null)
                return;

            double hitChance = ((attacker.mStats.mBaseHit + attacker.mStats.mHitMod + action.mHitBonus) -
                (target.mStats.mBaseEvade + target.mStats.mEvadeMod));
            bool isCrit = false;
            bool isHit = false;
            double attackRoll = mRand.NextDouble();

            if (attackRoll >= .95)
                isCrit = true;
            else if (attackRoll >= (1 - (hitChance / 100)))
                isHit = true;

            if (!action.mEvadable)
                isHit = true;

            switch (action.mAbilitySpell)
            {
                case AbilitySpell.ABILITY_BASH:
                    if (!isHit)
                        processMiss(attacker, target, false);
                    else
                        processAbility(attacker, target, action, isCrit);

                    // TODO 
                    // doesn't need to be defined here, but in the ability instead
                    attacker.mActionTimer += action.mUseTime;
                    break;

                default:
                    break;
            }// switch
        }// abilityAttack

        public void processAbility(CombatMob attacker, CombatMob target, Action ability, bool isCrit)
        {
            string damageString = string.Empty;
            int maxHp = target.mStats.mBaseMaxHp + target.mStats.mMaxHpMod;
            double damage = 0;

            if (ability.mDamScaling == DamageScaling.PERLEVEL)
            {
                int level = attacker.mStats.mLevel;
                damage = mRand.Next(level * ability.mBaseMinDamage, level * ability.mBaseMaxDamage) + 
                    ability.mDamageBonus;
            }// if

            if (isCrit)
                damage *= 1.5;

            damage *= (1 - ((double)target.mResistances[(int)ability.mDamType] / 100));

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

            if (checkDeath(attacker, target))
            {
                if (attacker.mStats.mCombatList.Count == 0)
                {
                    attacker.mFlagList.Remove(MobFlags.FLAG_INCOMBAT);
                    target.mStats.mCombatList.Clear();
                    return;
                }// if
            }// if
        }// processAbility

        public void attack(CombatMob attacker, CombatMob target, bool isBackstab)
        {
            double hitChance = hitChance = ((attacker.mStats.mBaseHit + attacker.mStats.mHitMod) -
                    (target.mStats.mBaseEvade + target.mStats.mEvadeMod));
            // TODO
            // Probably need to make this more generic
            if (isBackstab)
                hitChance += 10;
            
            bool isCrit = false;
            bool isHit = false;
            double attackRoll = mRand.NextDouble();
            // TODO
            // Investigate why random seeds suck so bad
            Console.WriteLine("attack roll " + attackRoll);

            if (attackRoll >= .95)
                isCrit = true;
            else if (attackRoll >= (1-(hitChance/100)))
                isHit = true;
            
            if (isHit | isCrit)
            {
                Equipment weapon = null;

                if ((Equipment)attacker.mEQList[(int)EQSlot.PRIMARY] != null)
                    weapon = (Equipment)attacker.mEQList[(int)EQSlot.PRIMARY];
                
                processHit(attacker, target, weapon, isCrit, isBackstab);
            }
            else
                processMiss(attacker, target, isBackstab);
        }// attack

        public void backstab(CombatMob attacker, CombatMob target)
        {
            attack(attacker, target, true);
            attacker.mActionTimer += 4;
        }// backstab

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

        private void processHit(CombatMob attacker, CombatMob target, Equipment weapon, 
            bool isCrit, bool isBackstab)
        {
            string damageString = string.Empty;
            double damage;

            if (weapon != null)
                damage = mRand.Next(weapon.mMinDam, weapon.mMaxDam) + attacker.mStats.mBaseDamBonus +
                    attacker.mStats.mDamBonusMod;
            else
                damage = mRand.Next(attacker.mStats.mBaseMinDam, attacker.mStats.mBaseMaxDam)
                    + attacker.mStats.mBaseDamBonus + attacker.mStats.mDamBonusMod;

            if (isBackstab)
            {
                damage *= 2;
                damage += mRand.Next(1 * attacker.mStats.mLevel + 6 * attacker.mStats.mLevel);
            }
            if (isCrit)
                damage *= 1.5;

            if(weapon != null)
                damage *= (1 - ((double)target.mResistances[(int)weapon.mDamType] / 100));
            else
                damage *= (1 - ((double)target.mResistances[(int)DamageType.PHYSICAL] / 100));

            if ((int)damage == 0)
                damage = 1;

            target.mStats.mCurrentHp -= (int)damage;

            if (attacker.mResType == ResType.PLAYER)
            {
                int maxHp = target.mStats.mBaseMaxHp + target.mStats.mMaxHpMod;

                if (!isCrit && !isBackstab)
                    damageString += "your attack " + damageToString(maxHp, damage) +
                        " the " + target.mName + " for " + (int)damage + " damage";
                else if (!isCrit && isBackstab)
                    damageString += "your backstab " + damageToString(maxHp, damage) +
                        " the " + target.mName + " for " + (int)damage + " damage";
                else if (isCrit && !isBackstab)
                    damageString += "your critical hit " + damageToString(maxHp, damage) +
                        " the " + target.mName + " for " + (int)damage + " damage";
                else
                    damageString += "your critical backstab " + damageToString(maxHp, damage) +
                        " the " + target.mName + " for " + (int)damage + " damage";

                ((CombatMob)attacker).mClientHandler.safeWrite(damageString);

                if (isBackstab)
                    Thread.Sleep(1000);
            }// if
            if (target.mResType == ResType.PLAYER)
            {
                damageString = string.Empty;
                if (isCrit)
                    damageString += attacker.mName + " critically hits you for " +
                        (int)damage + " damage";
                else
                    damageString += attacker.mName + " hits you for " +
                        (int)damage + " damage";

                ((CombatMob)target).mClientHandler.safeWrite(damageString);
            }// if
        }// processHit

        private void processMiss(CombatMob attacker, CombatMob target, bool isBackstab)
        {
            if (attacker.mResType == ResType.PLAYER)
                if(!isBackstab)
                    ((CombatMob)attacker).mClientHandler.safeWrite("you miss the " + target.mName);
                else
                    ((CombatMob)attacker).mClientHandler.safeWrite("your backstab misses the " + target.mName);
            if (target.mResType == ResType.PLAYER)
                if (!isBackstab)
                    ((CombatMob)target).mClientHandler.safeWrite(attacker.mName + " misses you");
                else
                    ((CombatMob)attacker).mClientHandler.safeWrite(attacker.mName + "'s backstab misses you");
        }// processMiss

        private bool checkDeath(CombatMob attacker, CombatMob target)
        {
            if (target.mStats.mCurrentHp <= 0)
            {
                attacker.mStats.mCombatList.Remove(target);
                if (attacker.mResType == ResType.PLAYER)
                {
                    ((CombatMob)attacker).mClientHandler.safeWrite("you have slain the " + target.mName);
                    target.slain(attacker);
                }
                if(target.mResType == ResType.PLAYER)
                    ((CombatMob)target).mClientHandler.safeWrite(target.slain(attacker));

                return true;
            }// if
            return false;
        }// checkDeath

    }// Class CombatHandler

}// Namespace _8th_Circle_Server
