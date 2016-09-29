﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class ComInventory : CommandClass
    {
        public ComInventory(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       GrammarType[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "Inventory:\n\n";

            foreach (Mob mob2 in mob.mInventory)
                clientString += " " + mob2.mName + "\n";

            return clientString;
        }// execute

    }// class ComInventory

}// namespace _8th_Circle_Server