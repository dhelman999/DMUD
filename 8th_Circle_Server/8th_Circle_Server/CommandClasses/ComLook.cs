using System.Collections;

namespace _8th_Circle_Server
{
    public class ComLook : CommandClass
    {
        public ComLook(string command, string shortName, int matchNumber, int maxTokens, MobType type,
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

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            Room currentRoom = mob.GetCurrentRoom();
            string clientString = "";

            if (commandQueue.Count == 1)
                clientString = currentRoom.viewed();
            // The player looked in a direction, print out that connected rooms
            // location
            else if (commandQueue.Count == 2)
                clientString = mob.GetCurrentRoom().viewed((string)commandQueue[1]);
            else if (commandQueue.Count == 3)
                clientString = ((Mob)commandQueue[2]).viewed(mob, (Preposition)commandQueue[1]);
            else
                clientString = ((Mob)commandQueue[2]).viewed(mob, (Preposition)commandQueue[1]);

            return clientString;
        }// execute

    }// class ComLook

}// namespace _8th_Circle_Server
