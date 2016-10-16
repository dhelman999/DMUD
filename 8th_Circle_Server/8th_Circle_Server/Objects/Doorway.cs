using System;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class Doorway : Mob
    {
        // Constants
        internal const int MAXROOMS = 10;

        // Member Variables
        private bool mIsOpen;
        private bool mIsLocked;
        private Room [] mRoomList;

        public Doorway() : base()
        {
            mIsOpen = false;
            mIsLocked = false;
            mRoomList = new Room[MAXROOMS];
        }// Constructor

        public Doorway(string name, MobFlags flags = MobFlags.NONE) : base(name, flags)
        {
            mIsOpen = false;
            mIsLocked = false;
            mRoomList = new Room[MAXROOMS];
        }// Constructor

        public Doorway(Doorway doorway) : base()
        {
            mIsOpen = doorway.mIsOpen;
            mIsLocked = doorway.mIsLocked;
            mRoomList = (Room [])doorway.mRoomList.Clone();
            mFlags = doorway.mFlags;
        }// Copy Constructor

        public override Mob Clone()
        {
            return new Doorway(this);
        }// Clone

        public override Mob Clone(string name)
        {
            return new Doorway(name);
        }// Clone

        public void reset()
        {
            if(mMemento != null)
            {
                Doorway memento = (Doorway)mMemento;
                mIsLocked = memento.mIsLocked;
                mIsOpen = memento.mIsOpen;
                mRoomList = memento.mRoomList;
                mFlags = memento.mFlags;
            }
        }// reset

        public override string open(Mob opener)
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";
            if (mIsLocked)
                return opener.GetCurrentRoom().getDoorString(this) + " is locked\n";
            if (mIsOpen)
                return opener.GetCurrentRoom().getDoorString(this) + " is already open\n";

            else
            {
                mIsOpen = true;

                for (int i = 0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                    {
                        Room currentRoom = mRoomList[i];
                        Utils.broadcast(currentRoom, opener, currentRoom.getDoorString(this) + " opens\n");
                    }
                }// for

                return "you open " + opener.GetCurrentRoom().getDoorString(this) + "\n";
            }// else
        }// open

        public override string close(Mob closer)
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";
            if (mIsLocked)
                return closer.GetCurrentRoom().getDoorString(this) + " is locked\n";
            if (!mIsOpen)
                return closer.GetCurrentRoom().getDoorString(this) + " is already closed\n";
            else
            {
                mIsOpen = false;

                for (int i = 0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                    {
                        Room currentRoom = mRoomList[i];
                        Utils.broadcast(currentRoom, closer, currentRoom.getDoorString(this) + " closes\n");
                    }
                }// for

                return "you close " + closer.GetCurrentRoom().getDoorString(this) + "\n";
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

        // Accessors
        public bool IsOpen() { return mIsOpen; }
        public bool IsLocked() { return mIsLocked; }
        public Room[] GetRoomList() { return mRoomList; }

    }// Class Doorway

}// Namespace _8th_Circle_Server
