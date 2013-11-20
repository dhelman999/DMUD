using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Area
    {
        enum connectionDirection
        {
            DIRECTION_NORTH=0,
            DIRECTION_SOUTH,
            DIRECTION_EAST,
            DIRECTION_WEST,
            DIRECTION_UP,
            DIRECTION_DOWN,
            DIRECTION_NORTHWEST,
            DIRECTION_NORTHEAST,
            DIRECTION_SOUTHWEST,
            DIRECTION_SOUTHEAST
        };

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
        public World mWorld;
        public ArrayList mConnectionList;
        public string mName;
        public string mDescription;

        public Area()
        {
            mAreaOffset = 0;
            mRoomList = new ArrayList();
            mPlayerList = new ArrayList();
            mWorld = null;
            mConnectionList = new ArrayList();
            mName = string.Empty;
            mDescription = string.Empty;
        }// Constructor

    }// Class Area

}// Namespace _8th_Circle_Server
