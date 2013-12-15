using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Container : Mob
    {
        // Member Variables
        public bool mIsOpen;
        public int mKeyId;

        public Container() : base()
        {
            mPrepList.Add(PrepositionType.PREP_FROM);
            mPrepList.Add(PrepositionType.PREP_IN);
            mIsOpen = false;
            mKeyId = -1;
        }// Constructor

        public Container(Container mob)
        {
            mName = mob.mName;
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
            mIsOpen = mob.mIsOpen;
            mMobId = mob.mMobId;
            mInstanceId = mob.mInstanceId;
            mKeyId = mob.mKeyId;
        }// Copy Constructor

        public override string viewed(Mob viewer, Preposition prep)
        {
            bool foundAtOrIn = false;
            string ret = string.Empty;

            foreach (PrepositionType pType in mPrepList)
            {
                if (pType == PrepositionType.PREP_AT ||
                    pType == PrepositionType.PREP_IN)
                {
                    foundAtOrIn = true;
                    break;
                }// if
            }// foreach

            if (foundAtOrIn && prep.prepType == PrepositionType.PREP_AT)
            {
                if(mIsOpen)
                   ret += mName + " is open\n";
                else
                   ret += mName + " is closed\n";
            }// if
            else if (foundAtOrIn && prep.prepType == PrepositionType.PREP_IN)
            {
                if (mIsOpen)
                {
                    ret += mName + " contains: \n\n";

                    if (mInventory.Count == 0)
                        ret += "Empty\n";
                    else
                    {
                        foreach (Mob mob in mInventory)
                            ret += mob.mName + "\n";
                    }// else
                }// if
                else
                {
                    ret += mName + " is closed, you cannot look inside\n";
                }// else
            }// if
            else
                ret += "You can't look like that";

            return ret;
        }// viewed

        public override string open(Mob mob)
        {
            string ret = string.Empty;

            if(mFlagList.Contains(objectFlags.FLAG_OPENABLE))
            {
                if(mIsOpen)
                    ret = mName + " is already open";
                else if (mFlagList.Contains(objectFlags.FLAG_LOCKED))
                {
                    return mName + " is locked";
                }// else
                else
                {
                    ret = "You open the " + mName;
                    mIsOpen = true;
                }// else
            }// if

            return ret;
        }// open

        public override string close(Mob mob)
        {
            string ret = string.Empty;

            if (mFlagList.Contains(objectFlags.FLAG_CLOSEABLE))
            {
                if (!mIsOpen)
                    ret = mName + " is already closed";
                else
                {
                    ret = "You close the " + mName;
                    mIsOpen = false;
                }// else
            }// if

            return ret;
        }// close

        public override string lck(Mob mob)
        {
            bool foundKey = false;
            foreach(Mob key in mob.mInventory)
            {
               if(key.mMobId == mKeyId)
                  foundKey = true;
            }// foreach

            if (foundKey)
            {
                if (mFlagList.Contains(objectFlags.FLAG_LOCKABLE))
                {
                    if (mIsOpen)
                        return "you cannot lock " + mName + ", it is open!";

                    if (mFlagList.Contains(objectFlags.FLAG_UNLOCKED))
                    {
                        this.mFlagList.Add(objectFlags.FLAG_LOCKED);
                        this.mFlagList.Remove(objectFlags.FLAG_UNLOCKED);
                        return "you lock " + mName;
                    }// if
                    else
                    {
                        return mName + " is not unlocked";
                    }// if
                }// if
                else
                    return "you can't lock " + mName;
            }// if
            else
                return "you don't have the right key to lock " + mName;
        }// lck

        public override string unlock(Mob mob)
        {
            bool foundKey = false;
            foreach (Mob key in mob.mInventory)
            {
                if (key.mMobId == mKeyId)
                    foundKey = true;
            }// foreach

            if (foundKey)
            {
                if (mIsOpen)
                    return "you cannot unlock " + mName + ", it is open!";

                if (mFlagList.Contains(objectFlags.FLAG_UNLOCKABLE))
                {
                    if (mFlagList.Contains(objectFlags.FLAG_LOCKED))
                    {
                        this.mFlagList.Add(objectFlags.FLAG_UNLOCKED);
                        this.mFlagList.Remove(objectFlags.FLAG_LOCKED);
                        return "you unlock " + mName;
                    }// if
                    else
                    {
                        return mName + " is not locked";
                    }// if
                }// if
                else
                    return "you can't unlock " + mName;
            }// if
            else
                return "you don't have the right key to unlock " + mName;
        }// unlock

        public override void respawn()
        {
            Container cont = new Container(this);
            cont.mIsActive = true;
            cont.mCurrentArea.mObjectList.Add(cont);
            cont.mCurrentRoom.mObjectList.Add(cont);
            cont.mWorld.mObjectList.Add(cont);
        }// respawn

    }// Class Container

}// Namespace _8th_Circle_Server
