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
        EVENT_START,
        EVENT_TELL_PLAYER = EVENT_START,
        EVENT_TELEPORT,
        EVENT_GPG_WALL_REMOVE,
        EVENT_END
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
                                eventData.trigger.mCurrentRoom.exitString());
                        }
                        break;

                    case EventFlag.EVENT_GPG_WALL_REMOVE:
                        ((Player)eventData.trigger).mClientHandler.safeWrite("You hear a massive " +
                            "rumbling far to the west");
                        Area area = mWorld.getArea((AreaID)eventData.data);
                        area.getRoom(RoomID.GPG_ROOM_41).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_40), Direction.WEST);
                        area.getRoom(RoomID.GPG_ROOM_41).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_47), Direction.SOUTHWEST);
                        area.getRoom(RoomID.GPG_ROOM_48).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_40), Direction.NORTHWEST);
                        area.getRoom(RoomID.GPG_ROOM_48).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_47), Direction.WEST);
                        area.getRoom(RoomID.GPG_ROOM_48).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_54), Direction.SOUTHWEST);
                        area.getRoom(RoomID.GPG_ROOM_55).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_47), Direction.NORTHWEST);
                        area.getRoom(RoomID.GPG_ROOM_55).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_54), Direction.WEST);
                        area.getRoom(RoomID.GPG_ROOM_55).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_61), Direction.SOUTHWEST);
                        area.getRoom(RoomID.GPG_ROOM_62).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_54), Direction.NORTHWEST);
                        area.getRoom(RoomID.GPG_ROOM_62).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_61), Direction.WEST);
                        area.getRoom(RoomID.GPG_ROOM_62).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_68), Direction.SOUTHWEST);
                        area.getRoom(RoomID.GPG_ROOM_69).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_61), Direction.NORTHWEST);
                        area.getRoom(RoomID.GPG_ROOM_69).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_68), Direction.WEST);
                        area.getRoom(RoomID.GPG_ROOM_69).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_74), Direction.SOUTHWEST);
                        area.getRoom(RoomID.GPG_ROOM_76).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_68), Direction.NORTHWEST);
                        area.getRoom(RoomID.GPG_ROOM_76).addDualLinks(
                            area.getRoom(RoomID.GPG_ROOM_75), Direction.WEST);
                        break;
                        
                    default:
                        Console.WriteLine("something went wrong...");
                        break;
                }// switch
            }// while
        }// processEvent

    }// Class EventHandler

}// Namespace _8th_Circle_Server
