using System;
using System.Collections.Generic;
using System.Collections;

namespace _8th_Circle_Server
{
    // Class CommandExecuter
    // Main parser and logic to execute the clients commands.  Design is that each area has its own command executer to promote resource
    // independence from other threads.  The parser defines a language that always starts with a verb that is followed by various combinations
    // of predicates and prepisitions.  Predicates are all objects in the game, from players to containers to doors to NPCs.  Prepisitions
    // are valid words used to interact with predicates, like 'in', 'on' etc.
    public class CommandExecuter
    {
        private Dictionary<PrepositionType, Preposition> mPrepDict;
        private List<Action> mAbilitySpellList;
        private Dictionary<Tuple<CommandName, int>, CommandClass> mCCDict;
        private Dictionary<GrammarType, Grammar[]> mGrammarDict;

        public CommandExecuter()
        {
            mPrepDict = new Dictionary<PrepositionType, Preposition>();
            mAbilitySpellList = new List<Action>();
            mCCDict = new Dictionary<Tuple<CommandName, int>, CommandClass>();
            mGrammarDict = new Dictionary<GrammarType, Grammar[]>();

            addGrammar();
            addCommands();
            addPrepositions();
            addAbilitySpells();
        }// Constructor

        // Add all valid combinations of grammar to each executers dictionary.  This is possible to have different grammars and allowable
        // verbs for each area, but currently they are all shared.
        private void addGrammar()
        {
            Grammar[] gramVerb = new Grammar[1];
            gramVerb[0] = Grammar.VERB;
            mGrammarDict.Add(GrammarType.GRAMMAR_VERB, gramVerb);

            Grammar[] gramVerbPred = new Grammar[2];
            gramVerbPred[0] = Grammar.VERB;
            gramVerbPred[1] = Grammar.PREDICATE;
            mGrammarDict.Add(GrammarType.GRAMMAR_VERB_PRED, gramVerbPred);

            Grammar[] gramVerbPredPred = new Grammar[3];
            gramVerbPredPred[0] = Grammar.VERB;
            gramVerbPredPred[1] = Grammar.PREDICATE;
            gramVerbPredPred[2] = Grammar.PREDICATE;
            mGrammarDict.Add(GrammarType.GRAMMAR_VERB_PRED_PRED, gramVerbPredPred);

            Grammar[] gramVerbPrepPred = new Grammar[3];
            gramVerbPrepPred[0] = Grammar.VERB;
            gramVerbPrepPred[1] = Grammar.PREP;
            gramVerbPrepPred[2] = Grammar.PREDICATE;
            mGrammarDict.Add(GrammarType.GRAMMAR_VERB_PREP_PRED, gramVerbPrepPred);

            Grammar[] gramVerbPredPrepPred = new Grammar[4];
            gramVerbPredPrepPred[0] = Grammar.VERB;
            gramVerbPredPrepPred[1] = Grammar.PREDICATE;
            gramVerbPredPrepPred[2] = Grammar.PREP;
            gramVerbPredPrepPred[3] = Grammar.PREDICATE;
            mGrammarDict.Add(GrammarType.GRAMMAR_VERB_PRED_PREP_PRED, gramVerbPredPrepPred);
        }// addGrammar

