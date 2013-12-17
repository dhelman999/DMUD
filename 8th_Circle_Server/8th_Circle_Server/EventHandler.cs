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
        EVENT_TELL_PLAYER,
        EVENT_TELEPORT
    };// EventFlag

    struct EventData
    {
        public EventFlag eventFlag;
        public Mob trigger;
        public Mob eventObject;
        public Room eventRoom;
        public validityType validity;
        public commandName commandName;
        public PrepositionType prepType;
        public Object data;

        public EventData(EventFlag eventFlag, 
                         Mob trigger, 
                         Mob eventObject, 
                         Room eventRoom,
                         validityType validity,
                         commandName commandName,
                         PrepositionType prepType,
                         Object data)
        {
            this.eventFlag = eventFlag;
            this.trigger = trigger;
            this.eventObject = eventObject;
            this.eventRoom = eventRoom;
            this.validity = validity;
            this.commandName = commandName;
            this.prepType = prepType;
            this.data = data;
        }// Constructor
    }// EventData

    class EventHandler
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public ArrayList mEventCache;
        public Queue mEventQueue;
        public World mWorld;

        private object mQueueLock;
        private Thread mSpinWorkThread; 

        public EventHandler(World world)
        {
            mEventCache = new ArrayList();
            mEventQueue = new Queue();
            mQueueLock = new object();
            mWorld = world;
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
                            ((Player)eventData.trigger).mClientHandler.safeWrite((string)eventData.data);
                        }// if
                        break;
                        
                    case EventFlag.EVENT_TELEPORT:
                        if (eventData.trigger is Player)
                        {
                            RoomID rid = (RoomID)eventData.data;
                            ((Area)mWorld.mAreaList[2]).getRoom(rid).addPlayer(eventData.trigger);
                            ((Player)eventData.trigger).mClientHandler.safeWrite("You feel a " +
                               "mystical energy whisk you away, only to find yourself...");
                            ((Player)eventData.trigger).mClientHandler.safeWrite(
                                eventData.trigger.mCurrentRoom.mDescription);
                            ((Player)eventData.trigger).mClientHandler.safeWrite(
                                eventData.trigger.mCurrentRoom.exitString());
                        }
                        break;

                    default:
                        Console.WriteLine("something went wrong...");
                        break;
                }// switch
            }// while
        }// processEvent

    }// Class EventHandler

}// Namespace _8th_Circle_Server
