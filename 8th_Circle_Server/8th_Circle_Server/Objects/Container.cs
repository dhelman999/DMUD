﻿
namespace _8th_Circle_Server
{
    public class Container : Mob
    {
        // Member Variables
        public bool mIsOpen;

        public Container(string name = "") : base()
        {
            if (name != "")
                mName = name;

            mPrepList.Add(PrepositionType.PREP_FROM);
            mPrepList.Add(PrepositionType.PREP_IN);
            mIsOpen = false;
        }// Constructor

        public Container(Container mob) : base(mob)
        {
            mIsOpen = mob.mIsOpen;
        }// Copy Constructor

        public override Mob Clone()
        {
            return new Container(this);
        }

        public override Mob Clone(string name)
        {
            return new Container(name);
        }

        public override string viewed(Mob viewer, Preposition prep)
        {
            string clientString = string.Empty;

            if (prep.prepType == PrepositionType.PREP_AT && mPrepList.Contains(PrepositionType.PREP_AT))
            {
                if(mIsOpen)
                    clientString += mName + " is open\n";
                else
                    clientString += mName + " is closed\n";
            }// if
            else if (prep.prepType == PrepositionType.PREP_IN && mPrepList.Contains(PrepositionType.PREP_IN))
            {
                if (mIsOpen)
                {
                    clientString += mName + " contains: \n\n";

                    if (mInventory.Count == 0)
                        clientString += "Empty\n";
                    else
                    {
                        foreach (Mob mob in mInventory)
                            clientString += mob.mName + "\n";
                    }// else
                }// if
                else
                    clientString += mName + " is closed, you cannot look inside\n";
            }// if
            else
                clientString += "You can't look like that\n";

            return clientString;
        }// viewed

        public override string open(Mob mob)
        {
            string ret = string.Empty;

            if(mFlagList.Contains(MobFlags.FLAG_OPENABLE))
            {
                if(mIsOpen)
                    ret = mName + " is already open\n";
                else if (mFlagList.Contains(MobFlags.FLAG_LOCKED))
                    return mName + " is locked\n";
                else
                {
                    ret = "You open the " + mName + "\n";
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

            if (mFlagList.Contains(MobFlags.FLAG_CLOSEABLE))
            {
                if (!mIsOpen)
                    ret = mName + " is already closed\n";
                else
                {
                    ret = "You close the " + mName + "\n";
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
                if (mFlagList.Contains(MobFlags.FLAG_LOCKABLE))
                {
                    if (mIsOpen)
                        return "you cannot lock " + mName + ", it is open!\n";

                    if (mFlagList.Contains(MobFlags.FLAG_UNLOCKED))
                    {
                        mFlagList.Add(MobFlags.FLAG_LOCKED);
                        mFlagList.Remove(MobFlags.FLAG_UNLOCKED);

                        if (mParent != null)
                            mParent.mIsRespawning = true;

                        return "you lock " + mName + "\n";
                    }// if
                    else
                        return mName + " is not unlocked\n";
                }// if
                else
                    return "you can't lock " + mName + "\n";
            }// if
            else
                return "you don't have the right key to lock " + mName + "\n";
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
                    return "you cannot unlock " + mName + ", it is open!\n";

                if (mFlagList.Contains(MobFlags.FLAG_UNLOCKABLE))
                {
                    if (mFlagList.Contains(MobFlags.FLAG_LOCKED))
                    {
                        mFlagList.Add(MobFlags.FLAG_UNLOCKED);
                        mFlagList.Remove(MobFlags.FLAG_LOCKED);

                        if (mParent != null)
                            mParent.mIsRespawning = true;

                        return "you unlock " + mName + "\n";
                    }// if
                    else
                        return mName + " is not locked\n";
                }// if
                else
                    return "you can't unlock " + mName + "\n";
            }// if
            else
                return "you don't have the right key to unlock " + mName + "\n";
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
