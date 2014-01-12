using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class Wizard : CombatMob
    {
        public Wizard() : base()
        {
        }// Constructor

        public Wizard(CombatMob cm) : base(cm)
        {
        }// Constructor

        public override string playerString()
        {
            return "\n" + mStats.mCurrentHp + "/" + (mStats.mBaseMaxHp + mStats.mMaxHpMod) + " hp " +
                mStats.mCurrentMana + "/" + (mStats.mBaseMaxMana + mStats.mMaxManaMod) + " mana\n";
        }// playerString

        private void addActions()
        {
            mStats.mActionList.Add(new Action("mystic shot", 4, 0, ActionType.SPELL));
        }// addActions

    }// Class Wizard

}// _8th_Circle_Server
