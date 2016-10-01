using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class Action : Mob
    {
        public int mCooldown;
        public int mUseTime;
        public int mManaCost;
        public int mBaseMinDamage;
        public int mBaseMaxDamage;
        public double mDamageMult;
        public int mDamageBonus;
        public int mHitBonus;
        public bool mEvadable;
        public bool mResistable;
        public ActionType mType;
        public DamageScaling mDamScaling;
        public DamageType mDamType;
        public AbilitySpell mAbilitySpell;
        public bool mWeaponRequired;

        public Action(string name, int cooldown, int useTime, ActionType type) : base()
        {
            mName = name;
            mCooldown = cooldown;
            mUseTime = useTime;
            mType = type;
        }// Action

    }// Class Action

}// Namespace _8th_Circle_Server
