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
        MAGIC,
        ACCESSORY,
        WEAPON,
        EQTYPE_END
    }// EQType

    public enum DamageType
    {
        PHYSICAL,
        DAMAGETYPE_START = PHYSICAL,
        MAGICAL,
        PURE,
        HEALING,
        DAMAGETYPE_END
    }// DamageType

    public enum MobType
    {
        NONHEROIC,
        MOBTYPE_START = NONHEROIC,
        WARRIOR,
        ROGUE,
        WIZARD,
        CLERIC,
        SPELLCASTER,
        ALL,
        MOBTYPE_END
    }// MobType

    public class CombatMob : Mob
    {
        public List<Mob> mEQList;
        public List<double> mResistances;
        public CombatStats mStats;
        public MobType mMobType;
        public ClientHandler mClientHandler;
        public string mQueuedCommand;

        public CombatMob() : base()
        {
            mEQList = new List<Mob>();
            mStats = new CombatStats();
            mQueuedCommand = string.Empty;

            for (EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
                mEQList.Add(null);

            mResistances = new List<double>();

            for (DamageType dt = DamageType.DAMAGETYPE_START; dt < DamageType.DAMAGETYPE_END; ++dt)
                mResistances.Add(0);

            fillResistances();         
            mFlagList.Add(MobFlags.FLAG_COMBATABLE);
            mResType = ResType.NPC;
        }// Constructor

        public CombatMob(CombatMob cm) : base(cm)
        {
            mEQList = new List<Mob>(cm.mEQList);
            mStats = new CombatStats(cm.mStats);
            mQueuedCommand = string.Empty;
            mResistances = new List<double>(cm.mResistances);
            mResType = cm.mResType;
            mClientHandler = cm.mClientHandler;
        }// Copy Constructor

        public CombatMob(string newName) : base(newName)
        {
            mEQList = new List<Mob>();
            mStats = new CombatStats();
            mQueuedCommand = string.Empty;

            for (EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
                mEQList.Add(null);

            mResistances = new List<double>();

            for (DamageType dt = DamageType.DAMAGETYPE_START; dt < DamageType.DAMAGETYPE_END; ++dt)
                mResistances.Add(0);

            fillResistances();
            mFlagList.Add(MobFlags.FLAG_COMBATABLE);
            mResType = ResType.NPC;
        }// Constructor

        public void fillResistances()
        {
            mResistances[(int)DamageType.MAGICAL] = ((double)mStats.mBaseMagicRes + mStats.mMagicResMod / 10);
            mResistances[(int)DamageType.PURE] = 0;
            mResistances[(int)DamageType.PHYSICAL] = (((double)mStats.mBaseArmor + mStats.mArmorMod + 
                                                     mStats.mBasePhysRes + mStats.mPhysResMod) / 10); 
        }// fillResistances

        public override string viewed(Mob mob, Preposition prep)
        {
            CombatMob viewer = null;
            string clientString = null;

            if (mob is CombatMob)
                viewer = (CombatMob)mob;

            if (prep.prepType == PrepositionType.PREP_AT && mPrepList.Contains(PrepositionType.PREP_AT))
            {
                if (viewer != null)
                {
                    double viewerHp = viewer.mStats.mBaseMaxHp + viewer.mStats.mMaxHpMod;
                    double targetHp = mStats.mBaseMaxHp + mStats.mMaxHpMod;
                    double lowEnd = viewerHp - (viewerHp*.1);
                    double highEnd = viewerHp + (viewerHp*.1);

                    if (targetHp >= highEnd)
                        clientString += mName + " looks stronger than you";
                    else if (targetHp <= lowEnd)
                        clientString += mName + " looks weaker than you";
                    else
                        clientString += "you both appear equal in strength";
                }// if

                return clientString + "\n" + mDescription + "\n";
            }// if
            else
                return "You can't look like that";
        }// viewed

        public override void respawn()
        {
            mIsRespawning = false;
            mCurrentRespawnTime = mStartingRespawnTime;
            mStats.mCurrentHp = mStats.mBaseMaxHp;
            CombatMob mob = new CombatMob(this);
            mChildren.Add(mob);
            mob.mCurrentArea.addRes(mob);
            mob.mCurrentRoom.addRes(mob);
            mob.mWorld.addRes(mob);
        }// respawn

        public override string fullheal()
        {
            mStats.mCurrentHp = mStats.mBaseMaxHp + mStats.mMaxHpMod;
            mStats.mCurrentMana = mStats.mBaseMaxMana + mStats.mMaxManaMod;

            return "you fully heal " + mName + "\n";
        }// fullheal

        public virtual string playerString()
        {
            return "\n" + mStats.mCurrentHp + "/" + (mStats.mBaseMaxHp + mStats.mMaxHpMod) + " hp\n";
        }// playerString

        public override string wearall()
        {
            string clientString = string.Empty;
            int tmpInvCount = 0;

            for (int i = 0; i < mInventory.Count; ++i)
            {
                tmpInvCount = mInventory.Count;

                if (mInventory[i] is Equipment)
                {
                    clientString += ((Equipment)mInventory[i]).wear(this) + "\n";

                    if (tmpInvCount != mInventory.Count)
                        --i;
                }// if
            }// for

            return clientString;
        }// wearall

        public override string removeall()
        {
            string clientString = string.Empty;

            for (int i = 0; i < mEQList.Count; ++i)
            { 
                if(mEQList[i] != null)
                    clientString += mEQList[i].remove(this) + "\n";
            }

            return clientString;
        }// wearall

        public virtual string slain(Mob mob)
        {
            if (mResType == ResType.PLAYER)
                return "you have been slain by " + mob.mName;
            else
                base.destroy();

            return string.Empty;
        }// slain

    }// class CombatMob

}// namespace _8th_Circle_Server
