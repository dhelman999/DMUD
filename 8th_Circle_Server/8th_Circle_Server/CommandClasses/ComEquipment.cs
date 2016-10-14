using System.Collections;

namespace _8th_Circle_Server
{
    public class ComEquipment : CommandClass
    {
        public ComEquipment(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            string clientString = "Equipment:\n\n";
            CombatMob player = (CombatMob)mob;

            for (EQSlot slot = EQSlot.EQSLOT_START; slot < EQSlot.EQSLOT_END; ++slot)
            {
                if (player[slot] == null)
                    clientString += slot + ":\n";
                else
                    clientString += slot.ToString() + ": " + player[slot].GetName() + "\n";
            }// for

            return clientString;
        }// execute

    }// class ComEquipment

}// namespace _8th_Circle_Server
