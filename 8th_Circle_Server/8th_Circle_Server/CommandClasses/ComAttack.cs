using System.Collections;

namespace _8th_Circle_Server
{
    public class ComAttack : CommandClass
    {
        public ComAttack(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            Mob target = (Mob)commandQueue[1];
            string clientString = null;

            if (!target.GetFlagList().Contains(MobFlags.FLAG_COMBATABLE) ||
                !(target is CombatMob))
            {
                return "you can't attack that";
            }

            CombatMob cm = (CombatMob)target;
            CombatMob attacker = (CombatMob)mob;

            if (cm.GetCombatList().Count == 0)
            {
                cm.GetFlagList().Add(MobFlags.FLAG_INCOMBAT);
                cm.GetCombatList().Add(attacker);
            }
            else if (!cm.GetCombatList().Contains(attacker))
                cm.GetCombatList().Add(attacker);

            if (attacker.GetCombatList().Count == 0)
            {
                attacker.GetFlagList().Add(MobFlags.FLAG_INCOMBAT);
                attacker.GetCombatList().Add(cm);
                attacker.SetPrimaryTarget(cm);
            }
            else if (!attacker.GetCombatList().Contains(cm))
            {
                attacker.GetCombatList().Add(cm);
                attacker.SetPrimaryTarget(cm);
            }

            if (!CombatHandler.sCurrentCombats.Contains(attacker))
                attacker.GetWorld().GetCombatHandler().enQueueCombat(attacker);

            return clientString;
        }// execute

    }// class ComAttack

}// namespace _8th_Circle_Server
