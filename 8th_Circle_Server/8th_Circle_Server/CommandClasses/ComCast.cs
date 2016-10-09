using System.Collections;

namespace _8th_Circle_Server
{
    public class ComCast : CommandClass
    {
        public ComCast(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            CommandClass currentCommand = (CommandClass)commandQueue[0];
            int commandIndex = 0;
            Room currentRoom = mob.GetCurrentRoom();
            string clientString = string.Empty;
            CombatMob target = null;

            switch (((Mob)commandQueue[++commandIndex]).GetName())
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
                    Action act = commandExecutioner.GetASList()[(int)AbilitySpell.SPELL_MYSTIC_SHOT];

                    if (((CombatMob)mob)[STAT.CURRENTMANA] < act.mManaCost)
                        return "you don't have enough mana for that";

                    if (((CombatMob)mob).GetCombatList().Count == 0)
                    {
                        Utils.SetFlag(ref mob.mFlags, MobFlags.FLAG_INCOMBAT);
                        ((CombatMob)mob).GetCombatList().Add(target);
                        target.GetCombatList().Add((CombatMob)mob);
                        Utils.SetFlag(ref target.mFlags, MobFlags.FLAG_INCOMBAT);
                        ArrayList attackQueue = new ArrayList();
                        CommandClass attackCommand = commandExecutioner.GetCCDict()[Utils.createTuple(commandName.COMMAND_ATTACK, 1)];
                        attackQueue.Add(attackCommand);

                        mob.GetWorld().GetCombatHandler().executeSpell((CombatMob)mob, target, act);
                        attackQueue.Add(target);
                        attackCommand.execute(attackQueue, target, commandExecutioner);
                    }
                    else
                        mob.GetWorld().GetCombatHandler().executeSpell((CombatMob)mob, target, act);
                    break;

                case "cure":
                    if (commandQueue.Count != 3 ||
                        ((Mob)commandQueue[++commandIndex] != null &&
                        !((Mob)commandQueue[commandIndex] is CombatMob)))
                    {
                        return "you can't cast cure like that";
                    }

                    target = (CombatMob)commandQueue[commandIndex];
                    act = commandExecutioner.GetASList()[(int)AbilitySpell.SPELL_CURE];

                    if (((CombatMob)mob)[STAT.CURRENTMANA] < act.mManaCost)
                        return "you don't have enough mana for that";

                    mob.GetWorld().GetCombatHandler().executeSpell((CombatMob)mob, target, act);
                    break;

                default:
                    clientString = "you don't know that spell\n";
                    break;
            }// switch

            return clientString;
        }// execute

    }// class ComCast

}// namespace _8th_Circle_Server
