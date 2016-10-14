﻿using System.Collections;

namespace _8th_Circle_Server
{
    public class ComInventory : CommandClass
    {
        public ComInventory(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, comType, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "Inventory:\n\n";

            foreach (Mob mob2 in mob.GetInv())
                clientString += " " + mob2.GetName() + "\n";

            return clientString;
        }// execute

    }// class ComInventory

}// namespace _8th_Circle_Server
