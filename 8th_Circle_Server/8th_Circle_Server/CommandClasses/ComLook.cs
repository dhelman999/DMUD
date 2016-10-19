using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComLook : CommandClass
    {
        public ComLook(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            Utils.SetFlag(ref mPredicate1, PredicateType.PLAYER);
            Utils.SetFlag(ref mPredicate1, PredicateType.NPC);
            Utils.SetFlag(ref mPredicate1, PredicateType.OBJECT);

            if (maxTokens == 3)
                Utils.SetFlag(ref mValidity, ValidityType.INVENTORY);
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            Room currentRoom = mob.GetCurrentRoom();
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (commandQueue.Count == 1)
                eCode = currentRoom.viewed(ref clientString);
            // The player looked in a direction, print out that connected rooms location
            else if (commandQueue.Count == 2)
                eCode = mob.GetCurrentRoom().viewed((String)commandQueue[1], ref clientString);
            // Player must have looked at or in something
            else if (commandQueue.Count == 3)
                eCode = ((Mob)commandQueue[2]).viewed(mob, (Preposition)commandQueue[1], ref clientString);
            else
                eCode = ((Mob)commandQueue[2]).viewed(mob, (Preposition)commandQueue[1], ref clientString);

            return eCode;
        }// execute

    }// class ComLook

}// namespace _8th_Circle_Server
