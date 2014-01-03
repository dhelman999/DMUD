using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class CombatStats
    {
        // TODO
        // Figure all these out, with attributes and fill
        // in the copy constructor for it.
        public ArrayList mCombatList;
        public int mLevel;
        public int mCurrentHp;
        public int mBaseMaxHp;
        public int mMaxHpMod;
        public int mBaseMinDam;
        public int mMinDamMod;
        public int mBaseMaxDam;
        public int mMaxDamMod;
        public int mBaseDamBonus;
        public int mDamBonusMod;
        public int mBaseHit;
        public int mHitMod;
        public int mBaseArmor;
        public int mArmorMod;
        public int mBasePhysRes;
        public int mPhysResMod;
        public int mBaseMaxPhysResBoost;
        public int mMaxPhysResBoostMod;
        public int mBaseFireRes;
        public int mFireResMod;
        public int mBaseMaxFireResBoost;
        public int mMaxFireResBoostMod;
        public int mBaseColdRes;
        public int mColdResMod;
        public int mBaseMaxColdResBoost;
        public int mMaxColdResBoostMod;
        public int mBaseLightningRes;
        public int mLightningResMod;
        public int mBaseMaxLightningResBoost;
        public int mMaxLightningResBoostMod;
        public int mBaseAcidRes;
        public int mAcidResMod;
        public int mBaseMaxAcidResBoost;
        public int mMaxAcidResBoostMod;
        public int mBaseForceRes;
        public int mForceResMod;
        public int mBaseMaxForceResBoost;
        public int mMaxForceResBoostMod;
        public int mBaseMagicRes;
        public int mMagicResMod;
        public int mBaseMaxMagicResBoost;
        public int mMaxMagicResBoostMod;

        public CombatStats()
        {
            mCombatList = new ArrayList();
            mLevel = 1;
            mCurrentHp = mBaseMaxHp = 50;
            mBaseMinDam = 1;
            mBaseMaxDam = 10;
            mBaseDamBonus = 1;
        }// Constructor

        public CombatStats(CombatStats cs)
        {
            this.mCombatList = (ArrayList)cs.mCombatList.Clone();
            this.mLevel = cs.mLevel;
            this.mCurrentHp = cs.mCurrentHp;
            this.mBaseMaxHp = cs.mBaseMaxHp;
            this.mBaseMinDam = cs.mBaseMinDam;
            this.mBaseMaxDam = cs.mBaseMaxDam;
            this.mBaseDamBonus = cs.mBaseDamBonus;
        }// copy Constructors

    }// Class CombatMob

}// Namespace _8th_Circle_Server
