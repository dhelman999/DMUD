﻿using System;
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

            pt = new Command("look", null, 1, 1, grammarType.VERB, gramVerb, commandName.COMMAND_LOOK,
                predicateType.INVALID, predicateType.INVALID, validityType.VALID_LOCAL);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", null, 1, 256, grammarType.VERB, gramVerbPred, commandName.COMMAND_LOOK,
                predicateType.PREDICATE_CUSTOM, predicateType.INVALID, validityType.VALID_LOCAL);
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

        public errorCode process(string command, ClientHandler clientHandler)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            string[] tokens = command.Split(' ');
            if (tokens.Length == 0)
                return ret;

            Command currentCommand = new Command();
            ArrayList commandList = new ArrayList();
            bool foundMatch = false;

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
                return ret;

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
                return ret;

            // If the player only sent us a single verb and we found a match, we are done
            // call execute on the verb with no arguements
            if (tokens.Length == 1)
                return execute(commandList, clientHandler);
            
            string subCommand = string.Empty;
 
            // In this case, the player must have sent us a verb that has multiple arguements.
            // This means we have to parse the grammar of the verb and add back the appropriate
            // predicates to the command list if they even exist.
            ret = populateCommandList(currentCommand, command.Substring(tokens[0].Length+1), 
                                      commandList, clientHandler);

            // If all predicates either didn't exist, or could not be validated, then we
            // error out as we cannot continue.
            if (ret != errorCode.E_OK)
                return errorCode.E_INVALID_SYNTAX;
            else
                // All predicates must have checked out, the commandList will be correctly
                // populated with all correct predicates in the right order according to
                // the verbs description.  Go ahead and execute the command
                ret = execute(commandList, clientHandler);

            return ret;
        }// process

        public errorCode execute(ArrayList commandQueue, ClientHandler clientHandler)
        {
            errorCode ret = errorCode.E_OK;

            Command currentCommand = new Command();
            currentCommand = (Command)commandQueue[0];
            bool wasMoveCommand = false;
            int commandIndex = 0;
            Room currentRoom = clientHandler.mPlayer.mCurrentRoom;

            // Process the commandList by moving a index through the commandQueue
            // Each command will handle the various predicates given to it
            switch (currentCommand.commandName)
            {
                // Says something to all players in the current room
                case commandName.COMMAND_SAY:
                    ++commandIndex;

                    foreach (Player player in currentRoom.mPlayerList)
                    {
                        if (player.Equals(clientHandler.mPlayer))
                        {
                            clientHandler.safeWrite("You say \"" + commandQueue[commandIndex] +
                                "\"");
                        }// if
                        else
                        {
                            player.mClientHandler.safeWrite(clientHandler.mPlayer.mName + " says " +
                                "\"" + commandQueue[commandIndex] + "\"");
                        }// else
                    }// foreach
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

                case commandName.COMMAND_NORTHWEST:
                    wasMoveCommand = true;
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move northwest");
                    break;

                case commandName.COMMAND_NORTHEAST:
                    wasMoveCommand = true;
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move northeast");
                    break;

                case commandName.COMMAND_SOUTHWEST:
                    wasMoveCommand = true;
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move southwest");
                    break;

                case commandName.COMMAND_SOUTHEAST:
                    wasMoveCommand = true;
                    if (!clientHandler.mPlayer.move(currentCommand.command))
                        clientHandler.safeWrite("You can't move southeast");
                    break;

                case commandName.COMMAND_LOOK:

                    // This is a single look command with no arguements, simply print
                    // out the current room the player is in.
                    if (commandQueue.Count == 1)
                        clientHandler.safeWrite(currentRoom.mDescription +
                            "\n" + currentRoom.exitString());
                    // The player looked in a direction, print out that connected rooms
                    // location
                    else if (commandQueue.Count > 1)
                    {
                        ret = errorCode.E_OK;

                        switch (((string)commandQueue[++commandIndex]).ToLower())
                        {
                            case "north":
                                if (currentRoom.mNorthLink != null)
                                    clientHandler.safeWrite(currentRoom.mNorthLink.mDescription +
                                        "\n" + currentRoom.mNorthLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the north");
                                break;

                            case "south":
                                if (currentRoom.mSouthLink != null)
                                    clientHandler.safeWrite(currentRoom.mSouthLink.mDescription +
                                        "\n" + currentRoom.mSouthLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the south");
                                break;

                            case "east":
                                if (currentRoom.mEastLink != null)
                                    clientHandler.safeWrite(currentRoom.mEastLink.mDescription +
                                        "\n" + currentRoom.mEastLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the east");
                                break;

                            case "west":
                                if (currentRoom.mWestLink != null)
                                    clientHandler.safeWrite(currentRoom.mWestLink.mDescription +
                                        "\n" + currentRoom.mWestLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the west");
                                break;

                            case "up":
                                if (currentRoom.mUpLink != null)
                                    clientHandler.safeWrite(currentRoom.mUpLink.mDescription +
                                        "\n" + currentRoom.mUpLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing above");
                                break;

                            case "down":
                                if (currentRoom.mDownLink != null)
                                    clientHandler.safeWrite(currentRoom.mDownLink.mDescription +
                                        "\n" + currentRoom.mDownLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing below");
                                break;

                            case "northwest":
                                if (currentRoom.mNorthwestLink != null)
                                    clientHandler.safeWrite(currentRoom.mNorthwestLink.mDescription +
                                        "\n" + currentRoom.mNorthwestLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the northwest");
                                break;

                            case "northeast":
                                if (currentRoom.mNortheastLink != null)
                                    clientHandler.safeWrite(currentRoom.mNortheastLink.mDescription +
                                        "\n" + currentRoom.mNortheastLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the northeast");
                                break;

                            case "southwest":
                                if (currentRoom.mSouthwestLink != null)
                                    clientHandler.safeWrite(currentRoom.mSouthwestLink.mDescription +
                                        "\n" + currentRoom.mSouthwestLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the southwest");
                                break;

                            case "southeast":
                                if (currentRoom.mSoutheastLink != null)
                                    clientHandler.safeWrite(currentRoom.mSoutheastLink.mDescription +
                                        "\n" + currentRoom.mSoutheastLink.exitString());
                                else
                                    clientHandler.safeWrite("There is nothing to the southeast");
                                break;

                            default:
                                clientHandler.safeWrite("You can't look in that direction");
                                break;
                        }// switch (((string)commandQueue[++commandIndex]).ToLower())
                    }// else if (commandQueue.Count > 1)
                    break;

                default:
                    ret = errorCode.E_INVALID_SYNTAX;
                    break;
            }// switch (currentCommand.commandName)

            // If the player moved, remember to actually print out the room
            if (wasMoveCommand)
            {
                clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.mDescription +
                    "\n" + clientHandler.mPlayer.mCurrentRoom.exitString());
            }// if

            return ret;

        }// execute

        private errorCode populateCommandList(Command currentCommand, string command,
                              ArrayList commandList, ClientHandler clientHandler)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            predicateType targetPredicate;
            int grammarIndex = 1;
            string[] tokens;
            int predicateCount = 0;

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
                    if (targetPredicate == predicateType.PREDICATE_PLAYER)
                    {
                        tokens = command.Split(' ');
                        ret = doesPredicateExist(tokens[0], targetPredicate, currentCommand.validity, commandList,
                                                 clientHandler);
                        command = command.Substring(tokens[0].Length + 1);

                    }// if
                    // If the predicate is custom, we simply dump the rest of the command
                    // back and let processing continue
                    else if (targetPredicate == predicateType.PREDICATE_CUSTOM)
                    {
                        commandList.Add(command);
                        ret = errorCode.E_OK;
                    }// else if

                    if (ret == errorCode.E_INVALID_SYNTAX)
                        return errorCode.E_INVALID_SYNTAX;
                }// if (currentCommand.grammar[grammarIndex++] == grammarType.PREDICATE)
            }// while (grammarIndex < currentCommand.grammar.Length)

            return ret;
        }// populateCommandList

        private errorCode doesPredicateExist(string name, predicateType predType,
                                        validityType validity, ArrayList commandQueue,
                                        ClientHandler clientHandler)
        {
            errorCode ret = errorCode.E_INVALID_SYNTAX;
            ArrayList targetList = new ArrayList();

            // Need to know which lists we need to search through to find the target predicate
            if (predType == predicateType.PREDICATE_PLAYER && validity == validityType.VALID_GLOBAL)
                targetList = clientHandler.mWorld.mPlayerList;

            // Search for a player
            foreach (Player player in targetList)
            {
                if (player.mName.ToLower().Contains(name.ToLower()))
                {
                    ret = errorCode.E_OK;
                    commandQueue.Add(player);
                    break;
                }// if
            }// foreach

            return ret;
        }// doesPredicateExist

    }// Class CommandExecuter

}// Namespace _8th_Circle_Server
