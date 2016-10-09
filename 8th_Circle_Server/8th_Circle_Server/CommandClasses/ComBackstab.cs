using System.Collections;

namespace _8th_Circle_Server
{
    public class ComBackstab : CommandClass
    {
        public ComBackstab(string command, string shortName, int matchNumber, int maxTokens, MobType type,
                       Grammar[] grammar, commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity, CommandType comType) :
            base(command, shortName, matchNumber, maxTokens, type, grammar, commandName, predicate1, predicate2, validity, comType)
        {
        }

        public override string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            CombatMob cm = (CombatMob)mob;
            string clientString = "";

            if (cm.GetMobType() != MobType.ROGUE)
                clientString = "you don't know how to backstab\n";
            else if (mob.GetFlagList().Contains(MobFlags.FLAG_INCOMBAT))
                clientString = "you can't backstab while in combat\n";
            else if ((cm[EQSlot.PRIMARY]) == null)
                clientString = "you can't backstab without a weapon!\n";
            else
            {
                CombatMob backstabTarget = ((CombatMob)commandQueue[1]);
                cm.GetWorld().GetCombatHandler().abilityAttack(cm, backstabTarget, commandExecutioner.GetASList()[(int)AbilitySpell.ABILITY_BACKSTAB]);
                commandQueue.Clear();

                CommandClass targetCommand = commandExecutioner.GetCCDict()[Utils.createTuple(commandName.COMMAND_ATTACK, 1)];
                commandQueue.Add(targetCommand);

                commandQueue.Add(backstabTarget);
                commandExecutioner.execute(targetCommand, commandQueue, mob);
            }// else

            return clientString;
        }// execute

    }// class ComBackstab

}// namespace _8th_Circle_Server
