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
            mMobType = MobType.CLERIC;
            mStats.mBaseMaxMana = 30;
            mStats.mCurrentMana = 30;
            mStats.mBaseEvade -= 5;
            addActions();
        }// Constructor

        public Cleric(CombatMob cm) : base(cm)
        {
            mMobType = MobType.CLERIC;
            mStats.mBaseMaxMana = 30;
            mStats.mCurrentMana = 30;
            mStats.mBaseEvade -= 5;
            addActions();
        }// Constructor

        public override string playerString()
        {
            return "\n" + mStats.mCurrentHp + "/" + (mStats.mBaseMaxHp + mStats.mMaxHpMod) + " hp " +
                   mStats.mCurrentMana + "/" + (mStats.mBaseMaxMana + mStats.mMaxManaMod) + " mana\n";
        }// playerString

        private void addActions()
        {
            mStats.mActionList.Add(new Mob("cure"));
        }// addActions

    }// Class Cleric

}// _8th_Circle_Server
