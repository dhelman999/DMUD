using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Area
    {
        //TODO
        // replace with direction
        enum connectionDirection
        {
            NORTH=0,
            SOUTH,
            EAST,
            WEST,
            UP,
            DOWN,
            NORTHWEST,
            NORTHEAST,
            SOUTHWEST,
            SOUTHEAST
        };// connectionDirection

        struct ConnectionPoint
        {
            Room localRoom;
            Room remoteRoom;
            connectionDirection connectPoint;

            public ConnectionPoint(Room localRoom, Room remoteRoom,
                                   connectionDirection connectPoint)
            {
                this.localRoom = localRoom;
                this.remoteRoom = remoteRoom;
                this.connectPoint = connectPoint;
            }// Constructor
        }// struct ConnectionPoints

        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public int mAreaOffset;
        public ArrayList mRoomList;
        public ArrayList mPlayerList;
        public ArrayList mNpcList;
        public ArrayList mObjectList;
        public ArrayList mFullMobList;
        public int mStartingRespawnTimer;
        public int mCurrentRespawnTimer;
        public World mWorld;
        public ArrayList mConnectionList;
        public string mName;
        public string mDescription;

        public Area()
        {
            mAreaOffset = 0;
            mStartingRespawnTimer = mCurrentRespawnTimer = 30;
            mRoomList = new ArrayList();
            mPlayerList = new ArrayList();
            mNpcList = new ArrayList();
            mObjectList = new ArrayList();
            mFullMobList = new ArrayList();
            mWorld = null;
            mConnectionList = new ArrayList();
            mName = string.Empty;
            mDescription = string.Empty;
        }// Constructor

    }// Class Area

}// Namespace _8th_Circle_Server
