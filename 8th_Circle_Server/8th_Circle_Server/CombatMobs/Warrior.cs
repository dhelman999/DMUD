using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class Warrior : CombatMob
    {
        public Warrior() : base()
        {
        }// Constructor

        public Warrior(CombatMob cm) : base(cm)
        {
        }// Constructor

        private void addActions()
        {
            mStats.mActionList.Add(new Action("bash", 4, 0, ActionType.ABILITY, 0));
        }// addActions

    }// Class Warrior

}// _8th_Circle_Server
