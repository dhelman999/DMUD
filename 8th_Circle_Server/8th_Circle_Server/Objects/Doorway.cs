using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum Direction
    {
        NORTH,
        SOUTH,
        EAST,
        WEST,
        UP,
        DOWN,
        NORTHWEST,
        NORTHEAST,
        SOUTHWEST,
        SOUTHEAST,
        INVALID
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
            mCurrentRoom = currentRoom;
        }// Constructor

        public override string open(ClientHandler clientHandler)
        {
            if (mIsLocked)
                return mName + " is locked";
            else
            {
                mIsOpen = true;
                for(int i=0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                        foreach (Player pl in mRoomList[i].mPlayerList)
                            pl.mClientHandler.safeWrite(mRoomList[i].getDoorString(this) + " opens");
                }// for

                return "you open " + clientHandler.mPlayer.mCurrentRoom.getDoorString(this);
            }// else
        }// open

        public override string close(ClientHandler clientHandler)
        {
            if (mIsLocked)
                return mName + " is locked";
            else
            {
                mIsOpen = false;
                for (int i = 0; i < mRoomList.Length; ++i)
                {
                    if (mRoomList[i] != null)
                        foreach (Player pl in mRoomList[i].mPlayerList)
                            pl.mClientHandler.safeWrite(mRoomList[i].getDoorString(this) + " closes");
                }// for

                return "you close " + clientHandler.mPlayer.mCurrentRoom.getDoorString(this);
            }// else
        }// close

        public override string exitString()
        {
            string ret = string.Empty;

            Direction direction = Direction.INVALID;
            for(int i=0;i < mCurrentRoom.mDoorwayList.Length; ++i)
            {
                if(mCurrentRoom.mDoorwayList[i] != null &&
                   mCurrentRoom.mDoorwayList[i].Equals(this))
                    direction = (Direction)(i);
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
