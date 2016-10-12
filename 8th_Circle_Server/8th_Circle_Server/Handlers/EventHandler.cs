using System;
using System.Collections;
using System.Threading;

namespace _8th_Circle_Server
{
    // TODO
    // Make this a class
    public struct EventData
    {
        public ValidityType validity;

        private EventFlag eventFlag;
        private Mob trigger;
        private Mob eventObject;
        private Room eventRoom;
        private CommandName CommandName;
        private PrepositionType prepType;
        private Object data;

        public EventData(EventFlag eventFlag, 
                         Mob trigger, 
                         Mob eventObject, 
                         Room eventRoom,
                         ValidityType validity,
                         CommandName CommandName,
                         PrepositionType prepType,
                         Object data)
        {
            this.eventFlag = eventFlag;
            this.trigger = trigger;
            this.eventObject = eventObject;
            this.eventRoom = eventRoom;
            this.validity = validity;
            this.CommandName = CommandName;
            this.prepType = prepType;
            this.data = data;
        }// Constructor

        // Accessors
        public EventFlag GetEvent() { return eventFlag; }
        public void SetEvent(EventFlag flag) { eventFlag = flag; }
        public CommandName GetCommand() { return CommandName; }
        public void SetCommand(CommandName command) { CommandName = command; }
        public void SetPrep(PrepositionType prep) { prepType = prep; }
        public Object GetData() { return data; }
        public void SetData(Object dat) { data = dat; }
        public Mob GetTrigger() { return trigger; }
        public void SetTrigger(Mob trig) { trigger = trig; }
        public Mob GetEventObject() { return eventObject; }
        public void SetEventObject(Mob obj) { eventObject = obj; }
        public void SetRoom(Room room) { eventRoom = room; }
        public PrepositionType GetPrepType() { return prepType; }

    }// EventData

    public class EventHandler
    {
        private ArrayList mEventCache;
        private Queue mEventQueue;
        private World mWorld;
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

        public void enQueueEvent(EventData eventData)
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
                Area area;
                CombatMob player;

                switch (eventData.GetEvent())
                {
                    case EventFlag.EVENT_TELL_PLAYER:
                        player = (CombatMob)eventData.GetTrigger();
                        string message = (string)eventData.GetData();
                        player.safeWrite(message);
                        break;
                        
                    case EventFlag.EVENT_TELEPORT:
                        player = (CombatMob)eventData.GetTrigger();
                        RoomID roomID = (RoomID)eventData.GetData();
                        Area targetArea = mWorld.GetAreas()[2];
                        Room targetRoom = targetArea[roomID];

                        targetRoom.addMobResource(player);
                        player.safeWrite("You feel a " + "mystical energy whisk you away, only to find yourself...");
                        player.safeWrite(targetRoom.exitString());
                        break;

                    case EventFlag.EVENT_GPG_WALL_REMOVE:
                        area = mWorld.getArea((AreaID)eventData.GetData());
                        area.broadcast("The area shakes and rumbles");

                        area[RoomID.GPG_ROOM_41].addDualLinks(area[RoomID.GPG_ROOM_40], Direction.WEST);
                        area[RoomID.GPG_ROOM_41].addDualLinks(area[RoomID.GPG_ROOM_47], Direction.SOUTHWEST);
                        area[RoomID.GPG_ROOM_48].addDualLinks(area[RoomID.GPG_ROOM_40], Direction.NORTHWEST);
                        area[RoomID.GPG_ROOM_48].addDualLinks(area[RoomID.GPG_ROOM_47], Direction.WEST);
                        area[RoomID.GPG_ROOM_48].addDualLinks(area[RoomID.GPG_ROOM_54], Direction.SOUTHWEST);
                        area[RoomID.GPG_ROOM_55].addDualLinks(area[RoomID.GPG_ROOM_47], Direction.NORTHWEST);
                        area[RoomID.GPG_ROOM_55].addDualLinks(area[RoomID.GPG_ROOM_54], Direction.WEST);
                        area[RoomID.GPG_ROOM_55].addDualLinks(area[RoomID.GPG_ROOM_61], Direction.SOUTHWEST);
                        area[RoomID.GPG_ROOM_62].addDualLinks(area[RoomID.GPG_ROOM_54], Direction.NORTHWEST);
                        area[RoomID.GPG_ROOM_62].addDualLinks(area[RoomID.GPG_ROOM_61], Direction.WEST);
                        area[RoomID.GPG_ROOM_62].addDualLinks(area[RoomID.GPG_ROOM_68], Direction.SOUTHWEST);
                        area[RoomID.GPG_ROOM_69].addDualLinks(area[RoomID.GPG_ROOM_61], Direction.NORTHWEST);
                        area[RoomID.GPG_ROOM_69].addDualLinks(area[RoomID.GPG_ROOM_68], Direction.WEST);
                        area[RoomID.GPG_ROOM_69].addDualLinks(area[RoomID.GPG_ROOM_74], Direction.SOUTHWEST);
                        area[RoomID.GPG_ROOM_76].addDualLinks(area[RoomID.GPG_ROOM_68], Direction.NORTHWEST);
                        area[RoomID.GPG_ROOM_76].addDualLinks(area[RoomID.GPG_ROOM_75], Direction.WEST);
                        break;

                    case EventFlag.EVENT_GPG_WALL_ADD:
                        area = mWorld.getArea((AreaID)eventData.GetData());
                        area.broadcast("The area shakes and rumbles");

                        area[RoomID.GPG_ROOM_41].removeDualLinks(Direction.WEST);
                        area[RoomID.GPG_ROOM_41].removeDualLinks(Direction.SOUTHWEST);
                        area[RoomID.GPG_ROOM_48].removeDualLinks(Direction.NORTHWEST);
                        area[RoomID.GPG_ROOM_48].removeDualLinks(Direction.WEST);
                        area[RoomID.GPG_ROOM_48].removeDualLinks(Direction.SOUTHWEST);
                        area[RoomID.GPG_ROOM_55].removeDualLinks(Direction.NORTHWEST);
                        area[RoomID.GPG_ROOM_55].removeDualLinks(Direction.WEST);
                        area[RoomID.GPG_ROOM_55].removeDualLinks(Direction.SOUTHWEST);
                        area[RoomID.GPG_ROOM_62].removeDualLinks(Direction.NORTHWEST);
                        area[RoomID.GPG_ROOM_62].removeDualLinks(Direction.WEST);
                        area[RoomID.GPG_ROOM_62].removeDualLinks(Direction.SOUTHWEST);
                        area[RoomID.GPG_ROOM_69].removeDualLinks(Direction.NORTHWEST);
                        area[RoomID.GPG_ROOM_69].removeDualLinks(Direction.WEST);
                        area[RoomID.GPG_ROOM_69].removeDualLinks(Direction.SOUTHWEST);
                        area[RoomID.GPG_ROOM_76].removeDualLinks(Direction.NORTHWEST);
                        area[RoomID.GPG_ROOM_76].removeDualLinks(Direction.WEST);
                        break;

                    default:
                        Console.WriteLine("something went wrong...");
                        break;
                }// switch

            }// while

        }// processEvent

    }// Class EventHandler

}// Namespace _8th_Circle_Server
