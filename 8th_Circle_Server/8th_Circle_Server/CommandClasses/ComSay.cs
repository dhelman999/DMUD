using System.Collections;

namespace _8th_Circle_Server
{
    public class ComSay : CommandClass
    {
        public ComSay(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            Room currentRoom = mob.mCurrentRoom;
            string clientString = "";

            foreach (CombatMob currentPlayer in currentRoom.getRes(ResType.PLAYER))
            {
                if (currentPlayer.Equals(mob))
                    clientString = "You say \"" + commandQueue[1] + "\"";
                else
                    clientString = mob.mName + " says " + "\"" + commandQueue[1] + "\"";

                currentPlayer.safeWrite(clientString);
            }// foreach

            return "";
        }// execute

    }// class ComSay

}// namespace _8th_Circle_Server
