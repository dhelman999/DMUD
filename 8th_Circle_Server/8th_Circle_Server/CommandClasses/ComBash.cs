using System.Collections;

namespace _8th_Circle_Server
{
    public class ComBash : CommandClass
    {
        public ComBash(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, CommandType comType, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, comType, validity)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            CombatMob cm = (CombatMob)mob;
            string clientString = "";

            if (cm.GetMobType() != MobType.WARRIOR)
                clientString = "you don't know how to bash";
            else if (!mob.HasFlag(MobFlags.FLAG_INCOMBAT))
                clientString = "you can't bash if you are not in combat\b";
            else
                cm.GetWorld().GetCombatHandler().abilityAttack(cm, null, commandExecutioner.GetASList()[(int)AbilitySpell.ABILITY_BASH]);

            return clientString;
        }// execute

    }// class ComBash

}// namespace _8th_Circle_Server
