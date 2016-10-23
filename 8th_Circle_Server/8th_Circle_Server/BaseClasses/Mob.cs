using System;
using System.Collections.Generic;
using System.Collections;

namespace _8th_Circle_Server
{
    // Base class for almost everything in the game besides locations.  Mob's generally implement various actions such as move,
    // lock unlock etc and inheritance is used for other mobs to go into more detail about what should be done for those actions.
    // Some actions like move are the same for all mobs, so the base class version is used, where as open might be different
    // between a doorway mob and a container mob.
    public class Mob
    {
        // MobFlags represent all 'attributes' of a Mob, such as hidden, in combat, open or closed, locked unlocked ect
        public MobFlags mFlags;

        // Standard things like their name, description, how they should appear in the exit strings, their resource type.
        protected String mName;
        protected ResType mResType;
        protected String mExitStr;
        protected String mShortDescription;
        protected String mDescription;
        protected World mWorld;

        // Starting and current variables.
        protected Room mStartingRoom;
        protected Room mCurrentRoom;
        protected Area mStartingArea;
        protected Area mCurrentArea;
        protected Mob mStartingOwner;
        protected Mob mCurrentOwner;
        protected int mStartingActionCounter;
        protected int mCurrentActionCounter;
        protected int mStartingRespawnTime;
        protected int mCurrentRespawnTime;

        // This is a global cooldown for the mob, if this is set, the mob cannot take any actions until its time expires
        protected int mActionTimer;

        // What prepisitions are applicable to this Mob.
        protected List<PrepositionType> mPrepList;

        // Mobs Inventory if any
        protected List<Mob> mInventory;

        // Events triggered by this mob
        protected List<EventData> mEventList;

        // Children and parent relationship goes into the prototype and respawn model, this is discussed in the PrototypeManger
        protected List<Mob> mChildren;
        protected Mob mParent;

        // Each mob has a game-wide identifier
        protected MobList mMobId;

        // Mobs can be opened or affected in special ways with other mobs that share their keyid
        protected int mKeyId;

        // Mementos help facilitate respawning and resetting a mob without duplicating a ton of starting/current member variables and mobflags
        protected Mob mMemento;

        // Random number generator if the Mob needs random actions
        private Random mRand;

        // Position in their current area
        private int[] mAreaLoc;

