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
            mRoomList[(int)Direction.NORTH] = currentRoom.mNorthLink;
            mRoomList[(int)Direction.SOUTH] = currentRoom.mSouthLink;
            mRoomList[(int)Direction.EAST] = currentRoom.mEastLink;
            mRoomList[(int)Direction.WEST] = currentRoom.mWestLink;
            mRoomList[(int)Direction.UP] = currentRoom.mUpLink;
            mRoomList[(int)Direction.DOWN] = currentRoom.mDownLink;
            mRoomList[(int)Direction.NORTHWEST] = currentRoom.mNorthwestLink;
            mRoomList[(int)Direction.NORTHEAST] = currentRoom.mNortheastLink;
            mRoomList[(int)Direction.SOUTHWEST] = currentRoom.mSouthwestLink;
            mRoomList[(int)Direction.SOUTHEAST] = currentRoom.mSoutheastLink;
            mCurrentRoom = currentRoom;
            currentRoom.mObjectList.Add(this);
            currentRoom.mCurrentArea.mObjectList.Add(this);
            currentRoom.mCurrentArea.mWorld.mObjectList.Add(this);
            mPrimaryExit = exit;
            mBidirectional = true;
            mCompanion = null;
            mIsToggled = false;
            closed();
        }// Constructor

        public Doorway(string name, Room currentRoom)
            : base()
        {
            mIsOpen = false;
            mIsLocked = false;
            mName = name;
            mExits = new ArrayList();
            mRoomList = new Room[MAXROOMS];
            mRoomList[(int)Direction.NORTH] = currentRoom.mNorthLink;
            mRoomList[(int)Direction.SOUTH] = currentRoom.mSouthLink;
            mRoomList[(int)Direction.EAST] = currentRoom.mEastLink;
            mRoomList[(int)Direction.WEST] = currentRoom.mWestLink;
            mRoomList[(int)Direction.UP] = currentRoom.mUpLink;
            mRoomList[(int)Direction.DOWN] = currentRoom.mDownLink;
            mRoomList[(int)Direction.NORTHWEST] = currentRoom.mNorthwestLink;
            mRoomList[(int)Direction.NORTHEAST] = currentRoom.mNortheastLink;
            mRoomList[(int)Direction.SOUTHWEST] = currentRoom.mSouthwestLink;
            mRoomList[(int)Direction.SOUTHEAST] = currentRoom.mSoutheastLink;
            mCurrentRoom = currentRoom;
            currentRoom.mObjectList.Add(this);
            currentRoom.mCurrentArea.mObjectList.Add(this);
            currentRoom.mCurrentArea.mWorld.mObjectList.Add(this);
            mBidirectional = true;
            mCompanion = null;
            mIsToggled = false;
            closed();
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
                    case Direction.NORTH:
                        if (mRoomList[(int)Direction.NORTH] != null)
                            mCurrentRoom.mNorthLink = mRoomList[(int)Direction.NORTH];                
                        break;

                    case Direction.SOUTH:
                        if (mRoomList[(int)Direction.SOUTH] != null)
                            mCurrentRoom.mSouthLink = mRoomList[(int)Direction.SOUTH];
                        break;

                    case Direction.EAST:
                        if (mRoomList[(int)Direction.EAST] != null)
                            mCurrentRoom.mEastLink = mRoomList[(int)Direction.EAST];
                        break;

                    case Direction.WEST:
                        if (mRoomList[(int)Direction.WEST] != null)
                            mCurrentRoom.mWestLink = mRoomList[(int)Direction.WEST];
                        break;

                    case Direction.UP:
                        if (mRoomList[(int)Direction.UP] != null)
                            mCurrentRoom.mUpLink = mRoomList[(int)Direction.UP];
                        break;

                    case Direction.DOWN:
                        if (mRoomList[(int)Direction.DOWN] != null)
                            mCurrentRoom.mDownLink = mRoomList[(int)Direction.DOWN];
                        break;

                    case Direction.NORTHWEST:
                        if (mRoomList[(int)Direction.NORTHWEST] != null)
                            mCurrentRoom.mNorthwestLink = mRoomList[(int)Direction.NORTHWEST];
                        break;

                    case Direction.NORTHEAST:
                        if (mRoomList[(int)Direction.NORTHEAST] != null)
                            mCurrentRoom.mNortheastLink = mRoomList[(int)Direction.NORTHEAST];
                        break;

                    case Direction.SOUTHWEST:
                        if (mRoomList[(int)Direction.SOUTHWEST] != null)
                            mCurrentRoom.mSouthwestLink = mRoomList[(int)Direction.SOUTHWEST];
                        break;

                    case Direction.SOUTHEAST:
                        if (mRoomList[(int)Direction.SOUTHEAST] != null)
                            mCurrentRoom.mSoutheastLink = mRoomList[(int)Direction.SOUTHEAST];
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
                    case Direction.NORTH:
                        if (mRoomList[(int)Direction.NORTH] != null)
                            mCurrentRoom.mNorthLink = null;
                        break;

                    case Direction.SOUTH:
                        if (mRoomList[(int)Direction.SOUTH] != null)
                            mCurrentRoom.mSouthLink = null;
                        break;

                    case Direction.EAST:
                        if (mRoomList[(int)Direction.EAST] != null)
                            mCurrentRoom.mEastLink = null;
                        break;

                    case Direction.WEST:
                        if (mRoomList[(int)Direction.WEST] != null)
                            mCurrentRoom.mWestLink = null;
                        break;

                    case Direction.UP:
                        if (mRoomList[(int)Direction.UP] != null)
                            mCurrentRoom.mUpLink = null;
                        break;

                    case Direction.DOWN:
                        if (mRoomList[(int)Direction.DOWN] != null)
                            mCurrentRoom.mDownLink = null;
                        break;

                    case Direction.NORTHWEST:
                        if (mRoomList[(int)Direction.NORTHWEST] != null)
                            mCurrentRoom.mNorthwestLink = null;
                        break;

                    case Direction.NORTHEAST:
                        if (mRoomList[(int)Direction.NORTHEAST] != null)
                            mCurrentRoom.mNortheastLink = null;
                        break;

                    case Direction.SOUTHWEST:
                        if (mRoomList[(int)Direction.SOUTHWEST] != null)
                            mCurrentRoom.mSouthwestLink = null;
                        break;

                    case Direction.SOUTHEAST:
                        if (mRoomList[(int)Direction.SOUTHEAST] != null)
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
