using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComTeleport : CommandClass
    {
        public ComTeleport(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            Utils.SetFlag(ref mPredicate1, PredicateType.PLAYER);
            Utils.SetFlag(ref mPredicate1, PredicateType.NPC);
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            return mob.teleport((Mob)commandQueue[1], ref clientString);
        }// execute

    }// class ComTeleport

}// namespace _8th_Circle_Server
