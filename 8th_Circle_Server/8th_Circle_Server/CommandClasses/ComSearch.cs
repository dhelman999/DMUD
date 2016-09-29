﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace _8th_Circle_Server
{
    public class ComSearch : CommandClass
    {
        public ComSearch(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       GrammarType[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "you start searching...\n";
            mob.mActionTimer = 4;
            mob.mFlagList.Add(MobFlags.FLAG_SEARCHING);
            Thread searchThread = new Thread(() => searchTask(mob));
            searchThread.Start();

            return clientString;
        }// execute

        public static void searchTask(Mob mob)
        {
            Thread.Sleep(4000);
            string searchResult = mob.search();

            if (mob.mResType == ResType.PLAYER)
                ((CombatMob)mob).mClientHandler.safeWrite(searchResult);

            mob.mFlagList.Remove(MobFlags.FLAG_SEARCHING);
        }// searchTask

    }// class ComSearch

}// namespace _8th_Circle_Server