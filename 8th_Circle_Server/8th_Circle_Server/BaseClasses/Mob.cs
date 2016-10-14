﻿using System;
using System.Collections.Generic;
using System.Collections;

namespace _8th_Circle_Server
{
    public class Mob
    {
        public MobFlags mFlags;

        protected string mName;
        protected ResType mResType;
        protected string mExitStr;
        protected string mShortDescription;
        protected string mDescription;
        protected World mWorld;
        protected Room mStartingRoom;
        protected Room mCurrentRoom;
        protected Area mStartingArea;
        protected Area mCurrentArea;
        protected Mob mStartingOwner;
        protected Mob mCurrentOwner;
        protected List<PrepositionType> mPrepList;
        protected List<Mob> mInventory;
        protected List<EventData> mEventList;
        protected List<Mob> mChildren;
        protected Mob mParent;
        protected MobList mMobId;
        protected int mKeyId;
        protected int mStartingRespawnTime;
        protected int mCurrentRespawnTime;
        protected bool mIsRespawning;
        protected int mActionTimer;
        protected int mStartingActionCounter;
        protected int mCurrentActionCounter;

        private Random mRand;
        private int[] mAreaLoc;

        public Mob()
        {
            mName = mDescription = mShortDescription = mExitStr = string.Empty;
            mAreaLoc = new int[3];
            mInventory = new List<Mob>();
            mPrepList = new List<PrepositionType>();
            mPrepList.Add(PrepositionType.PREP_AT);
            mEventList = new List<EventData>();
            mChildren = new List<Mob>();
            mInventory.Capacity = 20;
            mStartingRoom = mCurrentRoom = null;
            mStartingArea = mCurrentArea = null;
            mStartingOwner = mCurrentOwner = null;
            mStartingRespawnTime = mCurrentRespawnTime = 25;
            mMobId = MobList.MOB_START;
            mKeyId = (int)MobList.MOB_START;
            mActionTimer = 0;
            mStartingActionCounter = mCurrentActionCounter = 30;
            mRand = new Random();
            mFlags = MobFlags.NONE;
        }// Constructor

        public Mob(string name, MobFlags flags = MobFlags.NONE)
        {
            mName = mExitStr = name;
            mDescription = mShortDescription = string.Empty;
            mAreaLoc = new int[3];
            mInventory = new List<Mob>();
            mPrepList = new List<PrepositionType>();
            mPrepList.Add(PrepositionType.PREP_AT);
            mEventList = new List<EventData>();
            mChildren = new List<Mob>();
            mInventory.Capacity = 20;
            mStartingRespawnTime = mCurrentRespawnTime = 15;
            mStartingRoom = mCurrentRoom = null;
            mStartingArea = mCurrentArea = null;
            mStartingOwner = mCurrentOwner = null;
            mMobId = MobList.MOB_START;
            mKeyId = (int)MobList.MOB_START;
            mActionTimer = 0;
            mStartingActionCounter = mCurrentActionCounter = 30;
            mRand = new Random();
            mFlags = flags;
        }// Constructor

        public Mob(Mob mob)
        {
            mName = mob.mName;
            mExitStr = mob.mName;
            mDescription = mob.mDescription;
            mShortDescription = mob.mShortDescription;
            mWorld = mob.mWorld;
            mAreaLoc = mob.mAreaLoc;
            mInventory = new List<Mob>(mob.mInventory);
            mPrepList = new List<PrepositionType>(mob.mPrepList);
            mEventList = new List<EventData>();
            mEventList = new List<EventData>(mob.mEventList);
            mChildren = new List<Mob>(mob.mChildren);
            mParent = mob;
            mInventory.Capacity = mob.mInventory.Capacity;
            mStartingRespawnTime = mob.mStartingRespawnTime;
            mCurrentRespawnTime = mStartingRespawnTime;
            mStartingRoom = mob.mStartingRoom;
            mCurrentRoom = mob.mCurrentRoom;;
            mStartingArea = mob.mStartingArea;
            mCurrentArea = mob.mCurrentArea;
            mStartingOwner = mob.mStartingOwner;
            mCurrentOwner = mob.mCurrentOwner;
            mMobId = mob.mMobId;
            mKeyId = mob.mKeyId;
            mActionTimer = mob.mActionTimer;
            mResType = mob.mResType;
            mStartingActionCounter = mob.mStartingActionCounter;
            mCurrentActionCounter = mob.mCurrentActionCounter;
            mRand = new Random();
            mFlags = mob.mFlags;
        }// Copy Constructor

