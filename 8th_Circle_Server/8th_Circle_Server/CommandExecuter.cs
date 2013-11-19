using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum commandType
    {
        VERB = 0,
        NOUN,
        PREP,
        DATA,
        INVALID
    };// commandType

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
        COMMAND_INVALID
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
        public commandType type;
        public commandName commandName;
        
        public Command(string command, string shortName,
                       int matchNumber, commandType type, 
                       commandName commandName)
        {
            this.command = command;
            this.shortName = shortName;
            this.matchNumber = matchNumber;
            this.type = type;
            this.commandName = commandName;
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
            // Add Verbs
            Command pt = new Command("north", "n", 5, commandType.VERB, commandName.COMMAND_NORTH);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("south", "s", 5, commandType.VERB, commandName.COMMAND_SOUTH);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("east", "e", 2, commandType.VERB, commandName.COMMAND_EAST);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("west", "w", 1, commandType.VERB, commandName.COMMAND_WEST);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("up", "u", 1, commandType.VERB, commandName.COMMAND_UP);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("down", "d", 1, commandType.VERB, commandName.COMMAND_DOWN);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("northeast", "ne", 1, commandType.VERB, commandName.COMMAND_NORTHEAST);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("northwest", "nw", 6, commandType.VERB, commandName.COMMAND_NORTHWEST);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("southeast", "se", 6, commandType.VERB, commandName.COMMAND_SOUTHEAST);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("southwest", "sw", 6, commandType.VERB, commandName.COMMAND_SOUTHWEST);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("move", null, 1, commandType.VERB, commandName.COMMAND_MOVE);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", null, 1, commandType.VERB, commandName.COMMAND_LOOK);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("exit", null, 2, commandType.VERB, commandName.COMMAND_EXIT);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("say", null, 3, commandType.VERB, commandName.COMMAND_SAY);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("yell", null, 2, commandType.VERB, commandName.COMMAND_YELL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("tell", null, 1, commandType.VERB, commandName.COMMAND_TELL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            // Add Nouns
            pt = new Command("north", "n", 5, commandType.NOUN, commandName.COMMAND_NORTH);
            mCommandList.Add(pt);
            mNounList.Add(pt);

            pt = new Command("south", "s", 5, commandType.NOUN, commandName.COMMAND_SOUTH);
            mCommandList.Add(pt);
            mNounList.Add(pt);

            pt = new Command("east", "e", 2, commandType.NOUN, commandName.COMMAND_EAST);
            mCommandList.Add(pt);
            mNounList.Add(pt);

            pt = new Command("west", "w", 1, commandType.NOUN, commandName.COMMAND_WEST);
            mCommandList.Add(pt);
            mNounList.Add(pt);

            pt = new Command("up", "u", 1, commandType.NOUN, commandName.COMMAND_UP);
            mCommandList.Add(pt);
            mNounList.Add(pt);

            pt = new Command("down", "d", 1, commandType.NOUN, commandName.COMMAND_DOWN);
            mCommandList.Add(pt);
            mNounList.Add(pt);

            pt = new Command("northeast", "ne", 1, commandType.NOUN, commandName.COMMAND_NORTHEAST);
            mCommandList.Add(pt);
            mNounList.Add(pt);

            pt = new Command("northwest", "nw", 6, commandType.NOUN, commandName.COMMAND_NORTHWEST);
            mCommandList.Add(pt);
            mNounList.Add(pt);

            pt = new Command("southeast", "se", 6, commandType.NOUN, commandName.COMMAND_SOUTHEAST);
            mCommandList.Add(pt);
            mNounList.Add(pt);

            pt = new Command("southwest", "sw", 6, commandType.NOUN, commandName.COMMAND_SOUTHWEST);
            mCommandList.Add(pt);
            mNounList.Add(pt);
        }// addCommands;

        private void addGrammar()
        {
            commandType[] grammarType = new commandType[1];
            grammarType[0] = commandType.VERB;
            mGrammarList.Add(grammarType);

            grammarType = new commandType[2];
            grammarType[0] = commandType.VERB;
            grammarType[1] = commandType.NOUN;
            mGrammarList.Add(grammarType);

            grammarType = new commandType[3];
            grammarType[0] = commandType.VERB;
            grammarType[1] = commandType.NOUN;
            grammarType[2] = commandType.NOUN;
            mGrammarList.Add(grammarType);

            grammarType = new commandType[4];
            grammarType[0] = commandType.VERB;
            grammarType[1] = commandType.NOUN;
            grammarType[2] = commandType.PREP;
            grammarType[3] = commandType.NOUN;
            mGrammarList.Add(grammarType);
        }// addGrammar

        public errorCode process(string command, ClientHandler clientHandler)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            string[] tokens = command.Split(' ');
            string nounFinder = string.Empty;
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
                commandQueue.Add((commandType[])mGrammarList[0]);
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
                if (((commandType[])mGrammarList[i]).Length == tokens.Length)
                {
                    grammarIndex = i;
                    break;
                }// if
            }// for

            if (grammarIndex == -1)
                return errorCode.E_INVALID_SYNTAX;

            // Save the grammar type we are dealing with
            commandType[] grammarType = (commandType [])mGrammarList[grammarIndex];
            commandQueue.Add(grammarType);
            string currentToken = string.Empty;

            // Now, loop through all the tokens, and try to validate the tokens syntax and
            // correctness.  
            for (int i = 0; i < grammarType.Length; ++i)
            {
                currentToken = tokens[i];
                ArrayList targetList = new ArrayList();

                if (grammarType[i] == commandType.VERB)
                    targetList = mVerbList;
                else if (grammarType[i] == commandType.NOUN)
                    targetList = mNounList;
                else if (grammarType[i] == commandType.PREP)
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
            commandType[] grammarType = (commandType [])commandQueue[0];
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

        private bool doesNounExist(string name)
        {
            return false;
        }// doesNounExist

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

            if (com.type == commandType.NOUN && doesNounExist(currentToken))
            {
                matchFound = true;
                commandQueue.Add(com);
            }// if

            return matchFound;
        }// validateCommand

    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
