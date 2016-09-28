using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class ComTell : CommandClass
    {
        public ComTell(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       GrammarType[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter ce)
        {
            CombatMob player = (CombatMob)commandQueue[1];

            if (mob.mResType == ResType.PLAYER)
                ((CombatMob)mob).mClientHandler.safeWrite("You tell " + player.mName + " \"" + commandQueue[2] + "\"");

            player.mClientHandler.safeWrite(mob.mName + " tells you \"" + commandQueue[2] + "\"");

            return "";
        }// execute

    }// class ComTell

}// namespace _8th_Circle_Server
