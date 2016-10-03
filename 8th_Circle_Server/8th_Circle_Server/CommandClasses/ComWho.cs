using System.Collections;

namespace _8th_Circle_Server
{
    public class ComWho : CommandClass
    {
        public ComWho(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "";

            if (((string)commandQueue[1]).Equals("all"))
            {
                clientString = "\nPlayer\t\tArea\n\n";

                foreach (CombatMob pl in mob.mWorld.getRes(ResType.PLAYER))
                    clientString += pl.mName + "\t\t" + pl.mCurrentArea.mName + "\n";
            }// if
            else if (((string)commandQueue[1]).Equals("area"))
            {
                clientString = "\nPlayer\t\tArea\n\n";

                foreach (CombatMob pl in mob.mCurrentArea.getRes(ResType.PLAYER))
                    clientString += pl.mName + "\t\t" + pl.mCurrentArea.mName + "\n";
            }// else if
            else
                return "you can't use who like that";

            return clientString;
        }// execute

    }// class ComWho

}// namespace _8th_Circle_Server
