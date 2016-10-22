using System;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    // This is the base class for all locations in the world, it holds all of the mobs that are available to the world, are and rooms.
    // The ResourceHandler is a way to collect mobs in an organized fashion.  Rather than hold all objects, npcs, doorways in a single datastructure
    // and iterate through it looking for what you want, the ResourceHandler seperates them into seperate lists and indexes into them via an
    // enum.  So each resource has its own seperate list so you are sure what you are getting via simple indexing.  That way, if you iterate
    // through the list, it is through many less objects, and the objects are all the same type, so ifs or casting is needed.
    public class ResourceHandler
    {
        protected String mName;
        protected String mDescription;
        protected List<List<Mob>> mResources;

        public ResourceHandler()
        {
            mResources = new List<List<Mob>>();

            for (ResType i = ResType.RESOURCE_START; i < ResType.RESOURCE_END; ++i)
                mResources.Add(new List<Mob>());
        }// ResourceHandler

        public List<Mob> getRes(ResType resType)
        {
            return mResources[(int)resType];
        }// getRes

        public void addRes(Mob mob)
        {
            List<Mob> resourceList = mResources[(int)mob.GetResType()];

            // Don't allow duplicates for players
            if (mob.GetResType() == ResType.PLAYER && resourceList.Contains(mob))
                return;

            resourceList.Add(mob);
        }// addRes

        public void removeRes(Mob mob)
        {
            mResources[(int)mob.GetResType()].Remove(mob);
        }// removeRes  

        // Accessors
        public String GetName() { return mName; }
        public String GetDescription() { return mDescription; }
        public void SetDescription(String desc) { mDescription = desc; }

    }// class ResourceHandler
    
}// Namespace _8th_Circle_Server
