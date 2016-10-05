using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class Area : ResourceHandler
    {
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
        public PrototypeManager mProtoManager;

        public Area(World world) : base()
        {
            mAreaOffset = 0;
            mStartingRespawnTimer = mCurrentRespawnTimer = 30;
            mRoomList = new List<Room>();
            mFullMobList = new List<Mob>();
            mRevertList = new List<EventData>();
            mWorld = world;
            mCommandExecuter = new CommandExecuter();
            mProtoManager = new PrototypeManager();
            mAreaID = 0;
        }// Constructor

        // TODO
        // Make this a dictionary
        public Room getRoom(RoomID roomID)
        {
            foreach(Room room in mRoomList)
            {
                if (room.mRoomID == roomID)
                    return room;
            }// foreach

            return null;
        }// getRoom

        public List<Mob> GetPrototypeMobList()
        {
            return mProtoManager.GetPrototypeMobList();
        }// GetPrototypeMobList

        public Mob cloneMob(MOBLIST mobID, Room startingRoom, string name = "")
        {
            return mProtoManager.cloneMob(mobID, startingRoom, name);
        }// cloneMob

    }// Class Area

}// Namespace _8th_Circle_Server
