using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComWho : CommandClass
    {
        public ComWho(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            if (((String)commandQueue[1]).Equals("all"))
            {
                clientString += "\nPlayer\t\tArea\n\n";

                foreach (CombatMob pl in mob.GetWorld().getRes(ResType.PLAYER))
                    clientString += pl.GetName() + "\t\t" + pl.GetCurrentArea().GetName() + "\n";
            }// if
            else if (((String)commandQueue[1]).Equals("area"))
            {
                clientString += "\nPlayer\t\tArea\n\n";

                foreach (CombatMob pl in mob.GetCurrentArea().getRes(ResType.PLAYER))
                    clientString += pl.GetName() + "\t\t" + pl.GetCurrentArea().GetName() + "\n";
            }// else if
            else
                clientString = "you can't use who like that";

            return errorCode.E_OK;
        }// execute

    }// class ComWho

}// namespace _8th_Circle_Server
