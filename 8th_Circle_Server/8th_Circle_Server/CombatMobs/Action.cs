
using System;

namespace _8th_Circle_Server
{
    // There are mobs... that are also actions for combatmobs to use, that can be used by command classes to execute?
    // Basically these are really badly designed and need to be reworked with abilityspells and commandclasses in mind.
    // Anyway, these are used as all abilities and spells by mobs, commandclasses execution will pass these to the
    // combat handlers to actually process the combat, it isn't very clean.
    public class Action : Mob
    {
        // Actions have cooldowns before they can be used again
        public int mCooldown;

        // Combatmobs have a global cooldown which gets modified by this.
        public int mUseTime;

        // Mana cost of spells
        public int mManaCost;

        // Various combat stats
        public int mBaseMinDamage;
        public int mBaseMaxDamage;
        public double mDamageMult;
        public int mDamageBonus;
        public int mHitBonus;
        public bool mEvadable;
        public bool mResistable;
        public DamageScaling mDamScaling;
        public DamageType mDamType;

        // Type and corresponding ability spell information
        public ActionType mType;     
        public AbilitySpell mAbilitySpell;

        // Can this be used without a weapon?
        public bool mWeaponRequired;

        public Action(String name, int cooldown, int useTime, ActionType type) : base()
        {
            mName = name;
            mCooldown = cooldown;
            mUseTime = useTime;
            mType = type;
        }// Action

    }// Class Action

}// Namespace _8th_Circle_Server