        public virtual Mob Clone()
        {
            return new Mob(this);
        }

        public virtual Mob Clone(string name)
        {
            return new Mob(name);
        }

        public string move(string direction)
        {
            string clientString = string.Empty;

            if (HasFlag(MobFlags.INCOMBAT))
                return "you can't move while in combat\n";

            Direction dir = DirStrToEnum(direction);

            if (mCurrentRoom.GetRoomLinks()[(int)dir] != null &&
               (mCurrentRoom.getRes(ResType.DOORWAY)[(int)dir] == null ||
               ((Doorway)mCurrentRoom.getRes(ResType.DOORWAY)[(int)dir]).IsOpen()))
            { 
                clientString = changeRoom(mCurrentRoom.GetRoomLinks()[(int)dir]);
            }
            else
                return "you can't move that way\n";

            return clientString;
        }// move

        public string changeRoom(Room newRoom)
        {
            if (HasFlag(MobFlags.INCOMBAT))
                return "you can't do that while in combat\n";

            // Remove old references
            mCurrentRoom.removeRes(this);

            if (mCurrentArea != newRoom.GetCurrentArea())
            {
                mCurrentArea.removeRes(this);
                newRoom.GetCurrentArea().addRes(this);
                mCurrentArea = newRoom.GetCurrentArea();
            }// if

            // Add new references
            newRoom.addRes(this);
            mAreaLoc = newRoom.GetAreaLoc();
            mCurrentRoom = newRoom;

            return mCurrentRoom.exitString();
        }// changeRoom

        public virtual string used()
        {
            return string.Empty;
        }// used

        public virtual string viewed(Mob viewer, Preposition prep)
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";

            if (prep.prepType == PrepositionType.PREP_AT &&
                mPrepList.Contains(PrepositionType.PREP_AT))
            {
                return mDescription;
            }
            else
                return "You can't look like that\n";
        }// viewed

        public virtual string get(Mob mob)
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";

