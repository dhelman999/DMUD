using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public enum AreaID
    {
        AID_PROTOAREA,
        AID_START = AID_PROTOAREA,
        AID_GERALDINEMANOR,
        AID_NEWBIEAREA,
        AID_END
    }// AreaID

    public class Area : ResourceHandler
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public int mAreaOffset;
        public ArrayList mRoomList;
        public ArrayList mFullMobList;
        public int mStartingRespawnTimer;
        public int mCurrentRespawnTimer;
        public World mWorld;
        public CommandExecuter mCommandExecuter;
        public AreaID mAreaID;
        public ArrayList mRevertList;

        public Area() : base()
        {
            mAreaOffset = 0;
            mStartingRespawnTimer = mCurrentRespawnTimer = 30;
            mRoomList = new ArrayList();
            mFullMobList = new ArrayList();
            mRevertList = new ArrayList();
            mWorld = null;
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
