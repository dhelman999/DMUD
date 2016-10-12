using System.Collections;

namespace _8th_Circle_Server
{
    public class ComGetAll : CommandClass
    {
        public ComGetAll(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, CommandType comType, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, comType, validity)
        {
            Utils.SetFlag(ref mValidity, ValidityType.INVENTORY);
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "";

            if (commandQueue.Count == 1)
                clientString = mob.getall();
            else if (commandQueue.Count == 3)
                clientString = mob.getall(((Preposition)commandQueue[1]).prepType, (Container)commandQueue[2]);
            else
                clientString = "you can't getall like that";

            return clientString;
        }// execute

    }// class ComGetAll

}// namespace _8th_Circle_Server
