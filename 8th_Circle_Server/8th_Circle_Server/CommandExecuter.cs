using System;
using System.Collections.Generic;
using System.Collections;

namespace _8th_Circle_Server
{
    public class CommandExecuter
    {
        private Dictionary<PrepositionType, Preposition> mPrepDict;
        private List<Action> mAbilitySpellList;
        private Dictionary<Tuple<commandName, int>, CommandClass> mCCDict;
        private Dictionary<GrammarType, Grammar[]> mGrammarDict;

        public CommandExecuter()
        {
            mPrepDict = new Dictionary<PrepositionType, Preposition>();
            mAbilitySpellList = new List<Action>();
            mCCDict = new Dictionary<Tuple<commandName, int>, CommandClass>();
            mGrammarDict = new Dictionary<GrammarType, Grammar[]>();

            addGrammar();
            addCommands();
            addPrepositions();
            addAbilitySpells();
        }// Constructor

        private void addGrammar()
        {
            // Add Grammars
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

        private void addCommands()
        {
            // Add Verbs
            CommandClass commandClass = new ComUp("up", "u", 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_UP,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_UP, commandClass.maxTokens), commandClass);

            commandClass = new ComNorth("north", "n", 5, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_NORTH,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_NORTH, commandClass.maxTokens), commandClass);

            commandClass = new ComNorthEast("northeast", "ne", 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_NORTHEAST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_NORTHEAST, commandClass.maxTokens), commandClass);

