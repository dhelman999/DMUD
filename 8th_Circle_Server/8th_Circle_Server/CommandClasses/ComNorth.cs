﻿using System.Collections;

namespace _8th_Circle_Server
{
    public class ComNorth : CommandClass
    {
        public ComNorth(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            return mob.move(mCommand);
        }// execute

    }// class ComNorth

}// namespace _8th_Circle_Server
