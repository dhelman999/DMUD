using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;

namespace _8th_Circle_Server
{
    struct CombatData
    {
    }// EventData

    public class CombatHandler
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public Queue mCombatQueue;
        public World mWorld;

        private object mQueueLock;
        private Thread mSpinWorkThread;

        public CombatHandler(World world)
        {
            mCombatQueue = new Queue();
            mQueueLock = new object();
            mWorld = world;
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
                ch.attack(attacker, target);

                if (ch.checkDeath(attacker, target))
                {
                    if (combatList.Count == 0)
                    {
                        attacker.mFlagList.Remove(mobFlags.FLAG_INCOMBAT);
                        target.mStats.mCombatList.Clear();
                        return;
                    }// if
                }// if

                for (int i = 0; i < combatList.Count; ++i)
                {
                    target = (CombatMob)combatList[i];
                    ch.attack(target, attacker);

                    if (ch.checkDeath(target, attacker))
                    {
                        for (int j = 0; j < combatList.Count; ++j)
                        {
                            target = (CombatMob)combatList[j];
                            target.mFlagList.Remove(mobFlags.FLAG_INCOMBAT);  
                        }
                        attacker.mStats.mCombatList.Clear();
                        attacker.mFlagList.Remove(mobFlags.FLAG_INCOMBAT);
                        if (attacker is Player)
                            ((Player)attacker).mClientHandler.safeWrite(((Player)attacker).playerString());
                        return;
                    }// if
                }// for

                if (attacker is Player)
                    ((Player)attacker).mClientHandler.safeWrite(((Player)attacker).playerString());
                Thread.Sleep(4000);
            }// while(pl.mFlagList.Contains(mobFlags.FLAG_INCOMBAT))
        }// combatTask

        public void attack(CombatMob attacker, CombatMob target)
        {
            Player pl = null;
            if (attacker is Player)
                pl = (Player)attacker;
            Random rand = new Random();
            double hitChance = ((attacker.mStats.mBaseHit + attacker.mStats.mHitMod) -
                (target.mStats.mBaseEvade + target.mStats.mEvadeMod));
            bool isCrit = false;
            bool isHit = false;
            double attackRoll = rand.NextDouble();

            if (attackRoll >= .95)
                isCrit = true;
            else if (attackRoll >= (1-(hitChance/100)))
                isHit = true;
            
            if (isHit | isCrit)
            {
                Equipment weapon = null;

                if ((Equipment)attacker.mEQList[(int)EQSlot.PRIMARY] != null)
                    weapon = (Equipment)attacker.mEQList[(int)EQSlot.PRIMARY];
                
                processHit(attacker, target, weapon, isCrit);
            }
            else
                processMiss(attacker, target);
        }// attack

        private string damageToString(EQType type)
        {
            switch (type)
            {
                case EQType.SLASHING:
                    return "slash";

                case EQType.PIERCING:
                    return "pierce";

                case EQType.BLUDGEONING:
                    return "smash";

                default:
                    return "hit";
            }// switch
        }// damageToSring

        private void processHit(CombatMob attacker, CombatMob target, Equipment weapon, bool isCrit)
        {
            string damageString = string.Empty;
            double damage;
            Random rand = new Random();

            if (weapon != null)
                damage = rand.Next(weapon.mMinDam, weapon.mMaxDam) + attacker.mStats.mDamBonusMod;
            else
                damage = rand.Next(attacker.mStats.mBaseMinDam, attacker.mStats.mBaseMaxDam)
                    + attacker.mStats.mBaseDamBonus;

            if (isCrit)
                damage *= 1.5;

            if(weapon != null)
                damage *= (1 - ((double)target.mResistances[(int)weapon.mDamType] / 100));
            else
                damage *= (1 - ((double)target.mResistances[(int)DamageType.PHYSICAL] / 100));

            if (damage == 0)
                damage = 1;

            target.mStats.mCurrentHp -= (int)damage;

            if (attacker is Player)
            {
                if (isCrit && weapon != null)
                    damageString += "your " + damageToString(weapon.mType) + " critically hits the " +
                       target.mName + " for " + (int)damage + " damage";
                else if (isCrit && weapon == null)
                    damageString += "you critically hit the " +
                       target.mName + " for " + (int)damage + " damage";
                else if (weapon != null)
                    damageString += "you " + damageToString(weapon.mType) + " the " + target.mName +
                        " for " + (int)damage + " damage";
                else
                    damageString += "you hit the " + target.mName + " for "
                        + (int)damage + " damage";

                ((Player)attacker).mClientHandler.safeWrite(damageString);
            }// if
            if (target is Player)
            {
                damageString = string.Empty;
                if (isCrit)
                    damageString += attacker.mName + " critically hits you for " +
                        (int)damage + " damage";
                else
                    damageString += attacker.mName + " hits you for " +
                        (int)damage + " damage";

                ((Player)target).mClientHandler.safeWrite(damageString);
            }// if
        }// processHit

        private void processMiss(CombatMob attacker, CombatMob target)
        {
            if (attacker is Player)
                ((Player)attacker).mClientHandler.safeWrite("you miss the " + target.mName);
            if (target is Player)
                ((Player)target).mClientHandler.safeWrite(attacker.mName + " misses you");
        }// processMiss

        private bool checkDeath(CombatMob attacker, CombatMob target)
        {
            if (target.mStats.mCurrentHp <= 0)
            {
                attacker.mStats.mCombatList.Remove(target);
                if (attacker is Player)
                {
                    ((Player)attacker).mClientHandler.safeWrite("you have slain the " + target.mName);
                    target.slain(attacker);
                }
                if(target is Player)
                    ((Player)target).mClientHandler.safeWrite(target.slain(attacker));

                return true;
            }// if
            return false;
        }// checkDeath

    }// Class CombatHandler

}// Namespace _8th_Circle_Server
