using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public enum AbilitySpell
    {
        ABILITY_SPELL_START,

        // Abilities
        ABILITY_BACKSTAB,
        ABILITY_BASH,

        // Spells
        SPELL_MYSTIC_SHOT,
        SPELL_CURE,

        ABILITY_SPELL_END
    }// AbilitySpell

    public partial class CommandExecuter
    {
        public ArrayList mAbilitySpellList;

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
                        cm.mWorld.mCombatHandler.abilityAttack(cm, backstabTarget,
                            (Action)mAbilitySpellList[(int)AbilitySpell.ABILITY_BACKSTAB]);
                        commandQueue.Clear();

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
                        cm.mWorld.mCombatHandler.abilityAttack(cm, null, 
                            (Action)mAbilitySpellList[(int)AbilitySpell.ABILITY_BASH]);
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
            CombatMob target = null;
            
            switch (((Mob)commandQueue[++commandIndex]).mName)
            {
                case "mystic shot":
                    if (commandQueue.Count != 3 ||
                        ((Mob)commandQueue[++commandIndex] != null &&
                        !((Mob)commandQueue[commandIndex] is CombatMob)))
                        return "you can't cast mystic shot like that";
                    target = (CombatMob)commandQueue[commandIndex];
                    Action act = (Action)mAbilitySpellList[(int)AbilitySpell.SPELL_MYSTIC_SHOT];
                    if(((CombatMob)mob).mStats.mCurrentMana < act.mManaCost)
                        return "you don't have enough mana for that";

                    if (((CombatMob)mob).mStats.mCombatList.Count == 0)
                    {
                        mob.mFlagList.Add(MobFlags.FLAG_INCOMBAT);
                        ((CombatMob)mob).mStats.mCombatList.Add(target);
                        target.mStats.mCombatList.Add(mob);
                        target.mFlagList.Add(MobFlags.FLAG_INCOMBAT);                       

                        commandQueue.Clear();
                        foreach (Command com in mCommandList)
                        {
                            if (com.commandName == commandName.COMMAND_ATTACK)
                            {
                                commandQueue.Add(com);
                                break;
                            }
                        }
                        mob.mWorld.mCombatHandler.executeSpell((CombatMob)mob, target, act);
                        commandQueue.Add(target);
                        execute(commandQueue, mob);
                    }
                    else
                        mob.mWorld.mCombatHandler.executeSpell((CombatMob)mob, target, act);
                    break;

                case "cure":
                    if (commandQueue.Count != 3 ||
                        ((Mob)commandQueue[++commandIndex] != null &&
                        !((Mob)commandQueue[commandIndex] is CombatMob)))
                        return "you can't cast cure like that";
                    target = (CombatMob)commandQueue[commandIndex];
                    act = (Action)mAbilitySpellList[(int)AbilitySpell.SPELL_CURE];
                    if (((CombatMob)mob).mStats.mCurrentMana < act.mManaCost)
                        return "you don't have enough mana for that";

                    mob.mWorld.mCombatHandler.executeSpell((CombatMob)mob, target, act);
                    break;

                default:
                    clientString = "you don't know that spell\n";
                    break;
            }// switch

            return clientString;
        }// executeAbilityCommand

        private void addAbilitySpells()
        {
            Action act = new Action("bash", 4, 4, ActionType.ABILITY);
            act.mHitBonus = 5;
            act.mEvadable = true;
            act.mDamScaling = DamageScaling.PERLEVEL;
            act.mDamageBonus = 1;
            act.mBaseMinDamage = 1;
            act.mBaseMaxDamage = 6;
            act.mAbilitySpell = AbilitySpell.ABILITY_BASH;
            act.mWeaponRequired = false;
            mAbilitySpellList[(int)AbilitySpell.ABILITY_BASH] = act;

            act = new Action("backstab", 4, 4, ActionType.ABILITY);
            act.mHitBonus = 10;
            act.mEvadable = true;
            act.mDamScaling = DamageScaling.DAMAGEMULTPERLEVEL;
            act.mDamageMult = 2;
            act.mBaseMinDamage = 2;
            act.mBaseMaxDamage = 5;
            act.mWeaponRequired = true;
            act.mAbilitySpell = AbilitySpell.ABILITY_BACKSTAB;
            mAbilitySpellList[(int)AbilitySpell.ABILITY_BACKSTAB] = act;

            act = new Action("mystic shot", 4, 4, ActionType.SPELL);
            act.mDamType = DamageType.MAGICAL;
            act.mResistable = true;
            act.mDamScaling = DamageScaling.PERLEVEL;
            act.mBaseMinDamage = 2;
            act.mBaseMaxDamage = 9;
            act.mAbilitySpell = AbilitySpell.SPELL_MYSTIC_SHOT;
            act.mWeaponRequired = false;
            act.mManaCost = 5;
            mAbilitySpellList[(int)AbilitySpell.SPELL_MYSTIC_SHOT] = act;

            act = new Action("cure", 4, 4, ActionType.SPELL);
            act.mDamType = DamageType.HEALING;
            act.mResistable = false;
            act.mDamScaling = DamageScaling.PERLEVEL;
            act.mBaseMinDamage = 3;
            act.mBaseMaxDamage = 5;
            act.mAbilitySpell = AbilitySpell.SPELL_CURE;
            act.mWeaponRequired = false;
            act.mManaCost = 5;
            mAbilitySpellList[(int)AbilitySpell.SPELL_CURE] = act;
        }// addAbilitySpells

    }// Class CommandHandler

}// Namespace _8th_Circle_Server
