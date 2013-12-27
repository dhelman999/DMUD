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
            mAreaLoc = mob.mAreaLoc;
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
                ArrayList commandQueue = new ArrayList();
                Command com = new Command();
                foreach(Command cmd in mCurrentArea.mCommandExecuter.mVerbList)
                {
                    if (cmd.commandName == commandName.COMMAND_TELL)
                        com = cmd;
                }
                
                foreach (Player pl in mCurrentArea.mPlayerList)
                {
                    commandQueue.Add(com);
                    commandQueue.Add(pl);
                    commandQueue.Add("purrr");
                    mCurrentArea.mCommandExecuter.execute(commandQueue, this);
                    commandQueue.Clear();
                }
            }// if
            else
            {
                ArrayList commandQueue = new ArrayList();
                addExits(commandQueue);

                if (commandQueue.Count > 0)
                {
                    int index = (int)(commandQueue.Count * rand.NextDouble());
                    Command com = (Command)commandQueue[index];
                    commandQueue.Clear();
                    commandQueue.Add(com);
                    foreach (Player player in mCurrentRoom.mPlayerList)
                    {
                        player.mClientHandler.safeWrite(mName + " scampers off");
                    }// foreach
                    mCurrentArea.mCommandExecuter.execute(commandQueue, this);
                }// if
                else
                { // There must be no exits in the room the npc is trying to leave, so just stay put
                }
            }// else
        }// randomAction

        private void addExits(ArrayList commandQueue)
        {
           for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
           {
              if (mCurrentRoom.mRoomLinks[(int)dir] != null &&
              (mCurrentRoom.mDoorwayList[(int)dir] == null ||
              ((Doorway)mCurrentRoom.mDoorwayList[(int)dir]).mIsOpen))
              {
                 commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[(int)dir]);
              }// if
           }// for
        }// addExits

    }// Class Npc

}// Namespace _8th_Circle_Server
