using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum Direction
    {
        DIRECTION_NORTH,
        DIRECTION_SOUTH,
        DIRECTION_EAST,
        DIRECTION_WEST,
        DIRECTION_UP,
        DIRECTION_DOWN,
        DIRECTION_NORTHWEST,
        DIRECTION_NORTHEAST,
        DIRECTION_SOUTHWEST,
        DIRECTION_SOUTHEAST,
        INVALID
    }// Direction

    class Doorway : Mob
    {
        // Constants
        internal const int MAXROOMS = 10;

        // Member Variables
        public bool mIsOpen;
        public bool mIsLocked;
        public Direction mPrimaryExit;
        public ArrayList mExits;
        public Room [] mRoomList;
        public bool mBidirectional;
        public Doorway mCompanion;
        public bool mIsToggled;

        public Doorway() : base()
        {
            mIsOpen = false;
            mIsLocked = false;
            mRoomList = new Room[MAXROOMS];
            mExits = new ArrayList();
            mPrimaryExit = Direction.INVALID;
            mBidirectional = true;
            mCompanion = null;
            mIsToggled = false;
        }// Constructor

        public Doorway(string name, Direction exit, Room currentRoom)
            : base()
        {
            mIsOpen = false;
            mIsLocked = false;
            mName = name;
            mExits = new ArrayList();
            mExits.Add(exit);
            mRoomList = new Room[MAXROOMS];
            mRoomList[(int)Direction.DIRECTION_NORTH] = currentRoom.mNorthLink;
            mRoomList[(int)Direction.DIRECTION_SOUTH] = currentRoom.mSouthLink;
            mRoomList[(int)Direction.DIRECTION_EAST] = currentRoom.mEastLink;
            mRoomList[(int)Direction.DIRECTION_WEST] = currentRoom.mWestLink;
            mRoomList[(int)Direction.DIRECTION_UP] = currentRoom.mUpLink;
            mRoomList[(int)Direction.DIRECTION_DOWN] = currentRoom.mDownLink;
            mRoomList[(int)Direction.DIRECTION_NORTHWEST] = currentRoom.mNorthwestLink;
            mRoomList[(int)Direction.DIRECTION_NORTHEAST] = currentRoom.mNortheastLink;
            mRoomList[(int)Direction.DIRECTION_SOUTHWEST] = currentRoom.mSouthwestLink;
            mRoomList[(int)Direction.DIRECTION_SOUTHEAST] = currentRoom.mSoutheastLink;
            mCurrentRoom = currentRoom;
            currentRoom.mObjectList.Add(this);
            currentRoom.mCurrentArea.mObjectList.Add(this);
            currentRoom.mCurrentArea.mWorld.mObjectList.Add(this);
            mPrimaryExit = exit;
            mBidirectional = true;
            mCompanion = null;
            mIsToggled = false;
        }// Constructor

        public override string open(ClientHandler clientHandler)
        {
            if (mIsLocked)
                return mName + " is locked";
            else
            {
                mIsOpen = true;
                opened();
                if (mBidirectional && mCompanion != null && !mIsToggled)
                {
                    mIsToggled = mCompanion.mIsToggled = true;
                    mCompanion.open(clientHandler);
                    foreach (Player pl in mCurrentRoom.mPlayerList)
                        pl.mClientHandler.safeWrite(exitString() + " opens");
                    foreach (Player pl in mCompanion.mCurrentRoom.mPlayerList)
                        pl.mClientHandler.safeWrite(mCompanion.exitString() + " opens");
                }// if
                mIsToggled = false;

                return "you open " + exitString();
            }// else
        }// open

        public override string close(ClientHandler clientHandler)
        {
            if (mIsLocked)
                return mName + " is locked";
            else
            {
                mIsOpen = false;
                closed();
                if (mBidirectional && mCompanion != null && !mIsToggled)
                {
                    mIsToggled = mCompanion.mIsToggled = true;
                    mCompanion.close(clientHandler);
                    foreach (Player pl in mCurrentRoom.mPlayerList)
                        pl.mClientHandler.safeWrite(exitString() + " closes");
                    foreach (Player pl in mCompanion.mCurrentRoom.mPlayerList)
                        pl.mClientHandler.safeWrite(mCompanion.exitString() + " closes");
                }// if
                mIsToggled = false;

                return "you close " + exitString();
            }// else
        }// close

        public void opened()
        {
            foreach (Direction dir in mExits)
            {
                switch (dir)
                {
                    case Direction.DIRECTION_NORTH:
                        if (mRoomList[(int)Direction.DIRECTION_NORTH] != null)
                            mCurrentRoom.mNorthLink = mRoomList[(int)Direction.DIRECTION_NORTH];                
                        break;

                    case Direction.DIRECTION_SOUTH:
                        if (mRoomList[(int)Direction.DIRECTION_SOUTH] != null)
                            mCurrentRoom.mSouthLink = mRoomList[(int)Direction.DIRECTION_SOUTH];
                        break;

                    case Direction.DIRECTION_EAST:
                        if (mRoomList[(int)Direction.DIRECTION_EAST] != null)
                            mCurrentRoom.mEastLink = mRoomList[(int)Direction.DIRECTION_EAST];
                        break;

                    case Direction.DIRECTION_WEST:
                        if (mRoomList[(int)Direction.DIRECTION_WEST] != null)
                            mCurrentRoom.mWestLink = mRoomList[(int)Direction.DIRECTION_WEST];
                        break;

                    case Direction.DIRECTION_UP:
                        if (mRoomList[(int)Direction.DIRECTION_UP] != null)
                            mCurrentRoom.mUpLink = mRoomList[(int)Direction.DIRECTION_UP];
                        break;

                    case Direction.DIRECTION_DOWN:
                        if (mRoomList[(int)Direction.DIRECTION_DOWN] != null)
                            mCurrentRoom.mDownLink = mRoomList[(int)Direction.DIRECTION_DOWN];
                        break;

                    case Direction.DIRECTION_NORTHWEST:
                        if (mRoomList[(int)Direction.DIRECTION_NORTHWEST] != null)
                            mCurrentRoom.mNorthwestLink = mRoomList[(int)Direction.DIRECTION_NORTHWEST];
                        break;

                    case Direction.DIRECTION_NORTHEAST:
                        if (mRoomList[(int)Direction.DIRECTION_NORTHEAST] != null)
                            mCurrentRoom.mNortheastLink = mRoomList[(int)Direction.DIRECTION_NORTHEAST];
                        break;

                    case Direction.DIRECTION_SOUTHWEST:
                        if (mRoomList[(int)Direction.DIRECTION_SOUTHWEST] != null)
                            mCurrentRoom.mSouthwestLink = mRoomList[(int)Direction.DIRECTION_SOUTHWEST];
                        break;

                    case Direction.DIRECTION_SOUTHEAST:
                        if (mRoomList[(int)Direction.DIRECTION_SOUTHEAST] != null)
                            mCurrentRoom.mSoutheastLink = mRoomList[(int)Direction.DIRECTION_SOUTHEAST];
                        break;

                    default:
                        Console.WriteLine("something went wrong with " + mName);
                        break;
                }// switch
            }// foreach
        }// opened

        public void closed()
        {
            foreach (Direction dir in mExits)
            {
                switch (dir)
                {
                    case Direction.DIRECTION_NORTH:
                        if (mRoomList[(int)Direction.DIRECTION_NORTH] != null)
                            mCurrentRoom.mNorthLink = null;
                        break;

                    case Direction.DIRECTION_SOUTH:
                        if (mRoomList[(int)Direction.DIRECTION_SOUTH] != null)
                            mCurrentRoom.mSouthLink = null;
                        break;

                    case Direction.DIRECTION_EAST:
                        if (mRoomList[(int)Direction.DIRECTION_EAST] != null)
                            mCurrentRoom.mEastLink = null;
                        break;

                    case Direction.DIRECTION_WEST:
                        if (mRoomList[(int)Direction.DIRECTION_WEST] != null)
                            mCurrentRoom.mWestLink = null;
                        break;

                    case Direction.DIRECTION_UP:
                        if (mRoomList[(int)Direction.DIRECTION_UP] != null)
                            mCurrentRoom.mUpLink = null;
                        break;

                    case Direction.DIRECTION_DOWN:
                        if (mRoomList[(int)Direction.DIRECTION_DOWN] != null)
                            mCurrentRoom.mDownLink = null;
                        break;

                    case Direction.DIRECTION_NORTHWEST:
                        if (mRoomList[(int)Direction.DIRECTION_NORTHWEST] != null)
                            mCurrentRoom.mNorthwestLink = null;
                        break;

                    case Direction.DIRECTION_NORTHEAST:
                        if (mRoomList[(int)Direction.DIRECTION_NORTHEAST] != null)
                            mCurrentRoom.mNortheastLink = null;
                        break;

                    case Direction.DIRECTION_SOUTHWEST:
                        if (mRoomList[(int)Direction.DIRECTION_SOUTHWEST] != null)
                            mCurrentRoom.mSouthwestLink = null;
                        break;

                    case Direction.DIRECTION_SOUTHEAST:
                        if (mRoomList[(int)Direction.DIRECTION_SOUTHEAST] != null)
                            mCurrentRoom.mSoutheastLink = null;
                        break;

                    default:
                        Console.WriteLine("something went wrong with " + mName);
                        break;
                }// switch
            }// foreach
        }// closed

        public override string exitString()
        {
            string ret = string.Empty;

            switch (mPrimaryExit)
            {
                case Direction.DIRECTION_NORTH:
                    ret += "north";
                    break;

                case Direction.DIRECTION_SOUTH:
                    ret += "south";
                    break;

                case Direction.DIRECTION_EAST:
                    ret += "east";
                    break;

                case Direction.DIRECTION_WEST:
                    ret += "west";
                    break;

                case Direction.DIRECTION_UP:
                    ret += "up";
                    break;

                case Direction.DIRECTION_DOWN:
                    ret += "down";
                    break;

                case Direction.DIRECTION_NORTHWEST:
                    ret += "northwest";
                    break;

                case Direction.DIRECTION_NORTHEAST:
                    ret += "northeast";
                    break;

                case Direction.DIRECTION_SOUTHWEST:
                    ret += "southwest";
                    break;

                case Direction.DIRECTION_SOUTHEAST:
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
