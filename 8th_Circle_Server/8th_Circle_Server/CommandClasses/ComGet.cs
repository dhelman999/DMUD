using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComGet : CommandClass
    {
        public ComGet(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            Utils.SetFlag(ref mValidity, ValidityType.INVENTORY);
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;
            int commandIndex = 0;

            if (commandQueue.Count == 2)
                eCode = ((Mob)commandQueue[++commandIndex]).get(mob, ref clientString);
            else if (commandQueue.Count == 4)
            {
                eCode = ((Mob)commandQueue[++commandIndex]).get(mob,
                               ((Preposition)commandQueue[++commandIndex]).prepType,
                               (Container)commandQueue[++commandIndex],
                               ref clientString);
            }// else if
            else
                clientString = "you can't get like that";

            return eCode;
        }// execute

    }// class ComGet

}// namespace _8th_Circle_Server
