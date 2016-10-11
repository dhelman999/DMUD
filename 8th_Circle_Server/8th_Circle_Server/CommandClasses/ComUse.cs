﻿using System.Collections;

namespace _8th_Circle_Server
{
    public class ComUse : CommandClass
    {
        public ComUse(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, CommandType comType, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, comType, validity)
        {
            Utils.SetFlag(ref mValidity, ValidityType.INVENTORY);
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            return ((Mob)commandQueue[1]).use(mob);
        }// execute

    }// class ComUse

}// namespace _8th_Circle_Server
