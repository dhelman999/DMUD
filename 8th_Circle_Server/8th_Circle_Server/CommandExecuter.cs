using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum grammarType
    {
        VERB = 0,
        PREP,
        PREDICATE,
        INVALID
    };// grammarType

    enum predicateType
    {
        PREDICATE_OBJECT = 0,
        PREDICATE_PLAYER,
        PREDICATE_NPC,
        PREDICATE_PLAYER_OR_NPC,
        PREDICATE_OBJECT_OR_PLAYER,
        PREDICATE_OBJECT_OR_NPC,
        PREDICATE_ALL,
        PREDICATE_CUSTOM,
        INVALID
    }// predicateType

    enum validityType
    {
        VALID_INVENTORY = 0,
        VALID_LOCAL,
        VALID_INVLOCAL,
        VALID_AREA,
        VALID_GLOBAL,
        INVALID
    };// validityType

    enum PrepositionType
    {
        PREP_IN=0,
        PREP_ON,
        PREP_WITH,
        PREP_AT,
        PREP_FROM,
        PREP_OFF,
        PREP_INVALID
    };// PrepositionType

    enum commandName
    {
        INVALID,
        COMMAND_LOOK,
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
        COMMAND_INVENTORY,
        COMMAND_LOCK,
        COMMAND_UNLOCK,
        COMMAND_DROP,
        COMMAND_SPAWN,
        COMMAND_DESTROY
    };// commandName

    enum errorCode
    {
        E_OK = 0,
        E_INVALID_SYNTAX,
        E_INVALID_COMMAND_USAGE
    };// errorCode

    struct Preposition
    {
        public string name;
        public PrepositionType prepType;

        public Preposition(string name, PrepositionType prepType)
        {
            this.name = name;
            this.prepType = prepType;
        }// Constructor
    }// Prepositions

    struct Command
    {
        public string command;      
        public string shortName;
        public int matchNumber;
        public int maxTokens;
        public grammarType type;
        public grammarType[] grammar;
        public Preposition prep1Value;
        public Preposition prep2Value;
        public commandName commandName;
        public predicateType predicate1;
        public predicateType predicate2;
        public Mob predicate1Value;
        public Mob predicate2Value;
        public Mob commandOwner;
        public validityType validity;

        public Command(string command, string shortName, int matchNumber, 
                       int maxTokens, grammarType type, grammarType[] grammar,
                       commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity)
        {
            this.command = command;
            this.commandOwner = null;
            this.shortName = shortName;
            this.matchNumber = matchNumber;
            this.type = type;
            this.grammar = grammar;
            this.maxTokens = maxTokens;
            this.commandName = commandName;
            this.predicate1 = predicate1;
            this.predicate2 = predicate2;
            this.validity = validity;
            this.prep1Value = new Preposition();
            this.prep2Value = new Preposition();
            this.predicate1Value = predicate2Value = null;
        }// Constructor
    }// struct Command

    class CommandExecuter
    {
        // Debug
        internal const bool DEBUG = true;

        // Member Variables
        public ArrayList mVerbList;
        public ArrayList mPrepList;
        public ArrayList mCommandList;
        public ArrayList mGrammarList;

        public CommandExecuter()
        {
            mVerbList = new ArrayList();
            mPrepList = new ArrayList();
            mCommandList = new ArrayList();
            mGrammarList = new ArrayList();
            addCommands();
        }// Constructor

        private void addCommands()
        {
            // Add Grammars
            grammarType[] gramVerb = new grammarType[1];
            gramVerb[0] = grammarType.VERB;

            grammarType[] gramVerbPred = new grammarType[2];
            gramVerbPred[0] = grammarType.VERB;
            gramVerbPred[1] = grammarType.PREDICATE;

            grammarType[] gramVerbPredPred = new grammarType[3];
            gramVerbPredPred[0] = grammarType.VERB;
            gramVerbPredPred[1] = grammarType.PREDICATE;
            gramVerbPredPred[2] = grammarType.PREDICATE;

            grammarType[] gramVerbPrepPred = new grammarType[3];
            gramVerbPrepPred[0] = grammarType.VERB;
            gramVerbPrepPred[1] = grammarType.PREP;
            gramVerbPrepPred[2] = grammarType.PREDICATE;

            grammarType[] gramVerbPredPrepPred = new grammarType[4];
            gramVerbPredPrepPred[0] = grammarType.VERB;
            gramVerbPredPrepPred[1] = grammarType.PREDICATE;
            gramVerbPredPrepPred[2] = grammarType.PREP;
            gramVerbPredPrepPred[3] = grammarType.PREDICATE;

            // Add Verbs
            Command pt = new Command("north", "n", 5, 1, grammarType.VERB, gramVerb, commandName.COMMAND_NORTH,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("south", "s", 5, 1, grammarType.VERB, gramVerb, commandName.COMMAND_SOUTH,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("east", "e", 2, 1, grammarType.VERB, gramVerb, commandName.COMMAND_EAST,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("west", "w", 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_WEST,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("up", "u", 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_UP,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("down", "d", 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_DOWN,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("northeast", "ne", 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_NORTHEAST,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("northwest", "nw", 6, 1, grammarType.VERB, gramVerb, commandName.COMMAND_NORTHWEST,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("southeast", "se", 6, 1, grammarType.VERB, gramVerb, commandName.COMMAND_SOUTHEAST,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("southwest", "sw", 6, 1, grammarType.VERB, gramVerb, commandName.COMMAND_SOUTHWEST,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("exit", null, 2, 1, grammarType.VERB, gramVerb, commandName.COMMAND_EXIT,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", null, 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_LOOK,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", null, 1, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_LOOK,
                predicateType.PREDICATE_CUSTOM, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", null, 1, 3, grammarType.VERB, gramVerbPrepPred, commandName.COMMAND_LOOK,
                predicateType.PREDICATE_ALL, predicateType.INVALID, validityType.VALID_INVLOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("say", null, 3, 256, grammarType.VERB, gramVerbPred, commandName.COMMAND_SAY,
                predicateType.PREDICATE_CUSTOM, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("yell", null, 2, 256, grammarType.VERB, gramVerbPred, commandName.COMMAND_YELL,
                predicateType.PREDICATE_CUSTOM, predicateType.INVALID, validityType.VALID_AREA);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("tell", null, 1, 256, grammarType.VERB, gramVerbPredPred, commandName.COMMAND_TELL,
                predicateType.PREDICATE_PLAYER, predicateType.PREDICATE_CUSTOM, validityType.VALID_GLOBAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("open", null, 2, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_OPEN,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("close", null, 2, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_CLOSE,
                predicateType.PREDICATE_OBJECT, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("destroy", null, 3, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_DESTROY,
                predicateType.PREDICATE_OBJECT_OR_NPC, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("spawn", null, 3, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_SPAWN,
                predicateType.PREDICATE_CUSTOM, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("get", null, 1, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_GET,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("drop", null, 2, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_DROP,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_INVENTORY);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("inventory", null, 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_INVENTORY,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("lock", null, 2, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_LOCK,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("unlock", null, 2, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_UNLOCK,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            // Add prepositions
            mPrepList.Add(new Preposition("in", PrepositionType.PREP_IN));
            mPrepList.Add(new Preposition("on", PrepositionType.PREP_ON));
            mPrepList.Add(new Preposition("with", PrepositionType.PREP_WITH));
            mPrepList.Add(new Preposition("at", PrepositionType.PREP_AT));
            mPrepList.Add(new Preposition("from", PrepositionType.PREP_FROM));
            mPrepList.Add(new Preposition("off", PrepositionType.PREP_OFF));
            mPrepList.Add(new Preposition("on", PrepositionType.PREP_ON));
        }// addCommands;

        public void process(string command, Mob mob)
        {
            string[] tokens = command.Split(' ');
            Command currentCommand = new Command();
            ArrayList commandList = new ArrayList();
            bool foundMatch = false;

            if (command.Equals(string.Empty))
                return;

            // First, add all commands that either directly equal, or contain the 
            // first token in the command as this should always be a verb.
            foreach (Command com in mVerbList)
            {
                if (tokens[0].Equals(com.shortName))
                {
                    foundMatch = true;
                    currentCommand = com;
                    commandList.Add(com);
                }// if
                if (tokens[0].Length < com.matchNumber || tokens[0].Length > com.command.Length)
                {
                    continue;
                }// if
                if(com.command.Contains(tokens[0]))
                {
                    foundMatch = true;
                    currentCommand = com;
                    commandList.Add(com);
                }// if
            }// foreach

            // If the player's first token was not a verb, then it was not a valid command
            if (!foundMatch)
            {
                if(mob is Player)
                    ((Player)mob).mClientHandler.safeWrite(tokens[0] + " is not a valid command");
                return;
            }// if

            // If we have more than 1 verb with the same name, we have to identify the 
            // correct one we should use based on how many maximum tokens it allows
            if (commandList.Count > 1)
            {
                foundMatch = false;

                // If our command has the exact same number of tokens as the players
                // tokenized command string, then we are done, this is the one.
                foreach (Command com in commandList)
                {
                    if (tokens.Length == com.maxTokens)
                    {
                        currentCommand = com;
                        foundMatch = true;
                        break;
                    }// if
                }// foreach

                // Remove all other commands and add the one we found
                if (foundMatch)
                {
                    commandList.Clear();
                    commandList.Add(currentCommand);
                }// if
                // In this case, they have a variable number of tokens in their 
                // command that sits in between two commands maximum allowable
                // tokens, we need to find which command to use
                else
                {
                    foundMatch = false;

                    foreach (Command com in commandList)
                    {
                        if (!(tokens.Length > com.maxTokens))
                        {
                            currentCommand = com;
                            foundMatch = true;
                            commandList.Clear();
                            commandList.Add(com);
                            break;
                        }// if
                    }// foreach
                }// else
            }// (commandList.Count > 1)

            // If we didn't find a command, or if we somehow mistakenly added two
            // verbs, then something went wrong, bail out.
            if (!foundMatch || commandList.Count != 1)
            {
                if (currentCommand.commandName != commandName.INVALID)
                {
                    if (mob is Player)
                        ((Player)mob).mClientHandler.safeWrite("You can't " +
                           currentCommand.command + " like that");
                }
                else
                {
                    if(mob is Player)
                        ((Player)mob).mClientHandler.safeWrite(tokens[0] + " is not a valid command");
                }
                return;
            }// if

            // If the player only sent us a single verb and we found a match, we are done
            // call execute on the verb with no arguements
            if (tokens.Length == 1 && currentCommand.grammar.Length == 1)
            {
                execute(commandList, mob);
                return;
            }// if
            else if((tokens.Length > 1 && currentCommand.grammar.Length == 1) ||
                    (tokens.Length == 1 && currentCommand.grammar.Length > 1) ||
                    tokens.Length > currentCommand.maxTokens)
            {
                if(mob is Player)
                    ((Player)mob).mClientHandler.safeWrite("You can't use the " + currentCommand.command + 
                        " command that way");
                return;
            }// else if
            
            errorCode error = errorCode.E_OK;
            // In this case, the player must have sent us a verb that has multiple arguements.
            // This means we have to parse the grammar of the verb and add back the appropriate
            // predicates to the command list if they even exist.
            error = populateCommandList(currentCommand, command.Substring(tokens[0].Length+1), 
                                        commandList, mob);

            if (error == errorCode.E_OK)
                // All predicates must have checked out, the commandList will be correctly
                // populated with all correct predicates in the right order according to
                // the verbs description.  Go ahead and execute the command
                execute(commandList, mob);
            else if (error == errorCode.E_INVALID_COMMAND_USAGE)
            {
                if(mob is Player)
                   ((Player)mob).mClientHandler.safeWrite("you can't use the " + currentCommand.command + 
                       " command like that");
            }
            else if (error == errorCode.E_INVALID_SYNTAX)
            { } // do nothing it has been handled earlier
            else
            {
                if(mob is Player)
                   ((Player)mob).mClientHandler.safeWrite("You can't do that");
            }
        }// process

        public void execute(ArrayList commandQueue, Mob mob)
        {
            Command currentCommand = new Command();
            Command tempCommand;
            currentCommand = (Command)commandQueue[0];
            bool refreshRoomDesc = false;
            int commandIndex = 0;
            Room currentRoom = mob.mCurrentRoom;
            Player player;
            string clientString = string.Empty;

            // Process the commandList by moving a index through the commandQueue
            // Each command will handle the various predicates given to it
            switch (currentCommand.commandName)
            {
                // Says something to all players in the current room
                case commandName.COMMAND_SAY:
                    ++commandIndex;

                    foreach (Player currentPlayer in currentRoom.mPlayerList)
                    {
                        if (currentPlayer.Equals(mob))
                        {
                            if(mob is Player)
                            ((Player)mob).mClientHandler.safeWrite("You say \"" + commandQueue[commandIndex] +
                                "\"");
                        }// if
                        else
                        {
                            currentPlayer.mClientHandler.safeWrite(mob.mName + 
                                " says " + "\"" + commandQueue[commandIndex] + "\"");
                        }// else
                    }// foreach
                    break;

                case commandName.COMMAND_TELL:
                    ++commandIndex;
                    player = (Player)commandQueue[commandIndex++];
                    if(mob is Player)
                        ((Player)mob).mClientHandler.safeWrite("You tell " + player.mName + " \"" + 
                            commandQueue[commandIndex] + "\"");
                    player.mClientHandler.safeWrite(mob.mName + " tells you \"" + 
                        commandQueue[commandIndex] + "\"");
                    tempCommand = (Command)commandQueue[0];
                    tempCommand.commandOwner = mob;
                    tempCommand.predicate1Value = player;
                    commandQueue[0] = tempCommand;
                    break;

                case commandName.COMMAND_YELL:
                    ++commandIndex;
                    
                    foreach (Player currentPlayer in mob.mCurrentArea.mPlayerList)
                    {
                        if (currentPlayer.Equals(mob))
                        {
                            ((Player)mob).mClientHandler.safeWrite("You yell " + "\"" + commandQueue[commandIndex] +
                                "\"");
                        }// if
                        else
                        {
                            currentPlayer.mClientHandler.safeWrite(mob.mName +
                                " yells \"" + commandQueue[commandIndex] + "\"");
                        }// else
                    }// foreach
                    
                    break;

                case commandName.COMMAND_EXIT:
                    ((Player)mob).mClientHandler.safeWrite("exit was the command");
                    break;

                case commandName.COMMAND_NORTH:
                    refreshRoomDesc = true;
                    if (!mob.move(currentCommand.command))
                        ((Player)mob).mClientHandler.safeWrite("You can't move north");
                    break;

                case commandName.COMMAND_SOUTH:
                    refreshRoomDesc = true;
                    if (!mob.move(currentCommand.command))
                        ((Player)mob).mClientHandler.safeWrite("You can't move south");
                    break;

                case commandName.COMMAND_EAST:
                    refreshRoomDesc = true;
                    if (!mob.move(currentCommand.command))
                        ((Player)mob).mClientHandler.safeWrite("You can't move east");
                    break;

                case commandName.COMMAND_WEST:
                    refreshRoomDesc = true;
                    if (!mob.move(currentCommand.command))
                        ((Player)mob).mClientHandler.safeWrite("You can't move west");
                    break;

                case commandName.COMMAND_UP:
                    refreshRoomDesc = true;
                    if (!mob.move(currentCommand.command))
                        ((Player)mob).mClientHandler.safeWrite("You can't move up");
                    break;

                case commandName.COMMAND_DOWN:
                    refreshRoomDesc = true;
                    if (!mob.move(currentCommand.command))
                        ((Player)mob).mClientHandler.safeWrite("You can't move down");
                    break;

                case commandName.COMMAND_NORTHWEST:
                    refreshRoomDesc = true;
                    if (!mob.move(currentCommand.command))
                        ((Player)mob).mClientHandler.safeWrite("You can't move northwest");
                    break;

                case commandName.COMMAND_NORTHEAST:
                    refreshRoomDesc = true;
                    if (!mob.move(currentCommand.command))
                        ((Player)mob).mClientHandler.safeWrite("You can't move northeast");
                    break;

                case commandName.COMMAND_SOUTHWEST:
                    refreshRoomDesc = true;
                    if (!mob.move(currentCommand.command))
                        ((Player)mob).mClientHandler.safeWrite("You can't move southwest");
                    break;

                case commandName.COMMAND_SOUTHEAST:
                    refreshRoomDesc = true;
                    if (!mob.move(currentCommand.command))
                        ((Player)mob).mClientHandler.safeWrite("You can't move southeast");
                    break;

                case commandName.COMMAND_OPEN:
                    clientString = ((Mob)commandQueue[1]).open(mob);
                    tempCommand = (Command)commandQueue[0];
                    tempCommand.commandOwner = mob;
                    tempCommand.predicate1Value = (Mob)commandQueue[1];
                    commandQueue[0] = tempCommand;
                    ((Player)mob).mClientHandler.safeWrite(clientString);
                    break;

                case commandName.COMMAND_CLOSE:
                    clientString = ((Mob)commandQueue[1]).close(mob);
                    tempCommand = (Command)commandQueue[0];
                    tempCommand.commandOwner = mob;
                    tempCommand.predicate1Value = (Mob)commandQueue[1];
                    commandQueue[0] = tempCommand;
                    ((Player)mob).mClientHandler.safeWrite(clientString);
                    break;

                case commandName.COMMAND_DESTROY:
                    tempCommand = (Command)commandQueue[0];
                    tempCommand.commandOwner = mob;
                    tempCommand.predicate1Value = (Mob)commandQueue[1];
                    commandQueue[0] = tempCommand;
                    ((Player)mob).mClientHandler.safeWrite(((Mob)commandQueue[1]).destroy());
                    break;

                case commandName.COMMAND_GET:
                    tempCommand = (Command)commandQueue[0];
                    tempCommand.commandOwner = mob;
                    tempCommand.predicate1Value = (Mob)commandQueue[1];
                    commandQueue[0] = tempCommand;
                    ((Player)mob).mClientHandler.safeWrite(((Mob)commandQueue[1]).get(mob));
                    break;

                case commandName.COMMAND_DROP:
                    tempCommand = (Command)commandQueue[0];
                    tempCommand.commandOwner = mob;
                    tempCommand.predicate1Value = (Mob)commandQueue[1];
                    commandQueue[0] = tempCommand;
                    ((Player)mob).mClientHandler.safeWrite(((Mob)commandQueue[1]).drop(mob));
                    break;

                case commandName.COMMAND_INVENTORY:
                    tempCommand = (Command)commandQueue[0];
                    tempCommand.commandOwner = mob;
                    commandQueue[0] = tempCommand;
                    ((Player)mob).mClientHandler.safeWrite("Inventory:\n");
                    foreach (Mob mob2 in mob.mInventory)
                    {
                        if(mob2 is Player)
                            ((Player)mob2).mClientHandler.safeWrite(" " + mob.mName);
                    }// foreach
                    break;

                case commandName.COMMAND_LOCK:
                    tempCommand = (Command)commandQueue[0];
                    tempCommand.commandOwner = ((Player)mob).mClientHandler.mPlayer;
                    tempCommand.predicate1Value = (Mob)commandQueue[1];
                    commandQueue[0] = tempCommand;
                    ((Player)mob).mClientHandler.safeWrite(((Mob)commandQueue[1]).lck(mob));
                    break;

                case commandName.COMMAND_UNLOCK:
                    tempCommand = (Command)commandQueue[0];
                    tempCommand.commandOwner = ((Player)mob).mClientHandler.mPlayer;
                    tempCommand.predicate1Value = (Mob)commandQueue[1];
                    commandQueue[0] = tempCommand;
                    ((Player)mob).mClientHandler.safeWrite(((Mob)commandQueue[1]).unlock(mob));
                    break;

                case commandName.COMMAND_SPAWN:
                    try
                    {
                        int mobID = Int32.Parse((string)commandQueue[1]);
                        ArrayList fma = ((Player)mob).mClientHandler.mWorld.mFullMobList;
                        if (mobID < 0 ||
                            mobID > fma.Count)
                        {
                            if(mob is Player)
                                ((Player)mob).mClientHandler.safeWrite("MobID is outside the valid range");
                        }// if
                        else
                        {
                            if (fma[mobID] is Container)
                            {
                                Container cont = new Container();
                                cont = (Container)fma[mobID];
                                cont.mIsActive = true;
                                ((Player)mob).mClientHandler.mWorld.mObjectList.Add(cont);
                                cont.mCurrentArea = ((Player)mob).mClientHandler.mPlayer.mCurrentArea;
                                cont.mCurrentRoom = ((Player)mob).mClientHandler.mPlayer.mCurrentRoom;
                                ((Player)mob).mClientHandler.mPlayer.mCurrentArea.mObjectList.Add(cont);
                                ((Player)mob).mClientHandler.mPlayer.mCurrentRoom.mObjectList.Add(cont);
                                ((Player)mob).mClientHandler.safeWrite("spawning " + cont.mName);
                            }// if
                            else if (fma[mobID] is Mob)
                            {
                                Mob mob2 = new Mob();
                                mob2 = (Mob)fma[mobID];
                                mob2.mIsActive = true;
                                mob2.mCurrentArea = ((Player)mob).mClientHandler.mPlayer.mCurrentArea;
                                mob2.mCurrentRoom = ((Player)mob).mClientHandler.mPlayer.mCurrentRoom;
                                ((Player)mob).mClientHandler.mPlayer.mCurrentArea.mObjectList.Add(mob);
                                ((Player)mob).mClientHandler.mPlayer.mCurrentRoom.mObjectList.Add(mob);
                                ((Player)mob).mClientHandler.safeWrite("spawning " + mob.mName);
                            }// else if
                            else
                            {
                                if(mob is Player)
                                    ((Player)mob).mClientHandler.safeWrite("Something went wrong");
                            }// else
                        }// else
                    }// try
                    catch
                    {
                        if(mob is Player)
                            ((Player)mob).mClientHandler.safeWrite("That is not a valid MobID");
                    }// catch
                    break;

                case commandName.COMMAND_LOOK:

                    // This is a single look command with no arguements, simply print
                    // out the current room the player is in.
                    if (commandQueue.Count == 1)
                        ((Player)mob).mClientHandler.safeWrite(currentRoom.mDescription +
                            "\n" + currentRoom.exitString());
                    // The player looked in a direction, print out that connected rooms
                    // location
                    else if (commandQueue.Count == 2)
                    {
                        switch (((string)commandQueue[++commandIndex]).ToLower())
                        {
                            case "north":
                                if (currentRoom.mNorthLink != null)
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite(currentRoom.mNorthLink.mDescription +
                                           "\n" + currentRoom.mNorthLink.exitString());
                                }
                                else
                                {
                                    if(mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite("There is nothing to the north");
                                }
                                break;

                            case "south":
                                if (currentRoom.mSouthLink != null)
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite(currentRoom.mSouthLink.mDescription +
                                        "\n" + currentRoom.mSouthLink.exitString());
                                }
                                else
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite("There is nothing to the south");
                                }
                                break;

                            case "east":
                                if (currentRoom.mEastLink != null)
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite(currentRoom.mEastLink.mDescription +
                                            "\n" + currentRoom.mEastLink.exitString());
                                }
                                else
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite("There is nothing to the east");
                                }
                                break;

                            case "west":
                                if (currentRoom.mWestLink != null)
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite(currentRoom.mWestLink.mDescription +
                                            "\n" + currentRoom.mWestLink.exitString());
                                }
                                else
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite("There is nothing to the west");
                                }
                                break;

                            case "up":
                                if (currentRoom.mUpLink != null)
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite(currentRoom.mUpLink.mDescription +
                                            "\n" + currentRoom.mUpLink.exitString());
                                }
                                else
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite("There is nothing above");
                                }
                                break;

                            case "down":
                                if (currentRoom.mDownLink != null)
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite(currentRoom.mDownLink.mDescription +
                                            "\n" + currentRoom.mDownLink.exitString());
                                }
                                else
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite("There is nothing below");
                                }
                                break;

                            case "northwest":
                                if (currentRoom.mNorthwestLink != null)
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite(currentRoom.mNorthwestLink.mDescription +
                                            "\n" + currentRoom.mNorthwestLink.exitString());
                                }
                                else
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite("There is nothing to the northwest");
                                }
                                break;

                            case "northeast":
                                if (currentRoom.mNortheastLink != null)
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite(currentRoom.mNortheastLink.mDescription +
                                            "\n" + currentRoom.mNortheastLink.exitString());
                                }
                                else
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite("There is nothing to the northeast");
                                }
                                break;

                            case "southwest":
                                if (currentRoom.mSouthwestLink != null)
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite(currentRoom.mSouthwestLink.mDescription +
                                            "\n" + currentRoom.mSouthwestLink.exitString());
                                }
                                else
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite("There is nothing to the southwest");
                                }
                                break;

                            case "southeast":
                                if (currentRoom.mSoutheastLink != null)
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite(currentRoom.mSoutheastLink.mDescription +
                                            "\n" + currentRoom.mSoutheastLink.exitString());
                                }
                                else
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite("There is nothing to the southeast");
                                }
                                break;

                            default:
                                {
                                    if (mob is Player)
                                        ((Player)mob).mClientHandler.safeWrite("You can't look in that direction");
                                }
                                break;
                        }// switch (((string)commandQueue[++commandIndex]).ToLower())
                    }// else if (commandQueue.Count > 1)
                    else if (commandQueue.Count == 3)
                    {
                        clientString = string.Empty;
                        clientString = ((Mob)commandQueue[2]).viewed(mob,
                            (Preposition)commandQueue[1]);
                        ((Player)mob).mClientHandler.safeWrite(clientString);

                        tempCommand = (Command)commandQueue[0];
                        tempCommand.commandOwner = ((Player)mob).mClientHandler.mPlayer;
                        tempCommand.prep1Value = (Preposition)commandQueue[1];
                        tempCommand.predicate1Value = (Mob)commandQueue[2];
                        commandQueue[0] = tempCommand;
                    }// else if
                    else
                    {
                        clientString = string.Empty;
                        clientString = ((Mob)commandQueue[2]).viewed(mob,
                            (Preposition)commandQueue[1]);
                        ((Player)mob).mClientHandler.safeWrite(clientString);

                        tempCommand = (Command)commandQueue[0];
                        tempCommand.commandOwner = mob;
                        tempCommand.prep1Value = (Preposition)commandQueue[1];
                        tempCommand.predicate1Value = (Mob)commandQueue[2];
                        commandQueue[0] = tempCommand;

                    }// else
                    break;

                default:
                    if(mob is Player)
                        ((Player)mob).mClientHandler.safeWrite("huh?");
                    break;
            }// switch (currentCommand.commandName)

            // If the player moved, remember to actually print out the room
            if (refreshRoomDesc)
            {
                if(mob is Player)
                    ((Player)mob).mClientHandler.safeWrite(mob.mCurrentRoom.mDescription +
                        "\n" + mob.mCurrentRoom.exitString());
            }// if
            
            checkEvent((Command)commandQueue[0], mob);

        }// execute

        private void checkEvent(Command command, Mob mob)
        {
            if (command.predicate1 != predicateType.INVALID &&
                command.predicate2 != predicateType.INVALID)
            {

            }// if
            else if (command.predicate1 != predicateType.INVALID &&
                     command.predicate1 != predicateType.PREDICATE_CUSTOM)
            {
                if ( command.predicate1Value.mEventList.Count > 0)
                {
                    EventData eventData = (EventData)command.predicate1Value.mEventList[0];

                    if(eventData.commandName == command.commandName &&
                       eventData.prepType == command.prep1Value.prepType)
                    {
                        eventData.trigger = command.commandOwner;
                        eventData.eventObject = command.predicate1Value;
                        eventData.eventRoom = command.predicate1Value.mCurrentRoom;
                        eventData.validity = command.validity;
                        ((Player)mob).mClientHandler.mEventHandler.enQueueEvent(eventData);
                    }// if       
                }// if
            }// else if
            else
            {
            }// else
        }// checkEvent

        private errorCode populateCommandList(Command currentCommand, string command,
                              ArrayList commandList, Mob mob)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            predicateType targetPredicate;
            int grammarIndex = 1;
            string[] tokens;
            int predicateCount = 0;
            string errorString = currentCommand.command;

            // Loop until we have gone through all grammar specified by the commands
            // acceptable grammar set
            while (grammarIndex < currentCommand.grammar.Length)
            {
                // We need to know which predicate we need to examine
                if (currentCommand.grammar[grammarIndex++] == grammarType.PREDICATE)
                {
                    if (predicateCount++ == 0)
                        targetPredicate = currentCommand.predicate1;
                    else
                        targetPredicate = currentCommand.predicate2;

                    // If the predicate is a player, we only accept the very next token to
                    // search for a valid playername
                    if (targetPredicate != predicateType.INVALID &&
                        targetPredicate != predicateType.PREDICATE_CUSTOM)
                    {
                        tokens = command.Split(' ');
                        errorString += " " + tokens[0];
                        ret = doesPredicateExist(tokens[0], targetPredicate, currentCommand.validity, commandList,
                                                 mob);

                        if (ret != errorCode.E_OK)
                        {
                            if(mob is Player)
                               ((Player)mob).mClientHandler.safeWrite("Can't find " + tokens[0]);
                            break;
                        }// if

                        if ((grammarIndex < currentCommand.grammar.Length))
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
                        if(mob is Player)
                           ((Player)mob).mClientHandler.safeWrite("You can't " + errorString);
                        break;
                    }// if
                }// if (currentCommand.grammar[grammarIndex++] == grammarType.PREDICATE)
                else if (currentCommand.grammar[grammarIndex-1] == grammarType.PREP)
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
                            if((grammarIndex < currentCommand.grammar.Length))
                                command = command.Substring(tokens[0].Length + 1);
                            break;
                        }// if
                    }// foreach

                    if (ret == errorCode.E_INVALID_SYNTAX)
                    {
                        if(mob is Player)
                            ((Player)mob).mClientHandler.safeWrite("You are not able to " + errorString);
                        break;
                    }// if
                }// else if
            }// while (grammarIndex < currentCommand.grammar.Length)

            if (grammarIndex != currentCommand.grammar.Length)
            {
                return errorCode.E_INVALID_COMMAND_USAGE;
            }// if

            return ret;
        }// populateCommandList

        private errorCode doesPredicateExist(string name, predicateType predType,
                                             validityType validity, ArrayList commandQueue,
                                             Mob target)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            ArrayList targetList = new ArrayList();
            ArrayList targetPredicates = new ArrayList();
            string[] tokens = name.Split('.');

            // Need to know which lists we need to search through to find the target predicate
            if (predType == predicateType.PREDICATE_OBJECT ||
                predType == predicateType.PREDICATE_OBJECT_OR_NPC || 
                predType == predicateType.PREDICATE_OBJECT_OR_PLAYER ||
                predType == predicateType.PREDICATE_ALL)
            {
                if (validity == validityType.VALID_GLOBAL)
                    targetList.Add(target.mWorld.mObjectList);
                if (validity == validityType.VALID_AREA)
                    targetList.Add(target.mCurrentArea.mObjectList);
                if (validity == validityType.VALID_LOCAL)
                {
                    targetList.Add(target.mCurrentRoom.mObjectList);
                    targetList.Add(target.mCurrentRoom.mDoorwayList);
                }// if
                if(validity == validityType.VALID_INVENTORY)
                    targetList.Add(target.mInventory);
                if(validity == validityType.VALID_INVLOCAL)
                {
                    targetList.Add(target.mCurrentRoom.mObjectList);
                    targetList.Add(target.mInventory);
                }// if
            }// if

            if (predType == predicateType.PREDICATE_PLAYER || 
                predType == predicateType.PREDICATE_OBJECT_OR_PLAYER ||
                predType == predicateType.PREDICATE_PLAYER_OR_NPC ||
                predType == predicateType.PREDICATE_ALL)
            {
                if(validity == validityType.VALID_GLOBAL)
                    targetList.Add(target.mWorld.mPlayerList);
                if (validity == validityType.VALID_AREA)
                    targetList.Add(target.mWorld.mAreaList);
                if (validity == validityType.VALID_LOCAL)
                {
                    targetList.Add(target.mCurrentRoom.mPlayerList);
                    targetList.Add(target.mCurrentRoom.mDoorwayList);
                }// if
                if (validity == validityType.VALID_INVENTORY)
                    targetList.Add(target.mInventory);
                if (validity == validityType.VALID_INVLOCAL)
                {
                    targetList.Add(target.mCurrentRoom.mPlayerList);
                    targetList.Add(target.mInventory);
                }// if
            }// if

            if (predType == predicateType.PREDICATE_NPC || 
                predType == predicateType.PREDICATE_OBJECT_OR_NPC ||
                predType == predicateType.PREDICATE_PLAYER_OR_NPC ||
                predType == predicateType.PREDICATE_ALL)
            {
                if (validity == validityType.VALID_GLOBAL)
                    targetList.Add(target.mWorld.mNpcList);
                if (validity == validityType.VALID_AREA)
                    targetList.Add(target.mCurrentArea.mNpcList);
                if (validity == validityType.VALID_LOCAL)
                {
                    targetList.Add(target.mCurrentRoom.mNpcList);
                    targetList.Add(target.mCurrentRoom.mDoorwayList);
                }// if
                if (validity == validityType.VALID_INVENTORY)
                    targetList.Add(target.mInventory);
                if(validity == validityType.VALID_INVLOCAL)
                {
                    targetList.Add(target.mCurrentRoom.mNpcList);
                    targetList.Add(target.mInventory);
                }// if
            }// if
            // TODO
            // See if we have added the same list multiple times
            foreach(ArrayList ar in targetList)
            {
                if (ar.Count > 0)
                {
                    foreach (Mob mob in ar)
                    {
                        if (mob != null &&
                        validatePredicate(tokens[0].ToLower(),
                        mob.exitString(target.mCurrentRoom).ToLower()))
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

                        if (index <= targetPredicates.Count &&
                            index > 0)
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

    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
