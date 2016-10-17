using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComGetAll : CommandClass
    {
        public ComGetAll(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            Utils.SetFlag(ref mValidity, ValidityType.INVENTORY);
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (commandQueue.Count == 1)
                eCode = mob.getall(ref clientString);
            else if (commandQueue.Count == 3)
                eCode = mob.getall(((Preposition)commandQueue[1]).prepType, (Container)commandQueue[2], ref clientString);
            else
                clientString = "you can't getall like that";

            return eCode;
        }// execute

    }// class ComGetAll

}// namespace _8th_Circle_Server
