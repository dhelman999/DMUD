
using System;

namespace _8th_Circle_Server
{
    public class Container : Mob
    {
        // Member Variables
        private bool mIsOpen;

        public Container(String name = "") : base()
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

        public override Mob Clone(String name)
        {
            return new Container(name);
        }

        public override errorCode viewed(Mob viewer, Preposition prep, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (mFlags.HasFlag(MobFlags.HIDDEN))
            {
                clientString = "you can't do that\n";

                return eCode;
            }     

            if (prep.prepType == PrepositionType.PREP_AT && mPrepList.Contains(PrepositionType.PREP_AT))
            {
                if(mIsOpen)
                    clientString += mName + " is open\n";
                else
                    clientString += mName + " is closed\n";

                eCode = errorCode.E_OK;
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
                            clientString += mob.GetName() + "\n";
                    }// else

                    eCode = errorCode.E_OK;
                }// if
                else
                    clientString += mName + " is closed, you cannot look inside\n";
            }// if
            else
                clientString += "You can't look like that\n";

            return eCode;
        }// viewed

        public override errorCode open(Mob mob, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (mFlags.HasFlag(MobFlags.HIDDEN))
            {
                clientString = "you can't do that\n";

                return eCode;
            }   

            if(HasFlag(MobFlags.OPENABLE))
            {
                if(mIsOpen)
                    clientString = mName + " is already open\n";
                else if (HasFlag(MobFlags.LOCKED))
                    clientString = mName + " is locked\n";
                else
                {
                    clientString = "You open the " + mName + "\n";
                    mIsOpen = true;

                    if (mParent != null)
                        mParent.SetIsRespawning(true);

                    eCode = errorCode.E_OK;
                }// else
            }// if

            return eCode;
        }// open

        public override errorCode close(Mob mob, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (HasFlag(MobFlags.CLOSEABLE))
            {
                if (!mIsOpen)
                {
                    clientString = mName + " is already closed\n";

                    return eCode;
                }
                    
                else
                {
                    clientString = "You close the " + mName + "\n";
                    mIsOpen = false;

                    if (mParent != null)
                        mParent.SetIsRespawning(true);

                    eCode = errorCode.E_OK;
                }// else
            }// if

            return eCode;
        }// close

        public override errorCode lck(Mob mob, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            bool foundKey = false;

            foreach(Mob key in mob.GetInv())
            {
               if(key.GetKeyID() == mKeyId)
                  foundKey = true;
            }// foreach

            if (foundKey)
            {
                if (HasFlag(MobFlags.LOCKABLE))
                {
                    if (mIsOpen)
                    {
                        clientString = "you cannot lock " + mName + ", it is open!\n";

                        return eCode;
                    }   

                    if (HasFlag(MobFlags.UNLOCKED))
                    {
                        Utils.SetFlag(ref mFlags, MobFlags.LOCKED);
                        Utils.UnsetFlag(ref mFlags, MobFlags.UNLOCKED);

                        if (mParent != null)
                            mParent.SetIsRespawning(true);

                        clientString = "you lock " + mName + "\n";
                        eCode = errorCode.E_OK;
                    }// if
                    else
                        clientString = mName + " is not unlocked\n";
                }// if
                else
                    clientString = "you can't lock " + mName + "\n";
            }// if
            else
                clientString = "you don't have the right key to lock " + mName + "\n";

            return eCode;
        }// lck

        public override errorCode unlock(Mob mob, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            bool foundKey = false;

            foreach (Mob key in mob.GetInv())
            {
                if (key.GetKeyID() == mKeyId)
                    foundKey = true;
            }// foreach

            if (foundKey)
            {
                if (mIsOpen)
                {
                    clientString = "you cannot unlock " + mName + ", it is open!\n";

                    return eCode;
                }               

                if (HasFlag(MobFlags.UNLOCKABLE))
                {
                    if (HasFlag(MobFlags.LOCKED))
                    {
                        Utils.SetFlag(ref mFlags, MobFlags.UNLOCKED);
                        Utils.UnsetFlag(ref mFlags, MobFlags.LOCKED);

                        if (mParent != null)
                            mParent.SetIsRespawning(true);

                        clientString = "you unlock " + mName + "\n";
                        eCode = errorCode.E_OK;
                    }// if
                    else
                        clientString = mName + " is not locked\n";
                }// if
                else
                    clientString = "you can't unlock " + mName + "\n";
            }// if
            else
                clientString = "you don't have the right key to unlock " + mName + "\n";

            return eCode;
        }// unlock

        // Accessors
        public bool IsOpen() { return mIsOpen; }

    }// Class Container

}// Namespace _8th_Circle_Server
