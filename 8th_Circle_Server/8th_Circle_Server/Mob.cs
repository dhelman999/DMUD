using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class Mob
    {
        // Debug
        internal const bool DEBUG = true;

        // Member Variables
        public string mName;
        public World mWorld;
        public Room mCurrentRoom;
        public int[] mWorldLoc;

        public Mob()
        {
            mName = string.Empty;
            mWorldLoc = new int[3];
        }// Constructor

        public Mob(string name)
        {
            mName = name;
            mWorldLoc = new int[3];
        }// Constructor

    }// Class Mob

}// Namespace _8th_Circle_Server
