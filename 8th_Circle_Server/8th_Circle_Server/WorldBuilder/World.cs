using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public partial class World : ResourceHandler
    {
        // Debug
        internal const bool DEBUG = false;

        internal const int HOUSE_OFFSET = 10000;
        internal const int BAO = 1000;
        internal const int PROTO_OFFSET = 100;

        // Constants
        const int MAXXSIZE = 3;
        const int MAXYSIZE = 3;
        const int MAXZSIZE = 3;
        
        // Member Variables
        public CommandHandler mCommandHandler;
        public EventHandler mEventHandler;
        public AreaHandler mAreaHandler;
        public CombatHandler mCombatHandler;
        public ArrayList mAreaList;
        public ArrayList mRoomList; 
        public ArrayList mFullMobList;
        private Room[, ,] mWorldGrid;

        public World() : base()
        {
            mCommandHandler = new CommandHandler(this);
            mEventHandler = new EventHandler(this);
            mAreaHandler = new AreaHandler(this);
            mCombatHandler = new CombatHandler(this);
            mAreaList = new ArrayList();
            mRoomList = new ArrayList();
            mFullMobList = new ArrayList();
            mCommandHandler.start();
            mEventHandler.start();
            mAreaHandler.start();
            mCombatHandler.start();

            Area protoArea = new Area(this);
            protoArea.mName = "Proto Area";
            protoArea.mDescription = "The testing area for the 8th Circle";
            protoArea.mAreaID = AreaID.AID_PROTOAREA;
            protoArea.mAreaOffset = PROTO_OFFSET;

            mWorldGrid = new Room[MAXXSIZE, MAXYSIZE, MAXZSIZE];
            Room currentRoom = new Room();

            Room pa1 = new Room("Room 0,0,0", PROTO_OFFSET, PROTO_OFFSET, PROTO_OFFSET, RoomID.PROTO_1, protoArea);
            Room pa2 = new Room("Room 1,0,0", PROTO_OFFSET + 1, PROTO_OFFSET, PROTO_OFFSET, RoomID.PROTO_2, protoArea);
            Room pa3 = new Room("Room 2,0,0", PROTO_OFFSET + 2, PROTO_OFFSET, PROTO_OFFSET, RoomID.PROTO_3, protoArea);
            Room pa4 = new Room("Room 0,1,0", PROTO_OFFSET, PROTO_OFFSET + 1, PROTO_OFFSET, RoomID.PROTO_4, protoArea);
            Room pa5 = new Room("Room 1,1,0", PROTO_OFFSET + 1, PROTO_OFFSET + 1, PROTO_OFFSET, RoomID.PROTO_5, protoArea);
            Room pa6 = new Room("Room 2,1,0", PROTO_OFFSET + 2, PROTO_OFFSET + 1, PROTO_OFFSET, RoomID.PROTO_6, protoArea);
            Room pa7 = new Room("Room 0,2,0", PROTO_OFFSET, PROTO_OFFSET + 2, PROTO_OFFSET, RoomID.PROTO_7, protoArea);
            Room pa8 = new Room("Room 1,2,0", PROTO_OFFSET + 1, PROTO_OFFSET + 2, PROTO_OFFSET, RoomID.PROTO_8, protoArea);
            Room pa9 = new Room("Room 2,2,0", PROTO_OFFSET + 2, PROTO_OFFSET + 2, PROTO_OFFSET, RoomID.PROTO_9, protoArea);
            Room pa10 = new Room("Room 0,0,1", PROTO_OFFSET, PROTO_OFFSET, PROTO_OFFSET + 1, RoomID.PROTO_10, protoArea);
            Room pa11 = new Room("Room 1,0,1", PROTO_OFFSET + 1, PROTO_OFFSET, PROTO_OFFSET + 1, RoomID.PROTO_11, protoArea);
            Room pa12 = new Room("Room 2,0,1", PROTO_OFFSET + 2, PROTO_OFFSET, PROTO_OFFSET + 1, RoomID.PROTO_12, protoArea);
            Room pa13 = new Room("Room 0,1,1", PROTO_OFFSET, PROTO_OFFSET + 1, PROTO_OFFSET + 1, RoomID.PROTO_13, protoArea);
            Room pa14 = new Room("Room 1,1,1", PROTO_OFFSET + 1, PROTO_OFFSET + 1, PROTO_OFFSET + 1, RoomID.PROTO_14, protoArea);
            Room pa15 = new Room("Room 2,1,1", PROTO_OFFSET + 2, PROTO_OFFSET + 1, PROTO_OFFSET + 1, RoomID.PROTO_15, protoArea);
            Room pa16 = new Room("Room 0,2,1", PROTO_OFFSET, PROTO_OFFSET + 2, PROTO_OFFSET + 1, RoomID.PROTO_16, protoArea);
            Room pa17 = new Room("Room 1,2,1", PROTO_OFFSET + 1, PROTO_OFFSET + 2, PROTO_OFFSET + 1, RoomID.PROTO_17, protoArea);
            Room pa18 = new Room("Room 2,2,1", PROTO_OFFSET + 2, PROTO_OFFSET + 2, PROTO_OFFSET + 1, RoomID.PROTO_18, protoArea);
            Room pa19 = new Room("Room 0,0,2", PROTO_OFFSET, PROTO_OFFSET, PROTO_OFFSET + 2, RoomID.PROTO_19, protoArea);
            Room pa20 = new Room("Room 1,0,2", PROTO_OFFSET + 1, PROTO_OFFSET, PROTO_OFFSET + 2, RoomID.PROTO_20, protoArea);
            Room pa21 = new Room("Room 2,0,2", PROTO_OFFSET + 2, PROTO_OFFSET, PROTO_OFFSET + 2, RoomID.PROTO_21, protoArea);
            Room pa22 = new Room("Room 0,1,2", PROTO_OFFSET, PROTO_OFFSET + 1, PROTO_OFFSET + 2, RoomID.PROTO_22, protoArea);
            Room pa23 = new Room("Room 1,1,2", PROTO_OFFSET + 1, PROTO_OFFSET + 1, PROTO_OFFSET + 2, RoomID.PROTO_23, protoArea);
            Room pa24 = new Room("Room 2,1,2", PROTO_OFFSET + 2, PROTO_OFFSET + 1, PROTO_OFFSET + 2, RoomID.PROTO_24, protoArea);
            Room pa25 = new Room("Room 0,2,2", PROTO_OFFSET, PROTO_OFFSET + 2, PROTO_OFFSET + 2, RoomID.PROTO_25, protoArea);
            Room pa26 = new Room("Room 1,2,2", PROTO_OFFSET + 1, PROTO_OFFSET + 2, PROTO_OFFSET + 2, RoomID.PROTO_26, protoArea);
            Room pa27 = new Room("Room 2,2,2", PROTO_OFFSET + 2, PROTO_OFFSET + 2, PROTO_OFFSET + 2, RoomID.PROTO_27, protoArea);

            // Add Proto testing area
            mAreaList.Add(protoArea);
            createLinks();

            // Add all global mobs in the game
            addMobs();

            // Add specific mobs
            Container event_chest1 = new Container((Container)mFullMobList[(int)MOBLIST.EVENT_CHEST1]);
            event_chest1.mKeyId = 3;
            event_chest1.mStartingRoom = pa14;
            event_chest1.mStartingArea = protoArea;
            event_chest1.mName = "proto chest";
            addMob(event_chest1, pa14, protoArea);

            Mob basic_key = new Mob((Mob)mFullMobList[(int)MOBLIST.BASIC_KEY]);
            basic_key.mKeyId = 3;
            basic_key.mStartingRoom = pa27;
            basic_key.mStartingArea = protoArea;
            basic_key.mName = "proto key";
            addMob(basic_key, pa27, protoArea);

            // Add all global areas
            addAreas();

            // Register areas in the area handler
            mAreaHandler.registerArea(protoArea);     
        }// Constructor

        public Area getArea(AreaID areaID)
        {
            foreach (Area area in mAreaList)
            {
                if (area.mAreaID == areaID)
                    return area;
            }

            return null;
        }// getArea

        public Room getRoom(RoomID roomID, AreaID areaID)
        {
            Area area = getArea(areaID);

            foreach (Room room in area.mRoomList)
            {
                if (room.mRoomID == roomID)
                    return room;
            }

            return null;

        }// getRoom

        public Room getRoom(int x, int y, int z, AreaID areaID)
        {
            Area area = getArea(areaID);

            foreach (Room room in area.mRoomList)
            {
                if (room.mAreaLoc[0] == x &&
                    room.mAreaLoc[1] == y &&
                    room.mAreaLoc[2] == z)
                    return room;
            }

            return null;
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

        private void createLinks()
        {
            foreach (Room room in getArea(AreaID.AID_PROTOAREA).mRoomList)
            {
                Room nwRoom = getRoom(room.mAreaLoc[0] - 1, room.mAreaLoc[1] + 1,
                    room.mAreaLoc[2], AreaID.AID_PROTOAREA);
                Room nRoom = getRoom(room.mAreaLoc[0], room.mAreaLoc[1] + 1,
                    room.mAreaLoc[2], AreaID.AID_PROTOAREA);
                Room neRoom = getRoom(room.mAreaLoc[0] + 1, room.mAreaLoc[1] + 1,
                    room.mAreaLoc[2], AreaID.AID_PROTOAREA);
                Room wRoom = getRoom(room.mAreaLoc[0] - 1, room.mAreaLoc[1],
                    room.mAreaLoc[2], AreaID.AID_PROTOAREA);
                Room eRoom = getRoom(room.mAreaLoc[0] + 1, room.mAreaLoc[1],
                    room.mAreaLoc[2], AreaID.AID_PROTOAREA);
                Room swRoom = getRoom(room.mAreaLoc[0] - 1, room.mAreaLoc[1] - 1,
                    room.mAreaLoc[2], AreaID.AID_PROTOAREA);
                Room sRoom = getRoom(room.mAreaLoc[0], room.mAreaLoc[1] - 1,
                    room.mAreaLoc[2], AreaID.AID_PROTOAREA);
                Room seRoom = getRoom(room.mAreaLoc[0] + 1, room.mAreaLoc[1] - 1,
                    room.mAreaLoc[2], AreaID.AID_PROTOAREA);
                Room uRoom = getRoom(room.mAreaLoc[0], room.mAreaLoc[1],
                    room.mAreaLoc[2] + 1, AreaID.AID_PROTOAREA);
                Room dRoom = getRoom(room.mAreaLoc[0], room.mAreaLoc[1],
                    room.mAreaLoc[2] - 1, AreaID.AID_PROTOAREA);

                if (nwRoom != null)
                {
                    room.mRoomLinks[(int)Direction.NORTHWEST] = nwRoom;
                    nwRoom.mRoomLinks[(int)Direction.SOUTHEAST] = room;
                }
                if (nRoom != null)
                {
                    room.mRoomLinks[(int)Direction.NORTH] = nRoom;
                    nRoom.mRoomLinks[(int)Direction.SOUTH] = room;
                }
                if (neRoom != null)
                {
                    room.mRoomLinks[(int)Direction.NORTHEAST] = neRoom;
                    neRoom.mRoomLinks[(int)Direction.SOUTHWEST] = room;
                }
                if (wRoom != null)
                {
                    room.mRoomLinks[(int)Direction.WEST] = wRoom;
                    wRoom.mRoomLinks[(int)Direction.EAST] = room;
                }
                if (eRoom != null)
                {
                    room.mRoomLinks[(int)Direction.EAST] = eRoom;
                    eRoom.mRoomLinks[(int)Direction.WEST] = room;
                }
                if (swRoom != null)
                {
                    room.mRoomLinks[(int)Direction.SOUTHWEST] = swRoom;
                    swRoom.mRoomLinks[(int)Direction.NORTHEAST] = room;
                }
                if (sRoom != null)
                {
                    room.mRoomLinks[(int)Direction.SOUTH] = sRoom;
                    sRoom.mRoomLinks[(int)Direction.NORTH] = room;
                }
                if (seRoom != null)
                {
                    room.mRoomLinks[(int)Direction.SOUTHEAST] = seRoom;
                    seRoom.mRoomLinks[(int)Direction.NORTHWEST] = room;
                }
                if (uRoom != null)
                {
                    room.mRoomLinks[(int)Direction.UP] = uRoom;
                    uRoom.mRoomLinks[(int)Direction.DOWN] = room;
                }
                if (dRoom != null)
                {
                    room.mRoomLinks[(int)Direction.DOWN] = dRoom;
                    dRoom.mRoomLinks[(int)Direction.UP] = room;
                }
            }// foreach
        }// createLinks

        
    }// Class World

}// Namespace _8th_Circle_Server
