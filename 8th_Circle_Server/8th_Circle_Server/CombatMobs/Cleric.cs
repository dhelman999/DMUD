using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class Cleric : CombatMob
    {
        public Cleric() : base()
        {
        }// Constructor

        public Cleric(CombatMob cm) : base(cm)
        {
        }// Constructor

        private void addActions()
        {
            //mStats.mActionList.Add(new Action("bash", 4, 0, ActionType.ABILITY, 0));
        }// addActions

    }// Class Cleric

}// _8th_Circle_Server
