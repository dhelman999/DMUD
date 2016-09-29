using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class ComEquipment : CommandClass
    {
        public ComEquipment(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       GrammarType[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "Equipment:\n\n";
            CombatMob player = (CombatMob)mob;

            for (EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
            {
                if (player.mEQList[(int)slot] == null)
                    clientString += slot + ":\n";
                else
                    clientString += slot.ToString() + ": " + (player.mEQList[(int)slot]).mName + "\n";
            }// for

            return clientString;
        }// execute

    }// class ComEquipment

}// namespace _8th_Circle_Server
