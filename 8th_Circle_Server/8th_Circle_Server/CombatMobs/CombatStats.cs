using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class CombatStats
    {
        public List<CombatMob> mCombatList;
        public CombatMob mPrimaryTarget;
        public List<Mob> mActionList;
        public List<Action> mQueuedAction;
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
        public int mBaseEvade;
        public int mEvadeMod;
        public int mBaseArmor;
        public int mArmorMod;
        public int mBasePhysRes;
        public int mPhysResMod;
        public int mBaseMaxPhysResBoost;
        public int mMaxPhysResBoostMod;
        public int mBaseMagicRes;
        public int mMagicResMod;
        public int mBaseMaxMagicResBoost;
        public int mMaxMagicResBoostMod;
        public int mBaseMaxMana;
        public int mMaxManaMod;
        public int mCurrentMana;

        public CombatStats()
        {
            mCombatList = new List<CombatMob>();
            mActionList = new List<Mob>();
            mQueuedAction = new List<Action>();
            mLevel = 1;
            mCurrentHp = mBaseMaxHp = 50;
            mBaseMinDam = 1;
            mBaseMaxDam = 10;
            mBaseHit = 50;
        }// Constructor

        public CombatStats(CombatStats cs)
        {
            mCombatList = new List<CombatMob>(cs.mCombatList);
            mActionList = new List<Mob>(cs.mActionList);
            mQueuedAction = new List<Action>(cs.mQueuedAction);
            mLevel = cs.mLevel;
            mCurrentHp = cs.mCurrentHp;
            mBaseMaxHp = cs.mBaseMaxHp;
            mMaxHpMod = cs.mMaxHpMod;
            mBaseMinDam = cs.mBaseMinDam;
            mMinDamMod = cs.mMinDamMod;
            mBaseMaxDam = cs.mBaseMaxDam;
            mMaxDamMod = cs.mMaxDamMod;
            mBaseDamBonus = cs.mBaseDamBonus;
            mDamBonusMod = cs.mDamBonusMod;
            mBaseHit = cs.mBaseHit;
            mHitMod = cs.mHitMod;
            mBaseEvade = cs.mBaseEvade;
            mEvadeMod = cs.mEvadeMod;
            mBaseArmor = cs.mBaseArmor;
            mArmorMod = cs.mArmorMod;
            mBasePhysRes = cs.mBasePhysRes;
            mPhysResMod = cs.mPhysResMod;
            mBaseMaxPhysResBoost = cs.mBaseMaxPhysResBoost;
            mMaxPhysResBoostMod = cs.mMaxPhysResBoostMod;
            mBaseMagicRes = cs.mBaseMagicRes;
            mMagicResMod = cs.mMagicResMod;
            mBaseMaxMagicResBoost = cs.mBaseMaxMagicResBoost;
            mMaxMagicResBoostMod = cs.mMaxMagicResBoostMod;
        }// Copy Constructor

    }// Class CombatMob

}// Namespace _8th_Circle_Server
