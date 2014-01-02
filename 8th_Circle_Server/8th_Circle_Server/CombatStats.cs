using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class CombatStats
    {
        public ArrayList mCombatList;
        public int mLevel;
        public int mCurrentHp;
        public int mMaxHp;
        public int mBMinDam;
        public int mBMaxDam;
        public int mDamMod;

        public CombatStats()
        {
            mCombatList = new ArrayList();
            mLevel = 1;
            mCurrentHp = mMaxHp = 50;
            mBMinDam = 1;
            mBMaxDam = 10;
            mDamMod = 1;
        }// Constructor

        public CombatStats(CombatStats cs)
        {
            this.mCombatList = (ArrayList)cs.mCombatList.Clone();
            this.mLevel = cs.mLevel;
            this.mCurrentHp = cs.mCurrentHp;
            this.mMaxHp = cs.mMaxHp;
            this.mBMinDam = cs.mBMinDam;
            this.mBMaxDam = cs.mBMaxDam;
            this.mDamMod = cs.mDamMod;
        }// copy Constructors

    }// Class CombatMob

}// Namespace _8th_Circle_Server
