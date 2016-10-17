using System;
using System.Collections;
using System.Threading;

namespace _8th_Circle_Server
{
    public class ComSearch : CommandClass
    {
        public ComSearch(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            clientString = "you start searching...\n";
            mob.SetActionTimer(4);
            Utils.SetFlag(ref mob.mFlags, MobFlags.SEARCHING);
            Thread searchThread = new Thread(() => searchTask(mob));
            searchThread.Start();

            return errorCode.E_OK;
        }// execute

        public static void searchTask(Mob mob)
        {
            String clientString = String.Empty;
            Thread.Sleep(4000);
            mob.search(ref clientString);
            mob.safeWrite(clientString);
        }// searchTask

    }// class ComSearch

}// namespace _8th_Circle_Server
