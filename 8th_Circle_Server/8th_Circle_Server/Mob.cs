using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum objectFlags
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
        FLAG_IDENTIFYABLE,
        FLAG_STEALABLE,
        FLAG_DUPLICATABLE,
        FLAG_SEARCHING,
        FLAG_END
    };// flags

    public class Mob
    {
        // Debug
        internal const bool DEBUG = false;

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
        public int mMobId;
        public int mInstanceId;
        public int mStartingRespawnTime;
        public int mCurrentRespawnTime;
        public int mKeyId;
        public int mActionTimer;

        public Mob()
        {
            mName = mDescription = mShortDescription = mExitStr = string.Empty;
            mAreaLoc = new int[3];
            mInventory = new ArrayList();
            mPrepList = new ArrayList();
            mPrepList.Add(PrepositionType.PREP_AT);
            mFlagList = new ArrayList();
            mEventList = new ArrayList();
            mInventory.Capacity = 20;
            mStartingRoom = mCurrentRoom = null;
            mStartingArea = mCurrentArea = null;
            mStartingOwner = mCurrentOwner = null;
            mStartingRespawnTime = mCurrentRespawnTime = 25;
            mMobId = -1;
            mInstanceId = mKeyId = mActionTimer = 0;
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
            mInventory.Capacity = 20;
            mStartingRespawnTime = mCurrentRespawnTime = 15;
            mStartingRoom = mCurrentRoom = null;
            mStartingArea = mCurrentArea = null;
            mStartingOwner = mCurrentOwner = null;
            mMobId = -1;
            mInstanceId = mKeyId = mActionTimer = 0;
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
        }// Copy Constructor
        
        public string move(string direction)
        {
            string clientString;
            
            Direction dir = DirStrToEnum(direction);

            if (mCurrentRoom.mRoomLinks[(int)dir] != null &&
               (mCurrentRoom.mDoorwayList[(int)dir] == null ||
               ((Doorway)mCurrentRoom.mDoorwayList[(int)dir]).mIsOpen))
               clientString = changeRoom((Room)mCurrentRoom.mRoomLinks[(int)dir]);
            else
                return "you can't move that way";

            return clientString;

        }// move

        public string changeRoom(Room newRoom)
        {
            // Remove old references
            mCurrentRoom.removeRes(this);
            if (mCurrentArea != newRoom.mCurrentArea)
            {
                mCurrentArea.removeRes(this);
                mCurrentArea = newRoom.mCurrentArea;
                newRoom.mCurrentArea.removeRes(this);
            }// if

            // Add new references
            newRoom.addRes(this);
            mAreaLoc[0] = newRoom.mAreaLoc[0];
            mAreaLoc[1] = newRoom.mAreaLoc[1];
            mAreaLoc[2] = newRoom.mAreaLoc[2];
            mCurrentRoom = newRoom;

            return mCurrentRoom.exitString();
        }// changeRoom

        public virtual string used()
        {
            return string.Empty;
        }// used

        public virtual string viewed(Mob viewer, Preposition prep)
        {
            bool foundAt = false;
            foreach (PrepositionType pType in mPrepList)
            {
                if (pType == PrepositionType.PREP_AT)
                {
                    foundAt = true;
                    break;
                }// if
            }// foreach

            if (foundAt && prep.prepType == PrepositionType.PREP_AT)
                return mDescription;
            else
                return "You can't look like that";
        }// viewed

        public virtual string get(Mob mob)
        {
            if (mFlagList.Contains(objectFlags.FLAG_GETTABLE))
            {
                if (mob.mInventory.Count < mob.mInventory.Capacity)
                {
                    if (mFlagList.Contains(objectFlags.FLAG_DUPLICATABLE))
                    {
                        Mob dup = new Mob(this);
                        mob.mInventory.Add(this);

                        return "you get " + exitString(mCurrentRoom);
                    }
                    else
                    {
                        // TODO
                        // Need to have a common error string for hidden objects that
                        // does not include their name
                        if (!mFlagList.Contains(objectFlags.FLAG_HIDDEN))
                        {
                            mob.mCurrentArea.removeRes(this);
                            mob.mCurrentRoom.removeRes(this);
                            mob.mWorld.removeRes(this);
                            mob.mInventory.Add(this);

                            return "you get " + exitString(mCurrentRoom);
                        }
                        else
                            return "you can't find that";
                    }
                }// if
                else
                {
                    return "your inventory is full";
                }// else
            }// if
            else
                return "you can't get that";
                
        }// get

        // TODO
        // Needs to be a cleaner interface for this sort of thing
        public virtual string get(Mob mob, PrepositionType prepType, Mob container)
        {
            if (mFlagList.Contains(objectFlags.FLAG_GETTABLE))
            {
                if (mob.mInventory.Count < mob.mInventory.Capacity)
                {
                    if (mFlagList.Contains(objectFlags.FLAG_DUPLICATABLE))
                    {
                        Mob dup = new Mob(this);
                        mob.mInventory.Add(this);

                        return "you get " + exitString(mCurrentRoom);
                    }
                    else
                    {
                        if (container.mFlagList.Contains(objectFlags.FLAG_OPENABLE))
                        {
                            if (prepType == PrepositionType.PREP_FROM)
                            {
                                if (container.mInventory.Contains(this))
                                {
                                    mob.mCurrentArea.removeRes(this);
                                    mob.mCurrentRoom.removeRes(this);
                                    mob.mWorld.removeRes(this);
                                    container.mInventory.Remove(this);
                                    mob.mInventory.Add(this);

                                    return "you get " + exitString(mCurrentRoom);
                                }// if
                                else
                                    return container.mName + " does not contain a " + this.mName;
                            }// if (prepType == PrepositionType.PREP_FROM)
                            else
                                return "you can't get like that";
                        }// if (container.mFlagList.Contains(objectFlags.FLAG_OPENABLE))
                        else
                            return container.mName + " is closed";
                    }// else
                }// if (mob.mInventory.Count < mob.mInventory.Capacity)
                else
                    return "your inventory is full";
            }// if (mFlagList.Contains(objectFlags.FLAG_GETTABLE))
            else
                return "you can't get that";

        }// get

        public virtual string drop(Mob mob)
        {
            if (mFlagList.Contains(objectFlags.FLAG_DROPPABLE))
            {  
                mob.mInventory.Remove(this);
                mCurrentRoom = mob.mCurrentRoom;
                mCurrentRoom.addObject(this);

                return "you drop " + exitString(mCurrentRoom);
            }// if
            else
                return "you can't drop that";
        }// drop

        public virtual string open(Mob mob)
        {
            return "You can't open that";
        }// open

        public virtual string close(Mob mob)
        {
            return "You can't close that";
        }// close

        public virtual string lck(Mob mob)
        {
            return "You can't lock that";
        }// lck

        public virtual string unlock(Mob mob)
        {
            return "You can't unlock that";
        }// unlock

        public virtual string use(Mob mob)
        {
            // TODO
            // This is handled by the checkEvent
            string ret;
            if (mFlagList.Contains(objectFlags.FLAG_USEABLE))
                ret = "You use the " + mName;
            else
                ret = "You can't use that";

            return ret;
        }// unlock

        public virtual string destroy()
        {
            mCurrentArea.removeRes(this);
            mCurrentArea.removeRes(this);
            mCurrentOwner = null;
            mInventory.Clear();
            mEventList.Clear();
            mCurrentRoom.removeRes(this);
            mCurrentRoom.removeRes(this);
            mWorld.removeRes(this);
            mWorld.removeRes(this);

            return "destroying " + mName;
        }// destroy

        public virtual void respawn()
        {
            Mob mob = new Mob(this);
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
            return "you can't lock that";
        }// lck

        public virtual string unlock()
        {
            return "you can't unlock that";
        }// unlock

        public virtual string search()
        {
            string searchString = string.Empty;
            Random rand = new Random();
            ArrayList targetList = new ArrayList();
            targetList.Add(mCurrentRoom.getRes(ResType.OBJECT));
            targetList.Add(mCurrentRoom.getRes(ResType.PLAYER));
            targetList.Add(mCurrentRoom.getRes(ResType.NPC));
            targetList.Add(mCurrentRoom.mDoorwayList);
            bool found = false;

            foreach (ArrayList ar in targetList)
            {
                foreach (Mob mob in ar)
                {
                    if(mob != null &&
                       mob.mFlagList.Contains(objectFlags.FLAG_HIDDEN))
                    {
                        if (rand.NextDouble() > .5)
                        {
                            searchString += "you discover a " + mob.mName;
                            mob.mFlagList.Remove(objectFlags.FLAG_HIDDEN);
                            found = true;
                        }// if
                    }// if
                }// foreach
            }// foreach

            if (!found)
                searchString = "you find nothing";

            return searchString;
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

    }// Class Mob

}// Namespace _8th_Circle_Server
