using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum RoomID
    {
        INVALID,
        // Geraldine Manor
        GERALD_1ST_ENT,
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
        GPG_ROOM_77
    }// RoomID

    class Room
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public RoomID mRoomID;
        public string mDescription;
        public Room mNorthLink;
        public Room mSouthLink;
        public Room mEastLink;
        public Room mWestLink;
        public Room mUpLink;
        public Room mDownLink;
        public Room mNortheastLink;
        public Room mNorthwestLink;
        public Room mSouthwestLink;
        public Room mSoutheastLink;
        public int []mWorldLoc;
        public ArrayList mPlayerList;
        public ArrayList mNpcList;
        public ArrayList mObjectList;
        public Area mCurrentArea;
        public ArrayList mDoorwayList;

        public Room()
        {
            mDescription = string.Empty;
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
            mObjectList = new ArrayList();
            mWorldLoc = new int[3];
            mDoorwayList = new ArrayList();
            for (int i = 0; i < (int)Direction.INVALID; ++i)
                mDoorwayList.Add(null);
            mNorthLink = mSouthLink = mEastLink = mWestLink = mUpLink = mDownLink =
                mNortheastLink = mNorthwestLink = mSouthwestLink = mSoutheastLink = null;
        }// Constructor

        public Room(string desc)
        {
            mDescription = desc;
            mNorthLink = mSouthLink = mEastLink = mWestLink = mUpLink = mDownLink =
                mNortheastLink = mNorthwestLink = mSouthwestLink = mSoutheastLink = null;
            mWorldLoc = new int[3];
            mDoorwayList = new ArrayList();
            for (int i = 0; i < (int)Direction.INVALID; ++i)
                mDoorwayList.Add(null);
            mWorldLoc[0] = mWorldLoc[1] = mWorldLoc[2] = -1;
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
            mObjectList = new ArrayList();
        }// Constructor

        public Room(string desc, int xCoord, int yCoord, int zCoord, RoomID roomID)
        {
            mDescription = desc;
            mNorthLink = mSouthLink = mEastLink = mWestLink = mUpLink = mDownLink =
                mNortheastLink = mNorthwestLink = mSouthwestLink = mSoutheastLink = null;
            mWorldLoc = new int[3];
            mDoorwayList = new ArrayList();
            for (int i = 0; i < (int)Direction.INVALID; ++i)
                mDoorwayList.Add(null);

            mWorldLoc[0] = xCoord;
            mWorldLoc[1] = yCoord;
            mWorldLoc[2] = zCoord;
            this.mRoomID = roomID;
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
            mObjectList = new ArrayList();
        }// Constructor

        public Room(string desc, int xCoord, int yCoord, int zCoord, RoomID roomID, Area area)
        {
            mDescription = desc;
            mNorthLink = mSouthLink = mEastLink = mWestLink = mUpLink = mDownLink =
                mNortheastLink = mNorthwestLink = mSouthwestLink = mSoutheastLink = null;
            mWorldLoc = new int[3];
            mDoorwayList = new ArrayList();
            for (int i = 0; i < (int)Direction.INVALID; ++i)
                mDoorwayList.Add(null);

            mWorldLoc[0] = xCoord;
            mWorldLoc[1] = yCoord;
            mWorldLoc[2] = zCoord;
            this.mRoomID = roomID;
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
            mObjectList = new ArrayList();
            mCurrentArea = area;
            area.mRoomList.Add(this);
        }// Constructor

        public string exitString()
        {
            string exitStr = "Exits: ";
            if (mNorthLink != null && 
                (mDoorwayList[(int)Direction.NORTH] == null ||
                ((Doorway)mDoorwayList[(int)Direction.NORTH]).mIsOpen))
                exitStr += "North ";
            if (mSouthLink != null &&
                (mDoorwayList[(int)Direction.SOUTH] == null ||
                ((Doorway)mDoorwayList[(int)Direction.SOUTH]).mIsOpen))
                exitStr += "South ";
            if (mEastLink != null &&
                (mDoorwayList[(int)Direction.EAST] == null || 
                ((Doorway)mDoorwayList[(int)Direction.EAST]).mIsOpen))
                exitStr += "East ";
            if (mWestLink != null &&
                (mDoorwayList[(int)Direction.WEST] == null ||
                ((Doorway)mDoorwayList[(int)Direction.WEST]).mIsOpen))
                exitStr += "West ";
            if (mUpLink != null &&
                (mDoorwayList[(int)Direction.UP] == null ||
                ((Doorway)mDoorwayList[(int)Direction.UP]).mIsOpen))
                exitStr += "Up ";
            if (mDownLink != null &&
                (mDoorwayList[(int)Direction.DOWN] == null || 
                ((Doorway)mDoorwayList[(int)Direction.DOWN]).mIsOpen))
                exitStr += "Down ";
            if (mNorthwestLink != null &&
                (mDoorwayList[(int)Direction.NORTHWEST] == null || 
                ((Doorway)mDoorwayList[(int)Direction.NORTHWEST]).mIsOpen))
                exitStr += "Northwest ";
            if (mNortheastLink != null &&
                (mDoorwayList[(int)Direction.NORTHEAST] == null || 
                ((Doorway)mDoorwayList[(int)Direction.NORTHEAST]).mIsOpen))
                exitStr += "Northeast ";
            if (mSouthwestLink != null &&
                (mDoorwayList[(int)Direction.SOUTHWEST] == null ||
                ((Doorway)mDoorwayList[(int)Direction.SOUTHWEST]).mIsOpen))
                exitStr += "Southwest ";
            if (mSoutheastLink != null &&
                (mDoorwayList[(int)Direction.SOUTHEAST] == null ||
                ((Doorway)mDoorwayList[(int)Direction.SOUTHEAST]).mIsOpen))
                exitStr += "Southeast ";

            exitStr += "\n";
            for (Direction dir = Direction.NORTH; dir < Direction.INVALID; ++dir)
            {
                if (mDoorwayList[(int)dir] != null)
                {
                    exitStr += dir.ToString().ToLower() + " " + ((Doorway)mDoorwayList[(int)dir]).mName;
                    exitStr += "\n";
                }// if   
            }// for

            exitStr += "Objects: ";
            if (mObjectList.Count == 0)
                exitStr += "\n";

            for (int i = 0; i < mObjectList.Count; ++i)
                exitStr += ((Mob)mObjectList[i]).exitString(this) + "\n";          

            exitStr += "Npcs: ";
            if (mNpcList.Count == 0)
                exitStr += "\n";

            for (int i = 0; i < mNpcList.Count; ++i)
                exitStr += ((Mob)mNpcList[i]).mName + "\n";

            exitStr += "Players: ";
            for (int i = 0; i < mPlayerList.Count; ++i)
                exitStr += ((Player)mPlayerList[i]).mName + "\n";

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
                mob.mCurrentArea.mObjectList.Remove(mob);
                mob.mCurrentRoom.mObjectList.Remove(mob);
            }// if
            
            // Add new references
            mob.mCurrentArea = mCurrentArea;
            mob.mCurrentRoom = this;
            mob.mCurrentArea.mObjectList.Add(mob);
            mObjectList.Add(mob);
        }// addObject

        public void addNpc(Mob mob)
        {
            // Remove old references
            if (mob.mCurrentRoom != null && mob.mCurrentArea != null)
            {
                mob.mCurrentArea.mNpcList.Remove(mob);
                mob.mCurrentRoom.mNpcList.Remove(mob);
            }// if

            // Add new references
            mob.mCurrentArea = mCurrentArea;
            mob.mCurrentRoom = this;
            mob.mCurrentArea.mNpcList.Add(mob);
            mNpcList.Add(mob);
        }// addObject

        // TODO
        // Combine all these functions into 1
        public void addPlayer(Mob mob)
        {
            // Remove old references
            if (mob.mCurrentRoom != null && mob.mCurrentArea != null)
            {
                mob.mCurrentArea.mPlayerList.Remove(mob);
                mob.mCurrentRoom.mPlayerList.Remove(mob);
            }// if

            // Add new references
            mob.mCurrentArea = mCurrentArea;
            mob.mCurrentRoom = this;
            mob.mCurrentArea.mPlayerList.Add(mob);
            mPlayerList.Add(mob);
        }// addObject

        public void addDoor(Doorway door, Direction dir)
        {
            mDoorwayList[(int)dir] = door;
            door.mRoomList[(int)dir] = this;
        }// addDorr

        public string getDoorString(Doorway door)
        {
            int target = (int)Direction.INVALID;

            for (int i = 0; i < mDoorwayList.Count; ++i)
            {
                if (mDoorwayList[i] != null &&
                   mDoorwayList[i].Equals(door))
                    target = i;
            }// for

            return ((Direction)(target)).ToString().ToLower() + " " + door.mName;
        }// getDoorString

        public void generateSurroundingLinks(Room nwRoom, Room nRoom, Room neRoom,
            Room wRoom, Room eRoom, Room swRoom, Room sRoom, Room seRoom, Room upRoom, Room downRoom)
        {
            if (nwRoom != null)
            {
                this.mNorthwestLink = nwRoom;
                nwRoom.mSoutheastLink = this;
            }
            if (nRoom != null)
            {
                this.mNorthLink = nRoom;
                nRoom.mSouthLink = this;
            }
            if (neRoom != null)
            {
                this.mNortheastLink = neRoom;
                neRoom.mSouthwestLink = this;
            }
            if (wRoom != null)
            {
                this.mWestLink = wRoom;
                wRoom.mEastLink = this;
            }
            if (eRoom != null)
            {
                this.mEastLink = eRoom;
                eRoom.mWestLink = this;
            }
            if (swRoom != null)
            {
                this.mSouthwestLink = swRoom;
                swRoom.mNortheastLink = this;
            }
            if (sRoom != null)
            {
                this.mSouthLink = sRoom;
                sRoom.mNorthLink = this;
            }
            if (seRoom != null)
            {
                this.mSoutheastLink = seRoom;
                seRoom.mNorthwestLink = this;
            }
            if (upRoom != null)
            {
                this.mUpLink = upRoom;
                upRoom.mDownLink = this;
            }
            if (downRoom != null)
            {
                this.mDownLink = downRoom;
                downRoom.mUpLink = this;
            }
        }// generateSurroundingLinks

        public void removeDualLinks(Direction direction)
        {
            if (direction == Direction.NORTH)
            {
                this.mNorthLink.mSouthLink = null;
                this.mNorthLink = null;
            }
            else if (direction == Direction.SOUTH)
            {
                this.mSouthLink.mNorthLink = null;
                this.mSouthLink = null;
            }
            else if (direction == Direction.EAST)
            {
                this.mEastLink.mWestLink = null;
                this.mEastLink = null;
            }
            else if (direction == Direction.WEST)
            {
                this.mWestLink.mEastLink = null;
                this.mWestLink = null;
            }
            else if (direction == Direction.UP)
            {
                this.mUpLink.mDownLink = null;
                this.mUpLink = null;
            }
            else if (direction == Direction.DOWN)
            {
                this.mDownLink.mUpLink = null;
                this.mDownLink = null;
            }
            else if (direction == Direction.NORTHWEST)
            {
                this.mNorthwestLink.mSoutheastLink = null;
                this.mNorthwestLink = null;
            }
            else if (direction == Direction.NORTHEAST)
            {
                this.mNortheastLink.mSouthwestLink = null;
                this.mNortheastLink = null;
            }
            else if (direction == Direction.SOUTHWEST)
            {
                this.mSouthwestLink.mNortheastLink = null;
                this.mSouthwestLink = null;
            }
            else if (direction == Direction.SOUTHEAST)
            {
                this.mSoutheastLink.mNorthwestLink = null;
                this.mSoutheastLink = null;
            }
        }// removeDualLinks

        public void removeTripleLinks(Direction direction)
        {
            if (direction == Direction.NORTH)
            {
                if (mNorthLink != null)
                {
                    mNorthLink.mSouthLink = null;
                    mNorthLink = null;
                }
                if (mNorthwestLink != null)
                {
                    mNorthwestLink.mSoutheastLink = null;
                    mNorthwestLink = null;
                }
                if (mNortheastLink != null)
                {
                    mNortheastLink.mSouthwestLink = null;
                    mNortheastLink = null;
                }
            }
            else if (direction == Direction.SOUTH)
            {
                if (mSouthLink != null)
                {
                    mSouthLink.mNorthLink = null;
                    mSouthLink = null;
                }
                if (mSouthwestLink != null)
                {
                    mSouthwestLink.mNortheastLink = null;
                    mSouthwestLink = null;
                }
                if (mSoutheastLink != null)
                {
                    mSoutheastLink.mNorthwestLink = null;
                    mSoutheastLink = null;
                }
            }
            else if (direction == Direction.EAST)
            {
                if (mEastLink != null)
                {
                    mEastLink.mWestLink = null;
                    mEastLink = null;
                }
                if (mNortheastLink != null)
                {
                    mNortheastLink.mSouthwestLink = null;
                    mNortheastLink = null;
                }
                if (mSoutheastLink != null)
                {
                    mSoutheastLink.mNorthwestLink = null;
                    mSoutheastLink = null;
                }
            }
            else if (direction == Direction.WEST)
            {
                if (mWestLink != null)
                {
                    mWestLink.mEastLink = null;
                    mWestLink = null;
                }
                if (mNorthwestLink != null)
                {
                    mNorthwestLink.mSoutheastLink = null;
                    mNorthwestLink = null;
                }
                if (mSouthwestLink != null)
                {
                    mSouthwestLink.mNortheastLink = null;
                    mSouthwestLink = null;
                }
            }
        }// removeTripleLinks

        public void addDualLinks(Room targetRoom, Direction direction)
        {
            if (direction == Direction.NORTH)
            {
                this.mNorthLink = targetRoom;
                targetRoom.mSouthLink = this;
            }
            else if (direction == Direction.SOUTH)
            {
                this.mSouthLink = targetRoom;
                targetRoom.mNorthLink = this;
            }
            else if (direction == Direction.EAST)
            {
                this.mEastLink = targetRoom;
                targetRoom.mWestLink = this;
            }
            else if (direction == Direction.WEST)
            {
                this.mWestLink = targetRoom;
                targetRoom.mEastLink = this;
            }
            else if (direction == Direction.UP)
            {
                this.mUpLink = targetRoom;
                targetRoom.mDownLink = this;
            }
            else if (direction == Direction.DOWN)
            {
                this.mDownLink = targetRoom;
                targetRoom.mUpLink = this;
            }
            else if (direction == Direction.NORTHWEST)
            {
                this.mNorthwestLink = targetRoom;
                targetRoom.mSoutheastLink = this;
            }
            else if (direction == Direction.NORTHEAST)
            {
                this.mNortheastLink = targetRoom;
                targetRoom.mSouthwestLink = this;
            }
            else if (direction == Direction.SOUTHWEST)
            {
                this.mSouthwestLink = targetRoom;
                targetRoom.mNortheastLink = this;
            }
            else if (direction == Direction.SOUTHEAST)
            {
                this.mSoutheastLink = targetRoom;
                targetRoom.mNorthwestLink = this;
            }
        }// addDualLinks

    }// Class Room

}// Namespace _8th_Circle_Server
