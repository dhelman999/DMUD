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

        public Container() : base()
        {
            mPrepList.Add(PrepositionType.PREP_FROM);
            mPrepList.Add(PrepositionType.PREP_IN);
            mIsOpen = false;
        }// Constructor

        public Container(Container mob) : base(mob)
        {
            mIsOpen = mob.mIsOpen;
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

            if(mFlagList.Contains(mobFlags.FLAG_OPENABLE))
            {
                if(mIsOpen)
                    ret = mName + " is already open";
                else if (mFlagList.Contains(mobFlags.FLAG_LOCKED))
                {
                    return mName + " is locked";
                }// else
                else
                {
                    ret = "You open the " + mName;
                    mIsOpen = true;

                    if (mParent != null)
                        mParent.mIsRespawning = true;
                }// else
            }// if

            return ret;
        }// open

        public override string close(Mob mob)
        {
            string ret = string.Empty;

            if (mFlagList.Contains(mobFlags.FLAG_CLOSEABLE))
            {
                if (!mIsOpen)
                    ret = mName + " is already closed";
                else
                {
                    ret = "You close the " + mName;
                    mIsOpen = false;

                    if (mParent != null)
                        mParent.mIsRespawning = true;
                }// else
            }// if

            return ret;
        }// close

        public override string lck(Mob mob)
        {
            bool foundKey = false;
            foreach(Mob key in mob.mInventory)
            {
               if(key.mKeyId == mKeyId)
                  foundKey = true;
            }// foreach

            if (foundKey)
            {
                if (mFlagList.Contains(mobFlags.FLAG_LOCKABLE))
                {
                    if (mIsOpen)
                        return "you cannot lock " + mName + ", it is open!";

                    if (mFlagList.Contains(mobFlags.FLAG_UNLOCKED))
                    {
                        mFlagList.Add(mobFlags.FLAG_LOCKED);
                        mFlagList.Remove(mobFlags.FLAG_UNLOCKED);
                        if (mParent != null)
                            mParent.mIsRespawning = true;

                        return "you lock " + mName;
                    }// if
                    else
                        return mName + " is not unlocked";
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
                if (key.mKeyId == mKeyId)
                    foundKey = true;
            }// foreach

            if (foundKey)
            {
                if (mIsOpen)
                    return "you cannot unlock " + mName + ", it is open!";

                if (mFlagList.Contains(mobFlags.FLAG_UNLOCKABLE))
                {
                    if (mFlagList.Contains(mobFlags.FLAG_LOCKED))
                    {
                        mFlagList.Add(mobFlags.FLAG_UNLOCKED);
                        mFlagList.Remove(mobFlags.FLAG_LOCKED);
                        if (mParent != null)
                            mParent.mIsRespawning = true;

                        return "you unlock " + mName;
                    }// if
                    else
                        return mName + " is not locked";
                }// if
                else
                    return "you can't unlock " + mName;
            }// if
            else
                return "you don't have the right key to unlock " + mName;
        }// unlock

        public override void respawn()
        {
            mIsRespawning = false;
            mCurrentRespawnTime = mStartingRespawnTime;
            Container cont = new Container(this);
            mChildren.Add(cont);
            cont.mCurrentArea.addRes(cont);
            cont.mCurrentRoom.addRes(cont);
            cont.mWorld.addRes(cont);
        }// respawn

    }// Class Container

}// Namespace _8th_Circle_Server
