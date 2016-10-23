using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComTell : CommandClass
    {
        public ComTell(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            mPreCmdOps.Clear();
            mPostCmdOps.Clear();
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            CombatMob player = (CombatMob)commandQueue[1];

            mob.safeWrite("You tell " + player.GetName() + " \"" + commandQueue[2] + "\"");
            player.safeWrite(mob.GetName() + " tells you \"" + commandQueue[2] + "\"");

            return errorCode.E_OK;
        }// execute

    }// class ComTell

}// namespace _8th_Circle_Server
