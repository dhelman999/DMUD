using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum AreaID
    {
        AID_PROTOAREA,
        AID_GERALDINEMANOR,
        AID_NEWBIEAREA
    }// AreaID

    class Area
    {
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
        public CommandExecuter mCommandExecuter;
        public AreaID mAreaID;

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
            mCommandExecuter = new CommandExecuter();
            mAreaID = 0;
        }// Constructor

        public Room getRoom(RoomID roomID)
        {
            foreach(Room room in mRoomList)
            {
                if (room.mRoomID == roomID)
                    return room;
            }// foreach

            return null;
        }// getRoom

    }// Class Area

}// Namespace _8th_Circle_Server
