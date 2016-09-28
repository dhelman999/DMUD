using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class ComBackstab : CommandClass
    {
        public ComBackstab(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       GrammarType[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter ce)
        {
            CombatMob cm = (CombatMob)mob;
            string clientString = "";

            if (cm.mMobType != MobType.ROGUE)
                clientString = "you don't know how to backstab\n";
            else if (mob.mFlagList.Contains(MobFlags.FLAG_INCOMBAT))
                clientString = "you can't backstab while in combat\n";
            else if ((cm.mEQList[(int)EQSlot.PRIMARY]) == null)
                clientString = "you can't backstab without a weapon!\n";
            else
            {
                CommandClass targetCommand = null;
                CombatMob backstabTarget = ((CombatMob)commandQueue[1]);
                cm.mWorld.mCombatHandler.abilityAttack(cm, backstabTarget, ce.mAbilitySpellList[(int)AbilitySpell.ABILITY_BACKSTAB]);
                commandQueue.Clear();

                foreach (CommandClass com in ce.mCCList)
                {
                    if (com.commandName == commandName.COMMAND_ATTACK)
                    {
                        commandQueue.Add(com);
                        targetCommand = com;
                        break;
                    }
                }

                commandQueue.Add(backstabTarget);
                ce.execute(targetCommand, commandQueue, mob);
            }// else

            return clientString;
        }// execute

    }// class ComBackstab

}// namespace _8th_Circle_Server
