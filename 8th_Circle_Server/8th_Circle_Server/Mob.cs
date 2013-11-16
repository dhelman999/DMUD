using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Mob
    {
        // Debug
        internal const bool DEBUG = true;

        // Member Variables
        public string mName;
        public World mWorld;
        public Room mCurrentRoom;
        public int[] mWorldLoc;

        public Mob()
        {
            mName = string.Empty;
            mWorldLoc = new int[3];
        }// Constructor

        public Mob(string name)
        {
            mName = name;
            mWorldLoc = new int[3];
        }// Constructor

        public bool move(string direction)
        {
            bool ret = true;

            switch (direction)
            {
                case "north":
                    if (mCurrentRoom.mNorthLink != null)
                    {
                        mCurrentRoom.mPlayerList.Remove(this);
                        mCurrentRoom.mNorthLink.mPlayerList.Add(this);
                        ++mWorldLoc[1];
                        mCurrentRoom = mCurrentRoom.mNorthLink;
                    }
                    else
                    {
                        ret = false;
                    }
                    break;

                case "south":
                    if (mCurrentRoom.mSouthLink != null)
                    {
                        mCurrentRoom.mPlayerList.Remove(this);
                        mCurrentRoom.mSouthLink.mPlayerList.Add(this);
                        --mWorldLoc[1];
                        mCurrentRoom = mCurrentRoom.mSouthLink;
                    }
                    else
                    {
                        ret = false;
                    }
                    break;

                case "east":
                    if (mCurrentRoom.mEastLink != null)
                    {
                        mCurrentRoom.mPlayerList.Remove(this);
                        mCurrentRoom.mEastLink.mPlayerList.Add(this);
                        ++mWorldLoc[0];
                        mCurrentRoom = mCurrentRoom.mEastLink;
                    }
                    else
                    {
                        ret = false;
                    }
                    break;

                case "west":
                    if (mCurrentRoom.mWestLink != null)
                    {
                        mCurrentRoom.mPlayerList.Remove(this);
                        mCurrentRoom.mWestLink.mPlayerList.Add(this);
                        --mWorldLoc[0];
                        mCurrentRoom = mCurrentRoom.mWestLink;
                    }
                    else
                    {
                        ret = false;
                    }
                    break;

                case "up":
                    if (mCurrentRoom.mUpLink != null)
                    {
                        mCurrentRoom.mPlayerList.Remove(this);
                        mCurrentRoom.mUpLink.mPlayerList.Add(this);
                        ++mWorldLoc[2];
                        mCurrentRoom = mCurrentRoom.mUpLink;
                    }
                    else
                    {
                        ret = false;
                    }
                    break;

                case "down":
                    if (mCurrentRoom.mDownLink != null)
                    {
                        mCurrentRoom.mPlayerList.Remove(this);
                        mCurrentRoom.mDownLink.mPlayerList.Add(this);
                        --mWorldLoc[2];
                        mCurrentRoom = mCurrentRoom.mDownLink;
                    }
                    else
                    {
                        ret = false;
                    }
                    break;

                default:
                    ret = false;
                    break;
            }// switch

            return ret;

        }// move

    }// Class Mob

}// Namespace _8th_Circle_Server
