using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComSay : CommandClass
    {
        public ComSay(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            mPreCmdOps.Clear();
            mPostCmdOps.Clear();
        }

        public override errorCode execute(ArrayList commandQueue, Mob talker, CommandExecuter commandExecutioner, ref String clientString)
        {
            Room currentRoom = talker.GetCurrentRoom();
            String talkerString = "You say \"" + commandQueue[1] + "\"";
            String receiversString = talker.GetName() + " says " + "\"" + commandQueue[1] + "\"";

            Utils.Broadcast(currentRoom, talker, receiversString, talkerString);

            return errorCode.E_OK;
        }// execute

    }// class ComSay

}// namespace _8th_Circle_Server
