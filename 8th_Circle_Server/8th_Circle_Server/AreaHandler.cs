using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;

namespace _8th_Circle_Server
{
    class AreaHandler
    {
        // Debug
        internal const bool DEBUG = false;

        internal const int TICKTIME = 1;

        // Member Variables
        public ArrayList mAreaList;

        private object mQueueLock;
        private Thread mSpinWorkThread;

        public AreaHandler()
        {
            mAreaList = new ArrayList();
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
            Console.WriteLine("area handler tick!");

            foreach (Area area in mAreaList)
            {
                area.mCurrentRespawnTimer -= TICKTIME;
                
                // Update action timers
                foreach (Player pl in area.getRes(ResType.PLAYER))
                {
                    if (pl.mActionTimer > 0)
                        --(pl.mActionTimer);
                }

                // Check to respawn
                foreach (Mob mob in area.mFullMobList)
                {
                   bool found = false;
                   if ((mob.mCurrentRespawnTime-= TICKTIME) <= 0)
                   {
                       for (int i = 0; i < area.getRes(ResType.OBJECT).Count; ++i)
                      {
                          Mob tmp = (Mob)area.getRes(ResType.OBJECT)[i];
                         if (mob.mMobId == tmp.mMobId &&
                             mob.mInstanceId == tmp.mInstanceId)
                         {
                            tmp.destroy();
                            mob.mCurrentRespawnTime = mob.mStartingRespawnTime;
                            mob.respawn();
                            found = true;
                            break;
                         }// if
                      }// for

                      if (!found)
                      {
                         mob.mCurrentRespawnTime = mob.mStartingRespawnTime;
                         mob.respawn();
                      }// if
                   }// if
                }// foreach

                // TODO
                // Isn't there a better way to do this crap?
                // Check to despawn
                for (int i = 0; i < area.getRes(ResType.OBJECT).Count; ++i)
                {
                    if (area.mCurrentRespawnTimer <= 0)
                    {
                        Mob mob = null;
                        if (area.getRes(ResType.OBJECT)[i] != null)
                            mob = (Mob)area.getRes(ResType.OBJECT)[i];

                        if (mob != null &&
                            mob.mStartingRoom != mob.mCurrentRoom)
                            mob.destroy();
                    }// if

                    for (int j = 0; j < area.getRes(ResType.OBJECT).Count; ++j)
                    {
                        if (j == i)
                            continue;

                        if (((Mob)area.getRes(ResType.OBJECT)[i]).mMobId == ((Mob)area.getRes(ResType.OBJECT)[j]).mMobId &&
                            ((Mob)area.getRes(ResType.OBJECT)[i]).mInstanceId == ((Mob)area.getRes(ResType.OBJECT)[j]).mInstanceId)
                        {
                            ((Mob)area.getRes(ResType.OBJECT)[j]).destroy();
                        }// if
                    }// for
                }// for

                if(area.mCurrentRespawnTimer <= 0)
                    area.mCurrentRespawnTimer = area.mStartingRespawnTimer;

            }// foreach
        }// processArea

        // TODO
        // Add respawns to Npcs
        private void processNpcs()
        {
            foreach (Area area in mAreaList)
            {
                foreach (Npc npc in area.getRes(ResType.NPC))
                {
                    if ((npc.mCurrentActionCounter -= TICKTIME) <= 0)
                    {
                        npc.mCurrentActionCounter = npc.mStartingActionCounter;
                        npc.randomAction();
                    }// if
                }// foreach
            }// foreach
        }// processNpcs

        // TODO
        // Find a generic way to also reset event flags, object flags, and doorways,
        // such as hidden doorways.
    }// Class AreaHandler

}// Namespace _8th_Circle_Server
