using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class Equipment : Mob
    {
        public int mLevel;
        public EQType mType;
        public EQSlot mSlot;
        public Useable mUsedby;
        public int mHitMod;
        public int mMinDam;
        public int mMaxDam;
        public int mArmor;
        public int mPhysRes;
        public int mMaxPhysResBoost;
        public int mFireRes;
        public int mMaxFireResBoost;
        public int mColdRes;
        public int mMaxColdResBoost;
        public int mLightningRes;
        public int mMaxLightningResBoost;
        public int mAcidRes;
        public int mMaxAcidResBoost;
        public int mForceRes;
        public int mMaxForceResBoost;
        public ArrayList mEffects;

        public Equipment() : base()
        {
            mLevel = 1;
            mEffects = new ArrayList();
        }// constructor

        public Equipment(Equipment eq) : base(eq)
        {
            mLevel = eq.mLevel;
            mType = eq.mType;
            mSlot = eq.mSlot;
            mUsedby = eq.mUsedby;
            mHitMod = eq.mHitMod;
            mMinDam = eq.mMinDam;
            mMaxDam = eq.mMaxDam;
            mArmor = eq.mArmor;
            mPhysRes = eq.mPhysRes;
            mMaxPhysResBoost = eq.mMaxPhysResBoost;
            mFireRes = eq.mFireRes;
            mMaxFireResBoost = eq.mMaxFireResBoost;
            mColdRes = eq.mColdRes;
            mMaxColdResBoost = eq.mMaxColdResBoost;
            mLightningRes = eq.mLightningRes;
            mMaxLightningResBoost = eq.mMaxLightningResBoost;
            mAcidRes = eq.mAcidRes;
            mMaxAcidResBoost = eq.mMaxAcidResBoost;
            mForceRes = eq.mForceRes;
            mMaxForceResBoost = eq.mMaxForceResBoost;
            mEffects = (ArrayList)eq.mEffects.Clone();
        }// copy constructor

        public override string wear(Player pl)
        {
            if (pl.mEQList[(int)mSlot] == null)
            {
                pl.mEQList[(int)mSlot] = this;
                pl.mInventory.Remove(this);
                return "you equip the " + mName;
            }// if
            else
                return "you are already wearing the " + ((Mob)pl.mEQList[(int)mSlot]).mName;
        }// wear

        public override string remove(Player pl)
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
