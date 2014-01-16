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
            mMobType = MobType.WIZARD;
            mStats.mBaseHit -= 10;
            mStats.mBaseMaxHp -= 10;
            mStats.mCurrentHp -= 10;
            mStats.mBaseMaxMana = 40;
            mStats.mCurrentMana = 40;
            addActions();
        }// Constructor

        public Wizard(CombatMob cm) : base(cm)
        {
            mMobType = MobType.WIZARD;
            mStats.mBaseHit -= 10;
            mStats.mBaseMaxHp -= 10;
            mStats.mCurrentHp -= 10;
            mStats.mBaseMaxMana = 40;
            mStats.mCurrentMana = 40;
            addActions();
        }// Constructor

        public override string playerString()
        {
            return "\n" + mStats.mCurrentHp + "/" + (mStats.mBaseMaxHp + mStats.mMaxHpMod) + " hp " +
                mStats.mCurrentMana + "/" + (mStats.mBaseMaxMana + mStats.mMaxManaMod) + " mana\n";
        }// playerString

        private void addActions()
        {   
            mStats.mActionList.Add(new Mob("mystic shot"));
        }// addActions

    }// Class Wizard

}// _8th_Circle_Server
