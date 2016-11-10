using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class ComCast : CommandClass
    {
        public ComCast(String command, String shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, CommandName CommandName, PredicateType predicate1,
                       PredicateType predicate2, ValidityType validity = ValidityType.LOCAL) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, CommandName, predicate1, predicate2, validity)
        {
            Utils.SetFlag(ref mPredicate2, PredicateType.PLAYER);
            Utils.SetFlag(ref mPredicate2, PredicateType.NPC);

            mPreCmdOps.Clear();
            mPreCmdOps.Add(new Tuple<MobFlags, CmdOps>(MobFlags.SEARCHING, CmdOps.CHECK_TO_FAIL));
        }

        public override errorCode execute(ArrayList commandQueue, Mob caster, CommandExecuter commandExecutioner, ref String clientString)
        {
            CommandClass currentCommand = (CommandClass)commandQueue[0];
            int commandIndex = 0;
            Room currentRoom = caster.GetCurrentRoom();
            errorCode eCode = errorCode.E_INVALID_COMMAND_USAGE;
            CombatMob target = null;

            switch (((Mob)commandQueue[++commandIndex]).GetName())
            {
                case "mysticshot":
                    if (commandQueue.Count != 3 ||
                        ((Mob)commandQueue[++commandIndex] != null &&
                        !((Mob)commandQueue[commandIndex] is CombatMob)))
                    {
                        clientString = "you can't cast mystic shot like that";
                    }

                    target = (CombatMob)commandQueue[commandIndex];
                    Action act = commandExecutioner.GetASList()[(int)AbilitySpell.SPELL_MYSTIC_SHOT];

                    if (((CombatMob)caster)[STAT.CURRENTMANA] < act.mManaCost)
                        clientString = "you don't have enough mana for that";

                    if (((CombatMob)caster).GetCombatList().Count == 0)
                    {
                        Utils.SetFlag(ref caster.mFlags, MobFlags.INCOMBAT);
                        ((CombatMob)caster).GetCombatList().Add(target);
                        target.GetCombatList().Add((CombatMob)caster);
                        Utils.SetFlag(ref target.mFlags, MobFlags.INCOMBAT);
                        ArrayList attackQueue = new ArrayList();
                        CommandClass attackCommand = commandExecutioner.GetCCDict()[Utils.createTuple(CommandName.COMMAND_ATTACK, 2)];
                        attackQueue.Add(attackCommand);
                        String defenderString = String.Empty;

                        caster.GetWorld().GetCombatHandler().executeSpell((CombatMob)caster, target, act);
                        attackQueue.Add(caster);
                        attackCommand.execute(attackQueue, target, commandExecutioner, ref defenderString);
                        eCode = errorCode.E_OK;
                    }
                    else
                    {
                        caster.GetWorld().GetCombatHandler().executeSpell((CombatMob)caster, target, act);
                        eCode = errorCode.E_OK;
                    }
                        
                    break;

                case "cure":
                    if (commandQueue.Count != 3 ||
                        ((Mob)commandQueue[++commandIndex] != null &&
                        !((Mob)commandQueue[commandIndex] is CombatMob)))
                    {
                        clientString = "you can't cast cure like that";
                    }

                    target = (CombatMob)commandQueue[commandIndex];
                    act = commandExecutioner.GetASList()[(int)AbilitySpell.SPELL_CURE];

                    if (((CombatMob)caster)[STAT.CURRENTMANA] < act.mManaCost)
                    {
                        clientString = "you don't have enough mana for that";
                        break;
                    }     

                    caster.GetWorld().GetCombatHandler().executeSpell((CombatMob)caster, target, act);
                    eCode = errorCode.E_OK;
                    break;

                default:
                    clientString = "you don't know that spell\n";
                    break;
            }// switch

            return eCode;
        }// execute

    }// class ComCast

}// namespace _8th_Circle_Server
