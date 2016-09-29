using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;

namespace _8th_Circle_Server
{
    public enum GrammarType
    {
        GRAMMAR_START,
        VERB = GRAMMAR_START,
        PREP,
        PREDICATE,
        GRAMMAR_END
    };// GrammarType

    public enum predicateType
    {
        PREDICATE_START,
        PREDICATE_OBJECT = PREDICATE_START,
        PREDICATE_PLAYER,
        PREDICATE_NPC,
        PREDICATE_PLAYER_OR_NPC,
        PREDICATE_OBJECT_OR_PLAYER,
        PREDICATE_OBJECT_OR_NPC,
        PREDICATE_ALL,
        PREDICATE_CUSTOM,
        PREDICATE_SPELL,
        PREDICATE_END
    }// predicateType

    public enum validityType
    {
        VALID_START,
        VALID_INVENTORY = VALID_START,
        VALID_LOCAL,
        VALID_INVLOCAL,
        VALID_EQUIP,
        VALID_AREA,
        VALID_GLOBAL,
        VALID_END
    };// validityType

    public enum PrepositionType
    {
        PREP_START,
        PREP_IN = PREP_START,
        PREP_ON,
        PREP_WITH,
        PREP_AT,
        PREP_FROM,
        PREP_OFF,
        PREP_END
    };// PrepositionType

    public enum commandName
    {
        COMMAND_START,
        COMMAND_LOOK = COMMAND_START,
        COMMAND_EXIT,
        COMMAND_NORTH,
        COMMAND_SOUTH,
        COMMAND_EAST,
        COMMAND_WEST,
        COMMAND_UP,
        COMMAND_DOWN,
        COMMAND_NORTHEAST,
        COMMAND_NORTHWEST,
        COMMAND_SOUTHEAST,
        COMMAND_SOUTHWEST,
        COMMAND_SAY,
        COMMAND_YELL,
        COMMAND_TELL,
        COMMAND_OPEN,
        COMMAND_CLOSE,
        COMMAND_GET,
        COMMAND_GETALL,
        COMMAND_INVENTORY,
        COMMAND_EQUIPMENT,
        COMMAND_LOCK,
        COMMAND_UNLOCK,
        COMMAND_DROP,
        COMMAND_DROPALL,
        COMMAND_SPAWN,
        COMMAND_DESTROY,
        COMMAND_USE,
        COMMAND_ATTACK,
        COMMAND_SEARCH,
        COMMAND_WHO,
        COMMAND_FULLHEAL,
        COMMAND_TELEPORT,
        COMMAND_WEAR,
        COMMAND_WEARALL,
        COMMAND_REMOVE,
        COMMAND_REMOVEALL,
        COMMAND_REST,
        COMMAND_BACKSTAB,
        COMMAND_BASH,
        COMMAND_CAST,
        COMMAND_END
    };// commandName

    public enum errorCode
    {
        E_START,
        E_OK = E_START,
        E_INVALID_SYNTAX,
        E_INVALID_COMMAND_USAGE,
        E_END
    };// errorCode

    public struct Preposition
    {
        public string name;
        public PrepositionType prepType;

        public Preposition(string name, PrepositionType prepType)
        {
            this.name = name;
            this.prepType = prepType;
        }// Constructor
    }// Prepositions

    public enum CommandType
    {
        COMTYPE_START,
        GENERAL,
        ABILITY,
        SPELL,
        COMTYPE_END
    }// CommandType

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
        // Debug
        internal const bool DEBUG = true;

        // Member Variables
        public List<Preposition> mPrepList;
        public List<Action> mAbilitySpellList;
        public ArrayList mCCList;
        public ArrayList mGrammarList;

        public CommandExecuter()
        {
            mPrepList = new List<Preposition>();
            mAbilitySpellList = new List<Action>();
            mCCList = new ArrayList();
            mGrammarList = new ArrayList();
            mAbilitySpellList = new List<Action>();

            for (AbilitySpell abilitySpell = AbilitySpell.ABILITY_SPELL_START; abilitySpell < AbilitySpell.ABILITY_SPELL_END; ++abilitySpell)
                mAbilitySpellList.Add(null);

            addCommands();
            addAbilitySpells();
        }// Constructor

