using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComInventory : CommandClass
    {
        public ComInventory(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            mPreCmdOps.Clear();
            mPostCmdOps.Clear();
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            clientString = "Inventory:\n\n";

            foreach (Mob mob2 in mob.GetInv())
                clientString += " " + mob2.GetName() + "\n";

            return errorCode.E_OK;
        }// execute

    }// class ComInventory

}// namespace _8th_Circle_Server
