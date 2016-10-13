
namespace _8th_Circle_Server
{
    public class Equipment : Mob
    {
        private int mLevel;
        private EQType mType;
        private DamageType mDamType;
        private EQSlot mSlot;
        private Useable mUsedby;
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
        }// constructor

        public Equipment(Equipment eq, string name = "") : base(eq)
        {
            if (name != "")
                mName = name;

            mLevel = eq.mLevel;
            mType = eq.mType;
            mSlot = eq.mSlot;
            mUsedby = eq.mUsedby;
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
        }// copy constructor

        public override Mob Clone()
        {
            return new Equipment(this);
        }

        public override Mob Clone(string name)
        {
            return new Equipment(this, name);
        }

        public override string wear(CombatMob cm)
        {
            if (cm[mSlot] == null)
            {
                cm[mSlot] = this;
                cm.GetInv().Remove(this);

                return "you equip the " + mName;
            }// if
            else
                return "you are already wearing the " + cm[mSlot].GetName();
        }// wear

        public override string remove(CombatMob cm)
        {
            if (cm[mSlot] == null)
                return "";
            else
            {
                cm[mSlot] = null;
                cm.GetInv().Add(this);

                return "you remove the " + mName;
            }// else
        }// wear

        // Accessors
        public void SetType(EQType type) { mType = type; }
        public DamageType GetDamType() { return mDamType; }
        public EQSlot GetSlot() { return mSlot; }
        public void SetSlot(EQSlot slot) { mSlot = slot; }
        public int GetMinDam() { return mMinDam; }
        public void SetMinDam(int dam) { mMinDam = dam; }
        public int GetMaxDam() { return mMaxDam; }
        public void SetMaxDam(int dam) { mMaxDam = dam; }
        public int GetHitMod() { return mHitMod; }
        public void SetHitMod(int mod) { mHitMod = mod; }

    }// class Equipment

}// namespace _8th_Circle_Server
