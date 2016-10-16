using System;
using System.Collections;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class ComSpawn : CommandClass
    {
        public ComSpawn(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = string.Empty;
            Mob spawnedMob = null;

            if (commandQueue.Count > 2)
                return "you can't spawn like that\n";

            try
            {
                int mobNumber = int.Parse((string)commandQueue[1]);

                if (mobNumber >= (int)MobList.MOB_END || mobNumber <= (int)MobList.MOB_START)
                    return "that mob does not exist\n";

                spawnedMob = PrototypeManager.getFullGameRegisteredMob((MobList)mobNumber);
            }
            catch
            {
                return "please spawn using the mobs MobList id";
            }

            Room currentRoom = mob.GetCurrentRoom();

            if (spawnedMob != null)
            {
                currentRoom.addMobResource(spawnedMob);
                clientString = "you spawn " + spawnedMob.GetName() + "\n";
            }

            return clientString;
        }// execute

    }// class ComSpawn

}// namespace _8th_Circle_Server