            if (HasFlag(MobFlags.GETTABLE) &&
                mCurrentRoom.getRes(ResType.OBJECT).Contains(this))
            {
                if (mob.mInventory.Count < mob.mInventory.Capacity)
                {
                    if (HasFlag(MobFlags.DUPLICATABLE))
                    {
                        Mob dup = new Mob(this);
                        mCurrentOwner = mob;
                        mob.mInventory.Add(this);

                        return "you get " + exitString(mCurrentRoom) + "\n";
                    }
                    else
                    {                    
                        if (!HasFlag(MobFlags.HIDDEN))
                        {
                            mob.mWorld.totallyRemoveRes(this);
                            mCurrentOwner = mob;
                            mob.mInventory.Add(this);

                            if (mParent != null)
                            {
                                mParent.mChildren.Remove(this);
                                mParent.mIsRespawning = true;
                            }

                            return "you get " + exitString(mCurrentRoom) + "\n";
                        }
                        else
                            return "you can't do that\n";
                    }
                }// if
                else
                {
                    return "your inventory is full\n";
                }// else
            }// if
            else
                return "you can't get that\n";
                
        }// get

        public virtual string get(Mob mob, PrepositionType prepType, Container container)
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";

            if (HasFlag(MobFlags.GETTABLE))
            {
                if (mob.mInventory.Count < mob.mInventory.Capacity)
                {
                    if (HasFlag(MobFlags.DUPLICATABLE))
                    {
                        Mob dup = new Mob(this);
                        mCurrentOwner = mob;
                        mob.mInventory.Add(this);

                        return "you get " + exitString(mCurrentRoom) + "\n";
                    }
                    else if (container.HasFlag(MobFlags.OPENABLE) && container.IsOpen())
                    {
                        if (prepType == PrepositionType.PREP_FROM)
                        {
                            if (container.mInventory.Contains(this))
                            {
                                mob.mWorld.totallyRemoveRes(this);
                                container.mInventory.Remove(this);
                                mCurrentOwner = mob;
                                mob.mInventory.Add(this);

                                if (mParent != null)
                                {
                                    mParent.mChildren.Remove(this);
                                    mParent.mIsRespawning = true;
                                }
                                
                                return "you get " + exitString(mCurrentRoom) + "\n";
                            }// if
                            else
                                return container.mName + " does not contain a " + this.mName + "\n";
                        }// if (prepType == PrepositionType.PREP_FROM)
                        else
                            return "you can't get like that\n";
                    }// else if (container.HasFlag(MobFlags.OPENABLE) && container.IsOpen())
                    else
                        return container.mName + " is closed\n";
                }// if (mob.mInventory.Count < mob.mInventory.Capacity)
                else
                    return "your inventory is full\n";
            }// if (HasFlag(MobFlags.GETTABLE))
            else
                return "you can't get that\n";

        }// get

        public virtual string getall()
        {
            List<Mob> targetList = mCurrentRoom.getRes(ResType.OBJECT);
            string clientString = string.Empty;

            if (targetList.Count == 0)
                return "you can't do that";

            CommandExecuter cmdExecuter = GetCurrentArea().GetCommandExecutor();
            CommandClass get = cmdExecuter.GetCCDict()[Utils.createTuple(CommandName.COMMAND_GET, 2)];
            ArrayList ccList = new ArrayList();
            ccList.Add(get);
         
            int index = 0;
            int originalSize = targetList.Count;

            for (int loopCount = 0; loopCount < originalSize; ++loopCount)
            {
                int sizeBeforeGet = targetList.Count;
                Mob target = targetList[index];
                ccList.Add(target);

                // We could ignore most of this and simply call get directly on each object, but that would
                // bypass our event system because the main command is getall, so if any event is triggered
                // off of get and the command is getall, then it would not trigger, this way, it actually
                // is doing the real get command through the executer so it will trigger the event.
                clientString += cmdExecuter.execute(get, ccList, this);
                int sizeAfterGet = targetList.Count;

                // If the size is the same before and after, that means we didn't successfully get the object
                // I.E. we didn't have the right permission/flag whatever, that means we need to skip trying 
                // to get this object again by incrementing the index of the next get.
                if (sizeAfterGet == sizeBeforeGet)
                    ++index;

                ccList.Remove(target);
            }

            return clientString;
        }// getall

        public virtual string getall(PrepositionType prepType, Container container)
        {
            string clientString = string.Empty;
            List<Mob> containerInv = container.mInventory;

            if (container.HasFlag(MobFlags.HIDDEN))
                return clientString;
            if (!container.IsOpen())
                return "the " + container.GetName() + " is closed.";
            if (containerInv.Count == 0)
                return "there is nothing to get.";

            while(containerInv.Count > 0)
            {
                if (mInventory.Count < mInventory.Capacity)
                {
                    // Assumes that each item is gettable, if we were to make an item that isn't gettable unless certain
                    // circumstances are met (like the sword in the stone type of thing), this will need to be reworked.
                    clientString += containerInv[0].get(this, prepType, container);
                }
                else
                    break;
            }

            return clientString;
        }// getall

        public virtual string drop(Mob mob)
        {
            if (HasFlag(MobFlags.DROPPABLE))
            {  
                mob.mInventory.Remove(this);
                mCurrentRoom = mob.mCurrentRoom;
                mCurrentOwner = null;
                mCurrentRoom.addMobResource(this);

                if (mParent != null)
                {
                    mParent.mChildren.Add(this);
                    mParent.mIsRespawning = true;
                }

                return "you drop " + exitString(mCurrentRoom) + "\n";
            }// if
            else
                return "you can't drop that\n";
        }// drop

        public virtual string dropall()
        {
            string clientString = string.Empty;
            int tmpInvCount = 0;

            for (int i = 0; i < mInventory.Count; ++i)
            {
                tmpInvCount = mInventory.Count;
                clientString += mInventory[i].drop(this);

                if (tmpInvCount != mInventory.Count)
                    --i;
            }

            return clientString;
        }// dropall

        public virtual string open(Mob mob)
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";

            return "You can't open that\n";
        }// open

        public virtual string close(Mob mob)
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";

            return "You can't close that\n";
        }// close

        public virtual string lck(Mob mob)
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";

            return "You can't lock that\n";
        }// lck

        public virtual string unlock(Mob mob)
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";

            return "You can't unlock that\n";
        }// unlock

        public virtual string use(Mob mob)
        {
            // The actual processing of the event will be handled by checkEvent at the
            // end of command processing
            if (HasFlag(MobFlags.USEABLE) &&
                mEventList.Count > 0)
            {
                return "You use the " + mName + "\n";
            }
            else
                return "You can't use that\n";
        }// unlock

        public virtual string destroy()
        {
            if(this is CombatMob)
            {
                CombatMob target = (CombatMob)this;
                CombatHandler combatHandler = GetWorld().GetCombatHandler();
                combatHandler.endCombat(target);
            }

            mCurrentOwner = null;
            mInventory.Clear();
            mEventList.Clear(); 
            mWorld.totallyRemoveRes(this);

            if (mParent != null)
            {
                mParent.mChildren.Remove(this);
                mParent.mIsRespawning = true;
            }
            
            return "destroying " + mName + "\n";
        }// destroy

        public virtual void respawn()
        {
            mIsRespawning = false;
            mCurrentRespawnTime = mStartingRespawnTime;
            Mob mob = Clone();
            mChildren.Add(mob);
            mob.mCurrentArea.addRes(mob);
            mob.mCurrentRoom.addRes(mob);
            mob.mWorld.addRes(mob);
        }// respawn

        public virtual string exitString(Room currentRoom)
        {
            return mName;
        }// exitString

        public virtual string lck()
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";

            return "you can't lock that\n";
        }// lck

        public virtual string unlock()
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";

            return "you can't unlock that\n";
        }// unlock

        public virtual string fullheal()
        {
            if (mFlags.HasFlag(MobFlags.HIDDEN))
                return "you can't do that\n";

            return "you can't fullheal that\n";
        }// fullheal

        public virtual string wear(CombatMob mob)
        {
            return "you can't wear that\n";
        }// wear

        public virtual string wearall()
        {
            return "you can't wear that\n";
        }// wearall

        public virtual string remove(CombatMob mob)
        {
            return "you can't remove that\n";
        }// wear

        public virtual string removeall()
        {
            return "you can't remove that\n";
        }// wearall

        public virtual string teleport(Mob target)
        {
            return changeRoom(target.mCurrentRoom);
        }// teleport

        public virtual string search()
        {
            string searchString = string.Empty;
            List<List<Mob>> targetList = new List<List<Mob>>();
            targetList.Add(mCurrentRoom.getRes(ResType.OBJECT));
            targetList.Add(mCurrentRoom.getRes(ResType.PLAYER));
            targetList.Add(mCurrentRoom.getRes(ResType.NPC));
            targetList.Add(mCurrentRoom.getRes(ResType.DOORWAY));
            bool found = false;

            foreach (List<Mob> ar in targetList)
            {
                foreach (Mob mob in ar)
                {
                    if(mob != null && mob.HasFlag(MobFlags.HIDDEN))
                    {
                        searchString += "you discover a " + mob.mName;
                        Utils.UnsetFlag(ref mob.mFlags, MobFlags.HIDDEN);
                        found = true;
                    }// if
                }// foreach
            }// foreach

            if (!found)
                searchString = "you find nothing";

            Utils.UnsetFlag(ref mFlags, MobFlags.SEARCHING);

            return searchString + "\n";
        }// search

        public static Direction DirStrToEnum(string dirStr)
        {
            switch (dirStr)
            {
                case "north":
                    return Direction.NORTH;
                case "south":
                    return Direction.SOUTH;
                case "east":
                    return Direction.EAST;
                case "west":
                    return Direction.WEST;
                case "up":
                    return Direction.UP;
                case "down":
                    return Direction.DOWN;
                case "northwest":
                    return Direction.NORTHWEST;
                case "northeast":
                    return Direction.NORTHEAST;
                case "southwest":
                    return Direction.SOUTHWEST;
                case "southeast":
                    return Direction.SOUTHEAST;

                default:
                    return Direction.DIRECTION_END;
            }// switch
        }// DirStrToEnum

        // TODO
        // Needs to be more generic
        public void randomAction()
        {
            if (HasFlag(MobFlags.INCOMBAT))
                return;

            if (mRand.NextDouble() < .5)
            {
                // Max movement
                if (mMobId == MobList.MAX)
                {
                    Utils.broadcast(mCurrentRoom, this, mName + " purrs softly\n");

                    ArrayList commandQueue = new ArrayList();
                    CommandClass com = mCurrentArea.GetCommandExecutor().GetCCDict()[Utils.createTuple(CommandName.COMMAND_TELL, 256)];

                    foreach (CombatMob pl in mCurrentArea.getRes(ResType.PLAYER))
                    {
                        commandQueue.Add(com);
                        commandQueue.Add(pl);
                        commandQueue.Add("purrr");
                        mCurrentArea.GetCommandExecutor().execute(com, commandQueue, this);
                        commandQueue.Clear();
                    }
                }// if (mMobId == (int)MobList.MAX)
            }// if
            else
            {
                ArrayList commandQueue = new ArrayList();
                addExits(commandQueue);

                if (commandQueue.Count > 0)
                {
                    Utils.broadcast(mCurrentRoom, this, mName + " scampers off\n");

                    int index = (int)(commandQueue.Count * mRand.NextDouble());
                    CommandClass com = (CommandClass)commandQueue[index];
                    mCurrentArea.GetCommandExecutor().execute(com, commandQueue, this);
                }// if
                else
                { // There must be no exits in the room the CombatMob is trying to leave, so just stay put
                }
            }// else
        }// randomAction

        private void addExits(ArrayList commandQueue)
        {
            Dictionary<Tuple<CommandName, int>, CommandClass> commandDict = mCurrentArea.GetCommandExecutor().GetCCDict();
            Dictionary<Direction, CommandClass> directionalCommands = new Dictionary<Direction, CommandClass>();
            directionalCommands.Add(Direction.UP, commandDict[Utils.createTuple(CommandName.COMMAND_UP, 1)]);
            directionalCommands.Add(Direction.NORTH, commandDict[Utils.createTuple(CommandName.COMMAND_NORTH, 1)]);
            directionalCommands.Add(Direction.NORTHEAST, commandDict[Utils.createTuple(CommandName.COMMAND_NORTHEAST, 1)]);
            directionalCommands.Add(Direction.EAST, commandDict[Utils.createTuple(CommandName.COMMAND_EAST, 1)]);
            directionalCommands.Add(Direction.SOUTHEAST, commandDict[Utils.createTuple(CommandName.COMMAND_SOUTHEAST, 1)]);
            directionalCommands.Add(Direction.DOWN, commandDict[Utils.createTuple(CommandName.COMMAND_DOWN, 1)]);
            directionalCommands.Add(Direction.SOUTH, commandDict[Utils.createTuple(CommandName.COMMAND_SOUTH, 1)]);
            directionalCommands.Add(Direction.SOUTHWEST, commandDict[Utils.createTuple(CommandName.COMMAND_SOUTHWEST, 1)]);
            directionalCommands.Add(Direction.WEST, commandDict[Utils.createTuple(CommandName.COMMAND_WEST, 1)]);
            directionalCommands.Add(Direction.NORTHWEST, commandDict[Utils.createTuple(CommandName.COMMAND_UP, 1)]);

            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                if (mCurrentRoom.GetRoomLinks()[(int)dir] != null &&
                   (mCurrentRoom.getRes(ResType.DOORWAY)[(int)dir] == null ||
                   ((Doorway)mCurrentRoom.getRes(ResType.DOORWAY)[(int)dir]).IsOpen()))
                {
                    commandQueue.Add(directionalCommands[dir]);
                }// if
            }// for
        }// addExits

        public virtual string playerString()
        {
            return "";
        }

        public virtual void safeWrite(string clientString)
        {
            // not implemented
        }// safeWrite

        // Accessors
        public string GetName() { return mName; }
        public void SetName(string name) { mName = name; }
        public ResType GetResType() { return mResType; }
        public void SetResType(ResType resType) { mResType = resType; }
        public string GetDesc() { return mDescription; }
        public void SetDesc(string desc) { mDescription = desc; }
        public World GetWorld() { return mWorld; }
        public void SetWorld(World world) { mWorld = world; }
        public void SetAreaLoc(int index, int val) { mAreaLoc[index] = val; }
        public Room GetStartingRoom() { return mStartingRoom; }
        public void SetStartingRoom(Room room) { mStartingRoom = room; }
        public Room GetCurrentRoom() { return mCurrentRoom; }
        public void SetCurrentRoom(Room room) { mCurrentRoom = room; }
        public Area GetCurrentArea() { return mCurrentArea; }
        public void SetCurrentArea(Area area) { mCurrentArea = area; }
        public Area GetStartingArea() { return mStartingArea; }
        public void SetStartingArea(Area area) { mStartingArea = area; }
        public List<Mob> GetInv() { return mInventory; }
        public List<EventData> GetEventList() { return mEventList; }
        public List<Mob> GetChildren() { return mChildren; }
        public Mob GetParent() { return mParent; }
        public void SetParent(Mob parent) { mParent = parent; }
        public MobList GetMobID() { return mMobId; }
        public void SetMobID(MobList mobID) { mMobId = mobID; }
        public int GetKeyID() { return mKeyId; }
        public void SetKeyID(int keyID) { mKeyId = keyID; }
        public int GetStartingRespawnTime() { return mStartingRespawnTime; }
        public void SetStartingRespawnTime(int time) { mStartingRespawnTime = time; }
        public int GetCurrentRespawnTime() { return mCurrentRespawnTime; }
        public void SetCurrentRespawnTime(int time) { mCurrentRespawnTime = time; }
        public int DecRespawnTime(int time) { return (mCurrentRespawnTime = mCurrentRespawnTime - time); }
        public bool IsRespawning() { return mIsRespawning; }
        public void SetIsRespawning(bool respawning) { mIsRespawning = respawning; }
        public int GetActionTimer() { return mActionTimer; }
        public void SetActionTimer(int time) { mActionTimer = time; }
        public int ModifyActionTimer(int time) { return (mActionTimer = mActionTimer + time); }
        public int GetCurrentActionTimer() { return mCurrentActionCounter; }
        public void SetCurrentActionTimer(int time) { mCurrentActionCounter = time; }
        public int DecCurrentActionTimer(int time) { return (mCurrentActionCounter = mCurrentActionCounter - time); }
        public int GetStartingActionTimer() { return mStartingActionCounter; }
        public MobFlags GetFlags() { return mFlags; }
        public bool HasFlag(Enum flag) { return mFlags.HasFlag(flag); }
        public virtual Dictionary<EQSlot, Mob> GetEQList() { return null; }

    }// Class Mob

}// Namespace _8th_Circle_Server
