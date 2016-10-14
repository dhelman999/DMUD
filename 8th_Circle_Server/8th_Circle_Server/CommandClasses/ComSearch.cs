using System.Collections;
using System.Threading;

namespace _8th_Circle_Server
{
    public class ComSearch : CommandClass
    {
        public ComSearch(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, comType, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "you start searching...\n";
            mob.SetActionTimer(4);
            Utils.SetFlag(ref mob.mFlags, MobFlags.SEARCHING);
            Thread searchThread = new Thread(() => searchTask(mob));
            searchThread.Start();

            return clientString;
        }// execute

        public static void searchTask(Mob mob)
        {
            Thread.Sleep(4000);
            mob.safeWrite(mob.search());
        }// searchTask

    }// class ComSearch

}// namespace _8th_Circle_Server
