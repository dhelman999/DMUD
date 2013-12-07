using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;

namespace _8th_Circle_Server
{
    enum EventFlag
    {
        EVENT_TELL_PLAYER=0,
    };// EventFlag

    struct EventData
    {
        public EventFlag eventFlag;
        public Mob trigger;
        public Mob eventObject;
        public Room eventRoom;
        public validityType validity;

        public EventData(EventFlag eventFlag, 
                         Mob trigger, 
                         Mob eventObject, 
                         Room eventRoom,
                         validityType validity)
        {
            this.eventFlag = eventFlag;
            this.trigger = trigger;
            this.eventObject = eventObject;
            this.eventRoom = eventRoom;
            this.validity = validity;
        }// Constructor
    }// EventData

    class EventHandler
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public ArrayList mEventCache;
        public Queue mEventQueue;

        private object mQueueLock;
        private Thread mSpinWorkThread; 

        public EventHandler()
        {
            mEventCache = new ArrayList();
            mEventQueue = new Queue();
            mQueueLock = new object();
        }// Constructor

        public void start()
        {
            mSpinWorkThread = new Thread(() => spinWork(this));
            mSpinWorkThread.Start();
        }// start

        public static void spinWork(EventHandler eventHandler)
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }// try
                catch
                {
                    eventHandler.processEvent();
                }// catch
            }// while
        }// spinWork

        public void enQueueEvent(Object eventData)
        {
            lock (mQueueLock)
            {
                mEventQueue.Enqueue(eventData);
            }// lock
            mSpinWorkThread.Interrupt();
        }// enQueueEvent

        private void processEvent()
        {
            while (mEventQueue.Count > 0)
            {
                EventData eventData = (EventData)mEventQueue.Dequeue();

                switch (eventData.eventFlag)
                {
                    case EventFlag.EVENT_TELL_PLAYER:
                        if (eventData.trigger is Player)
                        {
                            ((Player)eventData.trigger).mClientHandler.safeWrite((string)mEventQueue.Dequeue());
                        }// if
                        break;

                    default:
                        Console.WriteLine("something went wrong...");
                        break;
                }// switch
            }// while
        }// processEvent

    }// Class EventHandler

}// Namespace _8th_Circle_Server
