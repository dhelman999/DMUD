using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public enum EQSlot
    {
        EQSLOT_START,
        HEAD = EQSLOT_START,
        NECK,
        BACK,
        CHEST,
        ARMS,
        HANDS,
        WAIST,
        LEGS,
        FEET,
        RING1,
        RING2,
        PRIMARY,
        SECONDARY,
        EQSLOT_END
    }// EQSlot

    public enum Useable
    {
        ALL,
        USEABLE_START = ALL,
        USEABLE_END
    }// Useable

    public enum EQType
    {
        LIGHT_ARMOR,
        EQTYPE_START = LIGHT_ARMOR,
        MEDIUM_ARMOR,
        HEAVY_ARMOR,
        SLASHING,
        PIERCING,
        BLUDGEONING,
        MAGIC,
        ACCESSORY,
        EQTYPE_END
    }// EQType

    public enum DamageType
    {
        PHYSICAL,
        DAMAGETYPE_START = PHYSICAL,
        MAGICAL,
        FIRE,
        COLD,
        LIGHTNING,
        ACID,
        FORCE,
        PURE,
        DAMAGETYPE_END
    }// DamageType

    public class CombatMob : Mob
    {
        public ArrayList mEQList;
        public ArrayList mResistances;
        public CombatStats mStats;

        public CombatMob() : base()
        {
            mEQList = new ArrayList();
            mStats = new CombatStats();
            for (EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
                mEQList.Add(null);
            mResistances = new ArrayList();
            for (DamageType dt = DamageType.DAMAGETYPE_START; dt < DamageType.DAMAGETYPE_END; ++dt)
                mResistances.Add(null);
            fillResistances();         
            mFlagList.Add(mobFlags.FLAG_COMBATABLE);
        }// Constructor

        public CombatMob(CombatMob cm) : base(cm)
        {
            mEQList = (ArrayList)cm.mEQList.Clone();
            mStats = new CombatStats(cm.mStats);
            mResistances = (ArrayList)cm.mResistances.Clone();
        }// Copy Constructor

        public CombatMob(string newName) : base(newName)
        {
            mEQList = new ArrayList();
            for (EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
                mEQList.Add(null);
            mStats = new CombatStats();
            mFlagList.Add(mobFlags.FLAG_COMBATABLE);
        }// Constructor

        public void fillResistances()
        {
            mResistances[(int)DamageType.PHYSICAL] = (((double)mStats.mBaseArmor + mStats.mArmorMod
                + mStats.mBasePhysRes + mStats.mPhysResMod) / 10);
            mResistances[(int)DamageType.MAGICAL] = ((double)mStats.mBaseMagicRes + mStats.mMagicResMod / 10);
            mResistances[(int)DamageType.FIRE] = ((double)mStats.mBaseFireRes + mStats.mFireResMod / 10);
            mResistances[(int)DamageType.COLD] = ((double)mStats.mBaseColdRes + mStats.mColdResMod / 10);
            mResistances[(int)DamageType.LIGHTNING] = ((double)mStats.mBaseLightningRes + mStats.mLightningResMod / 10);
            mResistances[(int)DamageType.ACID] = ((double)mStats.mBaseAcidRes + mStats.mAcidResMod / 10);
            mResistances[(int)DamageType.FORCE] = ((double)mStats.mBaseForceRes + mStats.mForceResMod / 10);
            mResistances[(int)DamageType.PURE] = 0;
        }// fillResistances
    }// class CombatMob

}// namespace _8th_Circle_Server
