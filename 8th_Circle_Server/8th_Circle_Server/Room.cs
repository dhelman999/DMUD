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
        internal const bool DEBUG = true;

        // Member Variables
        public string mDescription;
        public Room mNorthLink;
        public Room mSouthLink;
        public Room mEastLink;
        public Room mWestLink;
        public Room mUpLink;
        public Room mDownLink;
        public int mXPos;
        public int mYPos;
        public int mZPos;
        public ArrayList mPlayerList;
        public ArrayList mNpcList;

        public Room()
        {
            mDescription = string.Empty;
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
            mNorthLink = mSouthLink = mEastLink = mWestLink = mUpLink = mDownLink = null;
        }// Constructor

        public Room(string desc)
        {
            mDescription = desc;
            mNorthLink = mSouthLink = mEastLink = mWestLink = mUpLink = mDownLink = null;
            mXPos = mYPos = mZPos = -1;
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
        }// Constructor

        public Room(string desc, int xCoord, int yCoord, int zCoord)
        {
            mDescription = desc;
            mNorthLink = mSouthLink = mEastLink = mWestLink = mUpLink = mDownLink = null;
            mZPos = zCoord;
            mYPos = yCoord;
            mXPos = xCoord;
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
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

            exitStr += "\n";
            for (int i = 0; i < mNpcList.Count; i++)
            {
                exitStr += ((Mob)mNpcList[i]).mName + " ";
            }// for
            exitStr += "Players: ";
            for (int i = 0; i < mPlayerList.Count; i++)
            {
                exitStr += ((Mob)mPlayerList[i]).mName + " ";
            }// for
            exitStr += "\n";
            return exitStr;
        }// exitString

    }// Class Room

}// Namespace _8th_Circle_Server
