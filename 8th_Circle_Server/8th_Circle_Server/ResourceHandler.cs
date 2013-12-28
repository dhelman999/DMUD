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
        public ArrayList mResources;

        public ResourceHandler()
        {
            mResources = new ArrayList();
            for (int i = (int)ResType.RESOURCE_START; i < (int)ResType.RESOURCE_END; ++i)
                mResources.Add(new ArrayList());
        }// ResourceHandler

        public ArrayList getRes(ResType resType)
        {
            return (ArrayList)mResources[(int)resType];
        }// getRes

        public void addRes(Mob mob)
        {
            ((ArrayList)mResources[(int)mob.mResType]).Add(mob);
        }// addRes

        public void removeRes(Mob mob)
        {
            ((ArrayList)mResources[(int)mob.mResType]).Remove(mob);
        }// removeRes  

    }// class ResourceHandler
    
}// Namespace _8th_Circle_Server
