using System;

namespace _8th_Circle_Server
{
    public class Doorway : Mob
    {
        // Constants
        internal const int MAXROOMS = 10;

        // Member Variables
        private Room [] mRoomList;

        public Doorway() : base()
        {
            mRoomList = new Room[MAXROOMS];
        }// Constructor

        public Doorway(String name, MobFlags flags = MobFlags.NONE) : base(name, flags)
        {
            mRoomList = new Room[MAXROOMS];
        }// Constructor

        public Doorway(Doorway doorway) : base()
        {
            mRoomList = (Room [])doorway.mRoomList.Clone();
            mFlags = doorway.mFlags;
        }// Copy Constructor

        public override Mob Clone()
        {
            return new Doorway(this);
        }// Clone

        public override Mob Clone(String name)
        {
            return new Doorway(name);
        }// Clone

        public void reset()
        {
            if(mMemento != null)
            {
                Doorway memento = (Doorway)mMemento;
                mRoomList = memento.mRoomList;
                mFlags = memento.mFlags;
            }
        }// reset

        public override errorCode open(Mob opener, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (HasFlag(MobFlags.HIDDEN))
                clientString = "you can't do that\n";
            if (HasFlag(MobFlags.LOCKED))
                clientString = opener.GetCurrentRoom().getDoorString(this) + " is locked\n";
            if (HasFlag(MobFlags.OPEN))
                clientString = opener.GetCurrentRoom().getDoorString(this) + " is already open\n";

            else
            {
                Utils.SetFlag(ref mFlags, MobFlags.OPEN);

                for (int i = 0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                    {
                        Room currentRoom = mRoomList[i];
                        Utils.Broadcast(currentRoom, opener, currentRoom.getDoorString(this) + " opens\n");
                    }
                }// for

                clientString = "you open " + opener.GetCurrentRoom().getDoorString(this) + "\n";
                eCode = errorCode.E_OK;
            }// else

            return eCode;
        }// open

        public override errorCode close(Mob closer, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (HasFlag(MobFlags.HIDDEN))
                clientString = "you can't do that\n";
            if (HasFlag(MobFlags.LOCKED))
                clientString = closer.GetCurrentRoom().getDoorString(this) + " is locked\n";
            if (!HasFlag(MobFlags.OPEN))
                clientString = closer.GetCurrentRoom().getDoorString(this) + " is already closed\n";
            else
            {
                Utils.UnsetFlag(ref mFlags, MobFlags.OPEN);

                for (int i = 0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                    {
                        Room currentRoom = mRoomList[i];
                        Utils.Broadcast(currentRoom, closer, currentRoom.getDoorString(this) + " closes\n");
                    }
                }// for

                clientString = "you close " + closer.GetCurrentRoom().getDoorString(this) + "\n";
                eCode = errorCode.E_OK;
            }// else

            return eCode;
        }// close

        public override String exitString(Room currentRoom)
        {
            String ret = String.Empty;
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
        public Room[] GetRoomList() { return mRoomList; }

    }// Class Doorway

}// Namespace _8th_Circle_Server
