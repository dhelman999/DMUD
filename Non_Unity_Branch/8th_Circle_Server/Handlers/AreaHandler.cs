using System;
using System.Collections.Generic;
using System.Threading;

namespace _8th_Circle_Server
{
    // Handles the processing of an area which includes modifying timers, respawning things, resetting events etc.
    public class AreaHandler
    {
        // TICKTIME is a time in seconds in which each process loop will trigger, meaning things are processed once
        // every ticktime, can be used to speed up or slow down time effectively
        internal const int TICKTIME = 1;

        private World mWorld;

        // Each handler is generally implemented as a seperate thread doing various tasks
        private Thread mSpinWorkThread;

        // Area handlers can handle more than one area
        private List<Area> mAreaList;
        
        // Some basic thread safety within the handler, not really used and is a large design flaw that needs to be addressed before
        // this game actually becomes a real multi-player game.
        private object mQueueLock; 

        public AreaHandler(World world)
        {
            mAreaList = new List<Area>();
            mWorld = world;
            mQueueLock = new object();
        }// Constructor

        public void start()
        {
            mSpinWorkThread = new Thread(() => spinWork(this));
            mSpinWorkThread.Start();
        }// start

        // Main working function of the thread
        public static void spinWork(AreaHandler areaHandler)
        {
            bool processed = false;

            while (true)
            {
                processed = false;
                
                try
                {
                    Thread.Sleep(TICKTIME*1000);
                }// try
                catch
                {
                    areaHandler.processAreas();
                    areaHandler.processNpcs();
                    processed = true;
                }// catch

                if (!processed)
                {
                    areaHandler.processAreas();
                    areaHandler.processNpcs();
                }// if
            }// while
        }// spinWork

        // Registers the area to be processed by this handler
        public void registerArea(Area area)
        {
            lock (mQueueLock)
            {
                mAreaList.Add(area);
            }// lock
        }// enQueueArea
        
        // Process the area, modifies timers, respawns mobs, processes npcs.
        private void processAreas()
        {
            List<List<Mob>> targetList = new List<List<Mob>>();
            targetList.Add(mWorld.getRes(ResType.NPC));
            targetList.Add(mWorld.getRes(ResType.PLAYER));

            // Decrement the action timers for all mobs in the game
            foreach (List<Mob> ar in targetList)
            {
                foreach (Mob mob in ar)
                {
                    if (mob.GetActionTimer() > 0)
                        mob.ModifyActionTimer(-1);

                    if(mob is CombatMob && ((CombatMob)mob).GetQueuedCommand() != String.Empty)
                    {
                        CommandData cd = new CommandData(((CombatMob)mob).GetQueuedCommand(), (CombatMob)mob);
                        mWorld.GetCommandHandler().enQueueCommand(cd);
                        ((CombatMob)mob).SetQueuedCommand(String.Empty);
                    }
                }// foreach
            }// foreach

            // Check to see if mobs need to respawn
            foreach (Area area in mAreaList)
            {
                foreach(Mob parent in area.GetPrototypeMobList())
                {
                    if (parent.HasFlag(MobFlags.RESPAWNING) && (parent.DecRespawnTime(TICKTIME)) <= 0)
                    {
                        // Mobs cannot respawn while in combat
                        if (parent.HasFlag(MobFlags.INCOMBAT))
                        {
                            Utils.UnsetFlag(ref parent.mFlags, MobFlags.RESPAWNING);
                            parent.SetStartingRespawnTime(parent.GetStartingRespawnTime());
                        }
                        else
                        {
                            String clientString = String.Empty;

                            for (int i= 0; i< parent.GetChildren().Count; ++i)
                                parent.GetChildren()[i--].destroy(ref clientString);

                            Console.WriteLine("respawning " + parent.GetName());
                            parent.respawn();
                            Utils.UnsetFlag(ref parent.mFlags, MobFlags.RESPAWNING);
                        }
                    }// if (parent.IsRespawning() && (parent.DecRespawnTime(TICKTIME)) <= 0)
                }// foreach(Mob parent in area.GetPrototypeMobList())

                // If the area resets, respawn mobs from other areas
                if (area.DecrementRespawnTimer(TICKTIME) <= 0)
                {
                    targetList.Clear();
                    targetList.Add(area.getRes(ResType.OBJECT));
                    targetList.Add(area.getRes(ResType.NPC));

                    foreach (List<Mob> ar in targetList)
                    {
                        // Check to despawn mobs from other areas
                        for (int i = 0; i < ar.Count; ++i)
                        {
                            Mob mob = ar[i];

                            if (mob != null && mob.GetStartingArea() != area)
                            {
                                String clientString = String.Empty;
                                Console.WriteLine("despawning " + mob.GetName() + " from other area");
                                mob.destroy(ref clientString);
                                --i;
                            }
                        }
                    }// foreach (ArrayList ar in targetList)

                    // Revert events
                    for (int i = 0; i < area.GetRevertEvents().Count; ++i)
                    {
                        EventData ed = area.GetRevertEvents()[i];
                        mWorld.GetEventHandler().enQueueEvent(ed);
                    }// for

                    // Revert Doorway's initial state
                    foreach (KeyValuePair<RoomID, Room> keyValPair in area.GetRooms())
                    {
                        Room currentRoom = keyValPair.Value;
                        currentRoom.respawnDoorways();
                    }

                }// if (area.DecrementRespawnTimer(TICKTIME) <= 0)

                // Reset Area respawn timer
                if (area.GetCurrentRespawnTimer() <= 0)
                    area.ResetRespawnTimer();

            }// foreach (Area area in mAreaList)

        }// processAreas

        // Process all npcs, currently doesn't do much besides lets them do a random action.
        private void processNpcs()
        {
            foreach (Area area in mAreaList)
            {
                List<Mob> npcList = area.getRes(ResType.NPC);

                for (int i = 0; i < npcList.Count; ++i)
                {
                    CombatMob npc = (CombatMob)npcList[i];

                    if (npc.DecCurrentActionTimer(TICKTIME) <= 0)
                    {
                        npc.SetCurrentActionTimer(npc.GetStartingActionTimer());
                        npc.randomAction();
                    }
                }
            }// foreach (Area area in mAreaList)
        }// processNpcs

    }// Class AreaHandler

}// Namespace _8th_Circle_Server
