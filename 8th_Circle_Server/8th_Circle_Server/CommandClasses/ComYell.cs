﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class ComYell : CommandClass
    {
        public ComYell(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       GrammarType[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter ce)
        {
            Room currentRoom = mob.mCurrentRoom;
            string clientString = "";

            foreach (CombatMob currentPlayer in mob.mCurrentArea.getRes(ResType.PLAYER))
            {
                if (currentPlayer.Equals(mob))
                    clientString = "You yell " + "\"" + commandQueue[1] + "\"";
                else
                    clientString = mob.mName + " yells \"" + commandQueue[1] + "\"";

                currentPlayer.mClientHandler.safeWrite(clientString);
            }// foreach

            return "";
        }// execute

    }// class ComYell

}// namespace _8th_Circle_Server
