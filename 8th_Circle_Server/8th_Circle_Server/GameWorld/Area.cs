using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class Area : ResourceHandler
    {
        private int mAreaOffset;
        private int mStartingRespawnTimer;
        private int mCurrentRespawnTimer;
        private List<EventData> mRevertList;
        private CommandExecuter mCommandExecuter;
        private World mWorld;
        private AreaID mAreaID;
        private PrototypeManager mProtoManager;
        private Dictionary<RoomID, Room> mRoomList;

        public Area(World world, string name, AreaID areaID) : base()
        {
            mName = name;
            mAreaOffset = 0;
            mStartingRespawnTimer = mCurrentRespawnTimer = 30;
            mRoomList = new Dictionary<RoomID, Room>();
            mRevertList = new List<EventData>();
            mWorld = world;
            mWorld.GetAreas().Add(this);
            mCommandExecuter = new CommandExecuter();
            mProtoManager = new PrototypeManager();
            mAreaID = areaID;
        }// Constructor

        public Mob cloneMob(MobList mobID, Room startingRoom, string name = "", Mob prototype = null)
        {
            return mProtoManager.cloneMob(mobID, startingRoom, name, prototype);
        }// cloneMob

        public void RegisterRoom(RoomID roomID, Room newRoom)
        {
            mRoomList.Add(roomID, newRoom);
        }// RegisterRoom

        public void broadcast(string message)
        {
            foreach (CombatMob player in getRes(ResType.PLAYER))
                player.safeWrite(message);
        }// broadcast

        // Properties
        public Room this[RoomID roomID]
        {
            get { return mRoomList[roomID]; }
        }

        // Accessors
        public List<Mob> GetPrototypeMobList() { return mProtoManager.GetPrototypeMobList(); }
        public void ResetRespawnTimer() { mCurrentRespawnTimer = mStartingRespawnTimer; }
        public int GetAreaOffset() { return mAreaOffset; }
        public void SetAreaOffset(int offset) { mAreaOffset = offset; }
        public int GetCurrentRespawnTimer() { return mCurrentRespawnTimer; }
        public int DecrementRespawnTimer(int time) { return mCurrentRespawnTimer -= time; }
        public Dictionary<RoomID, Room> GetRooms() { return mRoomList; }
        public AreaID GetAreaID() { return mAreaID; }
        public CommandExecuter GetCommandExecutor() { return mCommandExecuter; }
        public List<EventData> GetRevertEvents() { return mRevertList; }

    }// Class Area

}// Namespace _8th_Circle_Server
