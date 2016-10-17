﻿using System;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class CombatMob : Mob
    {
        protected MobType mMobType;
        protected ClientHandler mClientHandler;

        private Dictionary<EQSlot, Mob> mEQList;
        private List<double> mResistances;
        private CombatStats mStats; 
        private String mQueuedCommand;

        public CombatMob() : base()
        {
            mEQList = new Dictionary<EQSlot, Mob>();
            mStats = new CombatStats();
            mQueuedCommand = String.Empty;

            for (EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
                mEQList.Add(slot, null);

            mResistances = new List<double>();

            for (DamageType dt = DamageType.DAMAGETYPE_START; dt < DamageType.DAMAGETYPE_END; ++dt)
                mResistances.Add(0);

            fillResistances();
            Utils.SetFlag(ref mFlags, MobFlags.COMBATABLE);
            mResType = ResType.NPC;
        }// Constructor

        public CombatMob(CombatMob cm) : base(cm)
        {
            mEQList = new Dictionary<EQSlot, Mob>(cm.mEQList);
            mStats = new CombatStats(cm.mStats);
            mQueuedCommand = String.Empty;
            mResistances = new List<double>(cm.mResistances);
            mResType = cm.mResType;
            mClientHandler = cm.mClientHandler;
        }// Copy Constructor

        public CombatMob(String newName) : base(newName)
        {
            mEQList = new Dictionary<EQSlot, Mob>();
            mStats = new CombatStats();
            mQueuedCommand = String.Empty;

            for (EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
                mEQList.Add(slot, null);

            mResistances = new List<double>();

            for (DamageType dt = DamageType.DAMAGETYPE_START; dt < DamageType.DAMAGETYPE_END; ++dt)
                mResistances.Add(0);

            fillResistances();
            Utils.SetFlag(ref mFlags, MobFlags.COMBATABLE);
            mResType = ResType.NPC;
        }// Constructor

        public override Mob Clone()
        {
            return new CombatMob(this);
        }

        public override Mob Clone(String name)
        {
            return new CombatMob(name);
        }

        public void fillResistances()
        {
            mResistances[(int)DamageType.MAGICAL] = ((double)this[STAT.BASEMAGICRES] + this[STAT.MAGICRESMOD]) / 10;
            mResistances[(int)DamageType.PURE] = 0;
            mResistances[(int)DamageType.PHYSICAL] = ((double)this[STAT.BASEARMOR] + this[STAT.ARMORMOD] + this[STAT.BASEPHYRES] + this[STAT.PHYRESMOD]) / 10;
        }// fillResistances

        public override errorCode viewed(Mob mob, Preposition prep, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;
            CombatMob viewer = null;

            if (mob is CombatMob)
                viewer = (CombatMob)mob;

            if (prep.prepType == PrepositionType.PREP_AT && mPrepList.Contains(PrepositionType.PREP_AT))
            {
                if (viewer != null)
                {
                    double viewerHp = viewer[STAT.BASEMAXHP] + viewer[STAT.MAXHPMOD];
                    double targetHp = this[STAT.BASEMAXHP] + this[STAT.MAXHPMOD];
                    double lowEnd = viewerHp - (viewerHp * .1);
                    double highEnd = viewerHp + (viewerHp * .1);

                    if (targetHp >= highEnd)
                        clientString += mName + " looks stronger than you";
                    else if (targetHp <= lowEnd)
                        clientString += mName + " looks weaker than you";
                    else
                        clientString += "you both appear equal in strength";

                    eCode = errorCode.E_OK;
                }// if

                clientString += "\n" + mDescription + "\n";
            }// if
            else
                clientString = "You can't look like that";

            return eCode;
        }// viewed

        public override errorCode fullheal(ref String clientString)
        {
            this[STAT.CURRENTHP] = this[STAT.BASEMAXHP] + this[STAT.MAXHPMOD];
            this[STAT.CURRENTMANA] = this[STAT.BASEMAXMANA] + this[STAT.MAXMANAMOD];

            clientString = "you fully heal " + mName + "\n";

            return errorCode.E_OK;
        }// fullheal

        public override String playerString()
        {
            return "\n" + this[STAT.CURRENTHP] + "/" + (this[STAT.BASEMAXHP] + this[STAT.MAXHPMOD]) + " hp\n";
        }// playerString

        public override errorCode wearall(ref String clientString)
        {
            int tmpInvCount = 0;

            for (int i = 0; i < mInventory.Count; ++i)
            {
                tmpInvCount = mInventory.Count;

                if (mInventory[i] is Equipment)
                {
                    ((Equipment)mInventory[i]).wear(this, ref clientString);

                    if (tmpInvCount != mInventory.Count)
                        --i;
                }// if
            }// for

            return errorCode.E_OK;
        }// wearall

        public override errorCode removeall(ref String clientString)
        {
            for(EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
            {
                if (this[slot] != null)
                    clientString += this[slot].remove(this, ref clientString) + "\n";
            }

            return errorCode.E_OK;
        }// removeall

        public virtual errorCode slain(Mob mob, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (mResType == ResType.PLAYER)
                clientString = "you have been slain by " + mob.GetName();
            else
                eCode = base.destroy(ref clientString);

            return eCode;
        }// slain

        public override void safeWrite(String clientString)
        {
            if(mClientHandler != null)
                mClientHandler.safeWrite(clientString);
        }// safeWrite

        // Properties
        public Mob this[EQSlot slot]
        {
            get { return mEQList[slot]; }
            set { mEQList[slot] = value; }
        }

        public int this[STAT stat]
        {
            get { return mStats.GetStats()[stat]; }
            set { mStats.GetStats()[stat] = value; }
        }

        public double this[DamageType damageType]
        {
            get { return mResistances[(int)damageType]; }
            set { mResistances[(int)damageType] = value; }
        }

        // Accessors
        public List<Mob> GetActionList() { return mStats.GetActionList(); }
        public void AddAction(Mob action) { mStats.AddAction(action); }
        public CombatMob GetPrimaryTarget() { return mStats.GetPrimaryTarget(); }
        public void SetPrimaryTarget(CombatMob target) { mStats.SetPrimaryTarget(target); }
        public List<CombatMob> GetCombatList() { return mStats.GetCombatList(); }
        public override Dictionary<EQSlot, Mob> GetEQList() { return mEQList; }
        public MobType GetMobType() { return mMobType; }
        public void SetMobType(MobType mobType) { mMobType = mobType; }
        public ClientHandler GetClientHandler() { return mClientHandler; }
        public void SetClientHandler(ClientHandler clientHandler) { mClientHandler = clientHandler; }
        public String GetQueuedCommand() { return mQueuedCommand; }
        public void SetQueuedCommand(String command) { mQueuedCommand = command; }

    }// class CombatMob

}// namespace _8th_Circle_Server
