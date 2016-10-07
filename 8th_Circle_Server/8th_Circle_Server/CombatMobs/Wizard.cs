﻿
namespace _8th_Circle_Server
{
    public class Wizard : CombatMob
    {
        public Wizard() : base()
        {
            mMobType = MobType.WIZARD;
            this[STAT.BASEHIT] = this[STAT.BASEHIT] - 10;
            this[STAT.BASEMAXHP] = this[STAT.BASEMAXHP] - 10;
            this[STAT.CURRENTHP] = this[STAT.CURRENTHP] - 10;
            this[STAT.BASEMAXMANA] = 40;
            this[STAT.CURRENTMANA] = 40;

            addActions();
        }// Constructor

        public Wizard(CombatMob cm) : base(cm)
        {
            mMobType = MobType.WIZARD;
            this[STAT.BASEHIT] = this[STAT.BASEHIT] - 10;
            this[STAT.BASEMAXHP] = this[STAT.BASEMAXHP] - 10;
            this[STAT.CURRENTHP] = this[STAT.CURRENTHP] - 10;
            this[STAT.BASEMAXMANA] = 40;
            this[STAT.CURRENTMANA] = 40;

            addActions();
        }// Constructor

        public override string playerString()
        {
            return "\n" + this[STAT.CURRENTHP] + "/" + (this[STAT.BASEMAXHP] + this[STAT.MAXHPMOD]) + " hp " +
                   this[STAT.CURRENTMANA] + "/" + (this[STAT.BASEMAXMANA] + this[STAT.MAXMANAMOD]) + " mana\n";
        }// playerString

        private void addActions()
        {   
            AddAction(new Mob("mystic shot"));
        }// addActions

    }// Class Wizard

}// _8th_Circle_Server
