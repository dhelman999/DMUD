using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class CombatStats
    {
        public int mLevel;
        public int mCurrentHp;
        public int mMaxHp;
        public int mBaseMinDamage;
        public int mBaseMaxDamage;
        public int mDamageModifier;

        public CombatStats()
        {
            mLevel = 1;
            mCurrentHp = mMaxHp = 100;
            mBaseMinDamage = 1;
            mBaseMaxDamage = 10;
            mDamageModifier = 0;
        }// Constructor

    }// Class CombatMob

}// Namespace _8th_Circle_Server
