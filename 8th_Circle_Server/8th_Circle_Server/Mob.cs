using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum MobFlags
    {
        FLAG_START,
        FLAG_OPENABLE = FLAG_START,
        FLAG_CLOSEABLE,
        FLAG_LOCKED,
        FLAG_LOCKABLE,
        FLAG_UNLOCKED,
        FLAG_UNLOCKABLE,
        FLAG_HIDDEN,
        FLAG_INVISIBLE,
        FLAG_GETTABLE,
        FLAG_DROPPABLE,
        FLAG_PUSHABLE,
        FLAG_STORABLE,
        FLAG_USEABLE,
        FLAG_INSPECTABLE,
        FLAG_WEARABLE,
        FLAG_IDENTIFYABLE,
        FLAG_STEALABLE,
        FLAG_DUPLICATABLE,
        FLAG_SEARCHING,
        FLAG_COMBATABLE,
        FLAG_INCOMBAT,
        FLAG_END
    };// flags

    public class Mob
    {
        // Debug
        internal const bool DEBUG = false;
        internal int SEED = 0;

        // Member Variables
        public string mName;
        public ResType mResType;
        public string mExitStr;
        public string mShortDescription;
        public string mDescription;
        public World mWorld;
        public int[] mAreaLoc;
        public Room mStartingRoom;
        public Room mCurrentRoom;
        public Area mStartingArea;
        public Area mCurrentArea;
        public Mob mStartingOwner;
        public Mob mCurrentOwner;
        public ArrayList mPrepList;
        public ArrayList mFlagList;
        public ArrayList mInventory;
        public ArrayList mEventList;
        public ArrayList mChildren;
        public Mob mParent;
        public int mMobId;
        public int mInstanceId;
        public int mStartingRespawnTime;
        public int mCurrentRespawnTime;
        public bool mIsRespawning;
        public int mKeyId;
        public int mActionTimer;
        public int mStartingActionCounter;
        public int mCurrentActionCounter;
        public Random mRand;

        public Mob()
        {
            mName = mDescription = mShortDescription = mExitStr = string.Empty;
            mAreaLoc = new int[3];
            mInventory = new ArrayList();
            mPrepList = new ArrayList();
            mPrepList.Add(PrepositionType.PREP_AT);
            mFlagList = new ArrayList();
            mEventList = new ArrayList();
            mChildren = new ArrayList();
            mInventory.Capacity = 20;
            mStartingRoom = mCurrentRoom = null;
            mStartingArea = mCurrentArea = null;
            mStartingOwner = mCurrentOwner = null;
            mStartingRespawnTime = mCurrentRespawnTime = 25;
            mMobId = mKeyId = -1;
            mInstanceId = mKeyId = mActionTimer = 0;
            mStartingActionCounter = mCurrentActionCounter = 30;
            mRand = new Random();
        }// Constructor

        public Mob(string name)
        {
            mName = mExitStr = name;
            mDescription = mShortDescription = string.Empty;
            mAreaLoc = new int[3];
            mInventory = new ArrayList();
            mPrepList = new ArrayList();
            mPrepList.Add(PrepositionType.PREP_AT);
            mFlagList = new ArrayList();
            mEventList = new ArrayList();
            mChildren = new ArrayList();
            mInventory.Capacity = 20;
            mStartingRespawnTime = mCurrentRespawnTime = 15;
            mStartingRoom = mCurrentRoom = null;
            mStartingArea = mCurrentArea = null;
            mStartingOwner = mCurrentOwner = null;
            mMobId = -1;
            mInstanceId = mKeyId = mActionTimer = 0;
            mStartingActionCounter = mCurrentActionCounter = 30;
            mRand = new Random();
        }// Constructor

        public Mob(Mob mob)
        {
            mName = mob.mName;
            mExitStr = mob.mName;
            mDescription = mob.mDescription;
            mShortDescription = mob.mShortDescription;
            mWorld = mob.mWorld;
            mAreaLoc = mob.mAreaLoc;
            mInventory = new ArrayList();
            mInventory = (ArrayList)mob.mInventory.Clone();
            mPrepList = new ArrayList();
            mPrepList = (ArrayList)mob.mPrepList.Clone();
            mFlagList = new ArrayList();
            mFlagList = (ArrayList)mob.mFlagList.Clone();
            mEventList = new ArrayList();
            mEventList = (ArrayList)mob.mEventList.Clone();
            mChildren = (ArrayList)mob.mChildren.Clone();
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
            mInstanceId = mob.mInstanceId;
            mKeyId = mob.mKeyId;
            mActionTimer = mob.mActionTimer;
            mResType = mob.mResType;
            mStartingActionCounter = mob.mStartingActionCounter;
            mCurrentActionCounter = mob.mCurrentActionCounter;
            mRand = new Random();
        }// Copy Constructor
        
        public string move(string direction)
        {
            string clientString = string.Empty;

            if (mFlagList.Contains(MobFlags.FLAG_INCOMBAT))
                return "you can't move while in combat\n";
    
            Direction dir = DirStrToEnum(direction);

            if (mCurrentRoom.mRoomLinks[(int)dir] != null &&
               (mCurrentRoom.getRes(ResType.DOORWAY)[(int)dir] == null ||
               ((Doorway)mCurrentRoom.getRes(ResType.DOORWAY)[(int)dir]).mIsOpen))
               clientString = changeRoom((Room)mCurrentRoom.mRoomLinks[(int)dir]);
            else
                return "you can't move that way\n";

            return clientString;

        }// move

        public string changeRoom(Room newRoom)
        {
            if (mFlagList.Contains(MobFlags.FLAG_INCOMBAT))
                return "you can't do that while in combat\n";

            // Remove old references
            mCurrentRoom.removeRes(this);
            if (mCurrentArea != newRoom.mCurrentArea)
            {
                mCurrentArea.removeRes(this);
                newRoom.mCurrentArea.addRes(this);
                mCurrentArea = newRoom.mCurrentArea;
            }// if

            // Add new references
            newRoom.addRes(this);
            mAreaLoc = newRoom.mAreaLoc;
            mCurrentRoom = newRoom;

            return mCurrentRoom.exitString();
        }// changeRoom

        public virtual string used()
        {
            return string.Empty;
        }// used

        public virtual string viewed(Mob viewer, Preposition prep)
        {
            if (prep.prepType == PrepositionType.PREP_AT &&
                mPrepList.Contains(PrepositionType.PREP_AT))
                return mDescription;
            else
                return "You can't look like that\n";
        }// viewed

        public virtual string get(Mob mob)
        {
            if (mFlagList.Contains(MobFlags.FLAG_GETTABLE) &&
                mCurrentRoom.getRes(ResType.OBJECT).Contains(this))
            {
                if (mob.mInventory.Count < mob.mInventory.Capacity)
                {
                    if (mFlagList.Contains(MobFlags.FLAG_DUPLICATABLE))
                    {
                        Mob dup = new Mob(this);
                        mCurrentOwner = mob;
                        mob.mInventory.Add(this);

                        return "you get " + exitString(mCurrentRoom) + "\n";
                    }
                    else
                    {
                        // TODO
                        // Need to have a common error string for hidden objects that
                        // does not include their name
                        if (!mFlagList.Contains(MobFlags.FLAG_HIDDEN))
                        {
                            mob.mCurrentArea.removeRes(this);
                            mob.mCurrentRoom.removeRes(this);
                            mob.mWorld.removeRes(this);
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
                            return "you can't find that\n";
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

        // TODO
        // Needs to be a cleaner interface for this sort of thing
        // Also, this probably won't work as a container when doing
        // get from with things that are on an object instead of
        // in an object
        public virtual string get(Mob mob, PrepositionType prepType, Container container)
        {
            if (mFlagList.Contains(MobFlags.FLAG_GETTABLE))
            {
                if (mob.mInventory.Count < mob.mInventory.Capacity)
                {
                    if (mFlagList.Contains(MobFlags.FLAG_DUPLICATABLE))
                    {
                        Mob dup = new Mob(this);
                        mCurrentOwner = mob;
                        mob.mInventory.Add(this);

                        return "you get " + exitString(mCurrentRoom) + "\n";
                    }
                    else if (container.mFlagList.Contains(MobFlags.FLAG_OPENABLE) &&
                             container.mIsOpen)
                    {
                        if (prepType == PrepositionType.PREP_FROM)
                        {
                            if (container.mInventory.Contains(this))
                            {
                                mob.mCurrentArea.removeRes(this);
                                mob.mCurrentRoom.removeRes(this);
                                mob.mWorld.removeRes(this);
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
                    }// if (container.mFlagList.Contains(MobFlags.FLAG_OPENABLE))
                    else
                        return container.mName + " is closed\n";
                }// if (mob.mInventory.Count < mob.mInventory.Capacity)
                else
                    return "your inventory is full\n";
            }// if (mFlagList.Contains(MobFlags.FLAG_GETTABLE))
            else
                return "you can't get that\n";

        }// get

        public virtual string getall()
        {
            string clientString = string.Empty;
            ArrayList targetList = mCurrentRoom.getRes(ResType.OBJECT);
            int tmpInvCount = 0;

            for (int i = 0; i < targetList.Count; ++i)
            {
                tmpInvCount = mInventory.Count;

                clientString += ((Mob)targetList[i]).get(this);
                if (tmpInvCount != mInventory.Count)
                    --i;
            }

            return clientString;
        }// getall

        public virtual string getall(PrepositionType prepType, Container container)
        {
            string clientString = string.Empty;
            ArrayList targetList = container.mInventory;
            int tmpInvCount = 0;

            for (int i = 0; i < targetList.Count; ++i)
            {
                tmpInvCount = mInventory.Count;

                // TODO
                // is --i the best way to do so?  I am okay if it is,
                // just reexamine and make sure
                clientString += ((Mob)targetList[i]).get(this, prepType, container);
                if (tmpInvCount != mInventory.Count)
                    --i;
            }// if

            return clientString;
        }// getall

        public virtual string drop(Mob mob)
        {
            if (mFlagList.Contains(MobFlags.FLAG_DROPPABLE))
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

                clientString += ((Mob)mInventory[i]).drop(this);
                if (tmpInvCount != mInventory.Count)
                    --i;
            }

            return clientString;
        }// dropall

        public virtual string open(Mob mob)
        {
            return "You can't open that\n";
        }// open

        public virtual string close(Mob mob)
        {
            return "You can't close that\n";
        }// close

        public virtual string lck(Mob mob)
        {
            return "You can't lock that\n";
        }// lck

        public virtual string unlock(Mob mob)
        {
            return "You can't unlock that\n";
        }// unlock

        public virtual string use(Mob mob)
        {
            // The actual processing of the event will be handled by checkEvent at the
            // end of command processing
            if (mFlagList.Contains(MobFlags.FLAG_USEABLE) &&
                mEventList.Count > 0)
                return "You use the " + mName + "\n";
            else
                return "You can't use that\n";
        }// unlock

        public virtual string destroy()
        {
            mCurrentArea.removeRes(this);
            mCurrentOwner = null;
            mInventory.Clear();
            mEventList.Clear();
            mCurrentRoom.removeRes(this);   
            mWorld.removeRes(this);
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
            Mob mob = new Mob(this);
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
            return "you can't lock that\n";
        }// lck

        public virtual string unlock()
        {
            return "you can't unlock that\n";
        }// unlock

        public virtual string fullheal()
        {
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
            ArrayList targetList = new ArrayList();
            targetList.Add(mCurrentRoom.getRes(ResType.OBJECT));
            targetList.Add(mCurrentRoom.getRes(ResType.PLAYER));
            targetList.Add(mCurrentRoom.getRes(ResType.NPC));
            targetList.Add(mCurrentRoom.getRes(ResType.DOORWAY));
            bool found = false;

            foreach (ArrayList ar in targetList)
            {
                foreach (Mob mob in ar)
                {
                    if(mob != null &&
                       mob.mFlagList.Contains(MobFlags.FLAG_HIDDEN))
                    {
                        if (mRand.NextDouble() > .5)
                        {
                            searchString += "you discover a " + mob.mName;
                            mob.mFlagList.Remove(MobFlags.FLAG_HIDDEN);
                            found = true;
                        }// if
                    }// if
                }// foreach
            }// foreach

            if (!found)
                searchString = "you find nothing";

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
            if (mFlagList.Contains(MobFlags.FLAG_INCOMBAT))
                return;

            if (mRand.NextDouble() < .5)
            {
                // Max movement
                if (mMobId == (int)MOBLIST.MAX)
                {
                    foreach (CombatMob player in mCurrentRoom.getRes(ResType.PLAYER))
                    {
                        player.mClientHandler.safeWrite(mName + " purrs softly\n");
                    }// foreach
                    ArrayList commandQueue = new ArrayList();
                    Command com = new Command();
                    foreach (Command cmd in mCurrentArea.mCommandExecuter.mCommandList)
                    {
                        if (cmd.commandName == commandName.COMMAND_TELL)
                            com = cmd;
                    }

                    foreach (CombatMob pl in mCurrentArea.getRes(ResType.PLAYER))
                    {
                        commandQueue.Add(com);
                        commandQueue.Add(pl);
                        commandQueue.Add("purrr");
                        mCurrentArea.mCommandExecuter.execute(commandQueue, this);
                        commandQueue.Clear();
                    }
                }// if (mMobId == (int)MOBLIST.MAX)
            }// if
            else
            {
                ArrayList commandQueue = new ArrayList();
                addExits(commandQueue);

                if (commandQueue.Count > 0)
                {
                    int index = (int)(commandQueue.Count * mRand.NextDouble());
                    Command com = (Command)commandQueue[index];
                    commandQueue.Clear();
                    commandQueue.Add(com);
                    foreach (CombatMob player in mCurrentRoom.getRes(ResType.PLAYER))
                    {
                        player.mClientHandler.safeWrite(mName + " scampers off\n");
                    }// foreach
                    mCurrentArea.mCommandExecuter.execute(commandQueue, this);
                }// if
                else
                { // There must be no exits in the room the CombatMob is trying to leave, so just stay put
                }
            }// else
        }// randomAction

        private void addExits(ArrayList commandQueue)
        {
            for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
            {
                if (mCurrentRoom.mRoomLinks[(int)dir] != null &&
                (mCurrentRoom.getRes(ResType.DOORWAY)[(int)dir] == null ||
                ((Doorway)mCurrentRoom.getRes(ResType.DOORWAY)[(int)dir]).mIsOpen))
                {
                    commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[(int)dir]);
                }// if
            }// for
        }// addExits

    }// Class Mob

}// Namespace _8th_Circle_Server
