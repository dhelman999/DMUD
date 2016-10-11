using System.Collections;

namespace _8th_Circle_Server
{
    public class ComSay : CommandClass
    {
        public ComSay(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, CommandType comType, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, comType, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            Room currentRoom = mob.GetCurrentRoom();
            string clientString = "";

            foreach (CombatMob currentPlayer in currentRoom.getRes(ResType.PLAYER))
            {
                if (currentPlayer.Equals(mob))
                    clientString = "You say \"" + commandQueue[1] + "\"";
                else
                    clientString = mob.GetName() + " says " + "\"" + commandQueue[1] + "\"";

                currentPlayer.safeWrite(clientString);
            }// foreach

            return "";
        }// execute

    }// class ComSay

}// namespace _8th_Circle_Server
