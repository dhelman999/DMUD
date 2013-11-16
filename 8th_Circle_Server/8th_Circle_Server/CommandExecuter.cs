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

        public CommandExecuter()
        {
            mVerbList = new ArrayList();
            mNounList = new ArrayList();
            mPrepList = new ArrayList();
            mCommandList = new ArrayList();

            Command pt;

            pt = new Command("move", 1, commandType.VERB, commandName.COMMAND_MOVE);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", 1, commandType.VERB, commandName.COMMAND_LOOK);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("exit", 1, commandType.VERB, commandName.COMMAND_EXIT);
            mCommandList.Add(pt);
            mVerbList.Add(pt);
        }// Constructor

        public errorCode execute(string command, ClientHandler clientHand)
        {
            errorCode ret = errorCode.E_OK;
            string[] tokens = command.Split('.');
            Command currentCommand;
            currentCommand.type = commandType.INVALID;
            currentCommand.commandName = commandName.COMMAND_INVALID;

            if (tokens.Length > 2)
                ret = errorCode.E_INVALID_SYNTAX;

            foreach (Command com in mCommandList)
            {
                if (command.Equals(com.command))
                {
                    currentCommand = com;
                }// if
            }// foreach

            if (currentCommand.type != commandType.VERB)
                return errorCode.E_INVALID_SYNTAX;

            switch (currentCommand.commandName)
            {
                case commandName.COMMAND_MOVE:
                    clientHand.safeWrite("move was the command");
                    break;

                case commandName.COMMAND_LOOK:
                    clientHand.safeWrite("look was the command");
                    break;

                case commandName.COMMAND_EXIT:
                    clientHand.safeWrite("exit was the command");
                    break;

                default:
                    ret = errorCode.E_INVALID_SYNTAX;
                    break;
            }// switch

            return ret;
        }// execute

    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
