﻿using System.Collections;

namespace _8th_Circle_Server
{
    public class ComFullHeal : CommandClass
    {
        public ComFullHeal(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            Utils.SetFlag(ref mPredicate1, PredicateType.PLAYER);
            Utils.SetFlag(ref mPredicate1, PredicateType.NPC);
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            return ((Mob)commandQueue[1]).fullheal();
        }// execute

    }// class ComFullHeal

}// namespace _8th_Circle_Server
