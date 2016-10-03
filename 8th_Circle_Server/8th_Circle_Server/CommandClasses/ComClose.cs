﻿using System.Collections;

namespace _8th_Circle_Server
{
    public class ComClose : CommandClass
    {
        public ComClose(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            return ((Mob)commandQueue[1]).close(mob);
        }// execute

    }// class ComClose

}// namespace _8th_Circle_Server
