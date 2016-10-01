using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    

    public class Area : ResourceHandler
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public int mAreaOffset;
        public int mStartingRespawnTimer;
        public int mCurrentRespawnTimer;
        public List<Room> mRoomList;
        public List<Mob> mFullMobList;
        public List<EventData> mRevertList;
        public World mWorld;
        public CommandExecuter mCommandExecuter;
        public AreaID mAreaID;

        public Area(World world) : base()
        {
            mAreaOffset = 0;
            mStartingRespawnTimer = mCurrentRespawnTimer = 300;
            mRoomList = new List<Room>();
            mFullMobList = new List<Mob>();
            mRevertList = new List<EventData>();
            mWorld = world;
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
