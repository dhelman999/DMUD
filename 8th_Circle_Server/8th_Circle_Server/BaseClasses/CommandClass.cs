using System;
using System.Collections;

namespace _8th_Circle_Server
{
    public class CommandClass
    {
        public ValidityType mValidity;
        public PredicateType mPredicate1;
        public PredicateType mPredicate2;

        protected String mCommand;

        private String mShortName;
        private int mMatchNumber;
        private int mMaxTokens;
        private Grammar[] mGrammar;
        private Preposition mPrep1Value;
        private Preposition mPrep2Value;
        private CommandName mCommandName;
        private Mob mPredicate1Value;
        private Mob mPredicate2Value;
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

        public virtual errorCode execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner, ref String clientString)
        {
            clientString = "huh";

            return errorCode.E_INVALID_COMMAND_USAGE;
        }// execute

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
