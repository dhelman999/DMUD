using System;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    // The prototype manager is responsible for logging prototypes of mobs, keeping a local copy for each instance, and understaning how to clone
    // the mobs, this is used for spawning and respawning mobs.  The design of the prototype starts with the ability to either take a copy
    // of the pre-generated full game mobs, then make your own modifications and place them in an areas specific list.  The instances
    // list holds a copy of all mobs that exist in the area that need to be managed/respawned.  These are referred to as the 'parent'
    // The parent does not interact in the game world, it merely acts as a prototype copy for mobs to respawn from.  This parent keeps
    // all of its original characteristics, mob flags, state ect so that when mobs respawn they merely make another copy from the parent.
    // These clones from the parents are the 'children'.  The parent and child keep references to each other and the children are the
    // mobs that actually interact with the game world.
    public class PrototypeManager
    {
        // The full game mob dictionary, holds the base prototypes for all game mobs.
        private static Dictionary<MobList, Mob> sFullGameMobList;

        // Holds the modified prototypes parents for this particular manager.
        private List<Mob> mPrototypeMobList;

        public PrototypeManager()
        {
            mPrototypeMobList = new List<Mob>();
        }// Constructor

        // Registers a new mob in the full game dictionary.
        public static bool registerFullGameMob(MobList mobID, Mob newMob)
        {
            bool ret = true;

            if (sFullGameMobList == null)
                sFullGameMobList = new Dictionary<MobList, Mob>();

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

        // Responsible for creating parents, children, and adds the children to the game world.
        public Mob cloneMob(MobList mobID, Room startingRoom, String name = "", Mob prototype = null)
        {
            // Create clone from prototype, if none specified, use the global game protoypes
            if(prototype == null)
                prototype = getFullGameRegisteredMob(mobID);

            Mob parent = prototype.Clone();
            Mob child = prototype.Clone();
            mPrototypeMobList.Add(parent);

            // Use the name specified, if none, use the global game prototypes name
            if(name != "")
            {
                parent.SetName(name);
                child.SetName(name);
            }

            // Setup parent/child relationship
            parent.GetChildren().Add(child);
            child.SetParent(parent);

            // If the Mob has other mobs in its inventory, we need to rearrange the parent child relationship
            // so that the inventory items' parents is now the container to which they belong, this will allow them
            // to respawn and despawn when dropped.
            foreach (Mob inventoryItem in parent.GetInv())
            {
                Mob invItemParent = inventoryItem.GetParent();
                invItemParent.GetChildren().Remove(inventoryItem);
                inventoryItem.SetParent(parent);
            }

            Area currentArea = startingRoom.GetCurrentArea();

            // Add starting positions for the parent
            parent.SetStartingArea(currentArea);
            parent.SetCurrentArea(currentArea);
            parent.SetStartingRoom(startingRoom);
            parent.SetCurrentRoom(startingRoom);

            // Add the child to the physical location
            startingRoom.addMobResource(child);

            return parent;
        }// cloneMob

        // Accessors
        public static Mob getFullGameRegisteredMob(MobList mobID)
        {
            if (sFullGameMobList == null)
                sFullGameMobList = new Dictionary<MobList, Mob>();

            return sFullGameMobList[mobID];
        }// getFullGameRegisteredMob

        public List<Mob> GetPrototypeMobList() { return mPrototypeMobList; }

    }// class PrototypeManager

}// namespace _8th_Circle_Server
