using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class World
    {
        const int MAXXSIZE = 3;
        const int MAXYSIZE = 3;
        const int MAXZSIZE = 3;

        public ArrayList mPlayerList;
        private Room[, ,] mWorldGrid;

        public World()
        {
            mPlayerList = new ArrayList();

            mWorldGrid = new Room[MAXZSIZE, MAXYSIZE, MAXXSIZE];
            mWorldGrid[0, 0, 0] = new Room("Room 0,0,0", 0, 0, 0);
            mWorldGrid[0, 0, 1] = new Room("Room 0,0,1", 0, 0, 1);
            mWorldGrid[0, 0, 2] = new Room("Room 0,0,2", 0, 0, 2);
            mWorldGrid[0, 1, 0] = new Room("Room 0,1,0", 0, 1, 0);
            mWorldGrid[0, 1, 1] = new Room("Room 0,1,1", 0, 1, 1);
            mWorldGrid[0, 1, 2] = new Room("Room 0,1,2", 0, 1, 2);
            mWorldGrid[0, 2, 0] = new Room("Room 0,2,0", 0, 2, 0);
            mWorldGrid[0, 2, 1] = new Room("Room 0,2,1", 0, 2, 1);
            mWorldGrid[0, 2, 2] = new Room("Room 0,2,2", 0, 2, 2);
            mWorldGrid[1, 0, 0] = new Room("Room 1,0,0", 1, 0, 0);
            mWorldGrid[1, 0, 1] = new Room("Room 1,0,1", 1, 0, 1);
            mWorldGrid[1, 0, 2] = new Room("Room 1,0,2", 1, 0, 2);
            mWorldGrid[1, 1, 0] = new Room("Room 1,1,0", 1, 1, 0);
            mWorldGrid[1, 1, 1] = new Room("Room 1,1,1", 1, 1, 1);
            mWorldGrid[1, 1, 2] = new Room("Room 1,1,2", 1, 1, 2);
            mWorldGrid[1, 2, 0] = new Room("Room 1,2,0", 1, 2, 0);
            mWorldGrid[1, 2, 1] = new Room("Room 1,2,1", 1, 2, 1);
            mWorldGrid[1, 2, 2] = new Room("Room 1,2,2", 1, 2, 2);
            mWorldGrid[2, 0, 0] = new Room("Room 2,0,0", 2, 0, 0);
            mWorldGrid[2, 0, 1] = new Room("Room 2,0,1", 2, 0, 1);
            mWorldGrid[2, 0, 2] = new Room("Room 2,0,2", 2, 0, 2);
            mWorldGrid[2, 1, 0] = new Room("Room 2,1,0", 2, 1, 0);
            mWorldGrid[2, 1, 1] = new Room("Room 2,1,1", 2, 1, 1);
            mWorldGrid[2, 1, 2] = new Room("Room 2,1,2", 2, 1, 2);
            mWorldGrid[2, 2, 0] = new Room("Room 2,2,0", 2, 2, 0);
            mWorldGrid[2, 2, 1] = new Room("Room 2,2,1", 2, 2, 1);
            mWorldGrid[2, 2, 2] = new Room("Room 2,2,2", 2, 2, 2);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        createLinks(getRoom(i, j, k));
                    }// for
                }// for
            }// for
        }// Constructor


        public Room getRoom(int z, int y, int x)
        {
            return mWorldGrid[z, y, x];
        }// getRoom

        public void createLinks(Room currentRoom)
        {
            // North Links
            if (currentRoom.mYPos < 2)
                currentRoom.mNorthLink = mWorldGrid[currentRoom.mZPos, currentRoom.mYPos + 1, currentRoom.mXPos];
            // West Links
            if (currentRoom.mXPos > 0)
                currentRoom.mWestLink = mWorldGrid[currentRoom.mZPos, currentRoom.mYPos, currentRoom.mXPos - 1];
            // East Links
            if (currentRoom.mXPos < 2)
                currentRoom.mEastLink = mWorldGrid[currentRoom.mZPos, currentRoom.mYPos, currentRoom.mXPos + 1];
            // South Links
            if (currentRoom.mYPos > 0)
                currentRoom.mSouthLink = mWorldGrid[currentRoom.mZPos, currentRoom.mYPos - 1, currentRoom.mXPos];
            // Up/Down Links
            if (currentRoom.mYPos < 2)
            {
                if (currentRoom.mXPos == 1 &&
                    currentRoom.mYPos == 1 &&
                    currentRoom.mZPos == 1)
                {
                    currentRoom.mUpLink = mWorldGrid[currentRoom.mZPos + 1, currentRoom.mYPos, currentRoom.mXPos];
                    currentRoom.mDownLink = mWorldGrid[currentRoom.mZPos - 1, currentRoom.mYPos, currentRoom.mXPos];
                    currentRoom.mUpLink.mDownLink = currentRoom;
                    currentRoom.mDownLink.mUpLink = currentRoom;
                }// if
            }// if
        }// createLinks
    }// Class World
}// Namespace _8th_Circle_Server
