using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Mob
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public string mName;
        public string mDescription;
        public World mWorld;
        public Area mCurrentArea;
        public Room mCurrentRoom;
        public int[] mWorldLoc;
        public ArrayList mInventory;

        public Mob()
        {
            mName = string.Empty;
            mWorldLoc = new int[3];
            mInventory = new ArrayList();
            mInventory.Capacity = 20;
        }// Constructor

        public Mob(string name)
        {
            mName = name;
            mWorldLoc = new int[3];
            mInventory = new ArrayList();
            mInventory.Capacity = 20;
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

                case "northwest":
                    if (mCurrentRoom.mNorthwestLink != null)
                    {
                        changeRoom(mCurrentRoom.mNorthwestLink);
                        ret = true;
                    }// if
                    break;

                case "northeast":
                    if (mCurrentRoom.mNortheastLink != null)
                    {
                        changeRoom(mCurrentRoom.mNortheastLink);
                        ret = true;
                    }// if
                    break;

                case "southwest":
                    if (mCurrentRoom.mSouthwestLink != null)
                    {
                        changeRoom(mCurrentRoom.mSouthwestLink);
                        ret = true;
                    }// if
                    break;

                case "southeast":
                    if (mCurrentRoom.mSoutheastLink != null)
                    {
                        changeRoom(mCurrentRoom.mSoutheastLink);
                        ret = true;
                    }// if
                    break;

                default:
                    ret = false;
                    break;
            }// switch

            return ret;

        }// move

        public string viewed(Preposition prep, ClientHandler clientHandler)
        {
            if (prep.prepType == PrepositionType.PREP_AT)
                return this.mDescription;
            else
                return "You can't look like that";
        }// viewed

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
