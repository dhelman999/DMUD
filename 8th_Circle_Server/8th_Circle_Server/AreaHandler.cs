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

        internal const int TICKTIME = 5;

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
            while (true)
            {
                try
                {
                    Thread.Sleep(TICKTIME*1000);
                }// try
                catch
                {
                    areaHandler.processAreas();
                }// catch

                areaHandler.processAreas();

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
                // Check to respawn
                foreach (Mob mob in area.mFullMobList)
                {
                   bool found = false;
                   if ((mob.mCurrentRespawnTime-= TICKTIME) <= 0)
                   {
                      for(int i = 0; i < area.mObjectList.Count; ++i)
                      {
                         Mob tmp = (Mob)area.mObjectList[i];
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
                for (int i = 0; i < area.mObjectList.Count; ++i)
                {
                    if (area.mCurrentRespawnTimer <= 0)
                    {
                        Mob mob = null;
                        if (area.mObjectList[i] != null)
                            mob = (Mob)area.mObjectList[i];

                        if (mob != null &&
                            mob.mStartingRoom != mob.mCurrentRoom)
                            mob.destroy();
                    }// if

                    for (int j = 0; j < area.mObjectList.Count; ++j)
                    {
                        if (j == i)
                            continue;

                        if (((Mob)area.mObjectList[i]).mMobId == ((Mob)area.mObjectList[j]).mMobId &&
                            ((Mob)area.mObjectList[i]).mInstanceId == ((Mob)area.mObjectList[j]).mInstanceId)
                        {
                            ((Mob)area.mObjectList[j]).destroy();
                        }// if
                    }// for
                }// for

                if(area.mCurrentRespawnTimer <= 0)
                    area.mCurrentRespawnTimer = area.mStartingRespawnTimer;

            }// foreach
        }// processArea

    }// Class AreaHandler

}// Namespace _8th_Circle_Server
