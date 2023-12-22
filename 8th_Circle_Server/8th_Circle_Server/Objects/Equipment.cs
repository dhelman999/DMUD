using System;

namespace _8th_Circle_Server
{
    // Equipment subclass, used by mobs to wear and add to their combat stats.
    // Overrides relevant base class functions for commands applicable to equipment.
    public class Equipment : Mob
    {
        private int mLevel;
        private EQType mType;
        private DamageType mDamType;
        private EQSlot mSlot;
        private int mBaseDamBonus;
        private int mMaxHpMod;
        private int mHitMod;
        private int mMinDam;
        private int mMaxDam;
        private int mArmor;
        private int mPhysRes;
        private int mMaxPhysRes;
        private int mFireRes;
        private int mMaxFireRes;
        private int mColdRes;
        private int mMaxColdRes;
        private int mLightningRes;
        private int mMaxLightningRes;
        private int mAcidRes;
        private int mMaxAcidRes;
        private int mForceRes;
        private int mMaxForceRes;

        public Equipment() : base()
        {
            mLevel = 1;
            mDamType = DamageType.PHYSICAL;
        }// Constructor

        public Equipment(Equipment eq, String name = "") : base(eq)
        {
            if (name != "")
                mName = name;

            mLevel = eq.mLevel;
            mType = eq.mType;
            mSlot = eq.mSlot;
            mHitMod = eq.mHitMod;
            mMinDam = eq.mMinDam;
            mMaxDam = eq.mMaxDam;
            mArmor = eq.mArmor;
            mBaseDamBonus = eq.mBaseDamBonus;
            mMaxHpMod = eq.mMaxHpMod;
            mPhysRes = eq.mPhysRes;
            mMaxPhysRes = eq.mMaxPhysRes;
            mFireRes = eq.mFireRes;
            mMaxFireRes = eq.mMaxFireRes;
            mColdRes = eq.mColdRes;
            mMaxColdRes = eq.mMaxColdRes;
            mLightningRes = eq.mLightningRes;
            mMaxLightningRes = eq.mMaxLightningRes;
            mAcidRes = eq.mAcidRes;
            mMaxAcidRes = eq.mMaxAcidRes;
            mForceRes = eq.mForceRes;
            mMaxForceRes = eq.mMaxForceRes;
            mDamType = DamageType.PHYSICAL;
        }// copy Constructor

        public override Mob Clone()
        {
            return new Equipment(this);
        }// Clone

        public override Mob Clone(String name)
        {
            return new Equipment(this, name);
        }// Clone

        public override errorCode wear(CombatMob cm, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (cm[mSlot] == null)
            {
                cm[mSlot] = this;
                cm.GetInv().Remove(this);

                clientString += "you equip the " + mName + "\n";
                eCode = errorCode.E_OK;
            }
            else
                clientString += "you are already wearing the " + cm[mSlot].GetName() + "\n";

            return eCode;
        }// wear

        public override errorCode remove(CombatMob cm, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (cm[mSlot] == null)
                return eCode;
            else
            {
                cm[mSlot] = null;
                cm.GetInv().Add(this);

                clientString += "you remove the " + mName;
                eCode = errorCode.E_OK;
            }// else

            return eCode;
        }// wear

        // Accessors
        public void SetType(EQType type) { mType = type; }
        public DamageType GetDamType() { return mDamType; }
        public void SetSlot(EQSlot slot) { mSlot = slot; }
        public int GetMinDam() { return mMinDam; }
        public void SetMinDam(int dam) { mMinDam = dam; }
        public int GetMaxDam() { return mMaxDam; }
        public void SetMaxDam(int dam) { mMaxDam = dam; }
        public void SetHitMod(int mod) { mHitMod = mod; }

    }// class Equipment

}// namespace _8th_Circle_Server
