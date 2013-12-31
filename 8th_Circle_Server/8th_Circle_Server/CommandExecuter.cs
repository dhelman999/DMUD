using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;

namespace _8th_Circle_Server
{
    public enum grammarType
    {
        GRAMMAR_START,
        VERB = GRAMMAR_START,
        PREP,
        PREDICATE,
        GRAMMAR_END
    };// grammarType

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
        PREDICATE_END
    }// predicateType

    public enum validityType
    {
        VALID_START,
        VALID_INVENTORY = VALID_START,
        VALID_LOCAL,
        VALID_INVLOCAL,
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
        COMMAND_INVENTORY,
        COMMAND_LOCK,
        COMMAND_UNLOCK,
        COMMAND_DROP,
        COMMAND_SPAWN,
        COMMAND_DESTROY,
        COMMAND_USE,
        COMMAND_SEARCH,
        COMMAND_WHO,
        COMMAND_FULLHEAL,
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

    public class CommandExecuter
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
            Command pt = new Command("up", "u", 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_UP,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("north", "n", 5, 1, grammarType.VERB, gramVerb, commandName.COMMAND_NORTH,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("northeast", "ne", 2, 1, grammarType.VERB, gramVerb, commandName.COMMAND_NORTHEAST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("east", "e", 2, 1, grammarType.VERB, gramVerb, commandName.COMMAND_EAST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("southeast", "se", 6, 1, grammarType.VERB, gramVerb, commandName.COMMAND_SOUTHEAST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("down", "d", 2, 1, grammarType.VERB, gramVerb, commandName.COMMAND_DOWN,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("south", "s", 5, 1, grammarType.VERB, gramVerb, commandName.COMMAND_SOUTH,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("southwest", "sw", 6, 1, grammarType.VERB, gramVerb, commandName.COMMAND_SOUTHWEST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("west", "w", 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_WEST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("northwest", "nw", 6, 1, grammarType.VERB, gramVerb, commandName.COMMAND_NORTHWEST,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("exit", null, 2, 1, grammarType.VERB, gramVerb, commandName.COMMAND_EXIT,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", null, 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_LOOK,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", null, 1, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_LOOK,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", null, 1, 3, grammarType.VERB, gramVerbPrepPred, commandName.COMMAND_LOOK,
                predicateType.PREDICATE_ALL, predicateType.PREDICATE_END, validityType.VALID_INVLOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("say", null, 3, 256, grammarType.VERB, gramVerbPred, commandName.COMMAND_SAY,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("yell", null, 2, 256, grammarType.VERB, gramVerbPred, commandName.COMMAND_YELL,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_AREA);
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

            pt = new Command("fullheal", "fh", 2, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_FULLHEAL,
                predicateType.PREDICATE_PLAYER_OR_NPC, predicateType.PREDICATE_CUSTOM, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("close", null, 2, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_CLOSE,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("destroy", null, 3, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_DESTROY,
                predicateType.PREDICATE_OBJECT_OR_NPC, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("spawn", null, 3, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_SPAWN,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("get", null, 1, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_GET,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_INVLOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("get", null, 1, 4, grammarType.VERB, gramVerbPredPrepPred, commandName.COMMAND_GET,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_OBJECT, validityType.VALID_INVLOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("drop", null, 2, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_DROP,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_INVENTORY);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("inventory", null, 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_INVENTORY,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
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

            pt = new Command("use", null, 2, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_USE,
                predicateType.PREDICATE_OBJECT, predicateType.PREDICATE_CUSTOM, validityType.VALID_INVLOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("search", null, 3, 1, grammarType.VERB, gramVerb, commandName.COMMAND_SEARCH,
                predicateType.PREDICATE_END, predicateType.PREDICATE_END, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("who", null, 2, 2, grammarType.VERB, gramVerbPred, commandName.COMMAND_WHO,
                predicateType.PREDICATE_CUSTOM, predicateType.PREDICATE_END, validityType.VALID_GLOBAL);
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
            string clientString = string.Empty;

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
                if (currentCommand.commandName != commandName.COMMAND_END)
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
            if (tokens.Length == 1 && 
                currentCommand.grammar.Length == 1 &&
                mob.mActionTimer == 0)
            {
                clientString = execute(commandList, mob);
                if (mob is Player)
                {
                    clientString += ((Player)mob).playerString();
                    ((Player)mob).mClientHandler.safeWrite(clientString);
                }
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
            {
                // All predicates must have checked out, the commandList will be correctly
                // populated with all correct predicates in the right order according to
                // the verbs description.  Go ahead and execute the command
                clientString = execute(commandList, mob);
                if (mob is Player)
                {
                    clientString += ((Player)mob).playerString();
                    ((Player)mob).mClientHandler.safeWrite(clientString);
                }
            }
            else if (error == errorCode.E_INVALID_COMMAND_USAGE)
            {
                if (mob is Player)
                    ((Player)mob).mClientHandler.safeWrite("you can't use the " + currentCommand.command +
                        " command like that");
            }
            else if (error == errorCode.E_INVALID_SYNTAX)
            { } // do nothing it has been handled earlier
            else
            {
                if (mob is Player)
                {
                    clientString += "You can't do that\n";
                    clientString += ((Player)mob).playerString();
                    ((Player)mob).mClientHandler.safeWrite(clientString);
                }
            }
        }// process

        public string execute(ArrayList commandQueue, Mob mob)
        {
            Command currentCommand = new Command();
            currentCommand = (Command)commandQueue[0];
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
                    foreach (Player currentPlayer in currentRoom.getRes(ResType.PLAYER))
                    {
                        if (currentPlayer.Equals(mob))
                            clientString = "You say \"" + commandQueue[commandIndex] + "\"";
                        else
                            clientString = mob.mName + " says " + "\"" + commandQueue[commandIndex] + "\"";

                        currentPlayer.mClientHandler.safeWrite(clientString);
                    }// foreach
                    return "";

                case commandName.COMMAND_TELL:
                    ++commandIndex;
                    player = (Player)commandQueue[commandIndex++];
                    if(mob is Player)
                        ((Player)mob).mClientHandler.safeWrite("You tell " + player.mName + " \"" + 
                            commandQueue[commandIndex] + "\"");
                    player.mClientHandler.safeWrite(mob.mName + " tells you \"" + 
                        commandQueue[commandIndex] + "\"");
                    break;

                case commandName.COMMAND_YELL:
                    ++commandIndex;
                    foreach (Player currentPlayer in mob.mCurrentArea.getRes(ResType.PLAYER))
                    {
                        if (currentPlayer.Equals(mob))
                            clientString = "You yell " + "\"" + commandQueue[commandIndex] + "\"";
                        else
                            clientString = mob.mName + " yells \"" + commandQueue[commandIndex] + "\"";

                        currentPlayer.mClientHandler.safeWrite(clientString);
                    }// foreach
                    return "";

                case commandName.COMMAND_EXIT:
                    clientString = "exit was the command";
                    break;

                case commandName.COMMAND_NORTH:
                case commandName.COMMAND_SOUTH:
                case commandName.COMMAND_EAST:
                case commandName.COMMAND_WEST:
                case commandName.COMMAND_UP:
                case commandName.COMMAND_DOWN:
                case commandName.COMMAND_NORTHWEST:
                case commandName.COMMAND_NORTHEAST:
                case commandName.COMMAND_SOUTHWEST:
                case commandName.COMMAND_SOUTHEAST:
                    clientString = mob.move(currentCommand.command);
                    break;

                case commandName.COMMAND_OPEN: 
                    clientString = ((Mob)commandQueue[++commandIndex]).open(mob);
                    break;

                case commandName.COMMAND_CLOSE:
                    clientString = ((Mob)commandQueue[++commandIndex]).close(mob);
                    break;

                case commandName.COMMAND_DESTROY:
                    clientString = ((Mob)commandQueue[++commandIndex]).destroy();
                    break;

                case commandName.COMMAND_GET:
                    ++commandIndex;
                    if (commandQueue.Count == 2)
                        clientString = ((Mob)commandQueue[commandIndex]).get(mob);
                    else if (commandQueue.Count == 4)
                    {
                        clientString = ((Mob)commandQueue[commandIndex]).get(mob, 
                                       ((Preposition)commandQueue[++commandIndex]).prepType,
                                       (Mob)commandQueue[++commandIndex]);
                    }// else if
                    else
                        clientString = "you can't get like that";
                    break;

                case commandName.COMMAND_DROP:
                    clientString = ((Mob)commandQueue[++commandIndex]).drop(mob);
                    break;

                case commandName.COMMAND_INVENTORY:
                    clientString += "Inventory:\n";
                    foreach (Mob mob2 in mob.mInventory)
                        clientString += " " + mob2.mName + "\n";
                    break;

                case commandName.COMMAND_LOCK:
                    clientString = ((Mob)commandQueue[++commandIndex]).lck(mob);
                    break;

                case commandName.COMMAND_UNLOCK:
                    clientString = ((Mob)commandQueue[++commandIndex]).unlock(mob);
                    break;

                case commandName.COMMAND_USE:
                    clientString = ((Mob)commandQueue[++commandIndex]).use(mob);
                    break;

                case commandName.COMMAND_SEARCH:
                    clientString = "you start searching...";
                    mob.mActionTimer = 4;
                    mob.mFlagList.Add(mobFlags.FLAG_SEARCHING);
                    Thread searchThread = new Thread(() => searchTask(mob));
                    searchThread.Start();
                    break;

                case commandName.COMMAND_FULLHEAL:
                    clientString = ((Mob)commandQueue[++commandIndex]).fullheal();
                    break;

                case commandName.COMMAND_WHO:
                    ++commandIndex;
                    if (((string)commandQueue[commandIndex]).Equals("all"))
                    {
                        clientString = "\nPlayer\t\tArea\n\n";
                        foreach (Player pl in mob.mWorld.getRes(ResType.PLAYER))
                            clientString += pl.mName + "\t\t" + pl.mCurrentArea.mName + "\n";
                    }// if
                    else if (((string)commandQueue[commandIndex]).Equals("area"))
                    {
                        clientString = "\nPlayer\t\tArea\n\n";
                        foreach (Player pl in mob.mCurrentArea.getRes(ResType.PLAYER))
                            clientString += pl.mName + "\t\t" + pl.mCurrentArea.mName + "\n";
                    }// else if
                    else
                        return "you can't use who like that";

                    return clientString;

                case commandName.COMMAND_SPAWN:
                    try
                    {
                        int mobID = Int32.Parse((string)commandQueue[++commandIndex]);
                        ArrayList fma = ((Player)mob).mClientHandler.mWorld.mFullMobList;
                        if (mobID < 0 ||
                            mobID > fma.Count)
                            clientString = "MobID is outside the valid range";
                        else
                        {
                            if (fma[mobID] is Container)
                            {
                                Container cont = new Container();
                                cont = (Container)fma[mobID];
                                ((Player)mob).mClientHandler.mWorld.addRes(cont);
                                cont.mCurrentArea = ((Player)mob).mClientHandler.mPlayer.mCurrentArea;
                                cont.mCurrentRoom = ((Player)mob).mClientHandler.mPlayer.mCurrentRoom;
                                ((Player)mob).mClientHandler.mPlayer.mCurrentArea.addRes(cont);
                                ((Player)mob).mClientHandler.mPlayer.mCurrentRoom.addRes(cont);
                                clientString = "spawning " + cont.mName;
                            }// if
                            else if (fma[mobID] is Mob)
                            {
                                Mob mob2 = new Mob();
                                mob2 = (Mob)fma[mobID];
                                mob2.mCurrentArea = ((Player)mob).mClientHandler.mPlayer.mCurrentArea;
                                mob2.mCurrentRoom = ((Player)mob).mClientHandler.mPlayer.mCurrentRoom;
                                ((Player)mob).mClientHandler.mPlayer.mCurrentArea.addRes(mob2);
                                ((Player)mob).mClientHandler.mPlayer.mCurrentRoom.addRes(mob2);
                                clientString = "spawning " + mob2.mName;
                            }// else if
                            else
                                clientString = "Something went wrong";
                        }// else
                    }// try
                    catch
                    { 
                        clientString = "That is not a valid MobID"; 
                    }
                    break;

                case commandName.COMMAND_LOOK:
                    // This is a single look command with no arguements, simply print
                    // out the current room the player is in.
                    if (commandQueue.Count == 1)
                        clientString = currentRoom.viewed();
                    // The player looked in a direction, print out that connected rooms
                    // location
                    else if (commandQueue.Count == 2)
                        clientString = mob.mCurrentRoom.viewed((string)commandQueue[++commandIndex]);
                    else if (commandQueue.Count == 3)
                        clientString = ((Mob)commandQueue[2]).viewed(mob,(Preposition)commandQueue[1]);
                    else
                        clientString = ((Mob)commandQueue[2]).viewed(mob, (Preposition)commandQueue[1]);
                    break;

                default:
                    clientString = "huh?";
                    break;
            }// switch (currentCommand.commandName)

            checkEvent(commandQueue, mob);

            return clientString;
        }// execute

        private void checkEvent(ArrayList commandQueue, Mob mob)
        {
            // Populate the event args with the triggers and room in which
            // the event was triggered.
            fillEventArgs(commandQueue, mob);

            Command command = (Command)commandQueue[0];

            if (command.predicate1 != predicateType.PREDICATE_END &&
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
                    if (targetPredicate != predicateType.PREDICATE_END &&
                        targetPredicate != predicateType.PREDICATE_CUSTOM)
                    {
                        tokens = command.Split(' ');
                        errorString += " " + tokens[0];
                        ret = doesPredicateExist(tokens[0], 
                                                 targetPredicate, 
                                                 currentCommand.validity, 
                                                 commandList,
                                                 mob);

                        if (ret != errorCode.E_OK)
                        {
                            if(mob is Player)
                               ((Player)mob).mClientHandler.safeWrite("Can't find " + tokens[0]);
                            break;
                        }// if

                        if (grammarIndex < currentCommand.grammar.Length)
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
                        if(mob is Player)
                            ((Player)mob).mClientHandler.safeWrite("You are not able to " + errorString);
                        break;
                    }// if
                }// else if
            }// while (grammarIndex < currentCommand.grammar.Length)

            if (grammarIndex != currentCommand.grammar.Length)
                return errorCode.E_INVALID_COMMAND_USAGE;

            return ret;
        }// populateCommandList

        private errorCode doesPredicateExist(string name, 
                                             predicateType predType,
                                             validityType validity, 
                                             ArrayList commandQueue,
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

        private void fillEventArgs(ArrayList commandQueue, Mob mob)
        {
            Command command = (Command)commandQueue[0];
            grammarType []grammar = command.grammar;
            int predicateCount = 0;
            int prepCount = 0;

            if (mob is Player)
                command.commandOwner = (Player)mob;
            else
                command.commandOwner = mob;

            for (int i = 1; i < grammar.Length; ++i)
            {
                if (grammar[i] == grammarType.PREDICATE)
                {
                    if (++predicateCount == 1)
                       if(!(commandQueue[i] is string))
                            command.predicate1Value = (Mob)commandQueue[i];
                    else
                       if (!(commandQueue[i] is string))
                          command.predicate2Value = (Mob)commandQueue[i];
                }// if
                else if (grammar[i] == grammarType.PREP)
                {
                    if (++prepCount == 1)
                        command.prep1Value = (Preposition)commandQueue[i];
                    else
                        command.prep2Value = (Preposition)commandQueue[i];
                }// if
            }// for

            commandQueue[0] = command;
        }// fillEventArgs

        public static void searchTask(Mob mob)
        {
            Thread.Sleep(4000);
            string searchResult = mob.search();
            if (mob is Player)
                ((Player)mob).mClientHandler.safeWrite(searchResult);
            mob.mFlagList.Remove(mobFlags.FLAG_SEARCHING);
        }// searchTask

    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
