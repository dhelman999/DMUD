
namespace _8th_Circle_Server
{
    // Base class for rogues
    public class Rogue : CombatMob
    {
        public Rogue() : base()
        {
            mMobType = MobType.ROGUE;
            this[STAT.BASEHIT] = this[STAT.BASEHIT] + 5;
            this[STAT.BASEDAMBONUSMOD] = 1;
            this[STAT.BASEEVADE] = this[STAT.BASEEVADE] + 5;
            this[STAT.BASEMAXHP] = this[STAT.BASEMAXHP] - 5;
            this[STAT.CURRENTHP] = this[STAT.CURRENTHP] - 5;

            fillResistances();
        }// Constructor

        public Rogue(CombatMob cm) : base(cm)
        {
            mMobType = MobType.ROGUE;
            this[STAT.BASEHIT] = this[STAT.BASEHIT] + 5;
            this[STAT.BASEDAMBONUSMOD] = 1;
            this[STAT.BASEEVADE] = this[STAT.BASEEVADE] + 5;
            this[STAT.BASEMAXHP] = this[STAT.BASEMAXHP] - 5;
            this[STAT.CURRENTHP] = this[STAT.CURRENTHP] - 5;

            fillResistances();
        }// Constructor

        private void addActions()
        {
            AddAction(new Action("backstab", 4, 0, ActionType.ABILITY));
        }// addActions

    }// Class Rogue

}// _8th_Circle_Server
