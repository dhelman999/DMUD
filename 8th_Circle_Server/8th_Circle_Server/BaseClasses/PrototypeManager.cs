﻿using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class PrototypeManager
    {
        private static Dictionary<MOBLIST, Mob> sFullGameMobList;
        private List<Mob> mPrototypeMobList;

        public PrototypeManager()
        {
            mPrototypeMobList = new List<Mob>();
        }// Constructor

        // Accessors
        public static Mob getFullGameRegisteredMob(MOBLIST mobID)
        {
            if(sFullGameMobList == null)
                sFullGameMobList = new Dictionary<MOBLIST, Mob>();

            return sFullGameMobList[mobID];
        }// getFullGameRegisteredMob

        public static bool registerFullGameMob(MOBLIST mobID, Mob newMob)
        {
            bool ret = true;

            if (sFullGameMobList == null)
                sFullGameMobList = new Dictionary<MOBLIST, Mob>();

            try
            {
                sFullGameMobList.Add(mobID, newMob);
            }
            catch
            {
                // Value already exists, prototypes must be unique
                ret = false;
            }

            return ret;
        }// registerFullGameMob

        public Mob cloneMob(MOBLIST mobID, Room startingRoom, string name = "", Mob prototype = null)
        {
            // Create clone from prototype, if none specified, use the global game protoypes
            if(prototype == null)
                prototype = getFullGameRegisteredMob(mobID);

            Mob parent = prototype.Clone();
            Mob child = prototype.Clone();
            mPrototypeMobList.Add(parent);

            // Use the name specified, if none, use the global game prototypes name
            if(name != "")
                parent.mName = child.mName = name;

            // Setup parent/child relationship
            parent.mChildren.Add(child);
            child.mParent = parent;

            Area currentArea = startingRoom.mCurrentArea;

            // Add starting positions for the parent
            parent.mStartingArea = currentArea;
            parent.mCurrentArea = currentArea;
            parent.mStartingRoom = startingRoom;
            parent.mCurrentRoom = startingRoom;

            // Add the child to the physical location
            startingRoom.addMobResource(child);

            return parent;
        }// cloneMob

        public List<Mob> GetPrototypeMobList()
        {
            return mPrototypeMobList;
        }// GetPrototypeMobList

    }// class PrototypeManager

}// namespace _8th_Circle_Server
