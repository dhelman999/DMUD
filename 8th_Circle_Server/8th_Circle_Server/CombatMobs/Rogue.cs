using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class Rogue : CombatMob
    {
        public Rogue()
            : base()
        {
        }// Constructor

        public Rogue(CombatMob cm)
            : base(cm)
        {
        }// Constructor

        private void addActions()
        {
            mStats.mActionList.Add(new Action("backstab", 4, 0, ActionType.ABILITY, 0));
        }// addActions

    }// Class Rogue

}// _8th_Circle_Server
