using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Npc : Mob
    {
        public int mStartingActionCounter;
        public int mCurrentActionCounter;

        public Npc() : base()
        {
            mStartingActionCounter = mCurrentActionCounter = 30;
        }// Constructor

        public Npc(string newName) : base()
        {
            mName = newName;
            mStartingActionCounter = mCurrentActionCounter = 30;
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
            mStartingActionCounter = mCurrentActionCounter = 30;
        }// Copy Constructor

        // TODO
        // Needs to be more generic
        public void randomAction()
        {
            Console.WriteLine("random action taken");
            Random rand = new Random();
            if (rand.NextDouble() < .5)
            {
                foreach (Player player in mCurrentRoom.mPlayerList)
                {
                    player.mClientHandler.safeWrite(mName + " purrs softly");
                }// foreach
            }// if
            else
            {
                ArrayList commandQueue = new ArrayList();
                addExits(commandQueue);

                int index = (int)(commandQueue.Count * rand.NextDouble());
                Command com = (Command)commandQueue[index];
                commandQueue.Clear();
                commandQueue.Add(com);
                foreach (Player player in mCurrentRoom.mPlayerList)
                {
                    player.mClientHandler.safeWrite(mName + " scampers off");
                }// foreach
                mCurrentArea.mCommandExecuter.execute(commandQueue, this);
            }// else
        }// randomAction

        // TODO
        // Needs to be more generic
        private void addExits(ArrayList commandQueue)
        {
            if(mCurrentRoom.mNorthLink != null &&
              (mCurrentRoom.mDoorwayList[(int)Direction.NORTH] == null ||
              ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.NORTH]).mIsOpen))
            {
                commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[0]);
            }// if
            if (mCurrentRoom.mSouthLink != null &&
              (mCurrentRoom.mDoorwayList[(int)Direction.SOUTH] == null ||
              ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.SOUTH]).mIsOpen))
            {
                commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[1]);
            }// if
            if (mCurrentRoom.mEastLink != null &&
              (mCurrentRoom.mDoorwayList[(int)Direction.EAST] == null ||
              ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.EAST]).mIsOpen))
            {
                commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[2]);
            }// if
            if (mCurrentRoom.mWestLink != null &&
              (mCurrentRoom.mDoorwayList[(int)Direction.WEST] == null ||
              ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.WEST]).mIsOpen))
            {
                commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[3]);
            }// if
            if (mCurrentRoom.mUpLink != null &&
              (mCurrentRoom.mDoorwayList[(int)Direction.UP] == null ||
              ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.UP]).mIsOpen))
            {
                commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[4]);
            }// if
            if (mCurrentRoom.mDownLink != null &&
              (mCurrentRoom.mDoorwayList[(int)Direction.DOWN] == null ||
              ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.DOWN]).mIsOpen))
            {
                commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[5]);
            }// if
            if (mCurrentRoom.mNortheastLink != null &&
              (mCurrentRoom.mDoorwayList[(int)Direction.NORTHEAST] == null ||
              ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.NORTHEAST]).mIsOpen))
            {
                commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[6]);
            }// if
            if (mCurrentRoom.mNorthwestLink != null &&
              (mCurrentRoom.mDoorwayList[(int)Direction.NORTHWEST] == null ||
              ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.NORTHWEST]).mIsOpen))
            {
                commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[7]);
            }// if
            if (mCurrentRoom.mSoutheastLink != null &&
              (mCurrentRoom.mDoorwayList[(int)Direction.SOUTHEAST] == null ||
              ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.SOUTHEAST]).mIsOpen))
            {
                commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[8]);
            }// if
            if (mCurrentRoom.mSouthwestLink != null &&
              (mCurrentRoom.mDoorwayList[(int)Direction.SOUTHWEST] == null ||
              ((Doorway)mCurrentRoom.mDoorwayList[(int)Direction.SOUTHWEST]).mIsOpen))
            {
                commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[9]);
            }// if
        }// addExits

    }// Class Npc

}// Namespace _8th_Circle_Server
