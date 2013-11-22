using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Containers : BaseObject
    {
        // Member Variables
        public ArrayList mStorage;

        public Containers() : base()
        {
            mStorage = new ArrayList();
            mStorage.Capacity = 10;
            mPrepList.Add(PrepositionType.PREP_FROM);
            mPrepList.Add(PrepositionType.PREP_IN);
        }// Constructor

        public override string viewed(Preposition prep, ClientHandler clientHandler)
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
                if(mFlagList.Contains(objectFlags.FLAG_OPEN))
                   ret += mName + " is open\n";
                else if(mFlagList.Contains(objectFlags.FLAG_CLOSED))
                   ret += mName + " is closed\n";
            }// if
            else if (foundAtOrIn && prep.prepType == PrepositionType.PREP_IN)
            {
                if (mFlagList.Contains(objectFlags.FLAG_OPEN))
                {
                    ret += mName + " contains: \n\n";

                    if (mStorage.Count == 0)
                        ret += "Empty\n";
                    else
                    {
                        foreach (BaseObject baseObject in mStorage)
                            ret += baseObject.mName + "\n";
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
    }// Class Containers

}// Namespace _8th_Circle_Server
