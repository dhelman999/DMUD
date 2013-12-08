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
            mSpinWorkThread.Interrupt();
        }// enQueueArea

        private void processAreas()
        {
            foreach (Area area in mAreaList)
            {
                Console.WriteLine("area handler tick!");
                foreach (Container mob in area.mFullMobList)
                {
                    if (area.mObjectList.Contains(mob))
                    {
                        Container mob2 = (Container)area.mObjectList[area.mObjectList.IndexOf(mob)];
                        if ((mob2.mRespawnTime-= TICKTIME) < 0)
                        {
                            Console.WriteLine("respawning " + mob.mName);
                            mob2.respawn();
                            mob2.mRespawnTime = mob.mRespawnTime;
                        }
                    }
                    else
                    {
                        //Console.WriteLine("2nd respawning " + mob.mName);
                        //mob.respawn();
                    }
                }
            }// foreach
        }// processArea

    }// Class AreaHandler

}// Namespace _8th_Circle_Server
