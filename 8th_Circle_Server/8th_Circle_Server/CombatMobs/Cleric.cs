
namespace _8th_Circle_Server
{
    public class Cleric : CombatMob
    {
        public Cleric() : base()
        {
            mMobType = MobType.CLERIC;
            this[STAT.BASEMAXMANA] = 30;
            this[STAT.CURRENTMANA] = 30;
            this[STAT.BASEEVADE] = this[STAT.BASEEVADE] - 5;
            fillResistances();
            addActions();
        }// Constructor

        public Cleric(CombatMob cm) : base(cm)
        {
            mMobType = MobType.CLERIC;
            this[STAT.BASEMAXMANA] = 30;
            this[STAT.CURRENTMANA] = 30;
            this[STAT.BASEEVADE] = this[STAT.BASEEVADE] - 5;
            fillResistances();
            addActions();
        }// Constructor

        public override string playerString()
        {
            return "\n" + this[STAT.CURRENTHP] + "/" + (this[STAT.BASEMAXHP] + this[STAT.MAXHPMOD]) + " hp " +
                   this[STAT.CURRENTMANA] + "/" + (this[STAT.BASEMAXMANA] + this[STAT.MAXMANAMOD]) + " mana\n";
        }// playerString

        private void addActions()
        {
            AddAction(new Mob("cure"));
        }// addActions

    }// Class Cleric

}// _8th_Circle_Server
