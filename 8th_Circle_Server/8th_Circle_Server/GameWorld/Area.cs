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
        public List<EventData> mRevertList;
        public World mWorld;
        public CommandExecuter mCommandExecuter;
        public AreaID mAreaID;
        public PrototypeManager mProtoManager;

        public Area(World world, string name, AreaID areaID) : base()
        {
            mName = name;
            mAreaOffset = 0;
            mStartingRespawnTimer = mCurrentRespawnTimer = 30;
            mRoomList = new List<Room>();
            mRevertList = new List<EventData>();
            mWorld = world;
            mWorld.mAreaList.Add(this);
            mCommandExecuter = new CommandExecuter();
            mProtoManager = new PrototypeManager();
            mAreaID = areaID;
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

        public Mob cloneMob(MOBLIST mobID, Room startingRoom, string name = "", Mob prototype = null)
        {
            return mProtoManager.cloneMob(mobID, startingRoom, name, prototype);
        }// cloneMob

    }// Class Area

}// Namespace _8th_Circle_Server
