using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum Direction
    {
        UP,
        DIRECTION_START = UP,
        NORTH,
        NORTHEAST,
        EAST,
        SOUTHEAST,
        DOWN,
        SOUTH,
        SOUTHWEST,
        WEST,
        NORTHWEST,
        DIRECTION_END = NORTHWEST
    }// Direction

    class Doorway : Mob
    {
        // Constants
        internal const int MAXROOMS = 10;

        // Member Variables
        public bool mIsOpen;
        public bool mIsLocked;
        public Room [] mRoomList;

        public Doorway() : base()
        {
            mIsOpen = false;
            mIsLocked = false;
            mRoomList = new Room[MAXROOMS];
        }// Constructor

        public Doorway(string name, Room currentRoom)
            : base()
        {
            mIsOpen = false;
            mIsLocked = false;
            mName = name;
            mRoomList = new Room[MAXROOMS];
        }// Constructor

        public override string open(Mob mob)
        {
            if (mIsLocked)
                return mob.mCurrentRoom.getDoorString(this) + " is locked";
            if (mIsOpen)
                return mob.mCurrentRoom.getDoorString(this) + " is already open";
            if (mFlagList.Contains(objectFlags.FLAG_HIDDEN))
                return "you can't find that";

            else
            {
                mIsOpen = true;
                for (int i = 0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                        foreach (Player pl in mRoomList[i].mPlayerList)
                            pl.mClientHandler.safeWrite(mRoomList[i].getDoorString(this) + " opens");
                }// for

                return "you open " + mob.mCurrentRoom.getDoorString(this);
            }// else
        }// open

        public override string close(Mob mob)
        {
            if (mIsLocked)
                return mob.mCurrentRoom.getDoorString(this) + " is locked";
            if (!mIsOpen)
                return mob.mCurrentRoom.getDoorString(this) + " is already closed";
            if (mFlagList.Contains(objectFlags.FLAG_HIDDEN))
                return "you can't find that";

            else
            {
                mIsOpen = false;
                for (int i = 0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                        foreach (Player pl in mRoomList[i].mPlayerList)
                            pl.mClientHandler.safeWrite(mRoomList[i].getDoorString(this) + " closes");
                }// for

                return "you close " + mob.mCurrentRoom.getDoorString(this);
            }// else
        }// close

        public override string exitString(Room currentRoom)
        {
            string ret = string.Empty;

            Direction direction = Direction.DIRECTION_END;

            for (int i = 0; i < currentRoom.mDoorwayList.Count; ++i)
            {
                if (currentRoom.mDoorwayList[i] != null &&
                    currentRoom.mDoorwayList[i].Equals(this))
                {
                    direction = (Direction)(i);
                    break;
                }
            }// for

            switch (direction)
            {
                case Direction.NORTH:
                    ret += "north";
                    break;

                case Direction.SOUTH:
                    ret += "south";
                    break;

                case Direction.EAST:
                    ret += "east";
                    break;

                case Direction.WEST:
                    ret += "west";
                    break;

                case Direction.UP:
                    ret += "up";
                    break;

                case Direction.DOWN:
                    ret += "down";
                    break;

                case Direction.NORTHWEST:
                    ret += "northwest";
                    break;

                case Direction.NORTHEAST:
                    ret += "northeast";
                    break;

                case Direction.SOUTHWEST:
                    ret += "southwest";
                    break;

                case Direction.SOUTHEAST:
                    ret += "southeast";
                    break;

                default:
                    Console.WriteLine("Something went wrong with " + mName);
                    break;
            }// switch

            ret += " " + mName;
            return ret;

        }// exitString

    }// Class Doorway

}// Namespace _8th_Circle_Server
