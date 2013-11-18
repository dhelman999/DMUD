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
            bool ret = false;

            switch (direction)
            {
                case "north":
                    if (mCurrentRoom.mNorthLink != null)
                    {
                        changeRoom(mCurrentRoom.mNorthLink);
                        ret = true;
                    }// if
                    break;

                case "south":
                    if (mCurrentRoom.mSouthLink != null)
                    {
                        changeRoom(mCurrentRoom.mSouthLink);
                        ret = true;
                    }// if
                    break;

                case "east":
                    if (mCurrentRoom.mEastLink != null)
                    {
                        changeRoom(mCurrentRoom.mEastLink);
                        ret = true;
                    }// if
                    break;

                case "west":
                    if (mCurrentRoom.mWestLink != null)
                    {
                        changeRoom(mCurrentRoom.mWestLink);
                        ret = true;
                    }// if
                    break;

                case "up":
                    if (mCurrentRoom.mUpLink != null)
                    {
                        changeRoom(mCurrentRoom.mUpLink);
                        ret = true;
                    }// if
                    break;

                case "down":
                    if (mCurrentRoom.mDownLink != null)
                    {
                        changeRoom(mCurrentRoom.mDownLink);
                        ret = true;
                    }// if
                    break;

                default:
                    ret = false;
                    break;
            }// switch

            return ret;

        }// move

        private void changeRoom(Room newRoom)
        {
            mCurrentRoom.mPlayerList.Remove(this);
            newRoom.mPlayerList.Add(this);
            mWorldLoc[0] = newRoom.mWorldLoc[0];
            mWorldLoc[1] = newRoom.mWorldLoc[1];
            mWorldLoc[2] = newRoom.mWorldLoc[2];
            mCurrentRoom = newRoom;
        }// changeRoom

    }// Class Mob

}// Namespace _8th_Circle_Server
