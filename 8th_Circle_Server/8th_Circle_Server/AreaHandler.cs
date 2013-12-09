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
            foreach (Area area in mAreaList)
            {
                
                Console.WriteLine("area handler tick!");
                foreach (Mob mob in area.mFullMobList)
                {
                    if ((mob.mCurrentRespawnTime -= TICKTIME) <= 0)
                    {
                        bool found = false;

                        for(int i = 0; i < area.mObjectList.Count; ++i)
                        {
                            Mob tmp = (Mob)area.mObjectList[i];
                            if (mob.mMobId == tmp.mMobId &&
                                mob.mInstanceId == tmp.mInstanceId)
                            {
                                tmp.destory();
                                mob.mCurrentRespawnTime = mob.mStartingRespawnTime;
                                mob.respawn();
                                found = true;
                                break;
                            }// if
                        }// for

                        // TODO
                        // Make mobs from other areas despawn after the timer
                        // so other dropped items/mobs won't litter areas
                        // they don't belong in
                        if (!found)
                        {
                            mob.mCurrentRespawnTime = mob.mStartingRespawnTime;
                            mob.respawn();
                        }// if
                    }// if
                }// foreach
            }// foreach
        }// processArea

    }// Class AreaHandler

}// Namespace _8th_Circle_Server
