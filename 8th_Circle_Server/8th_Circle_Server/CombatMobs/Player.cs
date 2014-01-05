﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class Player : CombatMob
    {
        // Member Variables
        public ClientHandler mClientHandler;

        public Player(ClientHandler ch) : base()
        {
            mFlagList.Add(mobFlags.FLAG_COMBATABLE);
            mClientHandler = ch;
            mResType = ResType.PLAYER;
        }// Constructor

        public Player() : base()
        {
            mFlagList.Add(mobFlags.FLAG_COMBATABLE);
            mClientHandler = null;
            mResType = ResType.PLAYER;
        }// Constructor

        public override string viewed(Mob viewer, Preposition prep)
        {
            if (prep.prepType == PrepositionType.PREP_AT &&
                mPrepList.Contains(PrepositionType.PREP_AT))
            {
                string clientString = string.Empty;

                if (viewer is Player)
                {
                    if (mStats.mCurrentHp > ((Player)viewer).mStats.mCurrentHp)
                        clientString += mName + " looks healthier than you";
                    else if (mStats.mCurrentHp < ((Player)viewer).mStats.mCurrentHp)
                        clientString += "you look healthier than " + mName;
                    else
                        clientString += "you both appear to have the same level of health";
                }
                return clientString + "\n" + mDescription + "\n";
            }
            else
                return "You can't look like that";
        }// viewed

        public override string fullheal()
        {
            mStats.mCurrentHp = mStats.mBaseMaxHp;
            return "you fully heal " + mName + "\n";
        }// fullheal

        public override string destroy()
        {
            return "You can't destroy a player!\n";
        }// destroy

        public string playerString()
        {
            return "\n" + mStats.mCurrentHp + "/" + (mStats.mBaseMaxHp + mStats.mMaxHpMod) + " hp\n";
        }// playerString

        // TODO
        // Clean this up
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
                    if(tmpInvCount != mInventory.Count)
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
                if (mEQList[i] is Equipment)
                    clientString += ((Equipment)mEQList[i]).remove(this) + "\n";
            }// for

            return clientString;
        }// wearall

        public override string slain(Mob mob)
        {
            return "you have been slain by " + mob.mName;
        }// slain

    }// Class Player

}// Namespace _8th_Circle_Server
