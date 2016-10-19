using System;
using System.Collections;
using System.Threading;

namespace _8th_Circle_Server
{
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
                        String message = (String)eventData.GetData();
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
                        Utils.Broadcast(area, null, "The area shakes and rumbles");

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
                        Utils.Broadcast(area, null, "The area shakes and rumbles");

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
            }// while (mEventQueue.Count > 0)
        }// processEvent

    }// Class EventHandler

}// Namespace _8th_Circle_Server
