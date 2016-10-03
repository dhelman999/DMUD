
namespace _8th_Circle_Server
{
    public class Rogue : CombatMob
    {
        public Rogue() : base()
        {
            mMobType = MobType.ROGUE;
            mStats.mBaseHit += 5;
            mStats.mBaseDamBonus = 1;
            mStats.mBaseEvade += 5;
            mStats.mBaseMaxHp -= 5;
            mStats.mCurrentHp -= 5;
        }// Constructor

        public Rogue(CombatMob cm) : base(cm)
        {
            mMobType = MobType.ROGUE;
            mStats.mBaseHit += 5;
            mStats.mBaseDamBonus = 1;
            mStats.mBaseEvade += 5;
            mStats.mBaseMaxHp -= 5;
            mStats.mCurrentHp -= 5;
        }// Constructor

        private void addActions()
        {
            mStats.mActionList.Add(new Action("backstab", 4, 0, ActionType.ABILITY));
        }// addActions

    }// Class Rogue

}// _8th_Circle_Server
