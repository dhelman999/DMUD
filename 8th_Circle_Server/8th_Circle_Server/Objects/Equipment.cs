
namespace _8th_Circle_Server
{
    public class Equipment : Mob
    {
        public int mLevel;
        public EQType mType;
        public DamageType mDamType;
        public EQSlot mSlot;
        public Useable mUsedby;
        public int mBaseDamBonus;
        public int mMaxHpMod;
        public int mHitMod;
        public int mMinDam;
        public int mMaxDam;
        public int mArmor;
        public int mPhysRes;
        public int mMaxPhysRes;
        public int mFireRes;
        public int mMaxFireRes;
        public int mColdRes;
        public int mMaxColdRes;
        public int mLightningRes;
        public int mMaxLightningRes;
        public int mAcidRes;
        public int mMaxAcidRes;
        public int mForceRes;
        public int mMaxForceRes;

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

        public override string wear(CombatMob pl)
        {
            if (pl.mEQList[(int)mSlot] == null)
            {
                pl.mEQList[(int)mSlot] = this;
                pl.mInventory.Remove(this);

                return "you equip the " + mName;
            }// if
            else
                return "you are already wearing the " + (pl.mEQList[(int)mSlot]).mName;
        }// wear

        public override string remove(CombatMob pl)
        {
            if (pl.mEQList[(int)mSlot] == null)
                return "";
            else
            {
                pl.mEQList[(int)mSlot] = null;
                pl.mInventory.Add(this);

                return "you remove the " + mName;
            }// else
        }// wear

        // TODO, can this be made generic inside the mob class? 
        // so I can avoid forgetting to implement this in the future,
        // it has already happened several times
        public override void respawn()
        {
            mIsRespawning = false;
            mCurrentRespawnTime = mStartingRespawnTime;
            Equipment mob = new Equipment(this);
            mChildren.Add(mob);
            mob.mCurrentArea.addRes(mob);
            mob.mCurrentRoom.addRes(mob);
            mob.mWorld.addRes(mob);
        }// respawn

    }// class Equipment

}// namespace _8th_Circle_Server
