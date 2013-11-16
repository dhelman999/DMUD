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
            string[] tokens = command.Split('.');
            Command currentCommand = new Command();
            currentCommand.type = commandType.INVALID;
            currentCommand.commandName = commandName.COMMAND_INVALID;

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

            for (int i = 0; i < grammarType.Length; ++i)
            {
                foreach (Command com in mCommandList)
                {
                    matchFound = false;
                    matchCounter = 0;

                    if (command.Length > com.command.Length || command.Length < com.matchNumber)
                        continue;

                    for (int j = 0; j < command.Length; ++j)
                    {
                        if (!command[j].Equals(com.command[j]))
                            break;
                        ++matchCounter;
                    }// for

                    if (matchCounter == command.Length)
                    {
                        matchFound = true;
                        currentCommand = com;
                        ret = errorCode.E_OK;
                        break;
                    }// if
                }// foreach

                if (matchFound && (currentCommand.type == grammarType[i]))
                {
                    break;
                }// if
                else
                {
                    ret = errorCode.E_INVALID_SYNTAX;
                }// else
            }// for     

            execute(currentCommand, clientHandler);

            return ret;
        }// execute

        private errorCode execute(Command currentCommand, ClientHandler clientHandler)
        {
            errorCode ret = errorCode.E_OK;

            switch (currentCommand.commandName)
            {
                case commandName.COMMAND_MOVE:
                    clientHandler.safeWrite("move was the command");
                    break;

                case commandName.COMMAND_LOOK:
                    clientHandler.safeWrite("look was the command");
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

    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
