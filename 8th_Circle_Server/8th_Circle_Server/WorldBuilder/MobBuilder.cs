using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum MOBLIST
    {
        MOB_START = -1,
        // Reserved for players
        PLAYER = MOB_START,

        // Objects
        BASIC_KEY,
        EVENT_CHEST1,
        EVENT_CHEST2,
        FIRST_CIRCLE,
        BASIC_CHEST,
        SWITCH,
        BASIC_SWORD,

        // NPCs
        GOBLIN_RUNT,
        MAX,

        MOB_END
    }// MOBLIST

    partial class World
    {
        private void addMobs()
        {
            // This order must be the same as the enum defined above
            Mob key = new Mob();
            key.mDescription = "An old brass key, what does it unlock?";
            key.mFlagList.Add(MobFlags.FLAG_GETTABLE);
            key.mFlagList.Add(MobFlags.FLAG_INSPECTABLE);
            key.mFlagList.Add(MobFlags.FLAG_STORABLE);
            key.mFlagList.Add(MobFlags.FLAG_DROPPABLE);
            key.mName = "brass key";
            key.mKeyId = (int)MOBLIST.BASIC_KEY;
            key.mInventory.Capacity = 0;
            key.mWorld = this;
            key.mMobId = (int)MOBLIST.BASIC_KEY;
            mFullMobList.Add(key);

            Container chest = new Container();
            chest.mDescription = "A sturdy, wooden chest.  It makes you wonder what is inside...";
            chest.mFlagList.Add(MobFlags.FLAG_OPENABLE);
            chest.mFlagList.Add(MobFlags.FLAG_CLOSEABLE);
            chest.mFlagList.Add(MobFlags.FLAG_LOCKED);
            chest.mFlagList.Add(MobFlags.FLAG_LOCKABLE);
            chest.mFlagList.Add(MobFlags.FLAG_UNLOCKABLE);
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
            chest.mKeyId = (int)MOBLIST.BASIC_KEY;
            chest.mStartingRespawnTime = 30;
            chest.mCurrentRespawnTime = 30;
            mFullMobList.Add(chest);

            Container chest2 = new Container();
            chest2.mDescription = "A sturdy, wooden chest.  It makes you wonder what is inside...";
            chest2.mFlagList.Add(MobFlags.FLAG_OPENABLE);
            chest2.mFlagList.Add(MobFlags.FLAG_CLOSEABLE);
            chest2.mFlagList.Add(MobFlags.FLAG_LOCKED);
            chest2.mFlagList.Add(MobFlags.FLAG_LOCKABLE);
            chest2.mFlagList.Add(MobFlags.FLAG_UNLOCKABLE);
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
            chest2.mKeyId = (int)MOBLIST.BASIC_KEY;
            chest2.mStartingRespawnTime = 30;
            chest2.mCurrentRespawnTime = 30;
            mFullMobList.Add(chest2);

            Mob first_circle = new Mob();
            first_circle.mDescription = "The first of eight ancient golden circles";
            first_circle.mFlagList.Add(MobFlags.FLAG_GETTABLE);
            first_circle.mFlagList.Add(MobFlags.FLAG_INSPECTABLE);
            first_circle.mFlagList.Add(MobFlags.FLAG_STORABLE);
            first_circle.mFlagList.Add(MobFlags.FLAG_DUPLICATABLE);
            first_circle.mFlagList.Add(MobFlags.FLAG_USEABLE);
            first_circle.mName = "1st Circle";
            eventData = new EventData();
            eventData.eventFlag = EventFlag.EVENT_TELEPORT;
            eventData.commandName = commandName.COMMAND_GET;
            eventData.data = RoomID.GPG_PLAYER_START;
            first_circle.mEventList.Add(eventData);
            first_circle.mInventory.Capacity = 0;
            first_circle.mWorld = this;
            first_circle.mMobId = (int)MOBLIST.FIRST_CIRCLE;
            mFullMobList.Add(first_circle);

            Container no_event_chest = new Container();
            no_event_chest.mDescription = "A sturdy, wooden chest.  It makes you wonder what is inside...";
            no_event_chest.mFlagList.Add(MobFlags.FLAG_OPENABLE);
            no_event_chest.mFlagList.Add(MobFlags.FLAG_CLOSEABLE);
            no_event_chest.mFlagList.Add(MobFlags.FLAG_LOCKED);
            no_event_chest.mFlagList.Add(MobFlags.FLAG_LOCKABLE);
            no_event_chest.mFlagList.Add(MobFlags.FLAG_UNLOCKABLE);
            no_event_chest.mName = "wooden chest";
            no_event_chest.mInventory.Capacity = 20;
            no_event_chest.mWorld = this;
            no_event_chest.mMobId = (int)MOBLIST.BASIC_CHEST;
            no_event_chest.mKeyId = (int)MOBLIST.BASIC_KEY;
            no_event_chest.mStartingRespawnTime = 30;
            no_event_chest.mCurrentRespawnTime = 30;
            mFullMobList.Add(no_event_chest);

            Mob basic_switch = new Mob();
            basic_switch.mDescription = "A switch, it must trigger something...";
            basic_switch.mFlagList.Add(MobFlags.FLAG_USEABLE);
            basic_switch.mName = "switch";
            basic_switch.mWorld = this;
            basic_switch.mMobId = (int)MOBLIST.SWITCH;
            basic_switch.mStartingRespawnTime = 30;
            basic_switch.mCurrentRespawnTime = 30;
            mFullMobList.Add(basic_switch);

            Equipment basic_sword = new Equipment();
            basic_sword.mDescription = "A rusty old sword, it is barely passable as a weapon";
            basic_sword.mFlagList.Add(MobFlags.FLAG_GETTABLE);
            basic_sword.mFlagList.Add(MobFlags.FLAG_DROPPABLE);
            basic_sword.mFlagList.Add(MobFlags.FLAG_INSPECTABLE);
            basic_sword.mFlagList.Add(MobFlags.FLAG_STORABLE);
            basic_sword.mFlagList.Add(MobFlags.FLAG_WEARABLE);
            basic_sword.mName = "Rusty Sword";
            basic_sword.mWorld = this;
            basic_sword.mMobId = (int)MOBLIST.BASIC_SWORD;
            basic_sword.mStartingRespawnTime = 30;
            basic_sword.mCurrentRespawnTime = 30;
            basic_sword.mMinDam = 2;
            basic_sword.mMaxDam = 11;
            basic_sword.mHitMod = 10;
            basic_sword.mType = EQType.WEAPON;
            basic_sword.mSlot = EQSlot.PRIMARY;
            mFullMobList.Add(basic_sword);

            // NPCs start here
            Npc goblin_runt = new Npc();
            goblin_runt.mDescription = "A runt of the goblin litter, truely a wretched creature.";
            goblin_runt.mName = "Goblin Runt";
            goblin_runt.mInventory.Capacity = 0;
            goblin_runt.mWorld = this;
            goblin_runt.mMobId = (int)MOBLIST.GOBLIN_RUNT;
            goblin_runt.mStartingRespawnTime = 30;
            goblin_runt.mCurrentRespawnTime = 30;
            goblin_runt.mStats.mBaseMaxDam = 5;
            goblin_runt.mStats.mCurrentHp = 50;
            goblin_runt.mStats.mBaseMaxHp = 50;
            goblin_runt.mStats.mBaseHit = 50;
            goblin_runt.fillResistances();
            mFullMobList.Add(goblin_runt);

            Npc max = new Npc();
            max.mDescription = "A super big fluffy cute black and white kitty cat... you just want to hug him";
            max.mName = "Max the MaineCoon";
            max.mInventory.Capacity = 0;
            max.mWorld = this;
            max.mMobId = (int)MOBLIST.MAX;
            max.mStartingRespawnTime = 10;
            max.mCurrentRespawnTime = 10;
            mFullMobList.Add(max);
        }// addMobs

        private void addNewMob(MOBLIST mobId, Room startingRoom, Area startingArea)
        {
            Mob mob = null;
            Container cont = null;
            Npc npc = null;

            if (mFullMobList[(int)mobId] is Container)
            {
                cont = new Container((Container)mFullMobList[(int)mobId]);
                mob = cont;
            }// if
            else if (mFullMobList[(int)mobId] is Npc)
            {
                npc = new Npc((Npc)mFullMobList[(int)mobId]);
                mob = npc;
            }// if
            else
                mob = new Mob((Mob)mFullMobList[(int)mobId]);

            int instanceCount = 1;
            foreach (Mob target in startingArea.mFullMobList)
            {
                if ((int)mobId == target.mMobId)
                    ++instanceCount;
            }
            mob.mInstanceId = instanceCount;
            mob.mStartingRoom = mob.mCurrentRoom = startingRoom;
            mob.mStartingArea = mob.mCurrentArea = startingArea;
            startingArea.mFullMobList.Add(mob);

            Mob mob2 = null;
            Container cont2 = null;
            Npc npc2 = null;

            if (mob is Container)
            {
                cont2 = new Container((Container)mob);
                mob2 = cont2;
            }// if
            else if (mob is Npc)
            {
                npc2 = new Npc((Npc)mob);
                mob2 = npc2;
            }// if
            else
                mob2 = new Mob(mob);

			startingRoom.addMobResource(mob2);
            mob2.mWorld.addRes(mob2);
        }// addNewMob

        public void addMob(Mob mob, Room startingRoom, Area startingArea)
        {
            int instanceCount = 1;
            foreach (Mob target in startingArea.mFullMobList)
            {
                if ((int)mob.mMobId == target.mMobId)
                    ++instanceCount;
            }
            mob.mInstanceId = instanceCount;
            mob.mStartingRoom = mob.mCurrentRoom = startingRoom;
            mob.mStartingArea = mob.mCurrentArea = startingArea;
            startingArea.mFullMobList.Add(mob);

            Mob mob2 = null;
            Container cont2 = null;
            Npc npc2 = null;
            Equipment eq = null;

            // TODO
            // Probably need to make these things from a list
            // so we don't have to remember to add them whenever
            // a new class is made
            if (mob is Container)
            {
                cont2 = new Container((Container)mob);
                mob2 = cont2;
            }// if
            else if (mob is Equipment)
            {
                eq = new Equipment((Equipment)mob);
                mob2 = eq;
            }// if
            else if (mob is Npc)
            {
                npc2 = new Npc((Npc)mob);
                mob2 = npc2;
            }// if 
            else
                mob2 = new Mob(mob);

            mob2.mParent = mob;
            startingRoom.addMobResource(mob2);
            mob.mChildren.Add(mob2);
            mob.mWorld.addRes(mob2);
        }// addMob

    }// class World

}// Namespace _8th_Circle_Server
