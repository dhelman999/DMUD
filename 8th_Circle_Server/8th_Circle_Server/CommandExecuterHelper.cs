using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public partial class CommandExecuter
    {
        public string executeAbilityCommand(ArrayList commandQueue, Mob mob)
        {
            Command currentCommand = new Command();
            currentCommand = (Command)commandQueue[0];
            int commandIndex = 0;
            Room currentRoom = mob.mCurrentRoom;
            string clientString = string.Empty;
            CombatMob cm;

            switch (currentCommand.commandName)
            {
                case commandName.COMMAND_BACKSTAB:
                    cm = (CombatMob)mob;

                    if (cm.mMobType != MobType.ROGUE)
                        clientString = "you don't know how to backstab\n";
                    else if (mob.mFlagList.Contains(MobFlags.FLAG_INCOMBAT))
                        clientString = "you can't backstab while in combat\n";
                    else if (((Equipment)cm.mEQList[(int)EQSlot.PRIMARY]) == null)
                        clientString = "you can't backstab without a weapon!\n";
                    else
                    {
                        CombatMob backstabTarget = ((CombatMob)commandQueue[++commandIndex]);
                        cm.mWorld.mCombatHandler.backstab(cm, backstabTarget);
                        commandQueue.Clear();
                        // TODO
                        // Gotta organize the commandqueue so each slot is indexed by the 
                        // command enum for easier access to commands
                        foreach (Command com in mCommandList)
                        {
                            if (com.commandName == commandName.COMMAND_ATTACK)
                            {
                                commandQueue.Add(com);
                                break;
                            }
                        }

                        commandQueue.Add(backstabTarget);
                        execute(commandQueue, cm);
                    }// else
                    break;

                case commandName.COMMAND_BASH:
                    cm = (CombatMob)mob;

                    if (cm.mMobType != MobType.WARRIOR)
                        clientString = "you don't know how to bash";
                    else if (!mob.mFlagList.Contains(MobFlags.FLAG_INCOMBAT))
                        clientString = "you can't bash if you are not in combat\b";
                    else
                        cm.mWorld.mCombatHandler.abilityAttack(cm, null, true, Ability.BASH);
                    break;

                default:
                    clientString = "you don't know that ability\n";
                    break;
            }// switch

            return clientString;
        }// executeAbilityCommand

        public string executeSpellCommand(ArrayList commandQueue, Mob mob)
        {
            Command currentCommand = new Command();
            currentCommand = (Command)commandQueue[0];
            int commandIndex = 0;
            Room currentRoom = mob.mCurrentRoom;
            string clientString = string.Empty;

            switch (currentCommand.commandName)
            {
                default:
                    clientString = "you don't know that spell\n";
                    break;
            }// switch

            return clientString;
        }// executeAbilityCommand

    }// Class CommandHandler

}// Namespace _8th_Circle_Server
