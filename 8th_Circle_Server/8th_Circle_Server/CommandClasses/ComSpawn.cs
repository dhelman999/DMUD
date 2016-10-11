using System;
using System.Collections;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class ComSpawn : CommandClass
    {
        public ComSpawn(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, CommandType comType, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, comType, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "";
            // TODO
            // implement this with the new prototype manager

            return clientString;
        }// execute

    }// class ComSpawn

}// namespace _8th_Circle_Server
