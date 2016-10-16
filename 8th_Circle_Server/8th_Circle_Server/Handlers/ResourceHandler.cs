using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class ResourceHandler
    {
        protected string mName;
        protected string mDescription;
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
        public string GetName() { return mName; }
        public string GetDescription() { return mDescription; }
        public void SetDescription(string desc) { mDescription = desc; }

    }// class ResourceHandler
    
}// Namespace _8th_Circle_Server
