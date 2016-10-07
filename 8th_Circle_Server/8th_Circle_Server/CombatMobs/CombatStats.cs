using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public enum STAT
    {
        STAT_START,
        LEVEL,
        CURRENTHP,
        BASEMAXHP,
        MAXHPMOD,
        BASEMINDAM,
        BASEMAXDAM,
        DAMBONUSMOD,
        BASEHIT,
        BASEEVADE,
        BASEARMOR,
        ARMORMOD,
        BASEPHYRES,
        BASEMAXPHYRES,
        MAXPHYRESBOOSTMOD,
        BASEMAGICRES,
        MAGICRESMOD,
        BASEMAXMAGICRESBOOST,
        MAXMAGICRESBOOSTMOD,
        BASEMAXMANA,
        MAXMANAMOD,
        CURRENTMANA,
        STAT_EMD
    }

    public class CombatStats
    {
        public List<CombatMob> mCombatList;
        public CombatMob mPrimaryTarget;
        public List<Mob> mActionList;
        public List<Action> mQueuedAction;
        public Dictionary<STAT, int> mStats;
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
            mStats = new Dictionary<STAT, int>();

            for(STAT currentStat = STAT.STAT_START; currentStat < STAT.STAT_EMD; ++currentStat)
                mStats.Add(currentStat, 0);

            mStats[STAT.LEVEL] = 1;
            mStats[STAT.CURRENTHP] = 50;
            mStats[STAT.BASEMAXHP] = 50;
            mStats[STAT.BASEMINDAM] = 1;
            mStats[STAT.BASEMAXDAM] = 10;
            mStats[STAT.BASEHIT] = 60;
        }// Constructor

        public CombatStats(CombatStats cs)
        {
            mCombatList = new List<CombatMob>(cs.mCombatList);
            mActionList = new List<Mob>(cs.mActionList);
            mQueuedAction = new List<Action>(cs.mQueuedAction);
            mStats = new Dictionary<STAT, int>();

            for (STAT currentStat = STAT.STAT_START; currentStat < STAT.STAT_EMD; ++currentStat)
                mStats.Add(currentStat, cs.mStats[currentStat]);
        }// Copy Constructor

    }// Class CombatMob

}// Namespace _8th_Circle_Server
