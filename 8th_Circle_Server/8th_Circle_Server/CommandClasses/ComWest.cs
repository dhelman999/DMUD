using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class ComWest : CommandClass
    {
        public ComWest(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            return mob.move(command);
        }// execute

    }// class ComWest

}// namespace _8th_Circle_Server
