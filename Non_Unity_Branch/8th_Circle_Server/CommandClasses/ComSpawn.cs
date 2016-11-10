using System;
using System.Collections;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class ComSpawn : CommandClass
    {
        public ComSpawn(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;
            Mob spawnedMob = null;

            if (commandQueue.Count > 2)
                clientString = "you can't spawn like that\n";

            try
            {
                int mobNumber = int.Parse((String)commandQueue[1]);

                if (mobNumber >= (int)MobList.MOB_END || mobNumber <= (int)MobList.MOB_START)
                    clientString = "that mob does not exist\n";

                spawnedMob = PrototypeManager.getFullGameRegisteredMob((MobList)mobNumber);
            }
            catch
            {
                clientString = "please spawn using the mobs MobList id";
            }

            Room currentRoom = mob.GetCurrentRoom();

            if (spawnedMob != null)
            {
                currentRoom.addMobResource(spawnedMob);
                clientString = "you spawn " + spawnedMob.GetName() + "\n";
                eCode = errorCode.E_OK;
            }

            return eCode;
        }// execute

    }// class ComSpawn

}// namespace _8th_Circle_Server
