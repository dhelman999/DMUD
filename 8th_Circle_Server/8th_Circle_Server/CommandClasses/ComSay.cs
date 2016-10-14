using System.Collections;

namespace _8th_Circle_Server
{
    public class ComSay : CommandClass
    {
        public ComSay(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob talker, CommandExecuter commandExecutioner)
        {
            Room currentRoom = talker.GetCurrentRoom();
            string talkerString = "You say \"" + commandQueue[1] + "\"";
            string receiversString = talker.GetName() + " says " + "\"" + commandQueue[1] + "\"";

            Utils.broadcast(currentRoom, talker, receiversString, talkerString);

            return "";
        }// execute

    }// class ComSay

}// namespace _8th_Circle_Server
