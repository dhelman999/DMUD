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
                Thread combatThread = new Thread(() => combatTask(mob));
                combatThread.Start();
            }// while
                
        }// processCombat

        public static void combatTask(CombatMob mob)
        {
            if (mob is Player)
            {
                Player pl = (Player)mob;
                Random rand = new Random();

                while (pl.mFlagList.Contains(mobFlags.FLAG_INCOMBAT))
                {
                    foreach (Npc npc in pl.mStats.mCombatList)
                    {
                        int damage = 0;

                        if (rand.NextDouble() > .25)
                        {
                            if (pl.mEQList[(int)EQSlot.PRIMARY] == null)
                            {
                                damage = rand.Next(pl.mStats.mBaseMinDam, pl.mStats.mBaseMaxDam) +
                                    pl.mStats.mBaseDamBonus;
                                pl.mClientHandler.safeWrite("you hit the " + npc.mName + " for " + damage + " damage");
                            }// if
                            else
                            {
                                string damageString = string.Empty;
                                Equipment weapon = (Equipment)pl.mEQList[(int)EQSlot.PRIMARY];

                                damage = rand.Next(weapon.mMinDam, weapon.mMaxDam) +
                                    pl.mStats.mBaseDamBonus;

                                if (weapon.mType == EQType.SLASHING)
                                    damageString += "you slash ";
                                else
                                    damageString += "you hit ";

                                pl.mClientHandler.safeWrite(damageString + "the " + npc.mName + " for " + damage + " damage");
                            }
                            if ((npc.mStats.mCurrentHp -= damage) <= 0)
                            {
                                pl.mClientHandler.safeWrite("you have slain the " + npc.mName);
                                pl.mFlagList.Remove(mobFlags.FLAG_INCOMBAT);
                                pl.mStats.mCombatList.Remove(npc);
                                npc.destroy();
                                break;
                            }
                        }
                        else
                            pl.mClientHandler.safeWrite("you miss " + npc.mName);

                        if (rand.NextDouble() > .25)
                        {
                            damage = rand.Next(npc.mStats.mBaseMinDam, npc.mStats.mBaseMaxDam) +
                                npc.mStats.mBaseDamBonus;
                            pl.mClientHandler.safeWrite(npc.mName + " hits you" + " for " + damage + " damage");
                            if ((pl.mStats.mCurrentHp -= damage) <= 0)
                            {
                                pl.mClientHandler.safeWrite("you have been slain by the " + npc.mName);
                                pl.mFlagList.Remove(mobFlags.FLAG_INCOMBAT);
                                npc.mFlagList.Remove(mobFlags.FLAG_INCOMBAT);
                                pl.mStats.mCombatList.Remove(npc);
                                break;
                            }
                        }
                        else
                            pl.mClientHandler.safeWrite(npc.mName + " misses you");
                    }// foreach (Npc npc in pl.mStats.mCombatList)

                    pl.mClientHandler.safeWrite(pl.playerString());
                    Thread.Sleep(4000);
                }// while(pl.mFlagList.Contains(mobFlags.FLAG_INCOMBAT))
            }// if
        }// combatTask

    }// Class CombatHandler

}// Namespace _8th_Circle_Server
