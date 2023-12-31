﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace _8th_Circle_Server
{
    // Class for handling events, Events aren't really a seperate class but they should be.  Like all handlers this spawns another thread
    // to wake up and handle events whenever a new event is queued up, meaning something triggered the event so we will need to process it.
    // The real design for this should have the events derive from a base class similar to the way CommandClasses are handled and the events
    // simply do some pre and post checking then call event.execute or something like that, instead the implementation of the events are
    // hardcoded in the handler which isn't really where they should be.
    public class EventHandler
    {
        private World mWorld;

        // Shared queue of events to process
        private Queue<EventData> mEventQueue;
        
        // Main worker thread to process events
        private Thread mSpinWorkThread;

        // Primitive lock to protect the event queue
        private object mQueueLock;

        public EventHandler(World world)
        {
            mEventQueue = new Queue<EventData>();
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
            while (true)
            {
                EventData eventData;

                // Protect the queue
                lock (mQueueLock)
                {
                    if (mEventQueue.Count > 0)
                        eventData = mEventQueue.Dequeue();
                    else
                        break;
                }
                    
                Area area;
                CombatMob player;

                // Events are hardcoded, process them here
                switch (eventData.GetEvent())
                {
                    // Send a message to a player
                    case EventID.EVENT_TELL_PLAYER:
                        player = (CombatMob)eventData.GetTrigger();
                        String message = (String)eventData.GetData();
                        player.safeWrite(message);
                        break;
                        
                    // Teleport a player to a room
                    case EventID.EVENT_TELEPORT:
                        player = (CombatMob)eventData.GetTrigger();
                        RoomID roomID = (RoomID)eventData.GetData();
                        Area targetArea = mWorld.GetAreas()[2];
                        Room targetRoom = targetArea[roomID];

                        targetRoom.addMobResource(player);
                        player.safeWrite("You feel a " + "mystical energy whisk you away, only to find yourself...");
                        player.safeWrite(targetRoom.exitString());
                        break;

                    // Remove connections of the area in the GPG
                    case EventID.EVENT_GPG_WALL_REMOVE:
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
                    
                    // Add connections to rooms in the GPG
                    case EventID.EVENT_GPG_WALL_ADD:
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
            }// while (true)
        }// processEvent

    }// Class EventHandler

}// Namespace _8th_Circle_Server
