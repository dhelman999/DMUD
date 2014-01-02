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

    public class CombatMob : Mob
    {
        public ArrayList mEQList;
        public CombatStats mStats;

        public CombatMob() : base()
        {
            mEQList = new ArrayList();
            for (EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
                mEQList.Add(null);
            mStats = new CombatStats();
            mFlagList.Add(mobFlags.FLAG_COMBATABLE);
        }// Constructor

        public CombatMob(CombatMob cm) : base(cm)
        {
            mEQList = (ArrayList)cm.mEQList.Clone();
            mStats = new CombatStats(cm.mStats);
        }// Copy Constructor

        public CombatMob(string newName) : base(newName)
        {
            mEQList = new ArrayList();
            for (EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
                mEQList.Add(null);
            mStats = new CombatStats();
            mFlagList.Add(mobFlags.FLAG_COMBATABLE);
        }// Constructor

    }// class CombatMob

}// namespace _8th_Circle_Server
