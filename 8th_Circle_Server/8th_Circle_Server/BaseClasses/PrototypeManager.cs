using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class PrototypeManager
    {
        private static Dictionary<MobList, Mob> sFullGameMobList;
        private List<Mob> mPrototypeMobList;

        public PrototypeManager()
        {
            mPrototypeMobList = new List<Mob>();
        }// Constructor

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

        public Mob cloneMob(MobList mobID, Room startingRoom, string name = "", Mob prototype = null)
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
