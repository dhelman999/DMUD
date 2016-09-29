using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class ComLook : CommandClass
    {
        public ComLook(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       GrammarType[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) : 
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1,predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            Room currentRoom = mob.mCurrentRoom;
            string clientString = "";

            if (commandQueue.Count == 0)
                clientString = currentRoom.viewed();
            // The player looked in a direction, print out that connected rooms
            // location
            else if (commandQueue.Count == 1)
                clientString = mob.mCurrentRoom.viewed((string)commandQueue[1]);
            else if (commandQueue.Count == 2)
                clientString = ((Mob)commandQueue[2]).viewed(mob, (Preposition)commandQueue[1]);
            else
                clientString = ((Mob)commandQueue[2]).viewed(mob, (Preposition)commandQueue[1]);

            return clientString;
        }// execute

    }// class ComLook

}// namespace _8th_Circle_Server
