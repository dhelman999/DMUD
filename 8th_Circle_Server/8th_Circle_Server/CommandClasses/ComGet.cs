using System.Collections;

namespace _8th_Circle_Server
{
    public class ComGet : CommandClass
    {
        public ComGet(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            Utils.SetFlag(ref mValidity, ValidityType.INVENTORY);
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = null;
            int commandIndex = 0;

            if (commandQueue.Count == 2)
                clientString = ((Mob)commandQueue[++commandIndex]).get(mob);
            else if (commandQueue.Count == 4)
            {
                clientString = ((Mob)commandQueue[++commandIndex]).get(mob,
                               ((Preposition)commandQueue[++commandIndex]).prepType,
                               (Container)commandQueue[++commandIndex]);
            }// else if
            else
                clientString = "you can't get like that";

            return clientString;
        }// execute

    }// class ComGet

}// namespace _8th_Circle_Server