        // Add all valid commands to this executers dictionary.  Commands are every verb allowed in the area/game, including spells and abilities.
        // There can be multiple commands of the same name, such as 'look', 'look west', and 'look in chest' would be 3 seperate commands 
        // each with different grammars.
        private void addCommands()
        {
            CommandClass commandClass = new ComUp("up", "u", 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_UP,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_UP, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComNorth("north", "n", 5, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_NORTH,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_NORTH, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComNorthEast("northeast", "ne", 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_NORTHEAST,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_NORTHEAST, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComEast("east", "e", 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_EAST,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_EAST, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComSouthEast("southeast", "se", 6, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_SOUTHEAST,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_SOUTHEAST, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComDown("down", "d", 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_DOWN,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_DOWN, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComSouth("south", "s", 5, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_SOUTH,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_SOUTH, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComSouthWest("southwest", "sw", 6, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_SOUTHWEST,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_SOUTHWEST, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComWest("west", "w", 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_WEST,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_WEST, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComNorthWest("northwest", "nw", 6, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_NORTHWEST,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_NORTHWEST, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComExit("exit", null, 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_EXIT,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_EXIT, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComLook("look", null, 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_LOOK,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_LOOK, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComLook("look", null, 1, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_LOOK,
                PredicateType.CUSTOM, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_LOOK, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComLook("look", null, 1, 3, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PREP_PRED], CommandName.COMMAND_LOOK,
                PredicateType.NONE, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_LOOK, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComSay("say", null, 3, 256, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_SAY,
               PredicateType.CUSTOM, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_SAY, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComYell("yell", null, 2, 256, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_YELL,
                PredicateType.CUSTOM, PredicateType.END, ValidityType.AREA);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_YELL, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComTell("tell", null, 1, 256, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED_PRED], CommandName.COMMAND_TELL,
                PredicateType.PLAYER, PredicateType.CUSTOM, ValidityType.GLOBAL);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_TELL, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComOpen("open", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_OPEN,
                PredicateType.OBJECT, PredicateType.CUSTOM);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_OPEN, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComFullHeal("fullheal", "fh", 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_FULLHEAL,
                PredicateType.NONE, PredicateType.CUSTOM);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_FULLHEAL, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComClose("close", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_CLOSE,
                PredicateType.OBJECT, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_CLOSE, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComDestroy("destroy", null, 3, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_DESTROY,
                PredicateType.NONE, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_DESTROY, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComSpawn("spawn", null, 3, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_SPAWN,
                PredicateType.CUSTOM, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_SPAWN, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComGet("get", null, 1, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_GET,
                PredicateType.OBJECT, PredicateType.CUSTOM);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_GET, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComGet("get", null, 1, 4, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED_PREP_PRED], CommandName.COMMAND_GET,
                PredicateType.OBJECT, PredicateType.OBJECT);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_GET, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComGetAll("getall", "ga", 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_GETALL,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_GETALL, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComGetAll("getall", "ga", 1, 3, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PREP_PRED], CommandName.COMMAND_GETALL,
                PredicateType.OBJECT, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_GETALL, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComDrop("drop", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_DROP,
                PredicateType.OBJECT, PredicateType.CUSTOM, ValidityType.INVENTORY);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_DROP, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComDropAll("dropall", "da", 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_DROPALL,
                PredicateType.END, PredicateType.END, ValidityType.INVENTORY);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_DROPALL, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComInventory("inventory", null, 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_INVENTORY,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_INVENTORY, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComEquipment("equipment", "eq", 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_EQUIPMENT,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_EQUIPMENT, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComLock("lock", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_LOCK,
                PredicateType.OBJECT, PredicateType.CUSTOM);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_LOCK, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComUnlock("unlock", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_UNLOCK,
                PredicateType.OBJECT, PredicateType.CUSTOM);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_UNLOCK, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComUse("use", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_USE,
                PredicateType.OBJECT, PredicateType.CUSTOM);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_USE, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComWear("wear", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_WEAR,
                PredicateType.OBJECT, PredicateType.END, ValidityType.INVENTORY);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_WEAR, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComWearAll("wearall", "wa", 5, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_WEARALL,
                PredicateType.END, PredicateType.END, ValidityType.INVENTORY);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_WEARALL, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComRemove("remove", null, 3, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_REMOVE,
                PredicateType.OBJECT, PredicateType.END, ValidityType.EQUIPMENT);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_REMOVE, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComRemoveAll("removeall", "ra", 5, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_REMOVEALL,
                PredicateType.END, PredicateType.END, ValidityType.EQUIPMENT);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_REMOVEALL, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComAttack("attack", "a", 1, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_ATTACK,
                PredicateType.NONE, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_ATTACK, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComTeleport("teleport", "tp", 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_TELEPORT,
                PredicateType.NONE, PredicateType.END, ValidityType.GLOBAL);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_TELEPORT, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComSearch("search", null, 3, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_SEARCH,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_SEARCH, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComWho("who", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_WHO,
                PredicateType.CUSTOM, PredicateType.END, ValidityType.GLOBAL);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_WHO, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComRest("rest", null, 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_REST,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_REST, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComBackstab("backstab", "bs", 2, 2, MobType.ROGUE, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], CommandName.COMMAND_BACKSTAB,
                PredicateType.NONE, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_BACKSTAB, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComBash("bash", "b", 1, 1, MobType.WARRIOR, mGrammarDict[GrammarType.GRAMMAR_VERB], CommandName.COMMAND_BASH,
                PredicateType.END, PredicateType.END);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_BASH, commandClass.GetMaxTokens()), commandClass);

            commandClass = new ComCast("cast", "c", 1, 3, MobType.SPELLCASTER, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED_PRED], CommandName.COMMAND_CAST,
                PredicateType.SPELL, PredicateType.NONE);
            mCCDict.Add(Utils.createTuple(CommandName.COMMAND_CAST, commandClass.GetMaxTokens()), commandClass);
        }// addCommands;

        // Add all allowable prepisitions to the area/game's dictionary.
        private void addPrepositions()
        {
            // Add prepositions
            mPrepDict.Add(PrepositionType.PREP_IN, new Preposition("in", PrepositionType.PREP_IN));
            mPrepDict.Add(PrepositionType.PREP_ON, new Preposition("on", PrepositionType.PREP_ON));
            mPrepDict.Add(PrepositionType.PREP_WITH, new Preposition("with", PrepositionType.PREP_WITH));
            mPrepDict.Add(PrepositionType.PREP_AT, new Preposition("at", PrepositionType.PREP_AT));
            mPrepDict.Add(PrepositionType.PREP_FROM, new Preposition("from", PrepositionType.PREP_FROM));
        }// addPrepositions

        // Add all allowable abilities/spells to the area/game's dictionary.
        private void addAbilitySpells()
        {
            // This order matters, it must align with the order defined in the enums in AbilitySpell.
            Action act = new Action("bash", 4, 4, ActionType.ABILITY);
            act.mHitBonus = 5;
            act.mEvadable = true;
            act.mDamScaling = DamageScaling.PERLEVEL;
            act.mDamageBonus = 1;
            act.mBaseMinDamage = 1;
            act.mBaseMaxDamage = 6;
            act.mAbilitySpell = AbilitySpell.ABILITY_BASH;
            act.mWeaponRequired = false;
            mAbilitySpellList.Add(act);

            act = new Action("backstab", 4, 4, ActionType.ABILITY);
            act.mHitBonus = 10;
            act.mEvadable = true;
            act.mDamScaling = DamageScaling.DAMAGEMULTPERLEVEL;
            act.mDamageMult = 2;
            act.mBaseMinDamage = 2;
            act.mBaseMaxDamage = 5;
            act.mWeaponRequired = true;
            act.mAbilitySpell = AbilitySpell.ABILITY_BACKSTAB;
            mAbilitySpellList.Add(act);

            act = new Action("mysticshot", 4, 4, ActionType.SPELL);
            act.mDamType = DamageType.MAGICAL;
            act.mResistable = true;
            act.mDamScaling = DamageScaling.PERLEVEL;
            act.mBaseMinDamage = 2;
            act.mBaseMaxDamage = 9;
            act.mAbilitySpell = AbilitySpell.SPELL_MYSTIC_SHOT;
            act.mWeaponRequired = false;
            act.mManaCost = 5;
            mAbilitySpellList.Add(act);

            act = new Action("cure", 4, 4, ActionType.SPELL);
            act.mDamType = DamageType.HEALING;
            act.mResistable = false;
            act.mDamScaling = DamageScaling.PERLEVEL;
            act.mBaseMinDamage = 3;
            act.mBaseMaxDamage = 5;
            act.mAbilitySpell = AbilitySpell.SPELL_CURE;
            act.mWeaponRequired = false;
            act.mManaCost = 5;
            mAbilitySpellList.Add(act);
        }// addAbilitySpells

        // Process the command in its string format.  It needs to be resolved to a verb by the parser, and then needs to process and get all
        // predicates and add to a commandList which holds the combination of the grammar items in it when resolved.  
        public void process(String command, Mob mob)
        {
            String[] tokens = command.Split(' ');
            ArrayList ccList = new ArrayList();
            CommandClass currentCC = null;
            String clientString = String.Empty;

            if (command.Equals(String.Empty))
                return;

            // First see if the shortname matches, if so we are done, otherwise match any subString
            errorCode eCode = initialMatching(tokens, ccList);

            if (eCode == errorCode.E_OK)
            {
                // Check to see if we have multiple verbs that could match
                currentCC = resolveMultipleVerbs(ccList, eCode, tokens);

                // In this case, the player must have sent us a verb that has multiple arguements.
                // This means we have to parse the grammar of the verb and add back the appropriate
                // predicates to the command list if they even exist.
                eCode = populateCommandList(command, tokens, currentCC, ccList, mob, ref clientString);

                // If all predicates check out, the commandList will be correctly populated with all 
                // correct predicates in the right order according to the verbs description.  
                // Go ahead and execute the command, otherwise construct the error string.
                if (eCode == errorCode.E_OK)
                    eCode = execute(currentCC, ccList, mob, ref clientString);
                else if (eCode == errorCode.E_INVALID_COMMAND_USAGE)
                    clientString = "you can't use the " + currentCC.GetCommand() + " command like that";
                else if (eCode == errorCode.E_INVALID_SYNTAX && clientString != String.Empty)
                    clientString = "you can't do that\n";
                else
                    clientString += "you can't do that\n";
            }// if (eCode == errorCode.E_OK)
            else
                clientString = tokens[0] + " is not a valid command";

            clientString += mob.playerString();
            mob.safeWrite(clientString);
        }// process

        private errorCode initialMatching(String[] tokens, ArrayList ccList)
        {
            errorCode foundMatch = errorCode.E_INVALID_COMMAND_USAGE;

            foreach (KeyValuePair<Tuple<CommandName, int>, CommandClass> commands in mCCDict)
            {
                CommandClass com = commands.Value;

                if (tokens[0].Equals(commands.Value.GetShortName()))
                {
                    foundMatch = errorCode.E_OK;
                    ccList.Add(com);
                    continue;
                }

                if (tokens[0].Length < com.GetMatchNumber() || tokens[0].Length > com.GetCommand().Length)
                    continue;

                if (com.GetCommand().Contains(tokens[0]))
                {
                    foundMatch = errorCode.E_OK;
                    ccList.Add(com);
                }
            }// foreach

            return foundMatch;
        }// initialMatching

        private CommandClass resolveMultipleVerbs(ArrayList ccList, errorCode eCode, String[] tokens)
        {
            CommandClass currentCC = null;

            // If we have more than 1 verb with the same name, we have to identify the 
            // correct one we should use based on how many maximum tokens it allows
            if (ccList.Count > 1)
            {
                eCode = errorCode.E_INVALID_COMMAND_USAGE;

                // If our command has the exact same number of tokens as the players
                // tokenized command String, then we are done, this is the one.
                foreach (CommandClass com in ccList)
                {
                    if (tokens.Length == com.GetMaxTokens())
                    {
                        currentCC = com;
                        eCode = errorCode.E_OK;
                        break;
                    }
                }// foreach

                // Remove all other commands and add the one we found
                if (eCode == errorCode.E_OK)
                {
                    ccList.Clear();
                    ccList.Add(currentCC);
                }// if
                 // In this case, they have a variable number of tokens in their 
                 // command that sits in between two commands maximum allowable
                 // tokens, we need to find which command to use
                else
                {
                    eCode = errorCode.E_INVALID_COMMAND_USAGE;

                    foreach (CommandClass com in ccList)
                    {
                        if (tokens.Length <= com.GetMaxTokens())
                        {
                            currentCC = com;
                            eCode = errorCode.E_OK;
                            ccList.Clear();
                            ccList.Add(com);
                            break;
                        }
                    }// foreach
                }// else
            }// (commandList.Count > 1)
            else
                currentCC = (CommandClass)ccList[0];

            return currentCC;
        }// resolveMultipleVerbs

        private errorCode populateCommandList(String command, String[] tokens, CommandClass currentCC, ArrayList commandList, Mob mob, ref String clientString)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            PredicateType targetPredicate;
            int grammarIndex = 0;
            int predicateCount = 0;
            String errorString = currentCC.GetCommand();
            String subCommand = String.Empty;

            // If we have a valid verb with a grammar of 1, we are done, execute it
            if (tokens.Length == 1 &&
                currentCC.GetGrammar().Length == 1 &&
                mob.GetActionTimer() == 0)
            {
                return errorCode.E_OK;
            }

            // Start processing from after the first verb
            if (tokens.Length > 1)
                subCommand = command.Substring((tokens[0].Length + 1));

            // Loop until we have gone through all grammar specified by the commands acceptable grammar set
            while (grammarIndex < currentCC.GetGrammar().Length)
            {
                // We need to know which predicate we need to examine
                if (currentCC.GetGrammar()[grammarIndex++] == Grammar.PREDICATE)
                {
                    if (predicateCount++ == 0)
                        targetPredicate = currentCC.mPredicate1;
                    else
                        targetPredicate = currentCC.mPredicate2;

                    // If the predicate is a player, we only accept the very next token to
                    // search for a valid playername
                    if (!targetPredicate.HasFlag(PredicateType.END) && !targetPredicate.HasFlag(PredicateType.CUSTOM))
                    {
                        // Something went wrong, we ran out of tokens to parse before we were done, must have been an invalid syntax
                        if (tokens.Length == 1)
                            return errorCode.E_INVALID_SYNTAX;

                        tokens = subCommand.Split(' ');
                        errorString += " " + tokens[0];

                        ret = doesPredicateExist(tokens[0], targetPredicate, currentCC.mValidity, commandList, mob);

                        if (ret != errorCode.E_OK)
                        {
                            clientString = "Can't find " + tokens[0];
                            break;
                        }

                        if (grammarIndex < currentCC.GetGrammar().Length && tokens.Length > 1)
                            subCommand = subCommand.Substring(tokens[0].Length + 1);
                    }// if
                    // If the predicate is custom, we simply dump the rest of the command back and let processing continue
                    else if (targetPredicate.HasFlag(PredicateType.CUSTOM))
                    {
                        commandList.Add(subCommand);
                        ret = errorCode.E_OK;
                    }

                    if (ret != errorCode.E_OK)
                    {
                        clientString = "You can't " + errorString;
                        break;
                    }
                }// if (currentCommand.grammar[grammarIndex++] == Grammar.PREDICATE)
                else if (currentCC.GetGrammar()[grammarIndex - 1] == Grammar.PREP)
                {
                    ret = errorCode.E_INVALID_SYNTAX;
                    tokens = subCommand.Split(' ');
                    errorString += " " + tokens[0];
                    String prepName = tokens[0];

                    foreach (KeyValuePair<PrepositionType, Preposition> prepPair in mPrepDict)
                    {
                        if (prepPair.Value.name.Equals(tokens[0]))
                        {
                            commandList.Add(prepPair.Value);
                            ret = errorCode.E_OK;

                            if ((grammarIndex < currentCC.GetGrammar().Length))
                            {
                                if (command.Length > tokens[0].Length)
                                    subCommand = subCommand.Substring(tokens[0].Length + 1);
                                else
                                    return errorCode.E_INVALID_COMMAND_USAGE;
                            }

                            break;
                        }// if (prepPair.Value.name.Equals(tokens[0]))
                    }// foreach

                    if (ret == errorCode.E_INVALID_SYNTAX)
                    {
                        clientString = "You are not able to " + errorString;
                        break;
                    }
                }// else if
            }// while (grammarIndex < currentCommand.grammar.Length)

            if (grammarIndex != currentCC.GetGrammar().Length)
                return errorCode.E_INVALID_COMMAND_USAGE;

            return ret;
        }// populateCommandList

        public errorCode execute(CommandClass commandClass, ArrayList commandQueue, Mob mob, ref String clientString)
        {
            commandClass.preExecute(commandQueue, mob, this);
            errorCode eCode = commandClass.execute(commandQueue, mob, this, ref clientString);

            // If the command fails we don't want to trigger events off of the command
            if(eCode == errorCode.E_OK)
                checkEvent(commandClass, commandQueue, mob);

            return eCode;
        }// execute

        private void checkEvent(CommandClass commandClass, ArrayList commandQueue, Mob mob)
        {
            // Populate the event args with the triggers and room in which the event was triggered.
            fillEventArgs(commandClass, commandQueue, mob);

            if (!commandClass.mPredicate1.HasFlag(PredicateType.END) &&
                !commandClass.mPredicate1.HasFlag(PredicateType.CUSTOM))
            {
                if ( commandClass.GetPred1().GetEventList().Count > 0)
                {
                    EventData eventData = commandClass.GetPred1().GetEventList()[0];

                    if(eventData.GetCommand() == commandClass.GetCommandName() &&
                       eventData.GetPrepType() == commandClass.GetPrep1().prepType)
                    {
                        eventData.SetTrigger(commandClass.GetCommOwner());
                        eventData.SetEventObject(commandClass.GetPred1());
                        eventData.SetRoom(commandClass.GetPred1().GetCurrentRoom());
                        eventData.validity = commandClass.mValidity;
                        ((CombatMob)mob).GetWorld().GetEventHandler().enQueueEvent(eventData);
                    }      
                }// if
            }// else if
        }// checkEvent

        private errorCode doesPredicateExist(String name, PredicateType predType, ValidityType validity, ArrayList commandQueue, Mob target)
        {
            ArrayList targetPredicates = new ArrayList();
            String[] tokens = name.Split('.');

            // Fill the targetPredicates with applicable predicates
            errorCode ret = extractPredicates(tokens, predType, validity, target, targetPredicates);

            if (ret == errorCode.E_OK)
            {
                ret = errorCode.E_INVALID_SYNTAX;

                if (tokens.Length == 1)
                {
                    commandQueue.Add(targetPredicates[0]);
                    ret = errorCode.E_OK;
                }
                else if (tokens.Length == 2)
                {
                    try
                    {
                        int index = Int32.Parse(tokens[1])-1;

                        if (index <= targetPredicates.Count && index > 0)
                        {
                            commandQueue.Add(targetPredicates[index]);
                            ret = errorCode.E_OK;
                        }
                    }// try

                    catch
                    {  // silently fail, the return code will correctly error
                    }
                }// else
            }// if (ret == errorCode.E_OK)

            return ret;
        }// doesPredicateExist

        private bool validatePredicate(String targetPred, String cmdString)
        {
            bool found = false;
            String subString = cmdString;
            int index = 0;
            char currentChar = '\0';

            for(int i = targetPred.Length; i > 0; --i)
            {
                currentChar = targetPred[targetPred.Length - i];

                if(i > subString.Length)
                    return false;

                while (true)
                {
                    found = false;
                    index = subString.IndexOf(currentChar);
                    int len = subString.Length;

                    if (index >= 0)
                    {
                        if (index < len)
                        {
                            found = true;
                            subString = subString.Substring(index + 1, len - index - 1);
                            break;
                        }
                    }// if (index >= 0)
                    else
                        return false;
                }// while
            }// foreach

            return found;
        }// validatePredicate

        private void fillEventArgs(CommandClass commandClass, ArrayList commandQueue, Mob mob)
        {
            Grammar[] grammar = commandClass.GetGrammar();
            int predicateCount = 0;
            int prepCount = 0;

            commandClass.SetCommOwner(mob);

            for (int i = 1; i < grammar.Length; ++i)
            {
                if (grammar[i] == Grammar.PREDICATE)
                {
                    if ((++predicateCount == 1) && !(commandQueue[i] is String))
                        commandClass.SetPred1((Mob)commandQueue[i]);
                    else if (!(commandQueue[i] is String))
                        commandClass.SetPred2((Mob)commandQueue[i]);
                }
                else if (grammar[i] == Grammar.PREP)
                {
                    if (++prepCount == 1)
                        commandClass.SetPrep1((Preposition)commandQueue[i]);
                    else
                        commandClass.SetPrep2((Preposition)commandQueue[i]);
                }
            }// for

            if(commandQueue.Count > 0)
                commandQueue[0] = commandClass;
        }// fillEventArgs

        // Fill the targetList with the extracted predicates from the commands and the applicable surroundings based on predicate type and validityType
        private errorCode extractPredicates(String[] tokens, PredicateType predType, ValidityType validity, Mob target, ArrayList targetPredicates)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            ArrayList targetList = new ArrayList();
            List<ResType> targetResTypes = new List<ResType>();
            World world = target.GetWorld();
            Area currentArea = target.GetCurrentArea();
            Room currentRoom = target.GetCurrentRoom();
            List<Mob> inventory = target.GetInv();

            if (predType.HasFlag(PredicateType.OBJECT))
                targetResTypes.Add(ResType.OBJECT);
            if (predType.HasFlag(PredicateType.PLAYER))
                targetResTypes.Add(ResType.PLAYER);
            if (predType.HasFlag(PredicateType.NPC))
                targetResTypes.Add(ResType.NPC);
            if (predType.HasFlag(PredicateType.DOORWAY))
                targetResTypes.Add(ResType.DOORWAY);

            foreach (ResType targetResType in targetResTypes)
            {
                if (validity.HasFlag(ValidityType.GLOBAL))
                    targetList.Add(world.getRes(targetResType));
                if (validity.HasFlag(ValidityType.AREA))
                    targetList.Add(currentArea.getRes(targetResType));
                if (validity.HasFlag(ValidityType.LOCAL))
                    targetList.Add(currentRoom.getRes(targetResType));
                if (predType.HasFlag(PredicateType.DOORWAY))
                    targetList.Add(currentRoom.getRes(ResType.DOORWAY));
            }// foreach (ResType targetResType in targetResTypes)

            if (validity.HasFlag(ValidityType.INVENTORY) && inventory != null)
            {
                targetList.Add(inventory);

                foreach (Mob cont in target.GetCurrentRoom().getRes(ResType.OBJECT))
                {
                    if (cont is Container)
                        targetList.Add(cont.GetInv());
                }
            }// if (validity.HasFlag(ValidityType.INVENTORY) && inventory != null)

            if (predType.HasFlag(PredicateType.SPELL))
                targetList.Add(((CombatMob)target).GetActionList());

            if (validity.HasFlag(ValidityType.EQUIPMENT))
            {
                Dictionary<EQSlot, Mob> eqList = target.GetEQList();

                if (eqList != null)
                {
                    foreach (KeyValuePair<EQSlot, Mob> keyValPair in eqList)
                    {
                        Mob targetMob = keyValPair.Value;

                        if (targetMob != null && validatePredicate(tokens[0].ToLower(), targetMob.exitString(target.GetCurrentRoom()).ToLower()))
                        {
                            ret = errorCode.E_OK;
                            targetPredicates.Add(targetMob);
                        }
                    }
                }
            }// if (validity.HasFlag(ValidityType.EQUIPMENT))

            foreach (List<Mob> subList in targetList)
            {
                if (subList.Count > 0)
                {
                    foreach (Mob mob in subList)
                    {
                        if (mob != null && validatePredicate(tokens[0].ToLower(), mob.exitString(target.GetCurrentRoom()).ToLower()))
                        {
                            ret = errorCode.E_OK;
                            targetPredicates.Add(mob);
                        }
                    }
                }
            }// foreach (List<Mob> subList in targetList)

            return ret;
        }// extractPredicates

        // Accessors
        public List<Action> GetASList() { return mAbilitySpellList; }
        public Dictionary<Tuple<CommandName, int>, CommandClass> GetCCDict() { return mCCDict; }

    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
