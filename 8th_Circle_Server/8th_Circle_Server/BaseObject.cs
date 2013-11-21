using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum objectFlags
    {
        FLAG_PLAYER_OWNED=0,
        FLAG_NPC_OWNED,
        FLAG_OPEN,
        FLAG_CLOSED,
        FLAG_LOCKED,
        FLAG_HIDDEN,
        FLAG_INVISIBLE,
        FLAG_GETTABLE,
        FLAG_PUSHABLE,
        FLAG_STORABLE,
        FLAG_USEABLE,
        FLAG_INSPECTABLE,
        FLAG_IDENTIFYABLE,
        FLAG_STEALABLE,
    };// flags

    class BaseObject
    {
        // Debug
        internal const bool DEBUG = false;

        // Member Variables
        public Room mStartingRoom;
        public Room mCurrentRoom;
        public Area mStartingArea;
        public Area mCurrentArea;
        public Mob mStartingOwner;
        public Mob mCurrentOwner;
        public World mWorld;
        public ArrayList mPrepList;
        public ArrayList mFlagList;
        public string mName;
        public string mDescription;

        public BaseObject()
        {
            mStartingRoom = mCurrentRoom = null;
            mStartingArea = mCurrentArea = null;
            mStartingOwner = mCurrentOwner = null;
            mPrepList = new ArrayList();
            mFlagList = new ArrayList();
            mName = string.Empty;
            mDescription = string.Empty;
            mWorld = null;
            mPrepList.Add(PrepositionType.PREP_AT);
        }// Constructor

        public string used()
        {
            return string.Empty;
        }// used

        public string viewed(Preposition prep, ClientHandler clientHandler)
        {
            bool foundAt = false;
            foreach (PrepositionType pType in mPrepList)
            {
                if (pType == PrepositionType.PREP_AT)
                    foundAt = true;
            }// foreach

            if (foundAt && prep.prepType == PrepositionType.PREP_AT)
                return mDescription;
            else
                return "You can't look like that";
        }// viewed

    }// Class BaseObject

}// Namespace _8th_Circle_Server
