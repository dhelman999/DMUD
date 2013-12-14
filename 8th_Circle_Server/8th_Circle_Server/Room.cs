using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Room
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
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

        public Room(string desc, int xCoord, int yCoord, int zCoord)
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
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
            mObjectList = new ArrayList();
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
            for (int i = 0; i < mObjectList.Count; ++i)
                exitStr += ((Mob)mObjectList[i]).exitString(this) + "\n";

            if (mObjectList.Count == 0)
                exitStr += "\n";
            for (int i = 0; i < mNpcList.Count; ++i)
                exitStr += ((Mob)mNpcList[i]).mName + "\n";

            exitStr += "Players: ";
            for (int i = 0; i < mPlayerList.Count; ++i)
                exitStr += ((Player)mPlayerList[i]).mName + "\n";

            exitStr += "\n";
            return exitStr;
        }// exitString

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

    }// Class Room

}// Namespace _8th_Circle_Server
