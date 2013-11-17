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

    struct Noun
    {
        public string name;
        public string shortName;
        public int matchNumber;

        public Noun(string name, string shortName, int matchNumber)
        {
            this.name = name;
            this.shortName = shortName;
            this.matchNumber = matchNumber;
        }// Constructor
    }// Noun

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

        public errorCode process(string command, ClientHandler clientHandler)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            string[] tokens = command.Split(' ');
            string nounFinder = string.Empty;
            Queue commandQueue = new Queue();

            int matchCounter = 0;
            bool matchFound = false;

            if (tokens.Length > 4)
                ret = errorCode.E_INVALID_SYNTAX;

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

            string currentToken = string.Empty;
            commandType[] grammarType = (commandType [])mGrammarList[grammarIndex];
            commandQueue.Enqueue(grammarType);

            for (int i = 0; i < grammarType.Length; ++i)
            {
                currentToken = tokens[i];

                if (grammarType[i] == commandType.VERB)
                {
                    foreach (Command com in mVerbList)
                    {
                        matchFound = false;
                        matchCounter = 0;

                        if (com.shortName != null && currentToken.Equals(com.shortName))
                        {
                            matchFound = true;
                            ret = errorCode.E_OK;
                            commandQueue.Enqueue(com);
                            break;
                        }

                        if (currentToken.Length > com.command.Length || currentToken.Length < com.matchNumber)
                            continue;

                        for (int j = 0; j < currentToken.Length; ++j)
                        {
                            if (!currentToken[j].Equals(com.command[j]))
                                break;
                            ++matchCounter;
                        }// for

                        if (matchCounter == currentToken.Length)
                        {
                            matchFound = true;
                            ret = errorCode.E_OK;
                            commandQueue.Enqueue(com);
                            break;
                        }// if
                    }// foreach
                }// if verb

                else if (grammarType[i] == commandType.NOUN)
                {
                    matchFound = false;

                    foreach (Noun noun in mNounList)
                    {
                        if (noun.name.Equals(currentToken))
                        {
                            commandQueue.Enqueue(noun);
                            ret = errorCode.E_OK;
                            matchFound = true;
                            break;
                        }// if
                    }// foreach

                    if (!matchFound)
                    {
                        matchFound = doesNounExist(currentToken);
                    }

                    foreach (Noun noun in mNounList)
                    {
                        matchFound = false;
                        matchCounter = 0;

                        if (noun.shortName != null && currentToken.Equals(noun.shortName))
                        {
                            matchFound = true;
                            ret = errorCode.E_OK;
                            commandQueue.Enqueue(noun);
                            break;
                        }

                        if (currentToken.Length > noun.name.Length || currentToken.Length < noun.matchNumber)
                            continue;

                        for (int j = 0; j < currentToken.Length; ++j)
                        {
                            if (!currentToken[j].Equals(noun.name[j]))
                                break;
                            ++matchCounter;
                        }// for

                        if (matchCounter == currentToken.Length)
                        {
                            matchFound = true;
                            ret = errorCode.E_OK;
                            commandQueue.Enqueue(noun);
                            break;
                        }// if
                    }// foreach
                }// else if
                else
                {
                    Console.WriteLine("bad grammar");
                    ret = errorCode.E_INVALID_SYNTAX;
                    break;
                }// else

            }// for

            if(matchFound)
                ret = execute(clientHandler, commandQueue);

            return ret;
        }// execute

        private errorCode execute(ClientHandler clientHandler,
                                  Queue commandQueue)
        {
            errorCode ret = errorCode.E_OK;
            commandType[] grammarType = (commandType [])commandQueue.Dequeue();
            Command currentCommand = new Command();
            Noun noun1 = new Noun();
            Noun noun2 = new Noun();
            Room currentRoom = clientHandler.mPlayer.mCurrentRoom;
            
            while (commandQueue.Count != 0)
            {
                if (commandQueue.Peek().GetType() == currentCommand.GetType())
                {
                    currentCommand = (Command)commandQueue.Dequeue();
                }
                else if (commandQueue.Peek().GetType() == noun1.GetType())
                {
                    noun1 = (Noun)commandQueue.Dequeue();
                }
            }
            if (commandQueue.Count != 0)
            {
                if (commandQueue.Peek().GetType() == noun2.GetType())
                    noun2 = (Noun)commandQueue.Dequeue();
            }

            bool wasMoveCommand = false;

            switch (currentCommand.commandName)
            {
                case commandName.COMMAND_MOVE:
                    
                    break;

                case commandName.COMMAND_LOOK:
                    if (grammarType.Length == 1)
                    {
                        clientHandler.safeWrite(currentRoom.mDescription +
                            "\n" + currentRoom.exitString());
                    }// if
                    else if (grammarType.Length == 2)
                    {
                        switch (noun1.name)
                        {
                            case "north":
                                if (currentRoom.mNorthLink != null)
                                {
                                    clientHandler.safeWrite(currentRoom.mNorthLink.mDescription +
                                        "\n" + currentRoom.mNorthLink.exitString());
                                }// if
                                else
                                {
                                    clientHandler.safeWrite("There is nothing to the north");
                                }// else
                                break;

                            case "south":
                                if (currentRoom.mSouthLink != null)
                                {
                                    clientHandler.safeWrite(currentRoom.mSouthLink.mDescription +
                                        "\n" + currentRoom.mSouthLink.exitString());
                                }// if
                                else
                                {
                                    clientHandler.safeWrite("There is nothing to the south");
                                }// else
                                break;

                            case "east":
                                if (currentRoom.mEastLink != null)
                                {
                                    clientHandler.safeWrite(currentRoom.mEastLink.mDescription +
                                        "\n" + currentRoom.mEastLink.exitString());
                                }// if
                                else
                                {
                                    clientHandler.safeWrite("There is nothing to the east");
                                }// else
                                break;

                            case "west":
                                if (currentRoom.mWestLink != null)
                                {
                                    clientHandler.safeWrite(currentRoom.mWestLink.mDescription +
                                        "\n" + currentRoom.mWestLink.exitString());
                                }// if
                                else
                                {
                                    clientHandler.safeWrite("There is nothing to the west");
                                }// else
                                break;

                            case "up":
                                if (currentRoom.mUpLink != null)
                                {
                                    clientHandler.safeWrite(currentRoom.mUpLink.mDescription +
                                        "\n" + currentRoom.mUpLink.exitString());
                                }// if
                                else
                                {
                                    clientHandler.safeWrite("There is nothing above");
                                }// else
                                break;

                            case "down":
                                if (currentRoom.mDownLink != null)
                                {
                                    clientHandler.safeWrite(currentRoom.mDownLink.mDescription +
                                        "\n" + currentRoom.mDownLink.exitString());
                                }// if
                                else
                                {
                                    clientHandler.safeWrite("There is nothing below");
                                }// else
                                break;

                            default:
                                clientHandler.safeWrite("You can't look that way");
                                break;
                        }// switch
                    }// else if grammar == 2
                    else if (grammarType.Length == 3)
                    {
                        clientHandler.safeWrite("You " + currentCommand.command + " " + noun1.name + " "
                            + noun2.name);
                    }// else if
                    else
                    {
                        ret = errorCode.E_INVALID_SYNTAX;
                    }// else
                    break;

                case commandName.COMMAND_EXIT:
                    clientHandler.safeWrite("exit was the command");
                    break;

                case commandName.COMMAND_NORTH:
                    wasMoveCommand = true;
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move north");
                    break;

                case commandName.COMMAND_SOUTH:
                    wasMoveCommand = true;
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move south");
                    break;

                case commandName.COMMAND_EAST:
                    wasMoveCommand = true;
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move east");
                    break;

                case commandName.COMMAND_WEST:
                    wasMoveCommand = true;
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move west");
                    break;

                case commandName.COMMAND_UP:
                    wasMoveCommand = true;
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move up");
                    break;

                case commandName.COMMAND_DOWN:
                    wasMoveCommand = true;
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move down");
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

        private void addCommands()
        {
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

            Noun noun = new Noun("north", "n", 2);
            mNounList.Add(noun);
            noun = new Noun("northwest", "nw", 6);
            mNounList.Add(noun);
            noun = new Noun("northeast", "ne", 6);
            mNounList.Add(noun);
            noun = new Noun("south", "s", 2);
            mNounList.Add(noun);
            noun = new Noun("southwest", "sw", 6);
            mNounList.Add(noun);
            noun = new Noun("southeast", "se", 6);
            mNounList.Add(noun);
            noun = new Noun("east", "e", 2);
            mNounList.Add(noun);
            noun = new Noun("west", "w", 2);
            mNounList.Add(noun);
            noun = new Noun("up", "u", 2);
            mNounList.Add(noun);
            noun = new Noun("down", "d", 2);
            mNounList.Add(noun);
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

        private bool doesNounExist(string name)
        {
            return false;
        }
    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
