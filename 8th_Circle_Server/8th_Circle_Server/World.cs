using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class World
    {
        // Debug
        internal const bool DEBUG = false;

        internal const int HOUSE_OFFSET = 10000;

        // Constants
        const int MAXXSIZE = 3;
        const int MAXYSIZE = 3;
        const int MAXZSIZE = 3;
        
        // Member Variables
        public CommandHandler mCommandHandler;
        public EventHandler mEventHandler;
        public AreaHandler mAreaHandler;
        public ArrayList mPlayerList;
        public ArrayList mNpcList;
        public ArrayList mAreaList;
        public ArrayList mRoomList;
        public ArrayList mObjectList;
        public ArrayList mFullMobList;
        private Room[, ,] mWorldGrid;

        public World()
        {
            mCommandHandler = new CommandHandler(this);
            mEventHandler = new EventHandler();
            mAreaHandler = new AreaHandler();
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
            mAreaList = new ArrayList();
            mRoomList = new ArrayList();
            mObjectList = new ArrayList();
            mFullMobList = new ArrayList();

            mCommandHandler.start();
            mEventHandler.start();
            mAreaHandler.start();

            Area protoArea = new Area();
            protoArea.mName = "Proto Area";
            protoArea.mDescription = "The testing area for the 8th Circle";
            protoArea.mWorld = this;

            Room currentRoom = new Room();

            mWorldGrid = new Room[MAXZSIZE, MAXYSIZE, MAXXSIZE];
            mWorldGrid[0, 0, 0] = new Room("Room 0,0,0", 0, 0, 0);
            mWorldGrid[1, 0, 0] = new Room("Room 1,0,0", 1, 0, 0);
            mWorldGrid[2, 0, 0] = new Room("Room 2,0,0", 2, 0, 0);
            mWorldGrid[0, 1, 0] = new Room("Room 0,1,0", 0, 1, 0);
            mWorldGrid[1, 1, 0] = new Room("Room 1,1,0", 1, 1, 0);
            mWorldGrid[2, 1, 0] = new Room("Room 2,1,0", 2, 1, 0);
            mWorldGrid[0, 2, 0] = new Room("Room 0,2,0", 0, 2, 0);
            mWorldGrid[1, 2, 0] = new Room("Room 1,2,0", 1, 2, 0);
            mWorldGrid[2, 2, 0] = new Room("Room 2,2,0", 2, 2, 0);
            mWorldGrid[0, 0, 1] = new Room("Room 0,0,1", 0, 0, 1);
            mWorldGrid[1, 0, 1] = new Room("Room 1,0,1", 1, 0, 1);
            mWorldGrid[2, 0, 1] = new Room("Room 2,0,1", 2, 0, 1);
            mWorldGrid[0, 1, 1] = new Room("Room 0,1,1", 0, 1, 1);
            mWorldGrid[1, 1, 1] = new Room("Room 1,1,1", 1, 1, 1);
            mWorldGrid[2, 1, 1] = new Room("Room 2,1,1", 2, 1, 1);
            mWorldGrid[0, 2, 1] = new Room("Room 0,2,1", 0, 2, 1);
            mWorldGrid[1, 2, 1] = new Room("Room 1,2,1", 1, 2, 1);
            mWorldGrid[2, 2, 1] = new Room("Room 2,2,1", 2, 2, 1);
            mWorldGrid[0, 0, 2] = new Room("Room 0,0,2", 0, 0, 2);
            mWorldGrid[1, 0, 2] = new Room("Room 1,0,2", 1, 0, 2);
            mWorldGrid[2, 0, 2] = new Room("Room 2,0,2", 2, 0, 2);
            mWorldGrid[0, 1, 2] = new Room("Room 0,1,2", 0, 1, 2);
            mWorldGrid[1, 1, 2] = new Room("Room 1,1,2", 1, 1, 2);
            mWorldGrid[2, 1, 2] = new Room("Room 2,1,2", 2, 1, 2);
            mWorldGrid[0, 2, 2] = new Room("Room 0,2,2", 0, 2, 2);
            mWorldGrid[1, 2, 2] = new Room("Room 1,2,2", 1, 2, 2);
            mWorldGrid[2, 2, 2] = new Room("Room 2,2,2", 2, 2, 2);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        currentRoom = getRoom(i, j, k);
                        currentRoom.mCurrentArea = protoArea;
                        mRoomList.Add(currentRoom);
                        protoArea.mRoomList.Add(currentRoom);
                        createLinks(currentRoom);
                    }// for
                }// for
            }// for

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        createSideLinks(getRoom(i, j, k));
                    }// for
                }// for
            }// for

            // Add Areas
            mAreaList.Add(protoArea);

            // TODO
            // Seperate out these different rooms/items etc into a more structured class
            // Add Objects
            Container chest = new Container();  
            chest.mDescription = "A sturdy, wooden chest.  It makes you wonder what is inside...";
            chest.mFlagList.Add(objectFlags.FLAG_OPENABLE);
            chest.mFlagList.Add(objectFlags.FLAG_CLOSEABLE);
            chest.mFlagList.Add(objectFlags.FLAG_LOCKED);
            chest.mName = "chest1";
            chest.mInventory.Capacity = 20;
            chest.mWorld = this;
            EventData eventData = new EventData();
            eventData.eventFlag = EventFlag.EVENT_TELL_PLAYER;
            eventData.commandName = commandName.COMMAND_LOOK;
            eventData.prepType = PrepositionType.PREP_IN;
            eventData.data = "A voice speaks to you from within " + chest.mName;
            chest.mEventList.Add(eventData);
            chest.mMobId = 1;
            chest.mIsActive = false;
            mFullMobList.Add(chest);

            Container chest2 = new Container();
            chest2.mDescription = "A sturdy, wooden chest.  It makes you wonder what is inside...";
            chest2.mFlagList.Add(objectFlags.FLAG_OPENABLE);
            chest2.mFlagList.Add(objectFlags.FLAG_CLOSEABLE);
            chest2.mFlagList.Add(objectFlags.FLAG_LOCKED);
            chest2.mName = "large wooden chest2";
            chest2.mInventory.Capacity = 20;
            chest2.mWorld = this;
            eventData = new EventData();
            eventData.eventFlag = EventFlag.EVENT_TELL_PLAYER;
            eventData.commandName = commandName.COMMAND_LOOK;
            eventData.prepType = PrepositionType.PREP_AT;
            eventData.data = "The " + chest.mName + " says \"hello!\"";
            chest2.mEventList.Add(eventData);
            chest2.mMobId = 2;
            chest2.mIsActive = false;
            mFullMobList.Add(chest2);

            //Container chest1a = new Container((Container)mFullMobList[1]);
            //chest1a.mInstanceId = 1;
            //getRoom(0, 0, 2).addObject(chest1a);
            //chest1a.mCurrentArea.mObjectList.Add(chest1a);
            //protoArea.mFullMobList.Add(chest);
            //mObjectList.Add(chest1a);

            Container chest2a = new Container((Container)mFullMobList[1]);
            chest2a.mInstanceId = 1;
            chest2a.mStartingRoom = chest2a.mCurrentRoom = getRoom(1, 1, 1);
            chest2a.mStartingArea = chest2a.mCurrentArea = chest2a.mStartingRoom.mCurrentArea;
            protoArea.mFullMobList.Add(chest2a);

            Container chest2b = new Container((Container)mFullMobList[1]);
            chest2b.mInstanceId = 2;
            chest2b.mStartingRoom = chest2b.mCurrentRoom = getRoom(1, 1, 1);
            chest2b.mStartingArea = chest2b.mCurrentArea = chest2b.mStartingRoom.mCurrentArea;
            protoArea.mFullMobList.Add(chest2b);

            Container chest2c = new Container((Container)protoArea.mFullMobList[0]);
            Container chest2d = new Container((Container)protoArea.mFullMobList[1]);
            getRoom(1, 1, 1).addObject(chest2c);
            getRoom(1, 1, 1).addObject(chest2d);
            
            mAreaHandler.registerArea(protoArea);

            createHouse();
        }// Constructor


        public Room getRoom(int z, int y, int x)
        {
            return mWorldGrid[z, y, x];
        }// getRoom

        public Area getArea(string areaName)
        {
            foreach (Area area in mAreaList)
            {
                if(area.mName.Equals(areaName))
                    return area;
            }// foreach

            return null;
        }// getArea

        private void createLinks(Room currentRoom)
        {
            // North Links
            if (currentRoom.mWorldLoc[1] < 2)
                currentRoom.mNorthLink = mWorldGrid[currentRoom.mWorldLoc[0], currentRoom.mWorldLoc[1] + 1,
                    currentRoom.mWorldLoc[2]];
            // West Links
            if (currentRoom.mWorldLoc[0] > 0)
                currentRoom.mWestLink = mWorldGrid[currentRoom.mWorldLoc[0] - 1, currentRoom.mWorldLoc[1],
                    currentRoom.mWorldLoc[2]];
            // East Links
            if (currentRoom.mWorldLoc[0] < 2)
                currentRoom.mEastLink = mWorldGrid[currentRoom.mWorldLoc[0] + 1, currentRoom.mWorldLoc[1],
                    currentRoom.mWorldLoc[2]];
            // South Links
            if (currentRoom.mWorldLoc[1] > 0)
                currentRoom.mSouthLink = mWorldGrid[currentRoom.mWorldLoc[0], currentRoom.mWorldLoc[1] - 1,
                    currentRoom.mWorldLoc[2]];
            // Up/Down Links
            if (currentRoom.mWorldLoc[1] < 2)
            {
                if (currentRoom.mWorldLoc[0] == 1 &&
                    currentRoom.mWorldLoc[1] == 1 &&
                    currentRoom.mWorldLoc[2] == 1)
                {
                    currentRoom.mUpLink = mWorldGrid[currentRoom.mWorldLoc[0], currentRoom.mWorldLoc[1],
                        currentRoom.mWorldLoc[2] + 1];
                    currentRoom.mDownLink = mWorldGrid[currentRoom.mWorldLoc[0], currentRoom.mWorldLoc[1],
                        currentRoom.mWorldLoc[2] - 1];
                    currentRoom.mUpLink.mDownLink = currentRoom;
                    currentRoom.mDownLink.mUpLink = currentRoom;
                }// if
            }// if
        }// createLinks

        private void createSideLinks(Room currentRoom)
        {
            // Northwest/Southeast Links
            if (currentRoom.mNorthLink != null && currentRoom.mNorthLink.mWestLink != null)
            {
                currentRoom.mNorthwestLink = currentRoom.mNorthLink.mWestLink;
                currentRoom.mNorthwestLink.mSoutheastLink = currentRoom;
            }// if

            // Northeast/Southwest Links
            if (currentRoom.mNorthLink != null && currentRoom.mNorthLink.mEastLink != null)
            {
                currentRoom.mNortheastLink = currentRoom.mNorthLink.mEastLink;
                currentRoom.mNortheastLink.mSouthwestLink = currentRoom;
            }// if
        }// createSideLinks

        public void createHouse()
        {
            Area geraldineArea = new Area();
            geraldineArea.mName = "Geraldine Estate";
            geraldineArea.mDescription = "The residence of the esteemed Renee and David";
            geraldineArea.mWorld = this;

            Room house1stEnteranceway = new Room("The enterance to the Geraldine Manor",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET);
            house1stEnteranceway.mCurrentArea = geraldineArea;
            Room house1stHallway = new Room("The west hallway is empty besides a few pictures",
                HOUSE_OFFSET - 1, HOUSE_OFFSET, HOUSE_OFFSET);
            house1stHallway.mCurrentArea = geraldineArea;
            Room house1stKitchen = new Room("The kitchen has a nice view of the outside to the west; " +
                "there are also stairs leading down and a doorway to the north",
                HOUSE_OFFSET - 2, HOUSE_OFFSET, HOUSE_OFFSET);
            house1stKitchen.mCurrentArea = geraldineArea;
            Room house1stDiningRoom = new Room("The dining room is blue with various pictures; one is " +
                "particularly interesting, featuring a type of chicken",
                HOUSE_OFFSET - 2, HOUSE_OFFSET - 1, HOUSE_OFFSET);
            house1stDiningRoom.mCurrentArea = geraldineArea;
            Room house1stLivingRoom = new Room("The living room is grey with a nice flatscreen tv along " +
                "the north wall",
                HOUSE_OFFSET, HOUSE_OFFSET - 1, HOUSE_OFFSET);
            house1stLivingRoom.mCurrentArea = geraldineArea;
            Room house1stBathroom = new Room("The powder room is a nice small comfortable bathroom with " +
                "a sink and toilet",
                HOUSE_OFFSET - 1, HOUSE_OFFSET - 1, HOUSE_OFFSET);
            house1stBathroom.mCurrentArea = geraldineArea;

            house1stEnteranceway.mWestLink = house1stHallway;
            house1stEnteranceway.mSouthLink = house1stLivingRoom;
            house1stHallway.mEastLink = house1stEnteranceway;
            house1stHallway.mSouthLink = house1stBathroom;
            house1stHallway.mWestLink = house1stKitchen;
            house1stKitchen.mEastLink = house1stHallway;
            house1stKitchen.mSouthLink = house1stDiningRoom;
            house1stDiningRoom.mNorthLink = house1stKitchen;
            house1stDiningRoom.mEastLink = house1stLivingRoom;
            house1stLivingRoom.mWestLink = house1stDiningRoom;
            house1stLivingRoom.mNorthLink = house1stEnteranceway;
            house1stBathroom.mNorthLink = house1stHallway;

            geraldineArea.mRoomList.Add(house1stEnteranceway);
            geraldineArea.mRoomList.Add(house1stHallway);
            geraldineArea.mRoomList.Add(house1stKitchen);
            geraldineArea.mRoomList.Add(house1stDiningRoom);
            geraldineArea.mRoomList.Add(house1stLivingRoom);
            geraldineArea.mRoomList.Add(house1stBathroom);

            getRoom(0, 0, 0).mWestLink = house1stEnteranceway;
            mAreaList.Add(geraldineArea);
        }// createHouse
    }// Class World

}// Namespace _8th_Circle_Server
