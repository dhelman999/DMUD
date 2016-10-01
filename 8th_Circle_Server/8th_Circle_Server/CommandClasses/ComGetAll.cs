using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class ComGetAll : CommandClass
    {
        public ComGetAll(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "";

            if (commandQueue.Count == 1)
                clientString = mob.getall();
            else if (commandQueue.Count == 3)
                clientString = mob.getall(((Preposition)commandQueue[1]).prepType, (Container)commandQueue[2]);
            else
                clientString = "you can't getall like that";

            return clientString;
        }// execute

    }// class ComGetAll

}// namespace _8th_Circle_Server
