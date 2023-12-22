using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComNorth : CommandClass
    {
        public ComNorth(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            return mob.move(mCommand, ref clientString);
        }// execute

    }// class ComNorth

}// namespace _8th_Circle_Server
