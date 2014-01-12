using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;

namespace _8th_Circle_Server
{
    public class AreaHandler
    {
        // Debug
        internal const bool DEBUG = false;

        internal const int TICKTIME = 1;

        // Member Variables
        public ArrayList mAreaList;
        public World mWorld;

        private object mQueueLock;
        private Thread mSpinWorkThread;

        public AreaHandler(World world)
        {
            mAreaList = new ArrayList();
            mWorld = world;
            mQueueLock = new object();
        }// Constructor

        public void start()
        {
            mSpinWorkThread = new Thread(() => spinWork(this));
            mSpinWorkThread.Start();
        }// start

        public static void spinWork(AreaHandler areaHandler)
        {
            bool processed;

            while (true)
            {
                processed = false;
                
                try
                {
                    Thread.Sleep(TICKTIME*1000);
                }// try
                catch
                {
                    areaHandler.processAreas();
                    areaHandler.processNpcs();
                    processed = true;
                }// catch

                if (!processed)
                {
                    areaHandler.processAreas();
                    areaHandler.processNpcs();
                }// if
            }// while
        }// spinWork

        public void registerArea(Area area)
        {
            lock (mQueueLock)
            {
                mAreaList.Add(area);
            }// lock
        }// enQueueArea

        private void processAreas()
        {
            ArrayList targetList = new ArrayList();
            targetList.Add(mWorld.getRes(ResType.NPC));
            targetList.Add(mWorld.getRes(ResType.PLAYER));

            // Decrement the action timers for all mobs in the game
            foreach (ArrayList ar in targetList)
            {
                foreach (Mob mob in ar)
                {
                    if (mob.mActionTimer > 0)
                        --(mob.mActionTimer);
                }// foreach
            }// foreach

            // Process each area
            foreach (Area area in mAreaList)
            {
                for (int i = 0; i < area.mFullMobList.Count; ++i)
                {
                    Mob mob = (Mob)area.mFullMobList[i];

                    if (mob.mIsRespawning &&
                       (mob.mCurrentRespawnTime -= TICKTIME) <= 0)
                    {
                        if (mob.mFlagList.Contains(MobFlags.FLAG_INCOMBAT))
                        {
                            mob.mIsRespawning = false;
                            mob.mCurrentRespawnTime = mob.mStartingRespawnTime;
                        }
                        else
                        {
                            for (int j = 0; j < mob.mChildren.Count; ++j)
                            {
                                ((Mob)mob.mChildren[j]).destroy();
                                --j;
                            }

                            Console.WriteLine("respawning " + mob.mName);
                            mob.respawn();
                        }
                    }
                }// for

                if ((area.mCurrentRespawnTimer -= TICKTIME) <= 0)
                {
                    targetList.Clear();
                    targetList.Add(area.getRes(ResType.OBJECT));
                    targetList.Add(area.getRes(ResType.NPC));

                    // Loop through all Mob arrays
                    foreach (ArrayList ar in targetList)
                    {
                        // Check to despawn mobs from other areas
                        for (int i = 0; i < ar.Count; ++i)
                        {
                            Mob mob = (Mob)ar[i];
                            if (mob != null &&
                                mob.mStartingArea != area)
                            {
                                Console.WriteLine("despawning " + mob.mName + " from other area");
                                mob.destroy();
                                --i;
                            }// if
                        }// for
                    }// foreach (ArrayList ar in targetList)

                    // Revert events
                    for (int i = 0; i < area.mRevertList.Count; ++i)
                    {
                        EventData ed = (EventData)area.mRevertList[i];
                        mWorld.mEventHandler.enQueueEvent(ed);
                    }// for

                    // Revert Doorway's initial state
                    foreach (Room room in area.mRoomList)
                        room.respawnDoorways();
                }// if (area.mCurrentRespawnTimer -= TICKTIME <= 0)

                // Reset Area respawn timer
                if (area.mCurrentRespawnTimer <= 0)
                    area.mCurrentRespawnTimer = area.mStartingRespawnTimer;

            }// foreach (Area area in mAreaList)

        }// processAreas

        private void processNpcs()
        {
            foreach (Area area in mAreaList)
            {
                ArrayList npcList = area.getRes(ResType.NPC);

                for (int i = 0; i < npcList.Count; ++i)
                {
                    CombatMob npc = (CombatMob)npcList[i];

                    if ((npc.mCurrentActionCounter -= TICKTIME) <= 0)
                    {
                        npc.mCurrentActionCounter = npc.mStartingActionCounter;
                        npc.randomAction();
                    }// if
                }// for
            }// foreach
        }// processNpcs

    }// Class AreaHandler

}// Namespace _8th_Circle_Server
