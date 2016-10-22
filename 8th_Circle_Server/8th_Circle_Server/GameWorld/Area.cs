using System;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    // The gameworld has 1 world, and subdivided into areas which are further subdivided into rooms.  Areas hold resources that include 
    // npcs, doorways objects, and players.  Each area also has prototype managers to manage all the mobs they contain.
    public class Area : ResourceHandler
    {
        private World mWorld;
        private AreaID mAreaID;
        private PrototypeManager mProtoManager;

        // Holds all rooms contained in the area.
        private Dictionary<RoomID, Room> mRoomList;

        // Areas can respawn, but do so a little differently than mobs, this was probably a mistake in the original design because 
        // they need additional logic to understand what to do when an area respawns.
        private int mStartingRespawnTimer;
        private int mCurrentRespawnTimer;

        // Areas can hold events to revert when they respawn.
        private List<EventData> mRevertList;

        // Each area has its own command executor to facilitate parallelism.  This also eases the burdon of race conditions since areas have
        // different resources and different command executers they won't fight over shared resources as much and will run faster with 
        // multiple threads.
        private CommandExecuter mCommandExecuter; 

        public Area(World world, String name, AreaID areaID) : base()
        {
            mName = name;
            mStartingRespawnTimer = mCurrentRespawnTimer = 30;
            mRoomList = new Dictionary<RoomID, Room>();
            mRevertList = new List<EventData>();
            mWorld = world;
            mWorld.GetAreas().Add(this);
            mCommandExecuter = new CommandExecuter();
            mProtoManager = new PrototypeManager();
            mAreaID = areaID;
        }// Constructor

        public Mob cloneMob(MobList mobID, Room startingRoom, String name = "", Mob prototype = null)
        {
            return mProtoManager.cloneMob(mobID, startingRoom, name, prototype);
        }// cloneMob

        public void RegisterRoom(RoomID roomID, Room newRoom)
        {
            mRoomList.Add(roomID, newRoom);
        }// RegisterRoom

        // Properties
        public Room this[RoomID roomID]
        {
            get { return mRoomList[roomID]; }
        }

        // Accessors
        public World GetWorld() { return mWorld; }
        public List<Mob> GetPrototypeMobList() { return mProtoManager.GetPrototypeMobList(); }
        public void ResetRespawnTimer() { mCurrentRespawnTimer = mStartingRespawnTimer; }
        public int GetCurrentRespawnTimer() { return mCurrentRespawnTimer; }
        public int DecrementRespawnTimer(int time) { return mCurrentRespawnTimer -= time; }
        public Dictionary<RoomID, Room> GetRooms() { return mRoomList; }
        public AreaID GetAreaID() { return mAreaID; }
        public CommandExecuter GetCommandExecutor() { return mCommandExecuter; }
        public List<EventData> GetRevertEvents() { return mRevertList; }

    }// Class Area

}// Namespace _8th_Circle_Server
