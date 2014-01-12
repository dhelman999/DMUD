using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public struct Action
    {
        string name;
        int cooldown;
        int useTime;
        ActionType type;
        int manaCost;

        public Action(string name, int cooldown, int useTime,
                      ActionType type, int manaCost)
        {
            this.name = name;
            this.cooldown = cooldown;
            this.useTime = useTime;
            this.type = type;
            this.manaCost = manaCost;
        }// Constructor
    }// Action

    public enum ActionType
    {
        ACTIONTYPE_START,
        ABILITY,
        SPELL,
        ACTIONTYPE_END
    }// ActionType

    public class CombatStats
    {
        public ArrayList mCombatList;
        public ArrayList mActionList;
        public ArrayList mQueuedAction;
        public int mLevel;
        public int mCurrentHp;
        public int mBaseMaxHp;
        public int mMaxHpMod;
        public int mBaseMinDam;
        public int mMinDamMod;
        public int mBaseMaxDam;
        public int mMaxDamMod;
        public int mBaseDamBonus;
        public int mDamBonusMod;
        public int mBaseHit;
        public int mHitMod;
        public int mBaseEvade;
        public int mEvadeMod;
        public int mBaseArmor;
        public int mArmorMod;
        public int mBasePhysRes;
        public int mPhysResMod;
        public int mBaseMaxPhysResBoost;
        public int mMaxPhysResBoostMod;
        public int mBaseMagicRes;
        public int mMagicResMod;
        public int mBaseMaxMagicResBoost;
        public int mMaxMagicResBoostMod;
        public int mBaseMaxMana;
        public int mMaxManaMod;
        public int mCurrentMana;

        public CombatStats()
        {
            mCombatList = new ArrayList();
            mActionList = new ArrayList();
            mQueuedAction = new ArrayList();
            mLevel = 1;
            mCurrentHp = mBaseMaxHp = 50;
            mBaseMinDam = 1;
            mBaseMaxDam = 10;
            mBaseHit = 50;
        }// Constructor

        public CombatStats(CombatStats cs)
        {
            mCombatList = (ArrayList)cs.mCombatList.Clone();
            mActionList = (ArrayList)cs.mActionList.Clone();
            mQueuedAction = (ArrayList)cs.mQueuedAction.Clone();
            mLevel = cs.mLevel;
            mCurrentHp = cs.mCurrentHp;
            mBaseMaxHp = cs.mBaseMaxHp;
            mMaxHpMod = cs.mMaxHpMod;
            mBaseMinDam = cs.mBaseMinDam;
            mMinDamMod = cs.mMinDamMod;
            mBaseMaxDam = cs.mBaseMaxDam;
            mMaxDamMod = cs.mMaxDamMod;
            mBaseDamBonus = cs.mBaseDamBonus;
            mDamBonusMod = cs.mDamBonusMod;
            mBaseHit = cs.mBaseHit;
            mHitMod = cs.mHitMod;
            mBaseEvade = cs.mBaseEvade;
            mEvadeMod = cs.mEvadeMod;
            mBaseArmor = cs.mBaseArmor;
            mArmorMod = cs.mArmorMod;
            mBasePhysRes = cs.mBasePhysRes;
            mPhysResMod = cs.mPhysResMod;
            mBaseMaxPhysResBoost = cs.mBaseMaxPhysResBoost;
            mMaxPhysResBoostMod = cs.mMaxPhysResBoostMod;
            mBaseMagicRes = cs.mBaseMagicRes;
            mMagicResMod = cs.mMagicResMod;
            mBaseMaxMagicResBoost = cs.mBaseMaxMagicResBoost;
            mMaxMagicResBoostMod = cs.mMaxMagicResBoostMod;
        }// Copy Constructor

    }// Class CombatMob

}// Namespace _8th_Circle_Server
