using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum MOBLIST
    {
        PLAYER,
        EVENT_CHEST1,
        EVENT_CHEST2,
        BRASS_KEY
    }// MOBLIST

    partial class World
    {
        private void addMobs()
        {
            Container chest = new Container();
            chest.mDescription = "A sturdy, wooden chest.  It makes you wonder what is inside...";
            chest.mFlagList.Add(objectFlags.FLAG_OPENABLE);
            chest.mFlagList.Add(objectFlags.FLAG_CLOSEABLE);
            chest.mFlagList.Add(objectFlags.FLAG_LOCKED);
            chest.mFlagList.Add(objectFlags.FLAG_LOCKABLE);
            chest.mFlagList.Add(objectFlags.FLAG_UNLOCKABLE);
            chest.mName = "chest1";
            chest.mInventory.Capacity = 20;
            chest.mWorld = this;
            EventData eventData = new EventData();
            eventData.eventFlag = EventFlag.EVENT_TELL_PLAYER;
            eventData.commandName = commandName.COMMAND_LOOK;
            eventData.prepType = PrepositionType.PREP_IN;
            eventData.data = "A voice speaks to you from within the chest";
            chest.mEventList.Add(eventData);
            chest.mMobId = (int)MOBLIST.EVENT_CHEST1;
            chest.mIsActive = false;
            chest.mKeyId = 3;
            chest.mStartingRespawnTime = 60;
            chest.mCurrentRespawnTime = 60;
            mFullMobList.Add(chest);

            Container chest2 = new Container();
            chest2.mDescription = "A sturdy, wooden chest.  It makes you wonder what is inside...";
            chest2.mFlagList.Add(objectFlags.FLAG_OPENABLE);
            chest2.mFlagList.Add(objectFlags.FLAG_CLOSEABLE);
            chest2.mFlagList.Add(objectFlags.FLAG_LOCKED);
            chest2.mFlagList.Add(objectFlags.FLAG_LOCKABLE);
            chest2.mFlagList.Add(objectFlags.FLAG_UNLOCKABLE);
            chest2.mName = "large wooden chest2";
            chest2.mInventory.Capacity = 20;
            chest2.mWorld = this;
            eventData = new EventData();
            eventData.eventFlag = EventFlag.EVENT_TELL_PLAYER;
            eventData.commandName = commandName.COMMAND_LOOK;
            eventData.prepType = PrepositionType.PREP_AT;
            eventData.data = "The " + chest.mName + " says \"hello!\"";
            chest2.mEventList.Add(eventData);
            chest2.mMobId = (int)MOBLIST.EVENT_CHEST2;
            chest2.mIsActive = false;
            chest2.mKeyId = 3;
            chest2.mStartingRespawnTime = 60;
            chest2.mCurrentRespawnTime = 60;
            mFullMobList.Add(chest2);

            Mob key = new Mob();
            key.mDescription = "An old brass key, what does it unlock?";
            key.mFlagList.Add(objectFlags.FLAG_GETTABLE);
            key.mFlagList.Add(objectFlags.FLAG_INSPECTABLE);
            key.mFlagList.Add(objectFlags.FLAG_STORABLE);
            key.mFlagList.Add(objectFlags.FLAG_DROPPABLE);
            key.mName = "brass key";
            key.mInventory.Capacity = 0;
            key.mWorld = this;
            key.mMobId = (int)MOBLIST.BRASS_KEY;
            key.mIsActive = false;
            mFullMobList.Add(key);
        }// addMobs

        // TODO
        // this new name only shows up in the predicate list in rooms, not for events since
        // the name is passed to the eventhandler upon creation of the intial object
        public void addMob(MOBLIST mobId, Room startingRoom, Area startingArea, string newName)
        {
            addNewMob(mobId-1, startingRoom, startingArea, newName);
        }

        // TODO
        // Find a better way to not adjust the index
        public void addMob(MOBLIST mobId, Room startingRoom, Area startingArea)
        {
            string name = ((Mob)mFullMobList[(int)mobId-1]).mName;
            addNewMob(mobId-1, startingRoom, startingArea, name);
        }

        private void addNewMob(MOBLIST mobId, Room startingRoom, Area startingArea, string newName)
        {
            Mob mob = null;
            Container cont = null;

            if (mFullMobList[(int)mobId] is Container)
            {
                cont = new Container((Container)mFullMobList[(int)mobId]);
                mob = cont;
            }
            else
                mob = new Mob((Mob)mFullMobList[(int)mobId]);

            int instanceCount = 1;
            foreach (Mob target in startingArea.mFullMobList)
            {
                if ((int)mobId+1 == target.mMobId)
                    ++instanceCount;
            }
            mob.mInstanceId = instanceCount;
            mob.mStartingRoom = mob.mCurrentRoom = startingRoom;
            mob.mStartingArea = mob.mCurrentArea = startingArea;
            startingArea.mFullMobList.Add(mob);

            Mob mob2 = null;
            Container cont2 = null;

            if (mob is Container)
            {
                cont2 = new Container((Container)mob);
                mob2 = cont2;
            }
            else
                mob2 = new Mob(mob);

            mob2.mName = newName;
            startingRoom.addObject(mob2);
        }// addMob

    }// class World

}// Namespace _8th_Circle_Server
