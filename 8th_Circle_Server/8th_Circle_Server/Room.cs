using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Room
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public string mDescription;
        public Room mNorthLink;
        public Room mSouthLink;
        public Room mEastLink;
        public Room mWestLink;
        public Room mUpLink;
        public Room mDownLink;
        public Room mNortheastLink;
        public Room mNorthwestLink;
        public Room mSouthwestLink;
        public Room mSoutheastLink;
        public int[] mWorldLoc;
        public ArrayList mPlayerList;
        public ArrayList mNpcList;
        public ArrayList mObjectList;
        public Area mCurrentArea;

        public Room()
        {
            mDescription = string.Empty;
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
            mObjectList = new ArrayList();
            mWorldLoc = new int[3];
            mNorthLink = mSouthLink = mEastLink = mWestLink = mUpLink = mDownLink =
                mNortheastLink = mNorthwestLink = mSouthwestLink = mSoutheastLink = null;
        }// Constructor

        public Room(string desc)
        {
            mDescription = desc;
            mNorthLink = mSouthLink = mEastLink = mWestLink = mUpLink = mDownLink =
                mNortheastLink = mNorthwestLink = mSouthwestLink = mSoutheastLink = null;
            mWorldLoc = new int[3];
            mWorldLoc[0] = mWorldLoc[1] = mWorldLoc[2] = -1;
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
            mObjectList = new ArrayList();
        }// Constructor

        public Room(string desc, int xCoord, int yCoord, int zCoord)
        {
            mDescription = desc;
            mNorthLink = mSouthLink = mEastLink = mWestLink = mUpLink = mDownLink =
                mNortheastLink = mNorthwestLink = mSouthwestLink = mSoutheastLink = null;
            mWorldLoc = new int[3];
            mWorldLoc[0] = xCoord;
            mWorldLoc[1] = yCoord;
            mWorldLoc[2] = zCoord;
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
            mObjectList = new ArrayList();
        }// Constructor

        public string exitString()
        {
            string exitStr = "Exits: ";
            if (mNorthLink != null)
                exitStr += "North ";
            if (mSouthLink != null)
                exitStr += "South ";
            if (mEastLink != null)
                exitStr += "East ";
            if (mWestLink != null)
                exitStr += "West ";
            if (mUpLink != null)
                exitStr += "Up ";
            if (mDownLink != null)
                exitStr += "Down ";
            if (mNorthwestLink != null)
                exitStr += "Northwest ";
            if (mNortheastLink != null)
                exitStr += "Northeast ";
            if (mSouthwestLink != null)
                exitStr += "Southwest ";
            if (mSoutheastLink != null)
                exitStr += "Southeast ";

            exitStr += "\n";
            exitStr += "Objects: ";
            for (int i = 0; i < mObjectList.Count; ++i)
            {
                exitStr += ((Mob)mObjectList[i]).mName + "\n";
            }// for
            for (int i = 0; i < mNpcList.Count; ++i)
            {
                exitStr += ((Mob)mNpcList[i]).mName + "\n";
            }// for
            exitStr += "Players: ";
            for (int i = 0; i < mPlayerList.Count; ++i)
            {
                exitStr += ((Player)mPlayerList[i]).mName + "\n";
            }// for
            exitStr += "\n";
            return exitStr;
        }// exitString

        // TODO
        // See if you need this or if it is better to split it into two functions
        public void addObject(Mob mob)
        {
            // Remove old references
            if (mob.mCurrentRoom != null && mob.mCurrentArea != null)
            {
                mob.mCurrentArea.mObjectList.Remove(mob);
                mob.mCurrentRoom.mObjectList.Remove(mob);
            }
            
            // Add new references
            mob.mCurrentArea = mCurrentArea;
            mob.mCurrentRoom = this;
            mob.mCurrentArea.mObjectList.Add(mob);
            mObjectList.Add(mob);
        }// addObject

    }// Class Room

}// Namespace _8th_Circle_Server
