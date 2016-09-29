using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class ComCast : CommandClass
    {
        public ComCast(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       GrammarType[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            CommandClass currentCommand = (CommandClass)commandQueue[0];
            int commandIndex = 0;
            Room currentRoom = mob.mCurrentRoom;
            string clientString = string.Empty;
            CombatMob target = null;

            switch (((Mob)commandQueue[++commandIndex]).mName)
            {
                // TODO
                // Make a spell class that inherits from Mob and handle all this crap
                case "mystic shot":
                    if (commandQueue.Count != 3 ||
                        ((Mob)commandQueue[++commandIndex] != null &&
                        !((Mob)commandQueue[commandIndex] is CombatMob)))
                    {
                        return "you can't cast mystic shot like that";
                    }

                    target = (CombatMob)commandQueue[commandIndex];
                    Action act = commandExecutioner.mAbilitySpellList[(int)AbilitySpell.SPELL_MYSTIC_SHOT];

                    if (((CombatMob)mob).mStats.mCurrentMana < act.mManaCost)
                        return "you don't have enough mana for that";

                    if (((CombatMob)mob).mStats.mCombatList.Count == 0)
                    {
                        mob.mFlagList.Add(MobFlags.FLAG_INCOMBAT);
                        ((CombatMob)mob).mStats.mCombatList.Add(target);
                        target.mStats.mCombatList.Add((CombatMob)mob);
                        target.mFlagList.Add(MobFlags.FLAG_INCOMBAT);
                        CommandClass attackCommand = null;
                        ArrayList attackQueue = new ArrayList();

                        foreach (CommandClass com in commandExecutioner.mCCList)
                        {
                            if (com.commandName == commandName.COMMAND_ATTACK)
                            {
                                attackQueue.Add(com);
                                attackCommand = com;
                                break;
                            }
                        }

                        mob.mWorld.mCombatHandler.executeSpell((CombatMob)mob, target, act);
                        attackQueue.Add(target);
                        attackCommand.execute(attackQueue, target, commandExecutioner);
                    }
                    else
                        mob.mWorld.mCombatHandler.executeSpell((CombatMob)mob, target, act);
                    break;

                case "cure":
                    if (commandQueue.Count != 3 ||
                        ((Mob)commandQueue[++commandIndex] != null &&
                        !((Mob)commandQueue[commandIndex] is CombatMob)))
                    {
                        return "you can't cast cure like that";
                    }

                    target = (CombatMob)commandQueue[commandIndex];
                    act = commandExecutioner.mAbilitySpellList[(int)AbilitySpell.SPELL_CURE];

                    if (((CombatMob)mob).mStats.mCurrentMana < act.mManaCost)
                        return "you don't have enough mana for that";

                    mob.mWorld.mCombatHandler.executeSpell((CombatMob)mob, target, act);
                    break;

                default:
                    clientString = "you don't know that spell\n";
                    break;
            }// switch

            return clientString;
        }// execute

    }// class ComCast

}// namespace _8th_Circle_Server