            commandClass = new ComEast("east", "e", 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_EAST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_EAST, commandClass.maxTokens), commandClass);

            commandClass = new ComSouthEast("southeast", "se", 6, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_SOUTHEAST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_SOUTHEAST, commandClass.maxTokens), commandClass);

            commandClass = new ComDown("down", "d", 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_DOWN,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_DOWN, commandClass.maxTokens), commandClass);

            commandClass = new ComSouth("south", "s", 5, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_SOUTH,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_SOUTH, commandClass.maxTokens), commandClass);

            commandClass = new ComSouthWest("southwest", "sw", 6, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_SOUTHWEST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_SOUTHWEST, commandClass.maxTokens), commandClass);

            commandClass = new ComWest("west", "w", 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_WEST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_WEST, commandClass.maxTokens), commandClass);

            commandClass = new ComNorthWest("northwest", "nw", 6, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_NORTHWEST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_NORTHWEST, commandClass.maxTokens), commandClass);

            commandClass = new ComExit("exit", null, 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_EXIT,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_EXIT, commandClass.maxTokens), commandClass);

            commandClass = new ComLook("look", null, 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_LOOK,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_LOOK, commandClass.maxTokens), commandClass);

            commandClass = new ComLook("look", null, 1, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_LOOK,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_LOOK, commandClass.maxTokens), commandClass);

            commandClass = new ComLook("look", null, 1, 3, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PREP_PRED], commandName.COMMAND_LOOK,
                predicateType.PREDICATE_ALL, predicateType.PREDICATE_END, validityType.VALID_INVLOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_LOOK, commandClass.maxTokens), commandClass);

            commandClass = new ComSay("say", null, 3, 256, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_SAY,
               predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_SAY, commandClass.maxTokens), commandClass);

            commandClass = new ComYell("yell", null, 2, 256, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_YELL,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_AREA, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_YELL, commandClass.maxTokens), commandClass);

            commandClass = new ComTell("tell", null, 1, 256, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED_PRED], commandName.COMMAND_TELL,
                predicateType.PREDICATE_PLAYER, predicateType.PREDICATE_CUSTOM, validityType.VALID_GLOBAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_TELL, commandClass.maxTokens), commandClass);

            commandClass = new ComOpen("open", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_OPEN,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_OPEN, commandClass.maxTokens), commandClass);

            commandClass = new ComFullHeal("fullheal", "fh", 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_FULLHEAL,
                predicateType.PREDICATE_PLAYER_OR_NPC, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_FULLHEAL, commandClass.maxTokens), commandClass);

            commandClass = new ComClose("close", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_CLOSE,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_CLOSE, commandClass.maxTokens), commandClass);

            commandClass = new ComDestroy("destroy", null, 3, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_DESTROY,
                predicateType.PREDICATE_OBJECT_OR_NPC, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_DESTROY, commandClass.maxTokens), commandClass);

            commandClass = new ComSpawn("spawn", null, 3, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_SPAWN,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_SPAWN, commandClass.maxTokens), commandClass);

            commandClass = new ComGet("get", null, 1, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_GET,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_INVLOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_GET, commandClass.maxTokens), commandClass);

            commandClass = new ComGet("get", null, 1, 4, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PREP_PRED], commandName.COMMAND_GET,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_OBJECT, validityType.VALID_INVLOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_GET, commandClass.maxTokens), commandClass);

            commandClass = new ComGetAll("getall", "ga", 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_GETALL,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_INVLOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_GETALL, commandClass.maxTokens), commandClass);

            commandClass = new ComGetAll("getall", "ga", 1, 3, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PREP_PRED], commandName.COMMAND_GETALL,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_END, validityType.VALID_INVLOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_GETALL, commandClass.maxTokens), commandClass);

            commandClass = new ComDrop("drop", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_DROP,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_INVENTORY, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_DROP, commandClass.maxTokens), commandClass);

            commandClass = new ComDropAll("dropall", "da", 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_DROPALL,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_INVENTORY, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_DROPALL, commandClass.maxTokens), commandClass);

            commandClass = new ComInventory("inventory", null, 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_INVENTORY,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_INVENTORY, commandClass.maxTokens), commandClass);

            commandClass = new ComEquipment("equipment", "eq", 2, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_EQUIPMENT,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_EQUIPMENT, commandClass.maxTokens), commandClass);

            commandClass = new ComLock("lock", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_LOCK,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_LOCK, commandClass.maxTokens), commandClass);

            commandClass = new ComUnlock("unlock", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_UNLOCK,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_UNLOCK, commandClass.maxTokens), commandClass);

            commandClass = new ComUse("use", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_USE,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_INVLOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_USE, commandClass.maxTokens), commandClass);

            commandClass = new ComWear("wear", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_WEAR,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_END, validityType.VALID_INVENTORY, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_WEAR, commandClass.maxTokens), commandClass);

            commandClass = new ComWearAll("wearall", "wa", 5, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_WEARALL,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_INVENTORY, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_WEARALL, commandClass.maxTokens), commandClass);

            commandClass = new ComRemove("remove", null, 3, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_REMOVE,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_END, validityType.VALID_EQUIP, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_REMOVE, commandClass.maxTokens), commandClass);

            commandClass = new ComRemoveAll("removeall", "ra", 5, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_REMOVEALL,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_EQUIP, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_REMOVEALL, commandClass.maxTokens), commandClass);

            commandClass = new ComAttack("attack", "a", 1, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_ATTACK,
                predicateType.PREDICATE_PLAYER_OR_NPC, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_ATTACK, commandClass.maxTokens), commandClass);

            commandClass = new ComTeleport("teleport", "tp", 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_TELEPORT,
                predicateType.PREDICATE_PLAYER_OR_NPC, predicateType.PREDICATE_END, validityType.VALID_GLOBAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_TELEPORT, commandClass.maxTokens), commandClass);

            commandClass = new ComSearch("search", null, 3, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_SEARCH,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_SEARCH, commandClass.maxTokens), commandClass);

            commandClass = new ComWho("who", null, 2, 2, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_WHO,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_GLOBAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_WHO, commandClass.maxTokens), commandClass);

            commandClass = new ComRest("rest", null, 1, 1, MobType.ALL, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_REST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.GENERAL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_REST, commandClass.maxTokens), commandClass);

            commandClass = new ComBackstab("backstab", "bs", 2, 2, MobType.ROGUE, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED], commandName.COMMAND_BACKSTAB,
                predicateType.PREDICATE_PLAYER_OR_NPC, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.ABILITY);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_BACKSTAB, commandClass.maxTokens), commandClass);

            commandClass = new ComBash("bash", "b", 1, 1, MobType.WARRIOR, mGrammarDict[GrammarType.GRAMMAR_VERB], commandName.COMMAND_BASH,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL, CommandType.ABILITY);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_BASH, commandClass.maxTokens), commandClass);

            commandClass = new ComCast("cast", "c", 1, 3, MobType.SPELLCASTER, mGrammarDict[GrammarType.GRAMMAR_VERB_PRED_PRED], commandName.COMMAND_CAST,
                predicateType.PREDICATE_SPELL, predicateType.PREDICATE_PLAYER_OR_NPC, validityType.VALID_LOCAL, CommandType.SPELL);
            mCCDict.Add(Utils.createTuple(commandName.COMMAND_CAST, commandClass.maxTokens), commandClass);
        }// addCommands;

        private void addPrepositions()
        {
            // Add prepositions
            mPrepDict.Add(PrepositionType.PREP_IN, new Preposition("in", PrepositionType.PREP_IN));
            mPrepDict.Add(PrepositionType.PREP_ON, new Preposition("on", PrepositionType.PREP_ON));
            mPrepDict.Add(PrepositionType.PREP_WITH, new Preposition("with", PrepositionType.PREP_WITH));
            mPrepDict.Add(PrepositionType.PREP_AT, new Preposition("at", PrepositionType.PREP_AT));
            mPrepDict.Add(PrepositionType.PREP_FROM, new Preposition("from", PrepositionType.PREP_FROM));
            mPrepDict.Add(PrepositionType.PREP_OFF, new Preposition("off", PrepositionType.PREP_OFF));
        }// addPrepositions

        private void addAbilitySpells()
        {
            // Order matters, no change change, if you do, update the ActionType enums
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

            act = new Action("mystic shot", 4, 4, ActionType.SPELL);
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

        public void process(string command, Mob mob)
        {
            string[] tokens = command.Split(' ');
            ArrayList ccList = new ArrayList();
            CommandClass currentCC = null;
            string clientString = string.Empty;

            if (command.Equals(string.Empty))
                return;

            // First see if the shortname matches, if so we are done, otherwise match any substring
            errorCode eCode = initialMatching(tokens, ccList);

            if (eCode == errorCode.E_OK)
            {
                // Check to see if we have multiple verbs that could match
                currentCC = resolveMultipleVerbs(ccList, eCode, tokens);

                // In this case, the player must have sent us a verb that has multiple arguements.
                // This means we have to parse the grammar of the verb and add back the appropriate
                // predicates to the command list if they even exist.
                eCode = populateCommandList(command, tokens, currentCC, ccList, mob, clientString);

                // All predicates must have checked out, the commandList will be correctly
                // populated with all correct predicates in the right order according to
                // the verbs description.  Go ahead and execute the command
                if (eCode == errorCode.E_OK)
                    clientString = execute(currentCC, ccList, mob);
                else if (eCode == errorCode.E_INVALID_COMMAND_USAGE)
                    clientString = "you can't use the " + currentCC.command + " command like that";
                else if (eCode == errorCode.E_INVALID_SYNTAX && clientString != String.Empty)
                    clientString = "You are not able to do that\n";
                else
                    clientString += "You can't do that\n";
            }// if (eCode == errorCode.E_OK)
            else
                clientString = tokens[0] + " is not a valid command";

            if (mob.mResType == ResType.PLAYER)
            {
                clientString += ((CombatMob)mob).playerString();
                ((CombatMob)mob).mClientHandler.safeWrite(clientString);
            }
        }// process

        private errorCode initialMatching(string[] tokens, ArrayList ccList)
        {
            errorCode foundMatch = errorCode.E_INVALID_COMMAND_USAGE;

            foreach (KeyValuePair<Tuple<commandName, int>, CommandClass> commands in mCCDict)
            {
                CommandClass com = commands.Value;

                if (tokens[0].Equals(commands.Value.shortName))
                {
                    foundMatch = errorCode.E_OK;
                    ccList.Add(com);
                    continue;
                }// if

                if (tokens[0].Length < com.matchNumber || tokens[0].Length > com.command.Length)
                    continue;

                if (com.command.Contains(tokens[0]))
                {
                    foundMatch = errorCode.E_OK;
                    ccList.Add(com);
                }// if
            }// foreach

            return foundMatch;
        }// initialMatching

        private CommandClass resolveMultipleVerbs(ArrayList ccList, errorCode eCode, string[] tokens)
        {
            CommandClass currentCC = null;

            // If we have more than 1 verb with the same name, we have to identify the 
            // correct one we should use based on how many maximum tokens it allows
            if (ccList.Count > 1)
            {
                eCode = errorCode.E_INVALID_COMMAND_USAGE;

                // If our command has the exact same number of tokens as the players
                // tokenized command string, then we are done, this is the one.
                foreach (CommandClass com in ccList)
                {
                    if (tokens.Length == com.maxTokens)
                    {
                        currentCC = com;
                        eCode = errorCode.E_OK;
                        break;
                    }// if
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
                        if (!(tokens.Length > com.maxTokens))
                        {
                            currentCC = com;
                            eCode = errorCode.E_OK;
                            ccList.Clear();
                            ccList.Add(com);
                            break;
                        }// if
                    }// foreach
                }// else
            }// (commandList.Count > 1)
            else
                currentCC = (CommandClass)ccList[0];

            return currentCC;
        }// resolveMultipleVerbs

        private errorCode populateCommandList(string command, string[] tokens, CommandClass currentCC, ArrayList commandList, Mob mob, string clientString)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            predicateType targetPredicate;
            int grammarIndex = 0;
            int predicateCount = 0;
            string errorString = currentCC.command;
            string subCommand = String.Empty;

            // If we have a valid verb with a grammar of 1, we are done, execute it
            if (tokens.Length == 1 &&
                currentCC.grammar.Length == 1 &&
                mob.mActionTimer == 0)
            {
                return errorCode.E_OK;
            }

            // TODO
            // what happens if there is only 1 token, and the grammer was 1 but the mob action timer wasnt?
            // Start processing from after the first verb
            if (tokens.Length > 1)
                subCommand = command.Substring((tokens[0].Length + 1));

            // Loop until we have gone through all grammar specified by the commands acceptable grammar set
            while (grammarIndex < currentCC.grammar.Length)
            {
                // We need to know which predicate we need to examine
                if (currentCC.grammar[grammarIndex++] == Grammar.PREDICATE)
                {
                    if (predicateCount++ == 0)
                        targetPredicate = currentCC.predicate1;
                    else
                        targetPredicate = currentCC.predicate2;

                    // If the predicate is a player, we only accept the very next token to
                    // search for a valid playername
                    if (targetPredicate != predicateType.PREDICATE_END && targetPredicate != predicateType.PREDICATE_CUSTOM)
                    {
                        tokens = subCommand.Split(' ');
                        errorString += " " + tokens[0];

                        ret = doesPredicateExist(tokens[0], targetPredicate, currentCC.validity, commandList, mob);

                        if (ret != errorCode.E_OK)
                        {
                            clientString = "Can't find " + tokens[0];
                            break;
                        }// if

                        if (grammarIndex < currentCC.grammar.Length && tokens.Length > 1)
                            subCommand = subCommand.Substring(tokens[0].Length + 1);
                    }// if
                    // If the predicate is custom, we simply dump the rest of the command
                    // back and let processing continue
                    else if (targetPredicate == predicateType.PREDICATE_CUSTOM)
                    {
                        commandList.Add(subCommand);
                        ret = errorCode.E_OK;
                    }// else if

                    if (ret != errorCode.E_OK)
                    {
                        clientString = "You can't " + errorString;
                        break;
                    }// if
                }// if (currentCommand.grammar[grammarIndex++] == Grammar.PREDICATE)
                else if (currentCC.grammar[grammarIndex - 1] == Grammar.PREP)
                {
                    ret = errorCode.E_INVALID_SYNTAX;
                    tokens = subCommand.Split(' ');
                    errorString += " " + tokens[0];
                    string prepName = tokens[0];

                    foreach (KeyValuePair<PrepositionType, Preposition> prepPair in mPrepDict)
                    {
                        if (prepPair.Value.name.Equals(tokens[0]))
                        {
                            commandList.Add(prepPair.Value);
                            ret = errorCode.E_OK;

                            if ((grammarIndex < currentCC.grammar.Length))
                            {
                                if (command.Length > tokens[0].Length)
                                    subCommand = subCommand.Substring(tokens[0].Length + 1);
                                else
                                    return errorCode.E_INVALID_COMMAND_USAGE;
                            }// if

                            break;
                        }// if
                    }// foreach

                    if (ret == errorCode.E_INVALID_SYNTAX)
                    {
                        clientString = "You are not able to " + errorString;
                        break;
                    }// if
                }// else if
            }// while (grammarIndex < currentCommand.grammar.Length)

            if (grammarIndex != currentCC.grammar.Length)
                return errorCode.E_INVALID_COMMAND_USAGE;

            return ret;
        }// populateCommandList

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
                    // Also, this triggers regardless if the action was a success, for example
                    // if you look in a closed chest, and the event is trigger on the look in command,
                    // it will happen either way, need a way to check for success.
                    if(eventData.commandName == commandClass.commandName &&
                       eventData.prepType == commandClass.prep1Value.prepType)
                    {
                        eventData.trigger = commandClass.commandOwner;
                        eventData.eventObject = commandClass.predicate1Value;
                        eventData.eventRoom = commandClass.predicate1Value.mCurrentRoom;
                        eventData.validity = commandClass.validity;
                        ((CombatMob)mob).mWorld.GetEventHandler().enQueueEvent(eventData);
                    }// if       
                }// if
            }// else if
        }// checkEvent

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
            Grammar[] grammar = commandClass.grammar;
            int predicateCount = 0;
            int prepCount = 0;

            if (mob.mResType == ResType.PLAYER)
                commandClass.commandOwner = (CombatMob)mob;
            else
                commandClass.commandOwner = mob;

            for (int i = 1; i < grammar.Length; ++i)
            {
                if (grammar[i] == Grammar.PREDICATE)
                {
                    if ((++predicateCount == 1) && !(commandQueue[i] is string))
                        commandClass.predicate1Value = (Mob)commandQueue[i];
                    else if (!(commandQueue[i] is string))
                        commandClass.predicate2Value = (Mob)commandQueue[i];
                }// if
                else if (grammar[i] == Grammar.PREP)
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

        // Accessors
        public List<Action> GetASList() { return mAbilitySpellList; }
        public Dictionary<Tuple<commandName, int>, CommandClass> GetCCDict() { return mCCDict; }
    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
