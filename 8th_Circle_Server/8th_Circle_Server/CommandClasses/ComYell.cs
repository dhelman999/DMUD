﻿using System.Collections;

namespace _8th_Circle_Server
{
    public class ComYell : CommandClass
    {
        public ComYell(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, CommandType comType, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, comType, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            Room currentRoom = mob.GetCurrentRoom();
            string clientString = "";

            foreach (CombatMob currentPlayer in mob.GetCurrentArea().getRes(ResType.PLAYER))
            {
                if (currentPlayer.Equals(mob))
                    clientString = "You yell " + "\"" + commandQueue[1] + "\"";
                else
                    clientString = mob.GetName() + " yells \"" + commandQueue[1] + "\"";

                currentPlayer.safeWrite(clientString);
            }// foreach

            return "";
        }// execute

    }// class ComYell

}// namespace _8th_Circle_Server
