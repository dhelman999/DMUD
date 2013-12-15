using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Npc : Mob
    {
        public Npc() : base()
        {
        }// Constructor

        public Npc(string newName) : base()
        {
            mName = newName;
        }// Constructor

        //TODO
        // Make these copy constructors better
        public Npc(Npc mob)
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
            mMobId = mob.mMobId;
            mInstanceId = mob.mInstanceId;
        }// Copy Constructor

    }// Class Npc

}// Namespace _8th_Circle_Server