        public Mob()
        {
            mName = mDescription = mShortDescription = mExitStr = String.Empty;
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

        public Mob(String name, MobFlags flags = MobFlags.NONE)
        {
            mName = mExitStr = name;
            mDescription = mShortDescription = String.Empty;
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

        public virtual Mob Clone(String name)
        {
            return new Mob(name);
        }

        // // Basic used functionality, moves the mob 1 room in the specified direction.
        public errorCode move(String direction, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;
            int dir = Utils.DirStrToInt(direction);
            List<Mob> doorways = mCurrentRoom.getRes(ResType.DOORWAY);

            if (mCurrentRoom.GetRoomLinks()[dir] != null &&
               (doorways[dir] == null ||
               (doorways[dir]).HasFlag(MobFlags.OPEN)))
            { 
                eCode = changeRoom(mCurrentRoom.GetRoomLinks()[dir], ref clientString);
            }
            else
                clientString = "you can't move that way\n";

            return eCode;
        }// move

        // Changes the mobs room to a new room, basic teleporting, or spawning.
        public errorCode changeRoom(Room newRoom, ref String clientString)
        {
            Area newArea = newRoom.GetCurrentArea();

            // Remove old references
            if (mCurrentRoom != null && mCurrentRoom != newRoom)
                mCurrentRoom.removeRes(this);

            if (mCurrentArea != null && mCurrentArea != newArea)
                mCurrentArea.removeRes(this);

            if (mCurrentArea != newArea)
            {
                newArea.addRes(this);
                mCurrentArea = newArea;
            }

            // Add new references
            newRoom.addRes(this);
            mAreaLoc = newRoom.GetAreaLoc();
            mCurrentRoom = newRoom;

            clientString = mCurrentRoom.exitString();

            return errorCode.E_OK;
        }// changeRoom

        // Basic used functionality
        public virtual String used()
        {
            return String.Empty;
        }// used

        // Basic viewed functionality, returns its description
        public virtual errorCode viewed(Mob viewer, Preposition prep, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (HasFlag(MobFlags.HIDDEN))
            {
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

                return eCode;
            }   

            if (prep.prepType == PrepositionType.PREP_AT &&
                mPrepList.Contains(PrepositionType.PREP_AT))
            {
                clientString = mDescription;
                eCode = errorCode.E_OK;
            }
            else
                clientString = "You can't look like that\n";

            return eCode;
        }// viewed

        // Basic get functionality, adds the item to the mobs inventory
        public virtual errorCode get(Mob mob, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (HasFlag(MobFlags.HIDDEN))
            {
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

                return eCode;
            }       

            if (HasFlag(MobFlags.GETTABLE) && mCurrentRoom.getRes(ResType.OBJECT).Contains(this))
            {
                if (mob.mInventory.Count < mob.mInventory.Capacity)
                {
                    if (HasFlag(MobFlags.DUPLICATABLE))
                    {
                        Mob dup = new Mob(this);
                        mCurrentOwner = mob;
                        mob.mInventory.Add(this);

                        clientString += "you get " + exitString(mCurrentRoom) + "\n";
                        eCode = errorCode.E_OK;
                    }
                    else
                    {                    
                        if (!HasFlag(MobFlags.HIDDEN))
                        {
                            mWorld.totallyRemoveRes(this);
                            mCurrentOwner = mob;
                            mob.mInventory.Add(this);

                            if (mParent != null)
                            {
                                mParent.mChildren.Remove(this);
                                Utils.SetFlag(ref mParent.mFlags, MobFlags.RESPAWNING);
                            }

                            clientString += "you get " + exitString(mCurrentRoom) + "\n";
                            eCode = errorCode.E_OK;
                        }
                        else
                            clientString = GLOBALS.RESPONSE_CANT_DO_THAT;
                    }
                }// if (mob.mInventory.Count < mob.mInventory.Capacity)
                else
                {
                    clientString = "your inventory is full\n";
                }// else
            }// if (HasFlag(MobFlags.GETTABLE) && mCurrentRoom.getRes(ResType.OBJECT).Contains(this))
            else
                clientString = "you can't get that\n";

            return eCode;
        }// get

        // Get functionality if the mob is contained in another object
        public virtual errorCode get(Mob mob, PrepositionType prepType, Container container, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (HasFlag(MobFlags.HIDDEN))
            {
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

                return eCode;
            }     

            if (HasFlag(MobFlags.GETTABLE))
            {
                if (mob.mInventory.Count < mob.mInventory.Capacity)
                {
                    if (HasFlag(MobFlags.DUPLICATABLE))
                    {
                        Mob dup = new Mob(this);
                        mCurrentOwner = mob;
                        mob.mInventory.Add(this);

                        clientString += "you get " + exitString(mCurrentRoom) + "\n";
                        eCode = errorCode.E_OK;
                    }
                    else if (container.HasFlag(MobFlags.OPENABLE) && container.HasFlag(MobFlags.OPEN))
                    {
                        if (prepType == PrepositionType.PREP_FROM)
                        {
                            if (container.mInventory.Contains(this))
                            {
                                mWorld.totallyRemoveRes(this);
                                container.mInventory.Remove(this);
                                mCurrentOwner = mob;
                                mob.mInventory.Add(this);

                                if (mParent != null)
                                {
                                    mParent.mChildren.Remove(this);
                                    Utils.SetFlag(ref mParent.mFlags, MobFlags.RESPAWNING);
                                }
                                
                                clientString += "you get " + exitString(mCurrentRoom) + "\n";
                                eCode = errorCode.E_OK;
                            }// if (container.mInventory.Contains(this))
                            else
                                clientString = container.mName + " does not contain a " + this.mName + "\n";
                        }// if (prepType == PrepositionType.PREP_FROM)
                        else
                            clientString = "you can't get like that\n";
                    }// else if (container.HasFlag(MobFlags.OPENABLE) && container.IsOpen())
                    else
                        clientString = container.mName + " is closed\n";
                }// if (mob.mInventory.Count < mob.mInventory.Capacity)
                else
                    clientString = "your inventory is full\n";
            }// if (HasFlag(MobFlags.GETTABLE))
            else
                clientString = "you can't get that\n";

            return eCode;
        }// get

        // Basic getall functionality, attempts to add all mobs in the room to its inventory
        public virtual errorCode getall(ref String clientSString)
        {
            List<Mob> targetList = mCurrentRoom.getRes(ResType.OBJECT);
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (targetList.Count == 0)
            {
                clientSString = "you can't do that";

                return eCode;
            }      

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
                eCode = cmdExecuter.execute(get, ccList, this, ref clientSString);
                int sizeAfterGet = targetList.Count;

                // If the size is the same before and after, that means we didn't successfully get the object
                // I.E. we didn't have the right permission/flag whatever, that means we need to skip trying 
                // to get this object again by incrementing the index of the next get.
                if (sizeAfterGet == sizeBeforeGet)
                    ++index;

                ccList.Remove(target);
            }

            return eCode;
        }// getall

        // TODO
        // Implement this the same way as the basic getall, by creating a new command so it can trigger events.
        // Getall functionality if the object is in another object
        public virtual errorCode getall(PrepositionType prepType, Container container, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;
            List<Mob> containerInv = container.mInventory;

            if (container.HasFlag(MobFlags.HIDDEN))
                return eCode;
            if (!container.HasFlag(MobFlags.OPEN))
                clientString = "the " + container.GetName() + " is closed.";
            if (containerInv.Count == 0)
                clientString = "there is nothing to get.";

            while (containerInv.Count > 0)
            {
                if (mInventory.Count < mInventory.Capacity)
                {
                    // Assumes that each item is gettable, if we were to make an item that isn't gettable unless certain
                    // circumstances are met (like the sword in the stone type of thing), this will need to be reworked.
                    eCode = containerInv[0].get(this, prepType, container, ref clientString);
                }
                else
                    break;
            }

            return eCode;
        }// getall

        // Basic drop functionality, removes the mob from its inventory and adds it to the rooms inventory
        public virtual errorCode drop(Mob mob, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (HasFlag(MobFlags.DROPPABLE))
            {  
                mob.mInventory.Remove(this);
                mCurrentRoom = mob.mCurrentRoom;
                mCurrentOwner = null;
                mCurrentRoom.addMobResource(this);

                if (mParent != null)
                {
                    mParent.mChildren.Add(this);
                    Utils.SetFlag(ref mParent.mFlags, MobFlags.RESPAWNING);
                }

                clientString += "you drop " + exitString(mCurrentRoom) + "\n";
                eCode = errorCode.E_OK;
            }// if
            else
                clientString += "you can't drop that\n";

            return eCode;
        }// drop

        // TODO should be implemented like the basic getall, made into a command so it can trigger events
        // Basic dropall functionality
        public virtual errorCode dropall(ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;
            int tmpInvCount = 0;

            for (int i = 0; i < mInventory.Count; ++i)
            {
                tmpInvCount = mInventory.Count;
                eCode = mInventory[i].drop(this, ref clientString);

                if (tmpInvCount != mInventory.Count)
                    --i;
            }

            return eCode;
        }// dropall

        // Basic open functionality
        public virtual errorCode open(Mob mob, ref String clientString)
        {
            if (HasFlag(MobFlags.HIDDEN))
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

            clientString = "You can't open that\n";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// open

        // Basic close functionality
        public virtual errorCode close(Mob mob, ref String clientString)
        {
            if (HasFlag(MobFlags.HIDDEN))
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

            clientString = "You can't close that\n";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// close

        // Basic lock functionality
        public virtual errorCode lck(Mob mob, ref String clientString)
        {
            if (HasFlag(MobFlags.HIDDEN))
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

            clientString = "You can't lock that\n";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// lck

        // Basic unlock functionality
        public virtual errorCode unlock(Mob mob, ref String clientString)
        {
            if (HasFlag(MobFlags.HIDDEN))
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

            clientString = "You can't unlock that\n";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// unlock

        // Basic use functionality
        public virtual errorCode use(Mob mob, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            // The actual processing of the event will be handled by checkEvent at the end of command processing
            if (HasFlag(MobFlags.USEABLE) &&
                mEventList.Count > 0)
            {
                clientString = "You use the " + mName + "\n";
                eCode = errorCode.E_OK;
            }
            else
                clientString = "You can't use that\n";

            return eCode;
        }// unlock

        // Forcefully destroy a mob
        public virtual errorCode destroy(ref String clientString)
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
                Utils.SetFlag(ref mParent.mFlags, MobFlags.RESPAWNING);
            }
            
            clientString = "destroying " + mName + "\n";

            return errorCode.E_OK;
        }// destroy

        // Basic respawn functionality
        // Respawn occurs by a parent which will then clone itself and create more children and add them back into the world
        public virtual void respawn()
        {
            mCurrentRespawnTime = mStartingRespawnTime;
            Mob clone = Clone();
            mChildren.Add(clone);
            mWorld.totallyAddRes(clone);
        }// respawn

        public virtual String exitString(Room currentRoom)
        {
            return mName;
        }// exitString

        // Basic lock functionality
        public virtual errorCode lck(ref String clientString)
        {
            if (HasFlag(MobFlags.HIDDEN))
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

            clientString = "you can't lock that\n";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// lck

        // Basic unlock functionality
        public virtual errorCode unlock(ref String clientString)
        {
            if (HasFlag(MobFlags.HIDDEN))
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

            clientString = "you can't unlock that\n";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// unlock

        // Basic fullheal functionality
        public virtual errorCode fullheal(ref String clientString)
        {
            if (HasFlag(MobFlags.HIDDEN))
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

            clientString = "you can't fullheal that\n";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// fullheal

        // Basic wear functionality
        public virtual errorCode wear(CombatMob mob, ref String clientString)
        {
            clientString += "you can't wear the " + mName + "\n";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// wear

        // Basic wearall functionality
        public virtual errorCode wearall(ref String clientString)
        {
            clientString = "you can't wear that\n";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// wearall

        // Basic remove functionality
        public virtual errorCode remove(CombatMob mob, ref String clientString)
        {
            clientString = "you can't remove that\n";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// wear

        // Basic removeall functionality
        public virtual errorCode removeall(ref String clientString)
        {
            clientString = "you can't remove that\n";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// wearall

        // Teleports the mob to the specified room
        public virtual errorCode teleport(Mob target, ref String clientString)
        {
            return changeRoom(target.mCurrentRoom, ref clientString);
        }// teleport

        // Does a random action, currently pretty terrible as it can only move or tell, Also, it shouldn't be in the mob base class.
        public void randomAction()
        {
            String clientString = String.Empty;

            if (HasFlag(MobFlags.INCOMBAT))
                return;

            if (mRand.NextDouble() < .5)
            {
                // Max movement
                if (mMobId == MobList.MAX)
                {
                    Utils.Broadcast(mCurrentRoom, this, mName + " purrs softly\n");

                    ArrayList commandQueue = new ArrayList();
                    CommandClass com = mCurrentArea.GetCommandExecutor().GetCCDict()[Utils.createTuple(CommandName.COMMAND_TELL, 256)];

                    foreach (CombatMob pl in mCurrentArea.getRes(ResType.PLAYER))
                    {
                        commandQueue.Add(com);
                        commandQueue.Add(pl);
                        commandQueue.Add("purrr");
                        mCurrentArea.GetCommandExecutor().execute(com, commandQueue, this, ref clientString);
                        commandQueue.Clear();
                    }
                }// if (mMobId == (int)MobList.MAX)
            }// if (mRand.NextDouble() < .5)
            else
            {
                ArrayList commandQueue = new ArrayList();
                mCurrentRoom.addExits(commandQueue);

                if (commandQueue.Count > 0)
                {
                    Utils.Broadcast(mCurrentRoom, this, mName + " scampers off\n");

                    int index = (int)(commandQueue.Count * mRand.NextDouble());
                    CommandClass com = (CommandClass)commandQueue[index];
                    mCurrentArea.GetCommandExecutor().execute(com, commandQueue, this, ref clientString);
                }
                else
                { // There must be no exits in the room the CombatMob is trying to leave, so just stay put
                }
            }// else
        }// randomAction

        // Basic playerString
        public virtual String playerString()
        {
            return "";
        }

        // Basic safeWrite
        public virtual void safeWrite(String clientString)
        {
            // intentionally not implemented
        }// safeWrite

        // Accessors
        public String GetName() { return mName; }
        public void SetName(String name) { mName = name; }
        public ResType GetResType() { return mResType; }
        public void SetResType(ResType resType) { mResType = resType; }
        public String GetDesc() { return mDescription; }
        public void SetDesc(String desc) { mDescription = desc; }
        public World GetWorld() { return mWorld; }
        public void SetWorld(World world) { mWorld = world; }
        public void SetAreaLoc(int index, int val) { mAreaLoc[index] = val; }
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
        public void SetMobID(MobList mobID) { mMobId = mobID; }
        public int GetKeyID() { return mKeyId; }
        public void SetKeyID(int keyID) { mKeyId = keyID; }
        public int GetStartingRespawnTime() { return mStartingRespawnTime; }
        public void SetStartingRespawnTime(int time) { mStartingRespawnTime = time; }
        public void SetCurrentRespawnTime(int time) { mCurrentRespawnTime = time; }
        public int DecRespawnTime(int time) { return (mCurrentRespawnTime = mCurrentRespawnTime - time); }
        public int GetActionTimer() { return mActionTimer; }
        public void SetActionTimer(int time) { mActionTimer = time; }
        public int ModifyActionTimer(int time) { return (mActionTimer = mActionTimer + time); }
        public void SetCurrentActionTimer(int time) { mCurrentActionCounter = time; }
        public int DecCurrentActionTimer(int time) { return (mCurrentActionCounter = mCurrentActionCounter - time); }
        public int GetStartingActionTimer() { return mStartingActionCounter; }
        public bool HasFlag(Enum flag) { return mFlags.HasFlag(flag); }
        public virtual Dictionary<EQSlot, Mob> GetEQList() { return null; }
        public void CreateMemento() { mMemento = Clone(); }

    }// Class Mob

}// Namespace _8th_Circle_Server
