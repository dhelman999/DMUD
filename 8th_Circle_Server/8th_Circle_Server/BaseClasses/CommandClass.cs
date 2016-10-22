using System;
using System.Collections;

namespace _8th_Circle_Server
{
    // Base class for all commands in the game.  All commands always start with a verb, then have many defining characteristics like
    // where they are valid, what grammar they expect, short names ect.  Each command is expected to derive from this class and implement
    // the functionality to execute the command.
    public abstract class CommandClass
    {
        // Validity defines where to look for predicates in the game world
        public ValidityType mValidity;

        
        

        // The fully qualified string name
        protected String mCommand;

        // The shortcut name, does not have to be the same letters of the command
        private String mShortName;
        // How many tokens this command expects, and how many are maximally allowed, this is multiple commands of the same name are
        // differentiated from each other, such as look, vs look west, vs look in chest.
        private int mMatchNumber;
        private int mMaxTokens;

        // The exact grammar applicable for the command to function
        private Grammar[] mGrammar;

        // Holds the prepisitions used to operate on the predicates
        private Preposition mPrep1Value;
        private Preposition mPrep2Value;

        // The enum representation of the command, probably can be combined with the mCommand
        private CommandName mCommandName;

        // Predicate type and predicates store what objects and their type need to be operated on
        public PredicateType mPredicate1;
        public PredicateType mPredicate2;
        private Mob mPredicate1Value;
        private Mob mPredicate2Value;

        // TODO
        // probably not needed when events have their own class.
        // Mainly used to understand who is executing the command, only used by events.
        private Mob mCommandOwner;

        public CommandClass()
        {
            Utils.SetFlag(ref mValidity, ValidityType.LOCAL);
        }// dummy Constructor

        public CommandClass(String command, String shortName, int matchNumber, int maxTokens, MobType type, 
                            Grammar[] grammar, CommandName CommandName, PredicateType predicate1, 
                            PredicateType predicate2, ValidityType validity = ValidityType.LOCAL)
        {
            mCommand = command;
            mCommandOwner = null;
            mShortName = shortName;
            mMatchNumber = matchNumber;
            mGrammar = grammar;
            mMaxTokens = maxTokens;
            mCommandName = CommandName;
            mPredicate1 = predicate1;
            mPredicate2 = predicate2;
            mValidity = validity;
            mPrep1Value = new Preposition();
            mPrep2Value = new Preposition();
            mPredicate1Value = mPredicate2Value = null;
        }// Constructor

        // TODO
        // Need to add pre and post command flags and checks to the command itself so that this can be generically added when the command
        // is created and then checked generically.
        // Any pre-execute conditions happen here 
        public virtual String preExecute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            String clientString = String.Empty;
            CommandClass commandClass = (CommandClass)commandQueue[0];

            if (!(commandClass.GetCommandName() == CommandName.COMMAND_REST ||
                commandClass.GetCommandName() == CommandName.COMMAND_LOOK))
            {
                Utils.UnsetFlag(ref mob.mFlags, MobFlags.RESTING);
            }

            return clientString;
        }// preExecute

        // Child classes must implement the execution of the command
        public abstract errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString);

        // Accessors
        public String GetCommand() { return mCommand; }
        public String GetShortName() { return mShortName; }
        public int GetMatchNumber() { return mMatchNumber; }
        public int GetMaxTokens() { return mMaxTokens; }
        public Grammar[] GetGrammar() { return mGrammar; }
        public Preposition GetPrep1() { return mPrep1Value; }
        public void SetPrep1(Preposition prep) { mPrep1Value = prep; }
        public Preposition GetPrep2() { return mPrep2Value; }
        public void SetPrep2(Preposition prep) { mPrep2Value = prep; }
        public CommandName GetCommandName() { return mCommandName; }
        public Mob GetPred1() { return mPredicate1Value; }
        public void SetPred1(Mob pred) { mPredicate1Value = pred; }
        public Mob GetPred2() { return mPredicate2Value; }
        public void SetPred2(Mob pred) { mPredicate2Value = pred; }
        public Mob GetCommOwner() { return mCommandOwner; }
        public void SetCommOwner(Mob owner) { mCommandOwner = owner; }

    }// class CommandClass

}// namespace _8th_Circle_Server
