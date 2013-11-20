﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class World
    {
        // Debug
        internal const bool DEBUG = false;
        
        // Constants
        const int MAXXSIZE = 3;
        const int MAXYSIZE = 3;
        const int MAXZSIZE = 3;
        
        // Member Variables
        public ArrayList mPlayerList;
        private Room[, ,] mWorldGrid;

        public World()
        {
            mPlayerList = new ArrayList();

            mWorldGrid = new Room[MAXZSIZE, MAXYSIZE, MAXXSIZE];
            mWorldGrid[0, 0, 0] = new Room("Room 0,0,0", 0, 0, 0);
            mWorldGrid[1, 0, 0] = new Room("Room 1,0,0", 1, 0, 0);
            mWorldGrid[2, 0, 0] = new Room("Room 2,0,0", 2, 0, 0);
            mWorldGrid[0, 1, 0] = new Room("Room 0,1,0", 0, 1, 0);
            mWorldGrid[1, 1, 0] = new Room("Room 1,1,0", 1, 1, 0);
            mWorldGrid[2, 1, 0] = new Room("Room 2,1,0", 2, 1, 0);
            mWorldGrid[0, 2, 0] = new Room("Room 0,2,0", 0, 2, 0);
            mWorldGrid[1, 2, 0] = new Room("Room 1,2,0", 1, 2, 0);
            mWorldGrid[2, 2, 0] = new Room("Room 2,2,0", 2, 2, 0);
            mWorldGrid[0, 0, 1] = new Room("Room 0,0,1", 0, 0, 1);
            mWorldGrid[1, 0, 1] = new Room("Room 1,0,1", 1, 0, 1);
            mWorldGrid[2, 0, 1] = new Room("Room 2,0,1", 2, 0, 1);
            mWorldGrid[0, 1, 1] = new Room("Room 0,1,1", 0, 1, 1);
            mWorldGrid[1, 1, 1] = new Room("Room 1,1,1", 1, 1, 1);
            mWorldGrid[2, 1, 1] = new Room("Room 2,1,1", 2, 1, 1);
            mWorldGrid[0, 2, 1] = new Room("Room 0,2,1", 0, 2, 1);
            mWorldGrid[1, 2, 1] = new Room("Room 1,2,1", 1, 2, 1);
            mWorldGrid[2, 2, 1] = new Room("Room 2,2,1", 2, 2, 1);
            mWorldGrid[0, 0, 2] = new Room("Room 0,0,2", 0, 0, 2);
            mWorldGrid[1, 0, 2] = new Room("Room 1,0,2", 1, 0, 2);
            mWorldGrid[2, 0, 2] = new Room("Room 2,0,2", 2, 0, 2);
            mWorldGrid[0, 1, 2] = new Room("Room 0,1,2", 0, 1, 2);
            mWorldGrid[1, 1, 2] = new Room("Room 1,1,2", 1, 1, 2);
            mWorldGrid[2, 1, 2] = new Room("Room 2,1,2", 2, 1, 2);
            mWorldGrid[0, 2, 2] = new Room("Room 0,2,2", 0, 2, 2);
            mWorldGrid[1, 2, 2] = new Room("Room 1,2,2", 1, 2, 2);
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

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        createSideLinks(getRoom(i, j, k));
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
            if (currentRoom.mWorldLoc[1] < 2)
                currentRoom.mNorthLink = mWorldGrid[currentRoom.mWorldLoc[0], currentRoom.mWorldLoc[1] + 1,
                    currentRoom.mWorldLoc[2]];
            // West Links
            if (currentRoom.mWorldLoc[0] > 0)
                currentRoom.mWestLink = mWorldGrid[currentRoom.mWorldLoc[0] - 1, currentRoom.mWorldLoc[1],
                    currentRoom.mWorldLoc[2]];
            // East Links
            if (currentRoom.mWorldLoc[0] < 2)
                currentRoom.mEastLink = mWorldGrid[currentRoom.mWorldLoc[0] + 1, currentRoom.mWorldLoc[1],
                    currentRoom.mWorldLoc[2]];
            // South Links
            if (currentRoom.mWorldLoc[1] > 0)
                currentRoom.mSouthLink = mWorldGrid[currentRoom.mWorldLoc[0], currentRoom.mWorldLoc[1] - 1,
                    currentRoom.mWorldLoc[2]];
            // Up/Down Links
            if (currentRoom.mWorldLoc[1] < 2)
            {
                if (currentRoom.mWorldLoc[0] == 1 &&
                    currentRoom.mWorldLoc[1] == 1 &&
                    currentRoom.mWorldLoc[2] == 1)
                {
                    currentRoom.mUpLink = mWorldGrid[currentRoom.mWorldLoc[0], currentRoom.mWorldLoc[1],
                        currentRoom.mWorldLoc[2] + 1];
                    currentRoom.mDownLink = mWorldGrid[currentRoom.mWorldLoc[0], currentRoom.mWorldLoc[1],
                        currentRoom.mWorldLoc[2] - 1];
                    currentRoom.mUpLink.mDownLink = currentRoom;
                    currentRoom.mDownLink.mUpLink = currentRoom;
                }// if
            }// if
        }// createLinks

        private void createSideLinks(Room currentRoom)
        {
            // Northwest/Southeast Links
            if (currentRoom.mNorthLink != null && currentRoom.mNorthLink.mWestLink != null)
            {
                currentRoom.mNorthwestLink = currentRoom.mNorthLink.mWestLink;
                currentRoom.mNorthwestLink.mSoutheastLink = currentRoom;
            }// if

            // Northeast/Southwest Links
            if (currentRoom.mNorthLink != null && currentRoom.mNorthLink.mEastLink != null)
            {
                currentRoom.mNortheastLink = currentRoom.mNorthLink.mEastLink;
                currentRoom.mNortheastLink.mSouthwestLink = currentRoom;
            }// if
        }// if
    }// Class World

}// Namespace _8th_Circle_Server
