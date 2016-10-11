using System.Collections;

namespace _8th_Circle_Server
{
    public class ComDestroy : CommandClass
    {
        public ComDestroy(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, CommandType comType, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, comType, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            return ((Mob)commandQueue[1]).destroy();
        }// execute

    }// class ComDestroy

}// namespace _8th_Circle_Server
