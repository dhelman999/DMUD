using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class Area : ResourceHandler
    {
        // Member Variables
        public int mAreaOffset;
        public int mStartingRespawnTimer;
        public int mCurrentRespawnTimer;
        public List<EventData> mRevertList;
        public World mWorld;
        public CommandExecuter mCommandExecuter;
        public AreaID mAreaID;
        public PrototypeManager mProtoManager;
        private Dictionary<RoomID, Room> mRoomList;

        public Area(World world, string name, AreaID areaID) : base()
        {
            mName = name;
            mAreaOffset = 0;
            mStartingRespawnTimer = mCurrentRespawnTimer = 30;
            mRoomList = new Dictionary<RoomID, Room>();
            mRevertList = new List<EventData>();
            mWorld = world;
            mWorld.mAreaList.Add(this);
            mCommandExecuter = new CommandExecuter();
            mProtoManager = new PrototypeManager();
            mAreaID = areaID;
        }// Constructor

        public Room this[RoomID roomID]
        {
            get
            {
                return mRoomList[roomID];
            }// Accessor
        }// [] Property

        public List<Mob> GetPrototypeMobList()
        {
            return mProtoManager.GetPrototypeMobList();
        }// GetPrototypeMobList

        public Mob cloneMob(MOBLIST mobID, Room startingRoom, string name = "", Mob prototype = null)
        {
            return mProtoManager.cloneMob(mobID, startingRoom, name, prototype);
        }// cloneMob

        public void RegisterRoom(RoomID roomID, Room newRoom)
        {
            mRoomList.Add(roomID, newRoom);
        }// RegisterRoom

        public Dictionary<RoomID, Room> GetRooms()
        {
            return mRoomList;
        }// GetRooms

    }// Class Area

}// Namespace _8th_Circle_Server
