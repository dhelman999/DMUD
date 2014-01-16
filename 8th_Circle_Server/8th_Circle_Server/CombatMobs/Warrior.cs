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
            mMobType = MobType.WARRIOR;
            mStats.mBasePhysRes = 250;
            mStats.mBaseEvade -= 15;
            mStats.mBaseDamBonus = 1;
            mStats.mBaseHit -= 5;
            mStats.mBaseMaxHp += 5;
            mStats.mCurrentHp += 5;
        }// Constructor

        public Warrior(CombatMob cm) : base(cm)
        {
            mMobType = MobType.WARRIOR;
            mStats.mBasePhysRes = 250;
            mStats.mBaseEvade -= 15;
            mStats.mBaseDamBonus = 1;
            mStats.mBaseHit -= 5;
            mStats.mBaseMaxHp += 5;
            mStats.mCurrentHp += 5;
        }// Constructor

        private void addActions()
        {
            mStats.mActionList.Add(new Action("bash", 4, 0, ActionType.ABILITY));
        }// addActions

    }// Class Warrior

}// _8th_Circle_Server
