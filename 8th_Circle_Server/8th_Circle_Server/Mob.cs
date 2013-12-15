using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum objectFlags
    {
        FLAG_PLAYER_OWNED = 0,
        FLAG_NPC_OWNED,
        FLAG_OPENABLE,
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
    };// flags

    class Mob
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public string mName;
        public string mExitStr;
        public string mShortDescription;
        public string mDescription;
        public World mWorld;
        public int[] mWorldLoc;
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
        public bool mIsActive;
        public int mMobId;
        public int mInstanceId;
        public int mStartingRespawnTime;
        public int mCurrentRespawnTime;

        public Mob()
        {
            mName = mDescription = mShortDescription = mExitStr = string.Empty;
            mWorldLoc = new int[3];
            mInventory = new ArrayList();
            mPrepList = new ArrayList();
            mPrepList.Add(PrepositionType.PREP_AT);
            mFlagList = new ArrayList();
            mEventList = new ArrayList();
            mInventory.Capacity = 20;
            mStartingRoom = mCurrentRoom = null;
            mStartingArea = mCurrentArea = null;
            mStartingOwner = mCurrentOwner = null;
            mIsActive = true;
            mStartingRespawnTime = mCurrentRespawnTime = 15;
            mMobId = -1;
            mInstanceId = 0;
        }// Constructor

        public Mob(string name)
        {
            mName = mExitStr = name;
            mDescription = mShortDescription = string.Empty;
            mWorldLoc = new int[3];
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
            mInstanceId = 0;
        }// Constructor

        public Mob(Mob mob)
        {
            mName = mob.mName;
            mExitStr = mob.mName;
            mDescription = mob.mDescription;
            mShortDescription = mob.mShortDescription;
            mWorld = mob.mWorld;
            mWorldLoc = mob.mWorldLoc;
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
        }// Copy Constructor

        public bool move(string direction)
        {
            bool ret = false;

            switch (direction)
            {
                case "north":
                    if (mCurrentRoom.mNorthLink != null &&
                        (mCurrentRoom.mDoorwayList[(int)Direction.NORTH] == null ||
                        ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.NORTH]).mIsOpen))
                    {
                        changeRoom(mCurrentRoom.mNorthLink);
                        ret = true;
                    }// if
                    break;

                case "south":
                    if (mCurrentRoom.mSouthLink != null &&
                        (mCurrentRoom.mDoorwayList[(int)Direction.SOUTH] == null ||
                        ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.SOUTH]).mIsOpen))
                    {
                        changeRoom(mCurrentRoom.mSouthLink);
                        ret = true;
                    }// if
                    break;

                case "east":
                    if (mCurrentRoom.mEastLink != null &&
                        (mCurrentRoom.mDoorwayList[(int)Direction.EAST] == null ||
                        ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.EAST]).mIsOpen))
                    {
                        changeRoom(mCurrentRoom.mEastLink);
                        ret = true;
                    }// if
                    break;

                case "west":
                    if (mCurrentRoom.mWestLink != null &&
                        (mCurrentRoom.mDoorwayList[(int)Direction.WEST] == null ||
                        ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.WEST]).mIsOpen))
                    {
                        changeRoom(mCurrentRoom.mWestLink);
                        ret = true;
                    }// if
                    break;

                case "up":
                    if (mCurrentRoom.mUpLink != null &&
                        (mCurrentRoom.mDoorwayList[(int)Direction.UP] == null ||
                        ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.UP]).mIsOpen))
                    {
                        changeRoom(mCurrentRoom.mUpLink);
                        ret = true;
                    }// if
                    break;

                case "down":
                    if (mCurrentRoom.mDownLink != null &&
                        (mCurrentRoom.mDoorwayList[(int)Direction.DOWN] == null ||
                        ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.DOWN]).mIsOpen))
                    {
                        changeRoom(mCurrentRoom.mDownLink);
                        ret = true;
                    }// if
                    break;

                case "northwest":
                    if (mCurrentRoom.mNorthwestLink != null &&
                        (mCurrentRoom.mDoorwayList[(int)Direction.NORTHWEST] == null ||
                        ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.NORTHWEST]).mIsOpen))
                    {
                        changeRoom(mCurrentRoom.mNorthwestLink);
                        ret = true;
                    }// if
                    break;

                case "northeast":
                    if (mCurrentRoom.mNortheastLink != null &&
                        (mCurrentRoom.mDoorwayList[(int)Direction.NORTHEAST] == null ||
                        ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.NORTHEAST]).mIsOpen))
                    {
                        changeRoom(mCurrentRoom.mNortheastLink);
                        ret = true;
                    }// if
                    break;

                case "southwest":
                    if (mCurrentRoom.mSouthwestLink != null &&
                        (mCurrentRoom.mDoorwayList[(int)Direction.SOUTHWEST] == null ||
                        ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.SOUTHWEST]).mIsOpen))
                    {
                        changeRoom(mCurrentRoom.mSouthwestLink);
                        ret = true;
                    }// if
                    break;

                case "southeast":
                    if (mCurrentRoom.mSoutheastLink != null &&
                        (mCurrentRoom.mDoorwayList[(int)Direction.SOUTHEAST] == null ||
                        ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.SOUTHEAST]).mIsOpen))
                    {
                        changeRoom(mCurrentRoom.mSoutheastLink);
                        ret = true;
                    }// if
                    break;

                default:
                    ret = false;
                    break;
            }// switch

            return ret;

        }// move

        public void changeRoom(Room newRoom)
        {
            // Remove old references
            if (this is Player)
            {
                mCurrentRoom.mPlayerList.Remove(this);
                if (mCurrentArea != newRoom.mCurrentArea)
                {
                    mCurrentArea.mPlayerList.Remove(this);
                    mCurrentArea = newRoom.mCurrentArea;
                    newRoom.mCurrentArea.mPlayerList.Add(this);
                }// if

                // Add new references
                newRoom.mPlayerList.Add(this);
                mWorldLoc[0] = newRoom.mWorldLoc[0];
                mWorldLoc[1] = newRoom.mWorldLoc[1];
                mWorldLoc[2] = newRoom.mWorldLoc[2];
                mCurrentRoom = newRoom;
            }// if
            else
            {
                mCurrentRoom.mNpcList.Remove(this);
                if (mCurrentArea != newRoom.mCurrentArea)
                {
                    mCurrentArea.mNpcList.Remove(this);
                    mCurrentArea = newRoom.mCurrentArea;
                    newRoom.mCurrentArea.mNpcList.Add(this);
                }// if

                // Add new references
                newRoom.mNpcList.Add(this);
                mWorldLoc[0] = newRoom.mWorldLoc[0];
                mWorldLoc[1] = newRoom.mWorldLoc[1];
                mWorldLoc[2] = newRoom.mWorldLoc[2];
                mCurrentRoom = newRoom;
            }// else
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
                    mob.mCurrentArea.mObjectList.Remove(this);
                    mob.mCurrentRoom.mObjectList.Remove(this);
                    mob.mWorld.mObjectList.Remove(this);
                    if (mob is Player)
                        this.mFlagList.Add(objectFlags.FLAG_PLAYER_OWNED);
                    else
                        this.mFlagList.Add(objectFlags.FLAG_NPC_OWNED);
                    mob.mInventory.Add(this);

                    return "you get " + exitString(mCurrentRoom);
                }// if
                else
                {
                    return "your inventory is full";
                }// else
            }// if
            else
                return "you can't get that";
                
        }// get

        public virtual string drop(Mob mob)
        {
            if (mFlagList.Contains(objectFlags.FLAG_DROPPABLE))
            {  
                if (mob is Player)
                    this.mFlagList.Remove(objectFlags.FLAG_PLAYER_OWNED);
                else
                    this.mFlagList.Remove(objectFlags.FLAG_NPC_OWNED);
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

        public virtual string destroy()
        {
            mCurrentArea.mObjectList.Remove(this);
            mCurrentArea.mNpcList.Remove(this);
            mCurrentOwner = null;
            mInventory.Clear();
            mEventList.Clear();
            mCurrentRoom.mNpcList.Remove(this);
            mCurrentRoom.mObjectList.Remove(this);
            mWorld.mNpcList.Remove(this);
            mWorld.mObjectList.Remove(this);
            mIsActive = false;

            return "destroying " + mName;
        }// destroy

        public virtual void respawn()
        {
            Mob mob = new Mob(this);
            mob.mIsActive = true;
            mob.mCurrentArea.mObjectList.Add(mob);
            mob.mCurrentRoom.mObjectList.Add(mob);
            mob.mWorld.mObjectList.Add(mob);
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

    }// Class Mob

}// Namespace _8th_Circle_Server
