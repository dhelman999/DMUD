using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    class CombatNpc : Npc
    {
        public CombatStats mStats;

        public CombatNpc()
        {
            mStats = new CombatStats();
            mFlagList.Add(mobFlags.FLAG_COMBATABLE);
        }// Constructor

        public CombatNpc(CombatNpc cNpc) : base(cNpc)
        {
            mStats = cNpc.mStats;
        }// Constructor

        public override string viewed(Mob viewer, Preposition prep)
        {
            bool foundAt = false;
            foreach (PrepositionType pType in mPrepList)
            {
                if (pType == PrepositionType.PREP_AT)
                {
                    foundAt = true;
                    break;
                }// if
            }// foreach

            if (foundAt && prep.prepType == PrepositionType.PREP_AT)
            {
                string clientString = string.Empty;

                if (viewer is Player)
                {
                    if (mStats.mMaxHp > ((Player)viewer).mStats.mCurrentHp)
                        clientString += " you look healthier than " + viewer.mName;
                    else if (mStats.mMaxHp < ((Player)viewer).mStats.mCurrentHp)
                        clientString += viewer.mName + " looks healthier than you";
                    else
                        clientString += "you both appear to have the same level of health";
                }
                return "\n" + mDescription;
            }
            else
                return "You can't look like that";
        }// viewed

        public override string fullheal()
        {
            mStats.mCurrentHp = mStats.mMaxHp;
            return "you fully heal " + mName;
        }// fullheal

    }// Class CombatNpc

}// namespace _8th_Circle_Server
