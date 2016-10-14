using System.Collections;
using System.Collections.Generic;

namespace _8th_Circle_Server
{
    public class ComAttack : CommandClass
    {
        public ComAttack(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, comType, validity)
        {
            Utils.SetFlag(ref mPredicate1, PredicateType.PLAYER);
            Utils.SetFlag(ref mPredicate1, PredicateType.NPC);
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            CombatMob attacker = (CombatMob)mob;
            CombatMob target = (CombatMob)commandQueue[1];
            string clientString = null;

            if (!target.HasFlag(MobFlags.COMBATABLE) || !(target is CombatMob))
                return "you can't attack that";

            if (target.GetCombatList().Count == 0)
            {
                Utils.SetFlag(ref target.mFlags, MobFlags.INCOMBAT);
                target.GetCombatList().Add(attacker);
            }
            else if (!target.GetCombatList().Contains(attacker))
                target.GetCombatList().Add(attacker);

            if (attacker.GetCombatList().Count == 0)
            {
                Utils.SetFlag(ref attacker.mFlags, MobFlags.INCOMBAT);
                attacker.GetCombatList().Add(target);
                attacker.SetPrimaryTarget(target);
            }
            else if (!attacker.GetCombatList().Contains(target))
            {
                attacker.GetCombatList().Add(target);
                attacker.SetPrimaryTarget(target);
            }

            CombatHandler combatHandler = attacker.GetWorld().GetCombatHandler();
            Queue<CombatMob> combatQueue = combatHandler.GetCombatQueue();

            lock (combatHandler.GetCombatLock())
            {
                if (!combatQueue.Contains(attacker))
                    combatHandler.enQueueCombat(attacker);
            }

            return clientString;
        }// execute

    }// class ComAttack

}// namespace _8th_Circle_Server
