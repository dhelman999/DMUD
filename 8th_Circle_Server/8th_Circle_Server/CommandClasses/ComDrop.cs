using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComDrop : CommandClass
    {
        public ComDrop(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            mPreCmdOps.Clear();
            mPostCmdOps.Clear();
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            return ((Mob)commandQueue[1]).drop(mob, ref clientString);
        }// execute

    }// class ComDrop

}// namespace _8th_Circle_Server
