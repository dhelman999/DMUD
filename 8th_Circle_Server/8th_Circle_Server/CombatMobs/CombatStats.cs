using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class CombatStats
    {
        private List<CombatMob> mCombatList;
        private CombatMob mPrimaryTarget;
        private List<Mob> mActionList;
        private List<Action> mQueuedAction;
        private Dictionary<STAT, int> mStats;

        public CombatStats()
        {
            mCombatList = new List<CombatMob>();
            mActionList = new List<Mob>();
            mQueuedAction = new List<Action>();
            mStats = new Dictionary<STAT, int>();
            mPrimaryTarget = null;

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
            mPrimaryTarget = null;

            for (STAT currentStat = STAT.STAT_START; currentStat < STAT.STAT_EMD; ++currentStat)
                mStats.Add(currentStat, cs.mStats[currentStat]);
        }// Copy Constructor

        // Accessors
        public Dictionary<STAT, int> GetStats() { return mStats; }
        public List<Mob> GetActionList() { return mActionList; }
        public void AddAction(Mob action) { mActionList.Add(action); }
        public CombatMob GetPrimaryTarget() { return mPrimaryTarget; }
        public void SetPrimaryTarget(CombatMob target) { mPrimaryTarget = target; }
        public List<CombatMob> GetCombatList() { return mCombatList; }

    }// Class CombatMob

}// Namespace _8th_Circle_Server
