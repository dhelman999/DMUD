using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public enum Direction
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

    public class Doorway : Mob
    {
        // Constants
        internal const int MAXROOMS = 10;

        // Member Variables
        public bool mIsOpen;
        public bool mStartingOpenState;
        public bool mIsLocked;
        public bool mStartingLockedState;
        public Room [] mRoomList;
        public ArrayList mStartingFlagList;

        public Doorway() : base()
        {
            mIsOpen = mStartingOpenState = false;
            mIsLocked = mStartingLockedState = false;
            mRoomList = new Room[MAXROOMS];
            mStartingFlagList = new ArrayList();
        }// Constructor

        public Doorway(string name) : base(name)
        {
            mIsOpen = mStartingOpenState = false;
            mIsLocked = mStartingLockedState = false;
            mRoomList = new Room[MAXROOMS];
            mStartingFlagList = new ArrayList();
        }// Constructor

        public override string open(Mob mob)
        {
            if (mIsLocked)
                return mob.mCurrentRoom.getDoorString(this) + " is locked\n";
            if (mIsOpen)
                return mob.mCurrentRoom.getDoorString(this) + " is already open\n";
            if (mFlagList.Contains(mobFlags.FLAG_HIDDEN))
                return "you can't find that\n";

            else
            {
                mIsOpen = true;
                for (int i = 0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                        foreach (Player pl in mRoomList[i].getRes(ResType.PLAYER))
                            pl.mClientHandler.safeWrite(mRoomList[i].getDoorString(this) + " opens\n");
                }// for

                return "you open " + mob.mCurrentRoom.getDoorString(this) + "\n";
            }// else
        }// open

        public override string close(Mob mob)
        {
            if (mIsLocked)
                return mob.mCurrentRoom.getDoorString(this) + " is locked\n";
            if (!mIsOpen)
                return mob.mCurrentRoom.getDoorString(this) + " is already closed\n";
            if (mFlagList.Contains(mobFlags.FLAG_HIDDEN))
                return "you can't find that\n";

            else
            {
                mIsOpen = false;
                for (int i = 0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                        foreach (Player pl in mRoomList[i].getRes(ResType.PLAYER))
                            pl.mClientHandler.safeWrite(mRoomList[i].getDoorString(this) + " closes\n");
                }// for

                return "you close " + mob.mCurrentRoom.getDoorString(this) + "\n";
            }// else
        }// close

        public override string exitString(Room currentRoom)
        {
            string ret = string.Empty;

            Direction direction = Direction.DIRECTION_END;

            for (int i = 0; i < currentRoom.getRes(ResType.DOORWAY).Count; ++i)
            {
                if (currentRoom.getRes(ResType.DOORWAY)[i] != null &&
                    currentRoom.getRes(ResType.DOORWAY)[i].Equals(this))
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
                    Console.WriteLine("Something went wrong with " + mName + "\n");
                    break;
            }// switch

            ret += " " + mName;
            return ret;

        }// exitString

    }// Class Doorway

}// Namespace _8th_Circle_Server
