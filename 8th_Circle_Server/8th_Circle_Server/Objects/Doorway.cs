using System;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class Doorway : Mob
    {
        // Constants
        internal const int MAXROOMS = 10;

        // Member Variables
        public bool mIsOpen;
        public bool mIsLocked;
        public Room [] mRoomList;
        public List<MobFlags> mStartingFlagList;
        public Memento mMemento;

        public Doorway() : base()
        {
            mIsOpen = false;
            mIsLocked = false;
            mRoomList = new Room[MAXROOMS];
            mStartingFlagList = new List<MobFlags>();
            mMemento = new Memento(this);
        }// Constructor

        public Doorway(string name) : base(name)
        {
            mIsOpen = false;
            mIsLocked = false;
            mRoomList = new Room[MAXROOMS];
            mStartingFlagList = new List<MobFlags>();
            mMemento = new Memento(this);
        }// Constructor

        public Doorway(Doorway doorway)
        {
            mIsOpen = doorway.mIsOpen;
            mIsLocked = doorway.mIsLocked;
            mRoomList = (Room [])doorway.mRoomList.Clone();
            mStartingFlagList = new List<MobFlags>(doorway.mStartingFlagList);
            mMemento = doorway.mMemento;
        }// Copy Constructor

        public void reset()
        {
            Doorway memento = (Doorway)(mMemento.getMemento(MementoType.DOORWAY));
            mIsOpen = memento.mIsOpen;
            mIsLocked = memento.mIsLocked;
            mRoomList = memento.mRoomList;
            mFlagList = new List<MobFlags>(mStartingFlagList);
        }

        public override string open(Mob mob)
        {
            if (mIsLocked)
                return mob.mCurrentRoom.getDoorString(this) + " is locked\n";
            if (mIsOpen)
                return mob.mCurrentRoom.getDoorString(this) + " is already open\n";
            if (mFlagList.Contains(MobFlags.FLAG_HIDDEN))
                return "you can't find that\n";

            else
            {
                mIsOpen = true;

                for (int i = 0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                    {
                        foreach (CombatMob pl in mRoomList[i].getRes(ResType.PLAYER))
                            pl.mClientHandler.safeWrite(mRoomList[i].getDoorString(this) + " opens\n");
                    }
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
            if (mFlagList.Contains(MobFlags.FLAG_HIDDEN))
                return "you can't find that\n";
            else
            {
                mIsOpen = false;

                for (int i = 0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                    {
                        foreach (CombatMob pl in mRoomList[i].getRes(ResType.PLAYER))
                            pl.mClientHandler.safeWrite(mRoomList[i].getDoorString(this) + " closes\n");
                    }
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
                if (currentRoom.getRes(ResType.DOORWAY)[i] != null && currentRoom.getRes(ResType.DOORWAY)[i].Equals(this))
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
