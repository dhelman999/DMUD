﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class Player : Mob
    {
        // Member Variables
        public ClientHandler mClientHandler;
        public CombatStats mStats;

        public Player(ClientHandler ch) : base()
        {
            mFlagList.Add(mobFlags.FLAG_COMBATABLE);
            mClientHandler = ch;
            mResType = ResType.PLAYER;
            mStats = new CombatStats();
        }// Constructor

        public Player() : base()
        {
            mFlagList.Add(mobFlags.FLAG_COMBATABLE);
            mClientHandler = null;
            mResType = ResType.PLAYER;
            mStats = new CombatStats();
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
                    if (mStats.mCurrentHp > ((Player)viewer).mStats.mCurrentHp)
                        clientString += " you look healthier than " + viewer.mName;
                    else if (mStats.mCurrentHp < ((Player)viewer).mStats.mCurrentHp)
                        clientString += viewer.mName + " looks healthier than you";
                    else
                        clientString += "you both appear to have the same level of health";
                }
                return clientString + "\n" + mDescription;
            }
            else
                return "You can't look like that";
        }// viewed

        public override string fullheal()
        {
            mStats.mCurrentHp = mStats.mMaxHp;
            return "you fully heal " + mName;
        }// fullheal

        public override string destroy()
        {
            return "You can't destroy a player!";
        }// destroy

        public string playerString()
        {
            return "\n" + mStats.mCurrentHp + "/" + mStats.mMaxHp + " hp\n";
        }// playerString

    }// Class Player

}// Namespace _8th_Circle_Server
