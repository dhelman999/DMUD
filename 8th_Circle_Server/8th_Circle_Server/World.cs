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
            chest.mFlagList.Add(objectFlags.FLAG_LOCKABLE);
            chest.mFlagList.Add(objectFlags.FLAG_UNLOCKABLE);
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
            chest.mKeyId = 3;
            chest.mStartingRespawnTime = 60;
            chest.mCurrentRespawnTime = 60;
            mFullMobList.Add(chest);

            Container chest2 = new Container();
            chest2.mDescription = "A sturdy, wooden chest.  It makes you wonder what is inside...";
            chest2.mFlagList.Add(objectFlags.FLAG_OPENABLE);
            chest2.mFlagList.Add(objectFlags.FLAG_CLOSEABLE);
            chest2.mFlagList.Add(objectFlags.FLAG_LOCKED);
            chest2.mFlagList.Add(objectFlags.FLAG_LOCKABLE);
            chest2.mFlagList.Add(objectFlags.FLAG_UNLOCKABLE);
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
            chest2.mKeyId = 3;
            chest2.mStartingRespawnTime = 60;
            chest2.mCurrentRespawnTime = 60;
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

            Mob key = new Mob();
            key.mDescription = "An old brass key, what does it unlock?";
            key.mFlagList.Add(objectFlags.FLAG_GETTABLE);
            key.mFlagList.Add(objectFlags.FLAG_INSPECTABLE);
            key.mFlagList.Add(objectFlags.FLAG_STORABLE);
            key.mName = "brass key";
            key.mInventory.Capacity = 0;
            key.mWorld = this;
            key.mMobId = 3;
            key.mIsActive = false;
            mFullMobList.Add(key);

            Mob key1a = new Mob((Mob)mFullMobList[2]);
            key1a.mInstanceId = 1;
            key1a.mStartingRoom = key1a.mCurrentRoom = getRoom(2, 2, 2);
            key1a.mStartingArea = key1a.mCurrentArea = key1a.mStartingRoom.mCurrentArea;
            protoArea.mFullMobList.Add(key1a);

            Mob key2a = new Mob((Mob)protoArea.mFullMobList[2]);
            getRoom(2, 2, 2).addObject(key2a);

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

            Room house1stentranceway = new Room("The entrance to the Geraldine Manor, there are stairs " +
                "leading up.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET);
            house1stentranceway.mCurrentArea = geraldineArea;
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

            Room house2ndHallway = new Room("The hallway to the 2nd floor.  This is a long corridor\n with " +
                "many rooms attached to it with stairs leading down at the base.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET + 1);
            house2ndHallway.mCurrentArea = geraldineArea;
            Room house2ndKittyroom = new Room("The kittyroom, there is not much here besides some litterboxes.",
                HOUSE_OFFSET-1, HOUSE_OFFSET, HOUSE_OFFSET + 1);
            house2ndKittyroom.mCurrentArea = geraldineArea;
            Room house2ndKittyCloset = new Room("The closet of the kittyroom holds various appliances such as " +
                "vaccuums and other cleaning supplies",
                HOUSE_OFFSET-1, HOUSE_OFFSET-1, HOUSE_OFFSET + 1);
            house2ndKittyCloset.mCurrentArea = geraldineArea;
            Room house2ndBathroom = new Room("A small master bathroom has a sink, shower and toilet",
                HOUSE_OFFSET+1, HOUSE_OFFSET, HOUSE_OFFSET + 1);
            house2ndBathroom.mCurrentArea = geraldineArea;
            Room house2ndBedroom = new Room("The master bedroom is huge with two sliding door closets\n and " +
                "windows on the north and northwest sides",
                HOUSE_OFFSET+1, HOUSE_OFFSET +1, HOUSE_OFFSET + 1);
            house2ndBedroom.mCurrentArea = geraldineArea;
            Room house2ndBlueroom = new Room("The blueroom has a large bookshelf, a sliding door closet and " +
                "a loveseat",
                HOUSE_OFFSET, HOUSE_OFFSET+1, HOUSE_OFFSET + 1);
            house2ndBlueroom.mCurrentArea = geraldineArea;

            Room houseBaseentrance = new Room("The bottom of the stairs leads to the basement.\n This " +
                " is a large basement that spans to the south with \nrooms attached on both sides.",
                HOUSE_OFFSET, HOUSE_OFFSET, HOUSE_OFFSET-1);
            houseBaseentrance.mCurrentArea = geraldineArea;
            Room houseBasepart2 = new Room("There is a piano here along the wall with light grey\n " +
                "carpet with the walls being a darker grey",
                HOUSE_OFFSET, HOUSE_OFFSET -1, HOUSE_OFFSET -1);
            houseBasepart2.mCurrentArea = geraldineArea;
            Room houseBasepart3 = new Room("There isn't much to this piece of the basement besides\n " +
                "some pictures on both the west and east walls.",
                HOUSE_OFFSET, HOUSE_OFFSET -2, HOUSE_OFFSET -1);
            houseBasepart3.mCurrentArea = geraldineArea;
            Room houseBasepart4 = new Room("You have reached the southern corner of the basement.\n " +
                "There is a computer desk here with a glowing PC and monitor.  There are all\n " +
                "sorts of figurines of wonderous power sitting on the desk along with pictures\n " + 
                "depicting awesome scenes of wonder and adventure.  Something about this room\n " +
                "seems filled with some sort of power.",
                HOUSE_OFFSET, HOUSE_OFFSET -3, HOUSE_OFFSET - 1);
            houseBasepart4.mCurrentArea = geraldineArea;
            Room houseBasepart5 = new Room("The southwest most edge of the basement, there is a\n " +
                "couch on the south end of the wall facing a TV in the corner beside a doorway",
                HOUSE_OFFSET -1, HOUSE_OFFSET -3, HOUSE_OFFSET - 1);
            houseBasepart5.mCurrentArea = geraldineArea;
            Room houseBaseBathroom = new Room("The bathroom has a standing shower as well as a long \n " +
                "vanity with an accompanying toilet",
                HOUSE_OFFSET-1, HOUSE_OFFSET -2, HOUSE_OFFSET -1);
            houseBaseBathroom.mCurrentArea = geraldineArea;
            Room houseBaseCloset = new Room("This is a closet that has large holding shelves with\n " +
                "board games from top to bottom.  This is an impressive collection indeed!",
                HOUSE_OFFSET-1, HOUSE_OFFSET -1, HOUSE_OFFSET - 1);
            houseBaseCloset.mCurrentArea = geraldineArea;
            Room houseBaseLaundryRoom = new Room("The laundry room has no carpet and has many shelves\n "  +
                "with various pieces of hardware and tools",
                HOUSE_OFFSET-1, HOUSE_OFFSET, HOUSE_OFFSET - 1);
            houseBaseLaundryRoom.mCurrentArea = geraldineArea;
            Room houseBaseSumpRoom = new Room("The sump pump room is bare concrete with a few shelves\n " +
                "for storage",
                HOUSE_OFFSET, HOUSE_OFFSET + 1, HOUSE_OFFSET + 1);
            houseBaseSumpRoom.mCurrentArea = geraldineArea;

            house2ndHallway.mCurrentArea = geraldineArea;
            house1stentranceway.mWestLink = house1stHallway;
            house1stentranceway.mSouthLink = house1stLivingRoom;
            house1stHallway.mEastLink = house1stentranceway;
            house1stHallway.mSouthLink = house1stBathroom;
            house1stHallway.mWestLink = house1stKitchen;
            house1stKitchen.mEastLink = house1stHallway;
            house1stKitchen.mSouthLink = house1stDiningRoom;
            house1stDiningRoom.mNorthLink = house1stKitchen;
            house1stDiningRoom.mEastLink = house1stLivingRoom;
            house1stLivingRoom.mWestLink = house1stDiningRoom;
            house1stLivingRoom.mNorthLink = house1stentranceway;
            house1stBathroom.mNorthLink = house1stHallway;
            
            Doorway house1stBathroomdwy = new Doorway("door", Direction.NORTH,
                house1stBathroom);
            Doorway house1stHalldwy = new Doorway("door", Direction.SOUTH,
                house1stHallway);
            house1stBathroomdwy.mCompanion = house1stHalldwy;
            house1stHalldwy.mCompanion = house1stBathroomdwy;
            
            house1stentranceway.mUpLink = house2ndHallway;
            house2ndHallway.mDownLink = house1stentranceway;
            house2ndHallway.mEastLink = house2ndKittyroom;
            house2ndHallway.mWestLink = house2ndBathroom;
            house2ndHallway.mSouthLink = house2ndBlueroom;
            house2ndHallway.mSouthwestLink = house2ndBedroom;
            house2ndKittyroom.mNorthLink = house2ndKittyCloset;
            house2ndKittyroom.mWestLink = house2ndHallway;
            house2ndKittyCloset.mSouthLink = house2ndKittyroom;
            house2ndBathroom.mEastLink = house2ndHallway;
            house2ndBathroom.mSouthLink = house2ndBedroom;
            house2ndBedroom.mNortheastLink = house2ndHallway;
            house2ndBedroom.mNorthLink = house2ndBathroom;
            house2ndBlueroom.mNorthLink = house2ndHallway;

            // TODO
            // This needs to be simplified, this is ridiculous
            Doorway house2ndHalldwy1 = new Doorway("door", Direction.WEST,
                house2ndHallway);
            Doorway house2ndHalldwy2 = new Doorway("door", Direction.EAST,
                house2ndHallway);
            Doorway house2ndHalldwy3 = new Doorway("door", Direction.SOUTH,
                house2ndHallway);
            Doorway house2ndHalldwy4 = new Doorway("door", Direction.SOUTHWEST,
                house2ndHallway);
            Doorway house2ndBathroomdwy1 = new Doorway("door", Direction.EAST,
                house2ndBathroom);
            Doorway house2ndBathroomdwy2 = new Doorway("door", Direction.SOUTH,
               house2ndBathroom);
            Doorway house2ndBlueroomdwy1 = new Doorway("door", Direction.NORTH,
                house2ndBlueroom);
            Doorway house2ndKittyroomdwy1 = new Doorway("door", Direction.WEST,
                house2ndKittyroom);
            Doorway house2ndBedroomdwy1 = new Doorway("door", Direction.NORTHEAST,
                house2ndBedroom);
            Doorway house2ndBedroomdwy2 = new Doorway("door", Direction.NORTH,
                house2ndBedroom);
            Doorway house2ndKittyroomdwy2 = new Doorway("door", Direction.NORTH,
                house2ndKittyroom);
            Doorway house2ndKittyClosetdwy = new Doorway("door", Direction.SOUTH,
                house2ndKittyCloset);
            house2ndHalldwy1.mCompanion = house2ndBathroomdwy1;
            house2ndHalldwy2.mCompanion = house2ndKittyroomdwy1;
            house2ndHalldwy3.mCompanion = house2ndBlueroomdwy1;
            house2ndHalldwy4.mCompanion = house2ndBedroomdwy1;
            house2ndBathroomdwy1.mCompanion = house2ndHalldwy1;
            house2ndBathroomdwy2.mCompanion = house2ndBedroomdwy2;
            house2ndBedroomdwy2.mCompanion = house2ndBathroomdwy2;
            house2ndKittyroomdwy1.mCompanion = house2ndHalldwy2;
            house2ndBlueroomdwy1.mCompanion = house2ndHalldwy3;
            house2ndBedroomdwy1.mCompanion = house2ndHalldwy4;
            house2ndKittyroomdwy2.mCompanion = house2ndKittyClosetdwy;
            house2ndKittyClosetdwy.mCompanion = house2ndKittyroomdwy2;

            houseBaseentrance.mUpLink = house1stKitchen;
            house1stKitchen.mDownLink = houseBaseentrance;
            houseBaseentrance.mSouthLink = houseBasepart2;
            houseBaseentrance.mWestLink = houseBaseLaundryRoom;
            houseBaseLaundryRoom.mEastLink = houseBaseentrance;
            houseBasepart2.mNorthLink = houseBaseentrance;
            houseBasepart2.mWestLink = houseBaseCloset;
            houseBasepart2.mEastLink = houseBaseSumpRoom;
            houseBasepart2.mSouthLink = houseBasepart3;
            houseBaseSumpRoom.mWestLink = houseBasepart2;
            houseBaseCloset.mEastLink = houseBasepart2;
            houseBasepart3.mNorthLink = houseBasepart2;
            houseBasepart3.mSouthLink = houseBasepart4;
            houseBasepart4.mNorthLink = houseBasepart3;
            houseBasepart4.mWestLink = houseBasepart5;
            houseBasepart5.mEastLink = houseBasepart4;
            houseBasepart5.mNorthLink = houseBaseBathroom;
            houseBaseBathroom.mSouthLink = houseBasepart5;

            Doorway testdoor = new Doorway("door", houseBasepart3);
            houseBasepart3.addDoor(testdoor, Direction.SOUTH);

            Doorway houseBaseBathroomdwy = new Doorway("door", Direction.SOUTH,
                houseBaseBathroom);
            Doorway houseBasementbdwy = new Doorway("door", Direction.NORTH,
                houseBasepart5);
            houseBaseBathroomdwy.mCompanion = houseBasementbdwy;
            houseBasementbdwy.mCompanion = houseBaseBathroomdwy;

            Doorway houseBaseLaundrydwy = new Doorway("door", Direction.EAST,
                houseBaseLaundryRoom);
            Doorway houseBasementbdwy2 = new Doorway("door", Direction.WEST,
                houseBaseentrance);
            houseBaseLaundrydwy.mCompanion = houseBasementbdwy2;
            houseBasementbdwy2.mCompanion = houseBaseLaundrydwy;

            Doorway houseBaseGameroomdwy = new Doorway("door", Direction.EAST,
                houseBaseCloset);
            Doorway houseBasementbdwy3 = new Doorway("door", Direction.WEST,
                houseBasepart2);
            houseBaseGameroomdwy.mCompanion = houseBasementbdwy3;
            houseBasementbdwy3.mCompanion = houseBaseGameroomdwy;

            Doorway houseBaseSumproomdwy = new Doorway("door", Direction.WEST,
                houseBaseSumpRoom);
            Doorway houseBasementbdwy4 = new Doorway("door", Direction.EAST,
                houseBasepart2);
            houseBaseSumproomdwy.mCompanion = houseBasementbdwy4;
            houseBasementbdwy4.mCompanion = houseBaseSumproomdwy;

            geraldineArea.mRoomList.Add(house1stentranceway);
            geraldineArea.mRoomList.Add(house1stHallway);
            geraldineArea.mRoomList.Add(house1stKitchen);
            geraldineArea.mRoomList.Add(house1stDiningRoom);
            geraldineArea.mRoomList.Add(house1stLivingRoom);
            geraldineArea.mRoomList.Add(house1stBathroom);
            geraldineArea.mRoomList.Add(house2ndHallway);
            geraldineArea.mRoomList.Add(house2ndBathroom);
            geraldineArea.mRoomList.Add(house2ndBedroom);
            geraldineArea.mRoomList.Add(house2ndBlueroom);
            geraldineArea.mRoomList.Add(house2ndKittyCloset);
            geraldineArea.mRoomList.Add(house2ndKittyroom);
            geraldineArea.mRoomList.Add(houseBaseBathroom);
            geraldineArea.mRoomList.Add(houseBaseCloset);
            geraldineArea.mRoomList.Add(houseBaseentrance);
            geraldineArea.mRoomList.Add(houseBaseLaundryRoom);
            geraldineArea.mRoomList.Add(houseBasepart2);
            geraldineArea.mRoomList.Add(houseBasepart3);
            geraldineArea.mRoomList.Add(houseBasepart4);
            geraldineArea.mRoomList.Add(houseBasepart5);
            geraldineArea.mRoomList.Add(houseBaseSumpRoom);

            getRoom(0, 0, 0).mWestLink = house1stentranceway;
            mAreaList.Add(geraldineArea);
        }// createHouse

    }// Class World

}// Namespace _8th_Circle_Server
