using System.Collections.Generic;

namespace _8th_Circle_Server
{
    class PrototypeManager
    {
        private Dictionary<MOBLIST, Mob> mMobRegistry;

        public PrototypeManager()
        {
            mMobRegistry = new Dictionary<MOBLIST, Mob>();
        }// Constructor

        // Accessors
        public Mob getRegisteredMob(MOBLIST mobID)
        {
            return mMobRegistry[mobID];
        }

        public bool registerMob(MOBLIST mobID, Mob newMob)
        {
            bool ret = true;

            try
            {
                mMobRegistry.Add(mobID, newMob);
            }
            catch
            {
                // Value already exists, prototypes must be unique
                ret = false;
            }

            return ret;
        }// registerMob

        public Mob cloneMob(MOBLIST mobID, Room startingRoom, string name)
        {
            // Create clone from prototype
            Mob prototype = getRegisteredMob(mobID);
            Mob parent = prototype.Clone();
            Mob child = prototype.Clone();
            parent.mName = child.mName = name;

            // Setup parent/child relationship
            parent.mChildren.Add(child);
            child.mParent = parent;

            Area currentArea = startingRoom.mCurrentArea;

            // Add starting positions for the parent and add it to the area's prototype list
            parent.mStartingArea = currentArea;
            parent.mCurrentArea = currentArea;
            parent.mStartingRoom = startingRoom;
            parent.mCurrentRoom = startingRoom;
            startingRoom.mCurrentArea.mFullMobList.Add(parent);

            // Add the child to the physical location
            startingRoom.addMobResource(child);

            return parent;
        }// cloneMob

    }// class PrototypeManager

}// namespace _8th_Circle_Server
