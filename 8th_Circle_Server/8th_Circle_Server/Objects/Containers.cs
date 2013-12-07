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

        public override string viewed(Mob viewer, Preposition prep, ClientHandler clientHandler)
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

        public override string open(ClientHandler clientHandler)
        {
            string ret = string.Empty;

            if(mFlagList.Contains(objectFlags.FLAG_OPENABLE))
            {
                if(mIsOpen)
                    ret = mName + " is already open";
                else
                {
                    ret = "You open the " + mName;
                    mIsOpen = true;
                }// else
            }// if

            return ret;
        }// open

        public override string close(ClientHandler clientHandler)
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

    }// Class Container

}// Namespace _8th_Circle_Server
