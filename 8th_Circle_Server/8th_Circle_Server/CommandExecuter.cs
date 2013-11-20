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
        VALID_AREA,
        VALID_GLOBAL,
        INVALID
    };// validityType

    enum commandName
    {
        COMMAND_MOVE=0,
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
        INVALID
    };// commandName

    enum errorCode
    {
        E_OK = 0,
        E_INVALID_SYNTAX
    };// errorCode

    struct Command
    {
        public string command;
        public string shortName;
        public int matchNumber;
        public int maxTokens;
        public grammarType type;
        public grammarType[] grammar;
        public commandName commandName;
        public predicateType predicate1;
        public predicateType predicate2;
        public validityType validity;

        public Command(string command, string shortName, int matchNumber, 
                       int maxTokens, grammarType type, grammarType[] grammar,
                       commandName commandName, predicateType predicate1,
                       predicateType predicate2, validityType validity)
        {
            this.command = command;
            this.shortName = shortName;
            this.matchNumber = matchNumber;
            this.type = type;
            this.grammar = grammar;
            this.maxTokens = maxTokens;
            this.commandName = commandName;
            this.predicate1 = predicate1;
            this.predicate2 = predicate2;
            this.validity = validity;
        }// Constructor
    }// Command

    class CommandExecuter
    {
        // Debug
        internal const bool DEBUG = true;

        // Member Variables
        public ArrayList mNounList;
        public ArrayList mVerbList;
        public ArrayList mPrepList;
        public ArrayList mCommandList;
        public ArrayList mGrammarList;

        public CommandExecuter()
        {
            mVerbList = new ArrayList();
            mNounList = new ArrayList();
            mPrepList = new ArrayList();
            mCommandList = new ArrayList();
            mGrammarList = new ArrayList();

            addCommands();
            addGrammar();
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

            pt = new Command("look", null, 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_LOOK,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", null, 1, 256, grammarType.VERB, gramVerbPred, commandName.COMMAND_LOOK,
                predicateType.PREDICATE_CUSTOM, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("exit", null, 2, 1, grammarType.VERB, gramVerb, commandName.COMMAND_EXIT,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
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
        }// addCommands;

        private void addGrammar()
        {
            grammarType[] grammar = new grammarType[1];
            grammar[0] = grammarType.VERB;
            mGrammarList.Add(grammar);

            grammar = new grammarType[2];
            grammar[0] = grammarType.VERB;
            grammar[1] = grammarType.PREDICATE;
            mGrammarList.Add(grammar);

            grammar = new grammarType[3];
            grammar[0] = grammarType.VERB;
            grammar[1] = grammarType.PREDICATE;
            grammar[2] = grammarType.PREDICATE;
            mGrammarList.Add(grammar);

            grammar = new grammarType[4];
            grammar[0] = grammarType.VERB;
            grammar[1] = grammarType.PREDICATE;
            grammar[2] = grammarType.PREP;
            grammar[3] = grammarType.PREDICATE;
            mGrammarList.Add(grammar);
        }// addGrammar

        public errorCode process(string command, ClientHandler clientHandler)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            string[] tokens = command.Split(' ');
            string PREDICATEFinder = string.Empty;
            ArrayList commandQueue = new ArrayList();
            Command currentCommand = new Command();
            bool matchFound = false;

            // First check for say, yell and tell as these are treated differently
            if(tokens[0].Equals("say") ||
               tokens[0].Equals("yell"))
            {
                string firstToken;
                if (tokens.Length < 2)
                    return ret;

                firstToken = tokens[1];
                string lastToken = tokens[tokens.Length-1];
                if (!firstToken[0].Equals('"') || 
                    !lastToken[lastToken.Length-1].Equals('"'))
                {
                    if (!firstToken[0].Equals('"'))
                    {
                        Console.WriteLine("first failed: " + firstToken[0]);
                    }
                    if (!lastToken[lastToken.Length - 1].Equals('"'))
                    {
                        Console.WriteLine("second failed: " + lastToken[lastToken.Length-1]);
                    }
                    return ret;
                }
                
                currentCommand.commandName = commandName.COMMAND_SAY;
                string sayString = command.Substring(3, command.Length - 3);
                currentCommand.command = sayString;
                commandQueue.Add((grammarType[])mGrammarList[0]);
                commandQueue.Add(currentCommand);
                execute(clientHandler, commandQueue);
                return errorCode.E_OK;       
            }// if
            else if (tokens[0].Equals("tell"))
            {
                commandQueue.Add((Command)mGrammarList[1]);
            }// else if

            // Anything more than 4 tokens is an error
            if (tokens.Length > 4)
                ret = errorCode.E_INVALID_SYNTAX;

            // First, find out which grammar from the master grammar list we are dealing with
            int grammarIndex = -1;
            for (int i = 0; i < mGrammarList.Count; ++i)
            {
                if (((grammarType[])mGrammarList[i]).Length == tokens.Length)
                {
                    grammarIndex = i;
                    break;
                }// if
            }// for

            if (grammarIndex == -1)
                return errorCode.E_INVALID_SYNTAX;

            // Save the grammar type we are dealing with
            grammarType[] grammar = (grammarType [])mGrammarList[grammarIndex];
            commandQueue.Add(grammar);
            string currentToken = string.Empty;

            // Now, loop through all the tokens, and try to validate the tokens syntax and
            // correctness.  
            for (int i = 0; i < grammar.Length; ++i)
            {
                currentToken = tokens[i];
                ArrayList targetList = new ArrayList();

                if (grammar[i] == grammarType.VERB)
                    targetList = mVerbList;
                else if (grammar[i] == grammarType.PREDICATE)
                    targetList = mNounList;
                else if (grammar[i] == grammarType.PREP)
                    targetList = mPrepList;
                else
                    ret = errorCode.E_INVALID_SYNTAX;

                foreach (Command com in targetList)
                {
                    if (validateCommand(commandQueue, currentToken, com))
                    {
                        ret = errorCode.E_OK;
                        matchFound = true;
                        break;
                    }// if
                }// foreach

                if (ret != errorCode.E_OK)
                    break;
            }// validate syntax for loop

            // We have found a valid syntax and a valid list of tokens, now we need to execute
            // the actions
            if(matchFound)
                ret = execute(clientHandler, commandQueue);

            return ret;
        }// execute

        private errorCode execute(ClientHandler clientHandler,
                                  ArrayList commandQueue)
        {
            errorCode ret = errorCode.E_OK;
            grammarType[] grammarType = (grammarType [])commandQueue[0];
            commandQueue.RemoveAt(0);
            Room currentRoom = clientHandler.mPlayer.mCurrentRoom;
            int commandIndex = 0;     
            bool wasMoveCommand = false;
            Command currentCommand = (Command)commandQueue[commandIndex];

            switch (((Command)commandQueue[commandIndex++]).commandName)
            {
                case commandName.COMMAND_SAY:
                    foreach (Player player in currentRoom.mPlayerList)
                    {
                        if (player.Equals(clientHandler.mPlayer))
                        {
                            clientHandler.safeWrite("You say" + currentCommand.command);
                        }// if
                        else
                        {
                            player.mClientHandler.safeWrite(clientHandler.mPlayer.mName + " says" +
                                currentCommand.command);
                        }// else
                    }// foreach
                    break;

                case commandName.COMMAND_MOVE:                
                    break;

                case commandName.COMMAND_LOOK:
                    if (grammarType.Length == 1)
                        clientHandler.safeWrite(currentRoom.mDescription +
                            "\n" + currentRoom.exitString());
                    else if (grammarType.Length == 2)
                    {
                        currentCommand = (Command)commandQueue[commandIndex];

                        switch (((Command)commandQueue[commandIndex++]).commandName)
                        {
                            case commandName.COMMAND_NORTH:
                                if (currentRoom.mNorthLink != null)
                                    clientHandler.safeWrite(currentRoom.mNorthLink.mDescription +
                                        "\n" + currentRoom.mNorthLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the north");
                                break;

                            case commandName.COMMAND_SOUTH:
                                if (currentRoom.mSouthLink != null)
                                    clientHandler.safeWrite(currentRoom.mSouthLink.mDescription +
                                        "\n" + currentRoom.mSouthLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the south");
                                break;

                            case commandName.COMMAND_EAST:
                                if (currentRoom.mEastLink != null)
                                    clientHandler.safeWrite(currentRoom.mEastLink.mDescription +
                                        "\n" + currentRoom.mEastLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the east");
                                break;

                            case commandName.COMMAND_WEST:
                                if (currentRoom.mWestLink != null)
                                    clientHandler.safeWrite(currentRoom.mWestLink.mDescription +
                                        "\n" + currentRoom.mWestLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the west");
                                break;

                            case commandName.COMMAND_UP:
                                if (currentRoom.mUpLink != null)
                                    clientHandler.safeWrite(currentRoom.mUpLink.mDescription +
                                        "\n" + currentRoom.mUpLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing above");
                                break;

                            case commandName.COMMAND_DOWN:
                                if (currentRoom.mDownLink != null)
                                    clientHandler.safeWrite(currentRoom.mDownLink.mDescription +
                                        "\n" + currentRoom.mDownLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing below");
                                break;

                            case commandName.COMMAND_NORTHWEST:
                                if (currentRoom.mNorthwestLink != null)
                                    clientHandler.safeWrite(currentRoom.mNorthwestLink.mDescription +
                                        "\n" + currentRoom.mNorthwestLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the northwest");
                                break;

                            case commandName.COMMAND_NORTHEAST:
                                if (currentRoom.mNortheastLink != null)
                                    clientHandler.safeWrite(currentRoom.mNortheastLink.mDescription +
                                        "\n" + currentRoom.mNortheastLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the northeast");
                                break;

                            case commandName.COMMAND_SOUTHWEST:
                                if (currentRoom.mSouthwestLink != null)
                                    clientHandler.safeWrite(currentRoom.mSouthwestLink.mDescription +
                                        "\n" + currentRoom.mSouthwestLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the southwest");
                                break;

                            case commandName.COMMAND_SOUTHEAST:
                                if (currentRoom.mSoutheastLink != null)
                                    clientHandler.safeWrite(currentRoom.mSoutheastLink.mDescription +
                                        "\n" + currentRoom.mSoutheastLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the southeast");
                                break;

                            default:
                                clientHandler.safeWrite("You can't look that way");
                                break;
                        }// switch
                    }// else if (grammarType.Length == 2)
                    else if (grammarType.Length == 3)
                    {
                        currentCommand = (Command)commandQueue[commandIndex];
                        clientHandler.safeWrite("You " + ((Command)commandQueue[0]).commandName
                            + " " + ((Command)commandQueue[1]).commandName + " " +
                            ((Command)commandQueue[2]).commandName);
                    }// else
                    else
                        ret = errorCode.E_INVALID_SYNTAX;
                    break;

                case commandName.COMMAND_EXIT:
                    clientHandler.safeWrite("exit was the command");
                    break;

                case commandName.COMMAND_NORTH:
                    wasMoveCommand = true;
                    currentCommand = (Command)commandQueue[commandIndex - 1];
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move north");
                    break;

                case commandName.COMMAND_SOUTH:
                    wasMoveCommand = true;
                    currentCommand = (Command)commandQueue[commandIndex - 1];
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move south");
                    break;

                case commandName.COMMAND_EAST:
                    wasMoveCommand = true;
                    currentCommand = (Command)commandQueue[commandIndex - 1];
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move east");
                    break;

                case commandName.COMMAND_WEST:
                    wasMoveCommand = true;
                    currentCommand = (Command)commandQueue[commandIndex - 1];
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move west");
                    break;

                case commandName.COMMAND_UP:
                    wasMoveCommand = true;
                    currentCommand = (Command)commandQueue[commandIndex - 1];
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move up");
                    break;

                case commandName.COMMAND_DOWN:
                    wasMoveCommand = true;
                    currentCommand = (Command)commandQueue[commandIndex - 1];
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move down");
                    break;

                case commandName.COMMAND_NORTHWEST:
                    wasMoveCommand = true;
                    currentCommand = (Command)commandQueue[commandIndex - 1];
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move northwest");
                    break;

                case commandName.COMMAND_NORTHEAST:
                    wasMoveCommand = true;
                    currentCommand = (Command)commandQueue[commandIndex - 1];
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move northeast");
                    break;

                case commandName.COMMAND_SOUTHWEST:
                    wasMoveCommand = true;
                    currentCommand = (Command)commandQueue[commandIndex - 1];
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move southwest");
                    break;

                case commandName.COMMAND_SOUTHEAST:
                    wasMoveCommand = true;
                    currentCommand = (Command)commandQueue[commandIndex - 1];
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move southeast");
                    break;

                default:
                    ret = errorCode.E_INVALID_SYNTAX;
                    break;
            }// switch

            if (wasMoveCommand)
            {
                clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.mDescription +
                    "\n" + clientHandler.mPlayer.mCurrentRoom.exitString());
            }// if

            return ret;

        }// execute

        private bool validateCommand(ArrayList commandQueue, 
                                     string currentToken, 
                                     Command com)
        {
            bool matchFound = false;
            int matchCounter = 0;

            // Check the shortname first
            if (com.shortName != null && currentToken.Equals(com.shortName))
            {
                commandQueue.Add(com);
                return true;
            }// if

            // Check bounds
            if (currentToken.Length > com.command.Length || currentToken.Length < com.matchNumber)
                return matchFound;

            // Do a character by character comparison trying to find a match
            for (int j = 0; j < currentToken.Length; ++j)
            {
                if (!currentToken[j].Equals(com.command[j]))
                    return matchFound;
                ++matchCounter;
            }// for

            // Found a match, queue the token
            if (matchCounter == currentToken.Length)
            {
                matchFound = true;
                commandQueue.Add(com);
            }// if

            if (com.type == grammarType.PREDICATE)
            {
                matchFound = true;
                commandQueue.Add(com);
            }// if

            return matchFound;
        }// validateCommand

        private bool validateCommand2(ArrayList commandQueue,
                                      string currentToken,
                                      Command com)
        {
            bool matchFound = false;
            int matchCounter = 0;

            // Check the shortname first
            if (com.shortName != null && currentToken.Equals(com.shortName))
            {
                commandQueue.Add(com);
                return true;
            }// if

            // Check bounds
            if (currentToken.Length > com.command.Length || currentToken.Length < com.matchNumber)
                return matchFound;

            // Do a character by character comparison trying to find a match
            for (int j = 0; j < currentToken.Length; ++j)
            {
                if (!currentToken[j].Equals(com.command[j]))
                    return matchFound;
                ++matchCounter;
            }// for

            // Found a match, queue the token
            if (matchCounter == currentToken.Length)
            {
                matchFound = true;
                commandQueue.Add(com);
            }// if

            if (com.type == grammarType.PREDICATE)
            {
                matchFound = true;
                commandQueue.Add(com);
            }// if

            return matchFound;
        }// validateCommand2

        private errorCode doesPredicateExist(string name, predicateType predType, 
                                        validityType validity, ArrayList commandQueue,
                                        ClientHandler clientHandler)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            ArrayList targetList = new ArrayList();

            if (predType == predicateType.PREDICATE_CUSTOM)
            {
                commandQueue.Add(name);
                return errorCode.E_OK;
            }
            if (predType == predicateType.PREDICATE_PLAYER && validity == validityType.VALID_GLOBAL)
                targetList = clientHandler.mWorld.mPlayerList;

            foreach (Player player in targetList)
            {
                if (player.mName.ToLower().Contains(name.ToLower()))
                {
                    ret = errorCode.E_OK;
                    commandQueue.Add(player);
                    break;
                }
            }

            return ret;
        }// doesPredicateExist2

        public errorCode process2(string command, ClientHandler clientHandler)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            string[] tokens = command.Split(' ');
            if (tokens.Length == 0)
                return ret;

            Command currentCommand = new Command();
            ArrayList commandList = new ArrayList();
            bool foundMatch = false;

            foreach (Command com in mVerbList)
            {
                if (tokens[0].Equals(com.shortName))
                {
                    foundMatch = true;
                    currentCommand = com;
                    break;
                }
                if (tokens[0].Length < com.matchNumber || tokens[0].Length > com.command.Length)
                {
                    continue;
                }
                if(com.command.Contains(tokens[0]))
                {
                    foundMatch = true;
                    currentCommand = com;
                    break;
                }
            }// foreach

            if (!foundMatch)
                return ret;

            commandList.Add(currentCommand);

            if (tokens.Length == 1)
                execute2(commandList, clientHandler);
            
            string subCommand = string.Empty;
 
            ret = populateCommandList(currentCommand, command.Substring(tokens[0].Length+1), 
                                      commandList, clientHandler);

            if (ret != errorCode.E_OK)
                return errorCode.E_INVALID_SYNTAX;
            else
                ret = execute2(commandList, clientHandler);

            return ret;
        }// validateCommand2

        errorCode populateCommandList(Command currentCommand, string command, 
                                      ArrayList commandList, ClientHandler clientHandler)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            predicateType targetPredicate;
            int grammarIndex = 1;
            string[] tokens;
            int predicateCount = 0;

            while(grammarIndex < currentCommand.grammar.Length)
            {
                if (currentCommand.grammar[grammarIndex++] == grammarType.PREDICATE)
                {
                    if (predicateCount++ == 0)
                        targetPredicate = currentCommand.predicate1;
                    else
                        targetPredicate = currentCommand.predicate2;

                    if (targetPredicate == predicateType.PREDICATE_PLAYER)
                    {
                        tokens = command.Split(' ');
                        ret = doesPredicateExist(tokens[0], targetPredicate, currentCommand.validity, commandList,
                                                 clientHandler);
                        command = command.Substring(tokens[0].Length + 1);

                    }
                    else if (targetPredicate == predicateType.PREDICATE_CUSTOM)
                    {
                        ret = doesPredicateExist(command, targetPredicate, currentCommand.validity, commandList,
                                                 clientHandler);
                    }

                    if (ret == errorCode.E_INVALID_SYNTAX)
                        return errorCode.E_INVALID_SYNTAX;
                }
            }

            return ret;
        }

        public errorCode execute2(ArrayList commandList, ClientHandler clientHandler)
        {
            Console.WriteLine("Command is: " + ((Command)commandList[0]).command);
            Console.WriteLine("target player is: " + ((Player)commandList[1]).mName);
            Console.WriteLine("tell is: " + ((string)commandList[2]));
            return errorCode.E_OK;
        }// execute2

    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
