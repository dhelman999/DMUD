using System;
using System.Collections.Generic;
using System.Threading;

namespace _8th_Circle_Server
{
    public class AreaHandler
    {
        internal const int TICKTIME = 1;

        // Member Variables
        public List<Area> mAreaList;
        public World mWorld;

        private object mQueueLock;
        private Thread mSpinWorkThread;

        public AreaHandler(World world)
        {
            mAreaList = new List<Area>();
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
            bool processed = false;

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
            List<List<Mob>> targetList = new List<List<Mob>>();
            targetList.Add(mWorld.getRes(ResType.NPC));
            targetList.Add(mWorld.getRes(ResType.PLAYER));

            // Decrement the action timers for all mobs in the game
            foreach (List<Mob> ar in targetList)
            {
                foreach (Mob mob in ar)
                {
                    if (mob.mActionTimer > 0)
                        --(mob.mActionTimer);

                    if(mob is CombatMob && ((CombatMob)mob).mQueuedCommand != string.Empty)
                    {
                        commandData cd = new commandData(((CombatMob)mob).mQueuedCommand, (CombatMob)mob);
                        mWorld.mCommandHandler.enQueueCommand(cd);
                        ((CombatMob)mob).mQueuedCommand = string.Empty;
                    }
                }// foreach
            }// foreach

            // Process each area
            foreach (Area area in mAreaList)
            {
                foreach(Mob parent in area.GetPrototypeMobList())
                {
                    if (parent.mIsRespawning && (parent.mCurrentRespawnTime -= TICKTIME) <= 0)
                    {
                        if (parent.mFlagList.Contains(MobFlags.FLAG_INCOMBAT))
                        {
                            parent.mIsRespawning = false;
                            parent.mCurrentRespawnTime = parent.mStartingRespawnTime;
                        }
                        else
                        {
                            for (int i= 0; i< parent.mChildren.Count; ++i)
                                parent.mChildren[i--].destroy();

                            Console.WriteLine("respawning " + parent.mName);
                            parent.respawn();
                        }
                    }
                }// foreach

                if ((area.mCurrentRespawnTimer -= TICKTIME) <= 0)
                {
                    targetList.Clear();
                    targetList.Add(area.getRes(ResType.OBJECT));
                    targetList.Add(area.getRes(ResType.NPC));

                    // Loop through all Mob arrays
                    foreach (List<Mob> ar in targetList)
                    {
                        // Check to despawn mobs from other areas
                        for (int i = 0; i < ar.Count; ++i)
                        {
                            Mob mob = ar[i];

                            if (mob != null && mob.mStartingArea != area)
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
                        EventData ed = area.mRevertList[i];
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
                List<Mob> npcList = area.getRes(ResType.NPC);

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
