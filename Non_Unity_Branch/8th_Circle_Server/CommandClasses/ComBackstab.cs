﻿using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComBackstab : CommandClass
    {
        public ComBackstab(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            Utils.SetFlag(ref mPredicate1, PredicateType.PLAYER);
            Utils.SetFlag(ref mPredicate1, PredicateType.NPC);
        }

        public override errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            CombatMob cm = (CombatMob)mob;
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;

            if (cm.GetMobType() != MobType.ROGUE)
                clientString = "you don't know how to backstab\n";
            else if (cm[EQSlot.PRIMARY] == null)
                clientString = "you can't backstab without a weapon!\n";
            else
            {
                CombatMob backstabTarget = ((CombatMob)commandQueue[1]);
                cm.GetWorld().GetCombatHandler().abilityAttack(cm, backstabTarget, commandExecutioner.GetASList()[(int)AbilitySpell.ABILITY_BACKSTAB]);
                commandQueue.Clear();

                CommandClass targetCommand = commandExecutioner.GetCCDict()[Utils.createTuple(CommandName.COMMAND_ATTACK, 2)];
                commandQueue.Add(targetCommand);

                commandQueue.Add(backstabTarget);
                eCode = commandExecutioner.execute(targetCommand, commandQueue, mob, ref clientString);
            }// else

            return eCode;
        }// execute

    }// class ComBackstab

}// namespace _8th_Circle_Server
