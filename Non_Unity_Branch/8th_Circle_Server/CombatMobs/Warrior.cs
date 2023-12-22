
namespace _8th_Circle_Server
{
    // Base class for warriors
    public class Warrior : CombatMob
    {
        public Warrior() : base()
        {
            mMobType = MobType.WARRIOR;
            this[STAT.BASEPHYRES] = 250;
            this[STAT.BASEEVADE] = this[STAT.BASEEVADE] - 15;
            this[STAT.BASEDAMBONUSMOD] = 1;
            this[STAT.BASEHIT] = this[STAT.BASEHIT] - 5;
            this[STAT.BASEMAXHP] = this[STAT.BASEMAXHP] + 5;
            this[STAT.CURRENTHP] = this[STAT.CURRENTHP] + 5;

            fillResistances();
        }// Constructor

        public Warrior(CombatMob cm) : base(cm)
        {
            mMobType = MobType.WARRIOR;
            this[STAT.BASEPHYRES] = 250;
            this[STAT.BASEEVADE] = this[STAT.BASEEVADE] - 15;
            this[STAT.BASEDAMBONUSMOD] = 1;
            this[STAT.BASEHIT] = this[STAT.BASEHIT] - 5;
            this[STAT.BASEMAXHP] = this[STAT.BASEMAXHP] + 5;
            this[STAT.CURRENTHP] = this[STAT.CURRENTHP] + 5;

            fillResistances();
        }// Constructor

        private void addActions()
        {
            AddAction(new Action("bash", 4, 0, ActionType.ABILITY));
        }// addActions

    }// Class Warrior

}// _8th_Circle_Server
