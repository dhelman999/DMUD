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
                    Thread.Sleep(10000);
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
            }// foreach
        }// processArea

    }// Class AreaHandler

}// Namespace _8th_Circle_Server
