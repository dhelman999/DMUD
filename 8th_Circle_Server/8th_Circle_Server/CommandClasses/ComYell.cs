using System.Collections;

namespace _8th_Circle_Server
{
    public class ComYell : CommandClass
    {
        public ComYell(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob yeller, CommandExecuter commandExecutioner)
        {
            string yellerString = "You yell " + "\"" + commandQueue[1] + "\"";
            string receiversString = yeller.GetName() + " yells \"" + commandQueue[1] + "\"";
            Utils.broadcast(yeller.GetCurrentArea(), yeller, receiversString, yellerString);

            return "";
        }// execute

    }// class ComYell

}// namespace _8th_Circle_Server
