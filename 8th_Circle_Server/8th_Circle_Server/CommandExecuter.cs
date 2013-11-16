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
        COMMAND_LOOK=1,
        COMMAND_EXIT=2,
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
        public int matchNumber;
        public commandType type;
        public commandName commandName;
        
        public Command(string command, int matchNumber, commandType type, commandName commandName)
        {
            this.command = command;
            this.matchNumber = matchNumber;
            this.type = type;
            this.commandName = commandName;
        }// Constructor
    }// Command

    struct Noun
    {
        public string name;
        public int matchNumber;

        public Noun(string name, int matchNumber)
        {
            this.name = name;
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
            Command currentCommand = new Command();
            currentCommand.type = commandType.INVALID;
            currentCommand.commandName = commandName.COMMAND_INVALID;
            Noun noun1 = new Noun();
            Noun noun2;

            int matchCounter = 0;
            bool matchFound = false;

            if (tokens.Length > 4)
                ret = errorCode.E_INVALID_SYNTAX;

            commandType[] grammarType = new commandType[1];
            grammarType[0] = commandType.INVALID;

            switch(tokens.Length)
            {
                case 0:
                    ret = errorCode.E_INVALID_SYNTAX;
                    break;

                case 1:
                    grammarType = new commandType[1];
                    grammarType[0] = commandType.VERB;
                    break;

                case 2:
                    grammarType = new commandType[2];
                    grammarType[0] = commandType.VERB;
                    grammarType[1] = commandType.NOUN;
                    break;

                case 3:
                    grammarType = new commandType[3];
                    grammarType[0] = commandType.VERB;
                    grammarType[1] = commandType.NOUN;
                    grammarType[2] = commandType.NOUN;
                    break;

                case 4:
                    grammarType = new commandType[4];
                    grammarType[0] = commandType.VERB;
                    grammarType[1] = commandType.NOUN;
                    grammarType[2] = commandType.PREP;
                    grammarType[1] = commandType.NOUN;
                    break;

                default:
                    ret = errorCode.E_INVALID_SYNTAX;
                    break;
            }// switch

            string currentToken = string.Empty;

            for (int i = 0; i < grammarType.Length; ++i)
            {
                currentToken = tokens[i];

                if (grammarType[i] == commandType.VERB)
                {
                    foreach (Command com in mVerbList)
                    {
                        matchFound = false;
                        matchCounter = 0;

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
                            currentCommand = com;
                            ret = errorCode.E_OK;
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
                            noun1 = noun;
                            ret = errorCode.E_OK;
                            matchFound = true;
                            break;
                        }// if
                    }// foreach

                    if (!matchFound)
                    {
                        matchFound = doesNounExist(currentToken);
                    }
                }// else if
                else
                {
                    Console.WriteLine("bad grammar");
                    ret = errorCode.E_INVALID_SYNTAX;
                    break;
                }// else

            }// for

            if(matchFound)
                execute(currentCommand, clientHandler, grammarType, noun1);

            return ret;
        }// execute

        private errorCode execute(Command currentCommand, 
                                  ClientHandler clientHandler,
                                  commandType[] grammarType,
                                  Noun noun1)
        {
            errorCode ret = errorCode.E_OK;

            switch (currentCommand.commandName)
            {
                case commandName.COMMAND_MOVE:
                    if (grammarType.Length == 2)
                    {
                        switch (noun1.name)
                        {
                            case "north":
                                if (!clientHandler.mPlayer.move(noun1.name))
                                    clientHandler.safeWrite("You can't move north");
                                break;

                            case "south":
                                if (!clientHandler.mPlayer.move(noun1.name))
                                    clientHandler.safeWrite("You can't move south");
                                break;

                            case "east":
                                if (!clientHandler.mPlayer.move(noun1.name))
                                    clientHandler.safeWrite("You can't move east");
                                break;

                            case "west":
                                if (!clientHandler.mPlayer.move(noun1.name))
                                    clientHandler.safeWrite("You can't move west");
                                break;

                            case "up":
                                if (!clientHandler.mPlayer.move(noun1.name))
                                    clientHandler.safeWrite("You can't move up");
                                break;

                            case "down":
                                if (!clientHandler.mPlayer.move(noun1.name))
                                    clientHandler.safeWrite("You can't move down");
                                break;

                            default:
                                clientHandler.safeWrite("You can't move that way");
                                break;
                        }// switch

                        clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.mDescription +
                            "\n" + clientHandler.mPlayer.mCurrentRoom.exitString());
                    }
                    else
                    {
                        ret = errorCode.E_INVALID_SYNTAX;
                    }
                    break;

                case commandName.COMMAND_LOOK:
                    if (grammarType.Length == 1)
                    {
                        clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.mDescription +
                            "\n" + clientHandler.mPlayer.mCurrentRoom.exitString());
                    }// if
                    else if (grammarType.Length == 2)
                    {
                        switch (noun1.name)
                        {
                            case "north":
                                if (clientHandler.mPlayer.mCurrentRoom.mNorthLink != null)
                                {
                                    clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.mNorthLink.mDescription +
                                        "\n" + clientHandler.mPlayer.mCurrentRoom.mNorthLink.exitString());
                                }
                                else
                                {
                                    clientHandler.safeWrite("There isn't anything to the north");
                                }
                                break;

                            case "south":
                                if (clientHandler.mPlayer.mCurrentRoom.mSouthLink != null)
                                {
                                    clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.mSouthLink.mDescription +
                                        "\n" + clientHandler.mPlayer.mCurrentRoom.mSouthLink.exitString());
                                }
                                else
                                {
                                    clientHandler.safeWrite("There isn't anything to the south");
                                }
                                break;

                            case "east":
                                if (clientHandler.mPlayer.mCurrentRoom.mEastLink != null)
                                {
                                    clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.mEastLink.mDescription +
                                        "\n" + clientHandler.mPlayer.mCurrentRoom.mEastLink.exitString());
                                }
                                else
                                {
                                    clientHandler.safeWrite("There isn't anything to the east");
                                }
                                break;

                            case "west":
                                if (clientHandler.mPlayer.mCurrentRoom.mWestLink != null)
                                {
                                    clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.mWestLink.mDescription +
                                        "\n" + clientHandler.mPlayer.mCurrentRoom.mWestLink.exitString());
                                }
                                else
                                {
                                    clientHandler.safeWrite("There isn't anything to the west");
                                }
                                break;

                            case "up":
                                if (clientHandler.mPlayer.mCurrentRoom.mUpLink != null)
                                {
                                    clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.mUpLink.mDescription +
                                        "\n" + clientHandler.mPlayer.mCurrentRoom.mUpLink.exitString());
                                }
                                else
                                {
                                    clientHandler.safeWrite("There isn't anything above");
                                }
                                break;

                            case "down":
                                if (clientHandler.mPlayer.mCurrentRoom.mDownLink != null)
                                {
                                    clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.mDownLink.mDescription +
                                        "\n" + clientHandler.mPlayer.mCurrentRoom.mDownLink.exitString());
                                }
                                else
                                {
                                    clientHandler.safeWrite("There isn't anything below");
                                }
                                break;

                            default:
                                clientHandler.safeWrite("You can't look that way");
                                break;
                        }// switch
                    }// else if grammar == 2
                    else
                    {
                        ret = errorCode.E_INVALID_SYNTAX;
                    }
                    break;

                case commandName.COMMAND_EXIT:
                    clientHandler.safeWrite("exit was the command");
                    break;

                default:
                    ret = errorCode.E_INVALID_SYNTAX;
                    break;
            }// switch

            
            return ret;

        }// execute

        private void addCommands()
        {
            Command pt = new Command("move", 1, commandType.VERB, commandName.COMMAND_MOVE);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", 1, commandType.VERB, commandName.COMMAND_LOOK);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("exit", 1, commandType.VERB, commandName.COMMAND_EXIT);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            Noun noun = new Noun("north", 2);
            mNounList.Add(noun);
            noun = new Noun("south", 2);
            mNounList.Add(noun);
            noun = new Noun("east", 2);
            mNounList.Add(noun);
            noun = new Noun("west", 2);
            mNounList.Add(noun);
            noun = new Noun("up", 2);
            mNounList.Add(noun);
            noun = new Noun("down", 2);
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
            Console.WriteLine("does noun exist?");
            return true;
        }
    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
