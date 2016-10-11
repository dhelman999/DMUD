using System.Collections;

namespace _8th_Circle_Server
{
    public class ComAttack : CommandClass
    {
        public ComAttack(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, CommandType comType, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, comType, validity)
        {
            Utils.SetFlag(ref mPredicate1, PredicateType.PLAYER);
            Utils.SetFlag(ref mPredicate1, PredicateType.NPC);
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            Mob target = (Mob)commandQueue[1];
            string clientString = null;

            if (!target.HasFlag(MobFlags.COMBATABLE) || !(target is CombatMob))
                return "you can't attack that";

            CombatMob cm = (CombatMob)target;
            CombatMob attacker = (CombatMob)mob;

            if (cm.GetCombatList().Count == 0)
            {
                Utils.SetFlag(ref cm.mFlags, MobFlags.INCOMBAT);
                cm.GetCombatList().Add(attacker);
            }
            else if (!cm.GetCombatList().Contains(attacker))
                cm.GetCombatList().Add(attacker);

            if (attacker.GetCombatList().Count == 0)
            {
                Utils.SetFlag(ref attacker.mFlags, MobFlags.INCOMBAT);
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
