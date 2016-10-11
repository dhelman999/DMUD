﻿using System.Collections;

namespace _8th_Circle_Server
{
    public class ComRemoveAll : CommandClass
    {
        public ComRemoveAll(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, CommandType comType, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, comType, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            return mob.removeall();
        }// execute

    }// class ComRemoveAll

}// namespace _8th_Circle_Server
