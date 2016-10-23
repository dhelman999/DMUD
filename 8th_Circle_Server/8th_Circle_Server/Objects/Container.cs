using System;

namespace _8th_Circle_Server
{
    // Container subclass to hold other mobs.  Overrides relevant base class functions for commands applicable to containers.
    public class Container : Mob
    {
        public Container(String name = "") : base()
        {
            if (name != "")
                mName = name;

            mPrepList.Add(PrepositionType.PREP_FROM);
            mPrepList.Add(PrepositionType.PREP_IN);
        }// Constructor

        public Container(Container mob) : base(mob)
        {
        }// Copy Constructor

        public override Mob Clone()
        {
            return new Container(this);
        }// Clone

        public override Mob Clone(String name)
        {
            return new Container(name);
        }// Clone

        public override errorCode viewed(Mob viewer, Preposition prep, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (HasFlag(MobFlags.HIDDEN))
            {
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

                return eCode;
            }     

            if (prep.prepType == PrepositionType.PREP_AT && mPrepList.Contains(PrepositionType.PREP_AT))
            {
                clientString += mDescription;

                eCode = errorCode.E_OK;
            }
            else if (prep.prepType == PrepositionType.PREP_IN && mPrepList.Contains(PrepositionType.PREP_IN))
            {
                if (HasFlag(MobFlags.OPEN))
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
                }
                else
                    clientString += mName + " is closed, you cannot look inside\n";
            }// if (prep.prepType == PrepositionType.PREP_AT && mPrepList.Contains(PrepositionType.PREP_AT))
            else
                clientString += "You can't look like that\n";

            return eCode;
        }// viewed

        public override errorCode open(Mob mob, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (HasFlag(MobFlags.HIDDEN))
            {
                clientString = GLOBALS.RESPONSE_CANT_DO_THAT;

                return eCode;
            }   

            if(HasFlag(MobFlags.OPENABLE))
            {
                if(HasFlag(MobFlags.OPEN))
                    clientString = mName + " is already open\n";
                else if (HasFlag(MobFlags.LOCKED))
                    clientString = mName + " is locked\n";
                else
                {
                    clientString = "You open the " + mName + "\n";
                    Utils.SetFlag(ref mFlags, MobFlags.OPEN);

                    if (mParent != null)
                        Utils.SetFlag(ref mParent.mFlags, MobFlags.RESPAWNING);

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
                if (!HasFlag(MobFlags.OPEN))
                {
                    clientString = mName + " is already closed\n";

                    return eCode;
                }
                    
                else
                {
                    clientString = "You close the " + mName + "\n";
                    Utils.UnsetFlag(ref mFlags, MobFlags.OPEN);

                    if (mParent != null)
                        Utils.SetFlag(ref mParent.mFlags, MobFlags.RESPAWNING);

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
                    if (HasFlag(MobFlags.OPEN))
                    {
                        clientString = "you cannot lock " + mName + ", it is open!\n";

                        return eCode;
                    }   

                    if (!HasFlag(MobFlags.LOCKED))
                    {
                        Utils.SetFlag(ref mFlags, MobFlags.LOCKED);

                        if (mParent != null)
                            Utils.SetFlag(ref mParent.mFlags, MobFlags.RESPAWNING);

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
                if (HasFlag(MobFlags.OPEN))
                {
                    clientString = "you cannot unlock " + mName + ", it is open!\n";

                    return eCode;
                }               

                if (HasFlag(MobFlags.UNLOCKABLE))
                {
                    if (HasFlag(MobFlags.LOCKED))
                    {
                        Utils.UnsetFlag(ref mFlags, MobFlags.LOCKED);

                        if (mParent != null)
                            Utils.SetFlag(ref mParent.mFlags, MobFlags.RESPAWNING);

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

    }// Class Container

}// Namespace _8th_Circle_Server
