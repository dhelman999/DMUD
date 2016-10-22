using System.Collections.Generic;

namespace _8th_Circle_Server
{
    // Base class for all combat stats which will be used by combat mobs and during combat in general.
    public class CombatStats
    {
        // Holds the active combatants
        private List<CombatMob> mCombatList;

        // Who is this mob currently targetting in combat?  Autoattacks depend on this
        private CombatMob mPrimaryTarget;

        // What actions are available during combat
        private List<Mob> mActionList;

        // What action is queued up at the next opportunity
        private List<Action> mQueuedAction;

        // Actual combat stats used in combat
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
