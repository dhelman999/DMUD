using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public enum ActionType
    {
        ACTIONTYPE_START,
        ABILITY,
        SPELL,
        ACTIONTYPE_END
    }// ActionType

    public enum DamageScaling
    {
        DAMAGESCALING_START,
        PERLEVEL,
        DAMAGEMULT,
        DAMAGEBONUS,
        DAMAGESCALING_END
    }// DamageScaling

    public class Action
    {
        public string mName;
        public int mCooldown;
        public int mUseTime;
        public int mManaCost;
        public int mBaseMinDamage;
        public int mBaseMaxDamage;
        public double mDamageMult;
        public int mDamageBonus;
        public int mHitBonus;
        public bool mEvadable;
        public int mResistable;
        public ActionType mType;
        public DamageScaling mDamScaling;
        public DamageType mDamType;
        public AbilitySpell mAbilitySpell;

        public Action(string name, int cooldown, int useTime, ActionType type)
        {
            mName = name;
            mCooldown = cooldown;
            mUseTime = useTime;
            mType = type;
        }// Action

    }// Class Action

}// Namespace _8th_Circle_Server
