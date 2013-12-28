using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum RoomID
    {
        ROOMID_START,

        // Geraldine Manor
        GERALD_1ST_ENT = ROOMID_START,
        GERALD_1ST_HALLWAY,
        GERALD_1ST_KITCHEN,
        GERALD_1ST_BATHROOM,
        GERALD_1ST_DININGROOM,
        GERALD_1ST_LIVINGROOM,
        GERALD_2ND_HALLWAY,
        GERALD_2ND_BATHROOM,
        GERALD_2ND_KITTYROOM,
        GERALD_2ND_KITTYCLOSET,
        GERALD_2ND_BLUEROOM,
        GERALD_2ND_BEDROOM,
        GERALD_BASE_PART1,
        GERALD_BASE_PART2,
        GERALD_BASE_PART3,
        GERALD_BASE_PART4,
        GERALD_BASE_PART5,
        GERALD_BASE_LAUNDRYROOM,
        GERALD_BASE_CLOSET,
        GERALD_BASE_SUMPROOM,
        GERALD_BASE_BATHROOM,

        // Goblin Prooving Grounds
        GPG_PLAYER_START,
        GPG_ROOM_1,
        GPG_ROOM_2,
        GPG_ROOM_3,
        GPG_ROOM_4,
        GPG_ROOM_5,
        GPG_ROOM_6,
        GPG_ROOM_7,
        GPG_ROOM_8,
        GPG_ROOM_9,
        GPG_ROOM_10,
        GPG_ROOM_11,
        GPG_ROOM_12,
        GPG_ROOM_13,
        GPG_ROOM_14,
        GPG_ROOM_15,
        GPG_ROOM_16,
        GPG_ROOM_17,
        GPG_ROOM_18,
        GPG_ROOM_19,
        GPG_ROOM_21,
        GPG_ROOM_22,
        GPG_ROOM_23,
        GPG_ROOM_24,
        GPG_ROOM_25,
        GPG_ROOM_26,
        GPG_ROOM_27,
        GPG_ROOM_28,
        GPG_ROOM_29,
        GPG_ROOM_30,
        GPG_ROOM_31,
        GPG_ROOM_32,
        GPG_ROOM_33,
        GPG_ROOM_34,
        GPG_ROOM_35,
        GPG_ROOM_36,
        GPG_ROOM_37,
        GPG_ROOM_38,
        GPG_ROOM_39,
        GPG_ROOM_40,
        GPG_ROOM_41,
        GPG_ROOM_42,
        GPG_ROOM_43,
        GPG_ROOM_44,
        GPG_ROOM_45,
        GPG_ROOM_46,
        GPG_ROOM_47,
        GPG_ROOM_48,
        GPG_ROOM_49,
        GPG_ROOM_50,
        GPG_ROOM_51,
        GPG_ROOM_52,
        GPG_ROOM_53,
        GPG_ROOM_54,
        GPG_ROOM_55,
        GPG_ROOM_56,
        GPG_ROOM_57,
        GPG_ROOM_58,
        GPG_ROOM_59,
        GPG_ROOM_60,
        GPG_ROOM_61,
        GPG_ROOM_62,
        GPG_ROOM_63,
        GPG_ROOM_64,
        GPG_ROOM_65,
        GPG_ROOM_66,
        GPG_ROOM_67,
        GPG_ROOM_68,
        GPG_ROOM_69,
        GPG_ROOM_70,
        GPG_ROOM_71,
        GPG_ROOM_72,
        GPG_ROOM_73,
        GPG_ROOM_74,
        GPG_ROOM_75,
        GPG_ROOM_76,
        GPG_ROOM_77,

        // Proto Area
        PROTO_1,
        PROTO_2,
        PROTO_3,
        PROTO_4,
        PROTO_5,
        PROTO_6,
        PROTO_7,
        PROTO_8,
        PROTO_9,
        PROTO_10,
        PROTO_11,
        PROTO_12,
        PROTO_13,
        PROTO_14,
        PROTO_15,
        PROTO_16,
        PROTO_17,
        PROTO_18,
        PROTO_19,
        PROTO_20,
        PROTO_21,
        PROTO_22,
        PROTO_23,
        PROTO_24,
        PROTO_25,
        PROTO_26,
        PROTO_27,

        // Last Room
        ROOMID_END
    }// RoomID

    class Room : ResourceHandler
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public RoomID mRoomID;
        public int []mAreaLoc;
        public Area mCurrentArea;
        public ArrayList mDoorwayList;
        public ArrayList mRoomLinks;

        public Room() : base()
        {
            mAreaLoc = new int[3];
            mDoorwayList = new ArrayList();
            mRoomLinks = new ArrayList();
            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                mDoorwayList.Add(null);
                mRoomLinks.Add(null);
            }               
        }// Constructor

        public Room(string desc) : base()
        {
            mDescription = desc;
            mAreaLoc = new int[3];
            mDoorwayList = new ArrayList();
            mRoomLinks = new ArrayList();
            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                mDoorwayList.Add(null);
                mRoomLinks.Add(null);
            }      
            mAreaLoc[0] = mAreaLoc[1] = mAreaLoc[2] = -1;
        }// Constructor

        public Room(string desc, int xCoord, int yCoord, int zCoord, RoomID roomID) : base()
        {
            mDescription = desc;
            mAreaLoc = new int[3];
            mDoorwayList = new ArrayList();
            mRoomLinks = new ArrayList();
            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                mDoorwayList.Add(null);
                mRoomLinks.Add(null);
            }      
            mAreaLoc[0] = xCoord;
            mAreaLoc[1] = yCoord;
            mAreaLoc[2] = zCoord;
            mRoomID = roomID;
        }// Constructor

        public Room(string desc, int xCoord, int yCoord, int zCoord, RoomID roomID, Area area) : base()
        {
            mDescription = desc;
            mAreaLoc = new int[3];
            mDoorwayList = new ArrayList();
            mRoomLinks = new ArrayList();
            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                mDoorwayList.Add(null);
                mRoomLinks.Add(null);
            }      
            mAreaLoc[0] = xCoord;
            mAreaLoc[1] = yCoord;
            mAreaLoc[2] = zCoord;
            mRoomID = roomID;
            mCurrentArea = area;
            area.mRoomList.Add(this);
        }// Constructor

        public string viewed()
        {
            return exitString();
        }// viewed

        public string viewed(string direction)
        {
            Room remoteRoom = null;
            bool validRoom = false;
            Direction dir = Mob.DirStrToEnum(direction);

            if (mRoomLinks[(int)dir] != null)
            {
                remoteRoom = (Room)mRoomLinks[(int)dir];
                validRoom = true;
            }

            if(validRoom)
                return remoteRoom.exitString();

            return "you can't look that way";
        }// viewed

        public string exitString()
        {
            string exitStr = mDescription + "\n" + "Exits: ";
            string tmp = string.Empty;
            int visibleObjects = 0;

            if (mRoomLinks[(int)Direction.NORTH] != null &&
                (mDoorwayList[(int)Direction.NORTH] == null ||
                ((Doorway)mDoorwayList[(int)Direction.NORTH]).mIsOpen))
                exitStr += "North ";
            if (mRoomLinks[(int)Direction.SOUTH] != null &&
                (mDoorwayList[(int)Direction.SOUTH] == null ||
                ((Doorway)mDoorwayList[(int)Direction.SOUTH]).mIsOpen))
                exitStr += "South ";
            if (mRoomLinks[(int)Direction.EAST] != null &&
                (mDoorwayList[(int)Direction.EAST] == null ||
                ((Doorway)mDoorwayList[(int)Direction.EAST]).mIsOpen))
                exitStr += "East ";
            if (mRoomLinks[(int)Direction.WEST] != null &&
                (mDoorwayList[(int)Direction.WEST] == null ||
                ((Doorway)mDoorwayList[(int)Direction.WEST]).mIsOpen))
                exitStr += "West ";
            if (mRoomLinks[(int)Direction.UP] != null &&
                (mDoorwayList[(int)Direction.UP] == null ||
                ((Doorway)mDoorwayList[(int)Direction.UP]).mIsOpen))
                exitStr += "Up ";
            if (mRoomLinks[(int)Direction.DOWN] != null &&
                (mDoorwayList[(int)Direction.DOWN] == null ||
                ((Doorway)mDoorwayList[(int)Direction.DOWN]).mIsOpen))
                exitStr += "Down ";
            if (mRoomLinks[(int)Direction.NORTHWEST] != null &&
                (mDoorwayList[(int)Direction.NORTHWEST] == null ||
                ((Doorway)mDoorwayList[(int)Direction.NORTHWEST]).mIsOpen))
                exitStr += "Northwest ";
            if (mRoomLinks[(int)Direction.NORTHEAST] != null &&
                (mDoorwayList[(int)Direction.NORTHEAST] == null ||
                ((Doorway)mDoorwayList[(int)Direction.NORTHEAST]).mIsOpen))
                exitStr += "Northeast ";
            if (mRoomLinks[(int)Direction.SOUTHWEST] != null &&
                (mDoorwayList[(int)Direction.SOUTHWEST] == null ||
                ((Doorway)mDoorwayList[(int)Direction.SOUTHWEST]).mIsOpen))
                exitStr += "Southwest ";
            if (mRoomLinks[(int)Direction.SOUTHEAST] != null &&
                (mDoorwayList[(int)Direction.SOUTHEAST] == null ||
                ((Doorway)mDoorwayList[(int)Direction.SOUTHEAST]).mIsOpen))
                exitStr += "Southeast ";

            exitStr += "\n";

            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                if (mDoorwayList[(int)dir] != null)
                {
                    if (!((Doorway)mDoorwayList[(int)dir]).mFlagList.Contains(objectFlags.FLAG_HIDDEN))
                    {
                        ++visibleObjects;
                        tmp += dir.ToString().ToLower() + " " + ((Doorway)mDoorwayList[(int)dir]).mName + "\n";        
                    }// if
                }// if   
            }// for

            if (visibleObjects != 0)
                exitStr += tmp;

            visibleObjects = 0;
            tmp = string.Empty;

            exitStr += "Objects: ";
            
            // TODO
            // Probably add something like a targetList instead of repeating this
            // multiple times
            for (int i = 0; i < getRes(ResType.OBJECT).Count; ++i)
            {
                if (!((Mob)getRes(ResType.OBJECT)[i]).mFlagList.Contains(objectFlags.FLAG_HIDDEN))
                {
                    ++visibleObjects;
                    tmp += ((Mob)getRes(ResType.OBJECT)[i]).exitString(this) + "\n";
                }// if
            }// for

            if (visibleObjects == 0)
                exitStr += "\n";
            else
                exitStr += tmp;

            tmp = string.Empty;

            exitStr += "Npcs: ";
            if (getRes(ResType.NPC).Count == 0)
                exitStr += "\n";

            for (int i = 0; i < getRes(ResType.NPC).Count; ++i)
            {
                if (!((Mob)getRes(ResType.NPC)[i]).mFlagList.Contains(objectFlags.FLAG_HIDDEN))
                    exitStr += ((Mob)getRes(ResType.NPC)[i]).mName + "\n";
            }// if

            exitStr += "Players: ";
            for (int i = 0; i < getRes(ResType.PLAYER).Count; ++i)
            {
                if (!((Mob)getRes(ResType.PLAYER)[i]).mFlagList.Contains(objectFlags.FLAG_HIDDEN))
                    exitStr += ((Player)getRes(ResType.PLAYER)[i]).mName + "\n";
            }// if

            exitStr += "\n";

            return exitStr;
        }// exitString

        // TODO
        // Do we need to have both a addobject and addnpc
        public void addObject(Mob mob)
        {
            // Remove old references
            if (mob.mCurrentRoom != null && mob.mCurrentArea != null)
            {
                mob.mCurrentArea.getRes(ResType.OBJECT).Remove(mob);
                mob.mCurrentRoom.getRes(ResType.OBJECT).Remove(mob);
            }// if
            
            // Add new references
            mob.mCurrentArea = mCurrentArea;
            mob.mCurrentRoom = this;
            mob.mCurrentArea.getRes(ResType.OBJECT).Add(mob);
            getRes(ResType.OBJECT).Add(mob);
        }// addObject

        public void addNpc(Mob mob)
        {
            // Remove old references
            if (mob.mCurrentRoom != null && mob.mCurrentArea != null)
            {
                mob.mCurrentArea.getRes(ResType.NPC).Remove(mob);
                mob.mCurrentRoom.getRes(ResType.NPC).Remove(mob);
            }// if

            // Add new references
            mob.mCurrentArea = mCurrentArea;
            mob.mCurrentRoom = this;
            mob.mCurrentArea.getRes(ResType.NPC).Add(mob);
            getRes(ResType.NPC).Add(mob);
        }// addObject

        // TODO
        // Combine all these functions into 1
        public void addPlayer(Mob mob)
        {
            // Remove old references
            if (mob.mCurrentRoom != null && mob.mCurrentArea != null)
            {
                mob.mCurrentArea.getRes(ResType.PLAYER).Remove(mob);
                mob.mCurrentRoom.getRes(ResType.PLAYER).Remove(mob);
            }// if

            // Add new references
            mob.mCurrentArea = mCurrentArea;
            mob.mCurrentRoom = this;
            mob.mCurrentArea.getRes(ResType.PLAYER).Add(mob);
            getRes(ResType.PLAYER).Add(mob);
        }// addObject

        public void addDoor(Doorway door, Direction dir)
        {
            mDoorwayList[(int)dir] = door;
            door.mRoomList[(int)dir] = this;
        }// addDorr

        public string getDoorString(Doorway door)
        {
            Direction dir;

            for (dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                if(mDoorwayList[(int)dir] != null &&
                   mDoorwayList[(int)dir].Equals(door))
                   break;
            }// for

            return dir.ToString().ToLower() + " " + door.mName;
        }// getDoorString

        public void removeDualLinks(Direction dir)
        {
            if(mRoomLinks[(int)dir] != null)
            {
                Direction oppositeDir = (Direction)(((int)dir + 5) % 10);
                Room linkedRoom = (Room)mRoomLinks[(int)dir];
                if (linkedRoom != null && 
                    linkedRoom.mRoomLinks[(int)oppositeDir] != null)
                    linkedRoom.mRoomLinks[(int)oppositeDir] = null;
                mRoomLinks[(int)dir] = null;
            }// if
        }// removeDualLinks

        public void removeTripleLinks(Direction dir)
        {
            removeDualLinks((Direction)(((int)dir + 9) % 10));
            removeDualLinks(dir);
            removeDualLinks((Direction)(((int)dir + 1) % 10));
        }// removeTripleLinks

        public void addDualLinks(Room targetRoom, Direction dir)
        {
            if (targetRoom != null)
            {
                Direction oppositeDir = (Direction)(((int)dir + 5) % 10);
                mRoomLinks[(int)dir] = targetRoom;
                targetRoom.mRoomLinks[(int)oppositeDir] = this;
            }// if
        }// addDualLinks

    }// Class Room

}// Namespace _8th_Circle_Server
