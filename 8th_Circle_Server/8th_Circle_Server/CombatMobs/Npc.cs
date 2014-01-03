using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Npc : CombatMob
    {
        public int mStartingActionCounter;
        public int mCurrentActionCounter;

        public Npc() : base()
        {
            mStartingActionCounter = mCurrentActionCounter = 30;
            mResType = ResType.NPC;
        }// Constructor

        public Npc(string newName) : base(newName)
        {
            mStartingActionCounter = mCurrentActionCounter = 30;
            mResType = ResType.NPC;
        }// Constructor

        public Npc(Npc mob) : base(mob)
        {
            mStartingActionCounter = mob.mStartingActionCounter;
            mCurrentActionCounter = mob.mCurrentActionCounter;
            mResType = ResType.NPC;
        }// Copy Constructor

        public override string viewed(Mob viewer, Preposition prep)
        {
            if (prep.prepType == PrepositionType.PREP_AT &&
                mPrepList.Contains(PrepositionType.PREP_AT))
            {
                string clientString = string.Empty;

                if (viewer is Player)
                {
                    if (mStats.mCurrentHp > ((Player)viewer).mStats.mCurrentHp)
                        clientString += mName + " looks healthier than you";
                    else if (mStats.mCurrentHp < ((Player)viewer).mStats.mCurrentHp)
                        clientString += "you look healthier than " + mName;
                    else
                        clientString += "you both appear to have the same level of health";
                }
                return clientString + "\n" + mDescription + "\n";
            }
            else
                return "You can't look like that";
        }// viewed

        // TODO
        // Needs to be more generic
        public void randomAction()
        {
            if (mFlagList.Contains(mobFlags.FLAG_INCOMBAT))
                return;

            Random rand = new Random();
            if (rand.NextDouble() < .5)
            {
                // Max movement
                if (mMobId == (int)MOBLIST.MAX)
                {
                    foreach (Player player in mCurrentRoom.getRes(ResType.PLAYER))
                    {
                        player.mClientHandler.safeWrite(mName + " purrs softly\n");
                    }// foreach
                    ArrayList commandQueue = new ArrayList();
                    Command com = new Command();
                    foreach (Command cmd in mCurrentArea.mCommandExecuter.mVerbList)
                    {
                        if (cmd.commandName == commandName.COMMAND_TELL)
                            com = cmd;
                    }

                    foreach (Player pl in mCurrentArea.getRes(ResType.PLAYER))
                    {
                        commandQueue.Add(com);
                        commandQueue.Add(pl);
                        commandQueue.Add("purrr");
                        mCurrentArea.mCommandExecuter.execute(commandQueue, this);
                        commandQueue.Clear();
                    }
                }// if (mMobId == (int)MOBLIST.MAX)
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
                    foreach (Player player in mCurrentRoom.getRes(ResType.PLAYER))
                    {
                        player.mClientHandler.safeWrite(mName + " scampers off\n");
                    }// foreach
                    mCurrentArea.mCommandExecuter.execute(commandQueue, this);
                }// if
                else
                { // There must be no exits in the room the npc is trying to leave, so just stay put
                }
            }// else
        }// randomAction

        public override void respawn()
        {
            mIsRespawning = false;
            mCurrentRespawnTime = mStartingRespawnTime;
            mStats.mCurrentHp = mStats.mBaseMaxHp;
            Npc mob = new Npc(this);
            mChildren.Add(mob);
            mob.mCurrentArea.addRes(mob);
            mob.mCurrentRoom.addRes(mob);
            mob.mWorld.addRes(mob);
        }// respawn

        public override string fullheal()
        {
            mStats.mCurrentHp = mStats.mBaseMaxHp;
            return "you fully heal " + mName + "\n";
        }// fullheal

        private void addExits(ArrayList commandQueue)
        {
           for (Direction dir = Direction.DIRECTION_START; dir <= Direction.DIRECTION_END; ++dir)
           {
              if (mCurrentRoom.mRoomLinks[(int)dir] != null &&
              (mCurrentRoom.getRes(ResType.DOORWAY)[(int)dir] == null ||
              ((Doorway)mCurrentRoom.getRes(ResType.DOORWAY)[(int)dir]).mIsOpen))
              {
                 commandQueue.Add(mCurrentArea.mCommandExecuter.mCommandList[(int)dir]);
              }// if
           }// for
        }// addExits

    }// Class Npc

}// Namespace _8th_Circle_Server
