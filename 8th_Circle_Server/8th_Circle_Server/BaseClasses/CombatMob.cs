using System;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    // Base Class for any mob that can enter combat.
    // This can be enough to fight, or it can be used as a base class for more specific mobs like warriors, rogues ect.
    public class CombatMob : Mob
    {
        // Mainly used to define what types of abilities this mob is allowed to have
        protected MobType mMobType;
        // Players are combat mobs, and need a client to write back to, or if something like a charm, or possess type ability was implemented,
        // you could transfer the client handler to the mob you were possessing and walk around and see what it sees ect.
        protected ClientHandler mClientHandler;

        // List of all equipment this mob is wearing
        private Dictionary<EQSlot, Mob> mEQList;
        // Combat resistances
        private List<double> mResistances;
        // Combat stats
        private CombatStats mStats;
        // Combat mobs can keep spamming commands, and the last one will be remembered so that as soon as its action counter is ready, it can
        // activate the command, so you don't waste time casting or using abilities during combat.
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
        }// Clone

        public override Mob Clone(String name)
        {
            return new CombatMob(name);
        }// Clone

        // Resistances add various things, like armor, and mods from spells, eventually the total is divided by 10 and reduces that damage
        // type by a %, 100 resistance would reduce damage by 10%.
        public void fillResistances()
        {
            mResistances[(int)DamageType.MAGICAL] = ((double)this[STAT.BASEMAGICRES] + this[STAT.MAGICRESMOD]) / 10;
            mResistances[(int)DamageType.PURE] = 0;
            mResistances[(int)DamageType.PHYSICAL] = ((double)this[STAT.BASEARMOR] + this[STAT.ARMORMOD] + this[STAT.BASEPHYRES] + this[STAT.PHYRESMOD]) / 10;
        }// fillResistances


        // A combat mob can be viewed and will not only give you its description, but how relatively healthy it is compared to the viewer.
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

        // Fully resotres all health and mana.
        public override errorCode fullheal(ref String clientString)
        {
            this[STAT.CURRENTHP] = this[STAT.BASEMAXHP] + this[STAT.MAXHPMOD];
            this[STAT.CURRENTMANA] = this[STAT.BASEMAXMANA] + this[STAT.MAXMANAMOD];

            clientString = "you fully heal " + mName + "\n";

            return errorCode.E_OK;
        }// fullheal

        // Standard hp/mana string
        public override String playerString()
        {
            return "\n" + this[STAT.CURRENTHP] + "/" + (this[STAT.BASEMAXHP] + this[STAT.MAXHPMOD]) + " hp\n";
        }// playerString

        // TODO
        //All these 'all' commands should actually construct the base command
        // and do them one by one rather than just calling .wear this way they can trigger events from the base command.
        // Attempts to wear all items in the mobs inventory.  
        public override errorCode wearall(ref String clientString)
        {
            int tmpInvCount = 0;

            for (int i = 0; i < mInventory.Count; ++i)
            {
                tmpInvCount = mInventory.Count;

                (mInventory[i]).wear(this, ref clientString);

                if (tmpInvCount != mInventory.Count)
                    --i;
            }// for

            return errorCode.E_OK;
        }// wearall

        // Attempts to remove all worn equipment
        public override errorCode removeall(ref String clientString)
        {
            for(EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
            {
                if (this[slot] != null)
                    this[slot].remove(this, ref clientString);
            }

            return errorCode.E_OK;
        }// removeall

        // The mob has been killed
        public virtual errorCode slain(Mob mob, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (mResType == ResType.PLAYER)
                clientString = "you have been slain by " + mob.GetName();
            else
                eCode = base.destroy(ref clientString);

            return eCode;
        }// slain

        // Non-player characters don't have a clienthandler, but they could... if they were possessed.
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
        public void SetClientHandler(ClientHandler clientHandler) { mClientHandler = clientHandler; }
        public String GetQueuedCommand() { return mQueuedCommand; }
        public void SetQueuedCommand(String command) { mQueuedCommand = command; }

    }// class CombatMob

}// namespace _8th_Circle_Server
