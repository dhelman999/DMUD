using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public enum ResType
    {
        OBJECT,
        RESOURCE_START = OBJECT,
        NPC,
        PLAYER,
        DOORWAY,
        RESOURCE_END
    }// ResType

    public class ResourceHandler
    {
        public string mName;
        public string mDescription;
        public List<List<Mob>> mResources;

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
            mResources[(int)mob.mResType].Add(mob);
        }// addRes

        public void removeRes(Mob mob)
        {
            mResources[(int)mob.mResType].Remove(mob);
        }// removeRes  

    }// class ResourceHandler
    
}// Namespace _8th_Circle_Server
