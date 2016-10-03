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

            if (!target.mFlagList.Contains(MobFlags.FLAG_COMBATABLE) ||
                !(target is CombatMob))
            {
                return "you can't attack that";
            }

            CombatMob cm = (CombatMob)target;
            CombatMob attacker = (CombatMob)mob;

            if (cm.mStats.mCombatList.Count == 0)
            {
                cm.mFlagList.Add(MobFlags.FLAG_INCOMBAT);
                cm.mStats.mCombatList.Add(attacker);
            }
            else if (!cm.mStats.mCombatList.Contains(attacker))
                cm.mStats.mCombatList.Add(attacker);

            if (attacker.mStats.mCombatList.Count == 0)
            {
                attacker.mFlagList.Add(MobFlags.FLAG_INCOMBAT);
                attacker.mStats.mCombatList.Add(cm);
                attacker.mStats.mPrimaryTarget = cm;
            }
            else if (!attacker.mStats.mCombatList.Contains(cm))
            {
                attacker.mStats.mCombatList.Add(cm);
                attacker.mStats.mPrimaryTarget = cm;
            }

            if (!CombatHandler.sCurrentCombats.Contains(attacker))
                attacker.mWorld.mCombatHandler.enQueueCombat(attacker);

            return clientString;
        }// execute

    }// class ComAttack

}// namespace _8th_Circle_Server
