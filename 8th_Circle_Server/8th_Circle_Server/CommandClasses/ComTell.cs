using System.Collections;

namespace _8th_Circle_Server
{
    public class ComTell : CommandClass
    {
        public ComTell(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, CommandType comType, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, comType, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            CombatMob player = (CombatMob)commandQueue[1];

            mob.safeWrite("You tell " + player.GetName() + " \"" + commandQueue[2] + "\"");
            player.safeWrite(mob.GetName() + " tells you \"" + commandQueue[2] + "\"");

            return "";
        }// execute

    }// class ComTell

}// namespace _8th_Circle_Server