        private void addCommands()
        {
            // Add Grammars
            GrammarType[] gramVerb = new GrammarType[1];
            gramVerb[0] = GrammarType.VERB;

            GrammarType[] gramVerbPred = new GrammarType[2];
            gramVerbPred[0] = GrammarType.VERB;
            gramVerbPred[1] = GrammarType.PREDICATE;

            GrammarType[] gramVerbPredPred = new GrammarType[3];
            gramVerbPredPred[0] = GrammarType.VERB;
            gramVerbPredPred[1] = GrammarType.PREDICATE;
            gramVerbPredPred[2] = GrammarType.PREDICATE;

            GrammarType[] gramVerbPrepPred = new GrammarType[3];
            gramVerbPrepPred[0] = GrammarType.VERB;
            gramVerbPrepPred[1] = GrammarType.PREP;
            gramVerbPrepPred[2] = GrammarType.PREDICATE;

            GrammarType[] gramVerbPredPrepPred = new GrammarType[4];
            gramVerbPredPrepPred[0] = GrammarType.VERB;
            gramVerbPredPrepPred[1] = GrammarType.PREDICATE;
            gramVerbPredPrepPred[2] = GrammarType.PREP;
            gramVerbPredPrepPred[3] = GrammarType.PREDICATE;

            // TODO
            // Find a way to make this indexable for easier access
            // Add Verbs
            CommandClass commandClass = new ComUp("up", "u", 1, 1, MobType.ALL, gramVerb, commandName.COMMAND_UP,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComNorth("north", "n", 5, 1, MobType.ALL, gramVerb, commandName.COMMAND_NORTH,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComNorthEast("northeast", "ne", 2, 1, MobType.ALL, gramVerb, commandName.COMMAND_NORTHEAST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComEast("east", "e", 2, 1, MobType.ALL, gramVerb, commandName.COMMAND_EAST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComSouthEast("southeast", "se", 6, 1, MobType.ALL, gramVerb, commandName.COMMAND_SOUTHEAST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComDown("down", "d", 2, 1, MobType.ALL, gramVerb, commandName.COMMAND_DOWN,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComSouth("south", "s", 5, 1, MobType.ALL, gramVerb, commandName.COMMAND_SOUTH,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComSouthWest("southwest", "sw", 6, 1, MobType.ALL, gramVerb, commandName.COMMAND_SOUTHWEST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComWest("west", "w", 1, 1, MobType.ALL, gramVerb, commandName.COMMAND_WEST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComNorthWest("northwest", "nw", 6, 1, MobType.ALL, gramVerb, commandName.COMMAND_NORTHWEST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComExit("exit", null, 2, 1, MobType.ALL, gramVerb, commandName.COMMAND_EXIT,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComLook("look", null, 1, 1, MobType.ALL, gramVerb, commandName.COMMAND_LOOK,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComLook("look", null, 1, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_LOOK,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComLook("look", null, 1, 3, MobType.ALL, gramVerbPrepPred, commandName.COMMAND_LOOK,
                predicateType.PREDICATE_ALL, predicateType.PREDICATE_END, validityType.VALID_INVLOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComSay("say", null, 3, 256, MobType.ALL, gramVerbPred, commandName.COMMAND_SAY,
               predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
               CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComYell("yell", null, 2, 256, MobType.ALL, gramVerbPred, commandName.COMMAND_YELL,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_AREA,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComTell("tell", null, 1, 256, MobType.ALL, gramVerbPredPred, commandName.COMMAND_TELL,
                predicateType.PREDICATE_PLAYER, predicateType.PREDICATE_CUSTOM, validityType.VALID_GLOBAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComOpen("open", null, 2, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_OPEN,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComFullHeal("fullheal", "fh", 2, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_FULLHEAL,
                predicateType.PREDICATE_PLAYER_OR_NPC, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComClose("close", null, 2, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_CLOSE,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComDestroy("destroy", null, 3, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_DESTROY,
                predicateType.PREDICATE_OBJECT_OR_NPC, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComSpawn("spawn", null, 3, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_SPAWN,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComGet("get", null, 1, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_GET,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_INVLOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComGet("get", null, 1, 4, MobType.ALL, gramVerbPredPrepPred, commandName.COMMAND_GET,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_OBJECT, validityType.VALID_INVLOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComGetAll("getall", "ga", 1, 1, MobType.ALL, gramVerb, commandName.COMMAND_GETALL,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_INVLOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComGetAll("getall", "ga", 1, 3, MobType.ALL, gramVerbPrepPred, commandName.COMMAND_GETALL,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_END, validityType.VALID_INVLOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComDrop("drop", null, 2, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_DROP,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_INVENTORY,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComDropAll("dropall", "da", 2, 1, MobType.ALL, gramVerb, commandName.COMMAND_DROPALL,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_INVENTORY,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComInventory("inventory", null, 1, 1, MobType.ALL, gramVerb, commandName.COMMAND_INVENTORY,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComEquipment("equipment", "eq", 2, 1, MobType.ALL, gramVerb, commandName.COMMAND_EQUIPMENT,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComLock("lock", null, 2, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_LOCK,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComUnlock("unlock", null, 2, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_UNLOCK,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComUse("use", null, 2, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_USE,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_INVLOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComWear("wear", null, 2, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_WEAR,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_END, validityType.VALID_INVENTORY,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComWearAll("wearall", "wa", 5, 1, MobType.ALL, gramVerb, commandName.COMMAND_WEARALL,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_INVENTORY,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComRemove("remove", null, 3, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_REMOVE,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_END, validityType.VALID_EQUIP,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComRemoveAll("removeall", "ra", 5, 1, MobType.ALL, gramVerb, commandName.COMMAND_REMOVEALL,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_EQUIP,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComAttack("attack", "a", 1, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_ATTACK,
                predicateType.PREDICATE_PLAYER_OR_NPC, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComTeleport("teleport", "tp", 2, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_TELEPORT,
                predicateType.PREDICATE_PLAYER_OR_NPC, predicateType.PREDICATE_END, validityType.VALID_GLOBAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComSearch("search", null, 3, 1, MobType.ALL, gramVerb, commandName.COMMAND_SEARCH,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComWho("who", null, 2, 2, MobType.ALL, gramVerbPred, commandName.COMMAND_WHO,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_GLOBAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComRest("rest", null, 1, 1, MobType.ALL, gramVerb, commandName.COMMAND_REST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.GENERAL);
            mCCList.Add(commandClass);

            commandClass = new ComBackstab("backstab", "bs", 2, 2, MobType.ROGUE, gramVerbPred, commandName.COMMAND_BACKSTAB,
                predicateType.PREDICATE_PLAYER_OR_NPC, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.ABILITY);
            mCCList.Add(commandClass);

            commandClass = new ComBash("bash", "b", 1, 1, MobType.WARRIOR, gramVerb, commandName.COMMAND_BASH,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL,
                CommandType.ABILITY);
            mCCList.Add(commandClass);

            commandClass = new ComCast("cast", "c", 1, 3, MobType.SPELLCASTER, gramVerbPredPred, commandName.COMMAND_CAST,
                predicateType.PREDICATE_SPELL, predicateType.PREDICATE_PLAYER_OR_NPC, validityType.VALID_LOCAL,
                CommandType.SPELL);
            mCCList.Add(commandClass);

            // Add prepositions
            mPrepList.Add(new Preposition("in", PrepositionType.PREP_IN));
            mPrepList.Add(new Preposition("on", PrepositionType.PREP_ON));
            mPrepList.Add(new Preposition("with", PrepositionType.PREP_WITH));
            mPrepList.Add(new Preposition("at", PrepositionType.PREP_AT));
            mPrepList.Add(new Preposition("from", PrepositionType.PREP_FROM));
            mPrepList.Add(new Preposition("off", PrepositionType.PREP_OFF));
            mPrepList.Add(new Preposition("on", PrepositionType.PREP_ON));
        }// addCommands;

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

        public void process(string command, Mob mob)
        {
            string[] tokens = command.Split(' ');
            ArrayList commandList = new ArrayList();
            ArrayList ccList = new ArrayList();
            bool foundMatch = false;
            string clientString = string.Empty;

            if (command.Equals(string.Empty))
                return;

            CommandClass currentCC = null;

            foreach (CommandClass com in mCCList)
            {
                if (tokens[0].Equals(com.shortName))
                {
                    foundMatch = true;
                    currentCC = com;
                    ccList.Add(com);
                }// if

                if (tokens[0].Length < com.matchNumber || tokens[0].Length > com.command.Length)
                    continue;

                if (com.command.Contains(tokens[0]))
                {
                    foundMatch = true;
                    currentCC = com;
                    ccList.Add(com);
                }// if
            }// foreach

            if(foundMatch)
            {
                // If the player's first token was not a verb, then it was not a valid command
                if (!foundMatch)
                {
                    if (mob.mResType == ResType.PLAYER)
                        ((CombatMob)mob).mClientHandler.safeWrite(tokens[0] + " is not a valid command");

                    return;
                }// if

                // If we have more than 1 verb with the same name, we have to identify the 
                // correct one we should use based on how many maximum tokens it allows
                if (ccList.Count > 1)
                {
                    foundMatch = false;

                    // If our command has the exact same number of tokens as the players
                    // tokenized command string, then we are done, this is the one.
                    foreach (CommandClass com in ccList)
                    {
                        if (tokens.Length == com.maxTokens)
                        {
                            currentCC = com;
                            foundMatch = true;
                            break;
                        }// if
                    }// foreach

                    // Remove all other commands and add the one we found
                    if (foundMatch)
                    {
                        ccList.Clear();
                        ccList.Add(currentCC);
                    }// if
                     // In this case, they have a variable number of tokens in their 
                     // command that sits in between two commands maximum allowable
                     // tokens, we need to find which command to use
                    else
                    {
                        foundMatch = false;

                        foreach (CommandClass com in ccList)
                        {
                            if (!(tokens.Length > com.maxTokens))
                            {
                                currentCC = com;
                                foundMatch = true;
                                ccList.Clear();
                                ccList.Add(com);
                                break;
                            }// if
                        }// foreach
                    }// else
                }// (commandList.Count > 1)

                if (tokens.Length == 1 &&
                    currentCC.grammar.Length == 1 &&
                    mob.mActionTimer == 0)
                {
                    clientString = execute(currentCC, commandList, mob);

                    if (mob.mResType == ResType.PLAYER)
                    {
                        clientString += ((CombatMob)mob).playerString();
                        ((CombatMob)mob).mClientHandler.safeWrite(clientString);
                    }

                    return;
                }// if

                errorCode error = errorCode.E_OK;

                // In this case, the player must have sent us a verb that has multiple arguements.
                // This means we have to parse the grammar of the verb and add back the appropriate
                // predicates to the command list if they even exist.
                error = populateCommandList(currentCC, command.Substring(tokens[0].Length + 1), ccList, mob);

                if (error == errorCode.E_OK)
                {
                    // All predicates must have checked out, the commandList will be correctly
                    // populated with all correct predicates in the right order according to
                    // the verbs description.  Go ahead and execute the command
                    clientString = execute(currentCC, ccList, mob);

                    if (mob.mResType == ResType.PLAYER)
                    {
                        clientString += ((CombatMob)mob).playerString();
                        ((CombatMob)mob).mClientHandler.safeWrite(clientString);
                    }
                }
                else if (error == errorCode.E_INVALID_COMMAND_USAGE)
                {
                    if (mob.mResType == ResType.PLAYER)
                        ((CombatMob)mob).mClientHandler.safeWrite("you can't use the " + currentCC.command + " command like that");
                }
                else if (error == errorCode.E_INVALID_SYNTAX)
                {
                } // do nothing it has been handled earlier
                else
                {
                    if (mob.mResType == ResType.PLAYER)
                    {
                        clientString += "You can't do that\n";
                        clientString += ((CombatMob)mob).playerString();
                        ((CombatMob)mob).mClientHandler.safeWrite(clientString);
                    }
                }

                return;
            }// if(foundMatch)
        }// process

        public string execute(CommandClass commandClass, ArrayList commandQueue, Mob mob)
        {
            string clientString = string.Empty;

            if (!(commandClass.commandName == commandName.COMMAND_REST ||
                commandClass.commandName == commandName.COMMAND_LOOK))
            {
                mob.mFlagList.Remove(MobFlags.FLAG_RESTING);
            }

            clientString = commandClass.execute(commandQueue, mob, this);
            checkEvent(commandClass, commandQueue, mob);

            return clientString;
        }// execute

        private void checkEvent(CommandClass commandClass, ArrayList commandQueue, Mob mob)
        {
            // Populate the event args with the triggers and room in which the event was triggered.
            fillEventArgs(commandClass, commandQueue, mob);

            if (commandClass.predicate1 != predicateType.PREDICATE_END &&
                commandClass.predicate1 != predicateType.PREDICATE_CUSTOM)
            {
                if ( commandClass.predicate1Value.mEventList.Count > 0)
                {
                    EventData eventData = commandClass.predicate1Value.mEventList[0];

                    // TODO
                    // This doesn't look like it supports predicate2 triggered events
                    if(eventData.commandName == commandClass.commandName &&
                       eventData.prepType == commandClass.prep1Value.prepType)
                    {
                        eventData.trigger = commandClass.commandOwner;
                        eventData.eventObject = commandClass.predicate1Value;
                        eventData.eventRoom = commandClass.predicate1Value.mCurrentRoom;
                        eventData.validity = commandClass.validity;
                        ((CombatMob)mob).mWorld.mEventHandler.enQueueEvent(eventData);
                    }// if       
                }// if
            }// else if
        }// checkEvent

        private errorCode populateCommandList(CommandClass currentCommand, string command, ArrayList commandList, Mob mob)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            predicateType targetPredicate;
            int grammarIndex = 0;
            string[] tokens;
            int predicateCount = 0;
            string errorString = currentCommand.command;

            // Loop until we have gone through all grammar specified by the commands acceptable grammar set
            while (grammarIndex < currentCommand.grammar.Length)
            {
                // We need to know which predicate we need to examine
                if (currentCommand.grammar[grammarIndex++] == GrammarType.PREDICATE)
                {
                    if (predicateCount++ == 0)
                        targetPredicate = currentCommand.predicate1;
                    else
                        targetPredicate = currentCommand.predicate2;

                    // If the predicate is a player, we only accept the very next token to
                    // search for a valid playername
                    if (targetPredicate != predicateType.PREDICATE_END && targetPredicate != predicateType.PREDICATE_CUSTOM)
                    {
                        tokens = command.Split(' ');
                        errorString += " " + tokens[0];

                        ret = doesPredicateExist(tokens[0], targetPredicate, currentCommand.validity, commandList, mob);

                        if (ret != errorCode.E_OK)
                        {
                            if (mob.mResType == ResType.PLAYER)
                                ((CombatMob)mob).mClientHandler.safeWrite("Can't find " + tokens[0]);

                            break;
                        }// if

                        if (grammarIndex < currentCommand.grammar.Length && tokens.Length > 1)
                            command = command.Substring(tokens[0].Length + 1);
                    }// if
                    // If the predicate is custom, we simply dump the rest of the command
                    // back and let processing continue
                    else if (targetPredicate == predicateType.PREDICATE_CUSTOM)
                    {
                        commandList.Add(command);
                        ret = errorCode.E_OK;
                    }// else if

                    if (ret != errorCode.E_OK)
                    {
                        if (mob.mResType == ResType.PLAYER)
                            ((CombatMob)mob).mClientHandler.safeWrite("You can't " + errorString);

                        break;
                    }// if
                }// if (currentCommand.grammar[grammarIndex++] == GrammarType.PREDICATE)
                else if (currentCommand.grammar[grammarIndex - 1] == GrammarType.PREP)
                {
                    ret = errorCode.E_INVALID_SYNTAX;
                    tokens = command.Split(' ');
                    errorString += " " + tokens[0];

                    foreach (Preposition prep in mPrepList)
                    {
                        if (prep.name.Equals(tokens[0]))
                        {
                            commandList.Add(prep);
                            ret = errorCode.E_OK;

                            if ((grammarIndex < currentCommand.grammar.Length))
                            {
                                if (command.Length > tokens[0].Length)
                                    command = command.Substring(tokens[0].Length + 1);
                                else
                                    return errorCode.E_INVALID_COMMAND_USAGE;
                            }// if
                            break;
                        }// if
                    }// foreach

                    if (ret == errorCode.E_INVALID_SYNTAX)
                    {
                        if (mob.mResType == ResType.PLAYER)
                            ((CombatMob)mob).mClientHandler.safeWrite("You are not able to " + errorString);

                        break;
                    }// if
                }// else if
            }// while (grammarIndex < currentCommand.grammar.Length)

            if (grammarIndex != currentCommand.grammar.Length)
                return errorCode.E_INVALID_COMMAND_USAGE;

            return ret;
        }// populateCommandList2

        private errorCode doesPredicateExist(string name, predicateType predType, validityType validity, 
                                             ArrayList commandQueue, Mob target)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            ArrayList targetList = new ArrayList();
            ArrayList targetPredicates = new ArrayList();
            string[] tokens = name.Split('.');

            // TODO
            // This probably needs to be turned into a mask switch statement or something
            // it can't go on like this...
            // Need to know which lists we need to search through to find the target predicate
            if (predType == predicateType.PREDICATE_OBJECT ||
                predType == predicateType.PREDICATE_OBJECT_OR_NPC || 
                predType == predicateType.PREDICATE_OBJECT_OR_PLAYER ||
                predType == predicateType.PREDICATE_ALL)
            {
                if (validity == validityType.VALID_GLOBAL)
                    targetList.Add(target.mWorld.getRes(ResType.OBJECT));
                if (validity == validityType.VALID_AREA)
                    targetList.Add(target.mCurrentArea.getRes(ResType.OBJECT));
                if (validity == validityType.VALID_LOCAL)
                {
                    targetList.Add(target.mCurrentRoom.getRes(ResType.OBJECT));
                    targetList.Add(target.mCurrentRoom.getRes(ResType.DOORWAY));
                }// if
                if(validity == validityType.VALID_INVENTORY)
                    targetList.Add(target.mInventory);
                if(validity == validityType.VALID_INVLOCAL)
                {
                    targetList.Add(target.mCurrentRoom.getRes(ResType.OBJECT));
                    targetList.Add(target.mInventory);

                    foreach(Mob cont in target.mCurrentRoom.getRes(ResType.OBJECT))
                    {
                        if (cont is Container)
                            targetList.Add(cont.mInventory);
                    }
                }// if
                if (validity == validityType.VALID_EQUIP)
                    targetList.Add(((CombatMob)target).mEQList);
            }// if

            if (predType == predicateType.PREDICATE_PLAYER || 
                predType == predicateType.PREDICATE_OBJECT_OR_PLAYER ||
                predType == predicateType.PREDICATE_PLAYER_OR_NPC ||
                predType == predicateType.PREDICATE_ALL)
            {
                if(validity == validityType.VALID_GLOBAL)
                    targetList.Add(target.mWorld.getRes(ResType.PLAYER));
                if (validity == validityType.VALID_AREA)
                    targetList.Add(target.mCurrentArea.getRes(ResType.PLAYER));
                if (validity == validityType.VALID_LOCAL)
                {
                    targetList.Add(target.mCurrentRoom.getRes(ResType.PLAYER));
                    targetList.Add(target.mCurrentRoom.getRes(ResType.DOORWAY));
                }// if
                if (validity == validityType.VALID_INVENTORY)
                    targetList.Add(target.mInventory);
                if (validity == validityType.VALID_INVLOCAL)
                {
                    targetList.Add(target.mCurrentRoom.getRes(ResType.PLAYER));
                    targetList.Add(target.mInventory);
                }// if
            }// if

            if (predType == predicateType.PREDICATE_NPC || 
                predType == predicateType.PREDICATE_OBJECT_OR_NPC ||
                predType == predicateType.PREDICATE_PLAYER_OR_NPC ||
                predType == predicateType.PREDICATE_ALL)
            {
                if (validity == validityType.VALID_GLOBAL)
                    targetList.Add(target.mWorld.getRes(ResType.NPC));
                if (validity == validityType.VALID_AREA)
                    targetList.Add(target.mCurrentArea.getRes(ResType.NPC));
                if (validity == validityType.VALID_LOCAL)
                {
                    targetList.Add(target.mCurrentRoom.getRes(ResType.NPC));
                    targetList.Add(target.mCurrentRoom.getRes(ResType.DOORWAY));
                }// if
                if (validity == validityType.VALID_INVENTORY)
                    targetList.Add(target.mInventory);
                if(validity == validityType.VALID_INVLOCAL)
                {
                    targetList.Add(target.mCurrentRoom.getRes(ResType.NPC));
                    targetList.Add(target.mInventory);
                }// if
            }// if

            if (predType == predicateType.PREDICATE_SPELL)
                targetList.Add(((CombatMob)target).mStats.mActionList);

            // TODO
            // See if we have added the same list multiple times
            foreach(List<Mob> ar in targetList)
            {
                if (ar.Count > 0)
                {
                    foreach (Mob mob in ar)
                    {
                        if (mob != null && validatePredicate(tokens[0].ToLower(), mob.exitString(target.mCurrentRoom).ToLower()))
                        {
                            ret = errorCode.E_OK;
                            targetPredicates.Add(mob);
                        }// if
                    }// foreach
                }// if
            }// foreach

            if (ret == errorCode.E_OK)
            {
                ret = errorCode.E_INVALID_SYNTAX;

                if (tokens.Length == 1)
                {
                    commandQueue.Add(targetPredicates[0]);
                    ret = errorCode.E_OK;
                }// if
                else if (tokens.Length == 2)
                {
                    try
                    {
                        int index = Int32.Parse(tokens[1])-1;

                        if (index <= targetPredicates.Count && index > 0)
                        {
                            commandQueue.Add(targetPredicates[index]);
                            ret = errorCode.E_OK;
                        }// if
                    }// try

                    catch
                    {  // silently fail, the return code will correctly error
                    }
                }// else
            }// if

            return ret;
        }// doesPredicateExist

        private bool validatePredicate(string targetPred, string cmdString)
        {
            bool found = true;
            string subString = cmdString;
            int index = 0;
            char c;

            for(int i = targetPred.Length; i > 0; --i)
            {
                c = targetPred[targetPred.Length - i];

                if(i > subString.Length)
                    return false;

                while (true)
                {
                    found = false;
                    index = subString.IndexOf(c);
                    int len = subString.Length;

                    if (index >= 0)
                    {
                        if (index < len)
                        {
                            found = true;
                            subString = subString.Substring(index + 1, len - index - 1);
                            break;
                        }// if
                    }// if
                    else
                        return false;
                }// while
            }// foreach

            return found;
        }// validatePredicate

        private void fillEventArgs(CommandClass commandClass, ArrayList commandQueue, Mob mob)
        {
            GrammarType[] grammar = commandClass.grammar;
            int predicateCount = 0;
            int prepCount = 0;

            if (mob.mResType == ResType.PLAYER)
                commandClass.commandOwner = (CombatMob)mob;
            else
                commandClass.commandOwner = mob;

            for (int i = 1; i < grammar.Length; ++i)
            {
                if (grammar[i] == GrammarType.PREDICATE)
                {
                    if ((++predicateCount == 1) && !(commandQueue[i] is string))
                        commandClass.predicate1Value = (Mob)commandQueue[i];
                    else if (!(commandQueue[i] is string))
                        commandClass.predicate2Value = (Mob)commandQueue[i];
                }// if
                else if (grammar[i] == GrammarType.PREP)
                {
                    if (++prepCount == 1)
                        commandClass.prep1Value = (Preposition)commandQueue[i];
                    else
                        commandClass.prep2Value = (Preposition)commandQueue[i];
                }// if
            }// for

            if(commandQueue.Count > 0)
                commandQueue[0] = commandClass;
        }// fillEventArgs

    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
