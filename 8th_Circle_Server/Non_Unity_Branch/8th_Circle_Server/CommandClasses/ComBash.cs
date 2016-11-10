using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComBash : CommandClass
    {
        public ComBash(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            mPreCmdOps.Clear();
            mPreCmdOps.Add(new Tuple<MobFlags, CmdOps>(MobFlags.INCOMBAT, CmdOps.CHECK_TO_PASS));
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;
            CombatMob cm = (CombatMob)mob;

            if (cm.GetMobType() != MobType.WARRIOR)
                clientString = "you don't know how to bash";
            else
            {
                cm.GetWorld().GetCombatHandler().abilityAttack(cm, null, commandExecutioner.GetASList()[(int)AbilitySpell.ABILITY_BASH]);
                eCode = errorCode.E_OK;
            }   

            return eCode;
        }// execute

    }// class ComBash

}// namespace _8th_Circle_Server
