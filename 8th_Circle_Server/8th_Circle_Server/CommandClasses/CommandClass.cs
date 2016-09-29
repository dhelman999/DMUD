using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    public class CommandClass
    {
        public string command;
        public string shortName;
        public int matchNumber;
        public int maxTokens;
        public MobType usableBy;
        public GrammarType[] grammar;
        public Preposition prep1Value;
        public Preposition prep2Value;
        public commandName commandName;
        public predicateType predicate1;
        public predicateType predicate2;
        public Mob predicate1Value;
        public Mob predicate2Value;
        public Mob commandOwner;
        public validityType validity;
        public CommandType comType;

        public CommandClass(string command, string shortName, int matchNumber, int maxTokens, MobType type, 
                            GrammarType[] grammar, commandName commandName, predicateType predicate1, 
                            predicateType predicate2, validityType validity, CommandType comType)
        {
            this.command = command;
            this.commandOwner = null;
            this.shortName = shortName;
            this.matchNumber = matchNumber;
            this.usableBy = type;
            this.grammar = grammar;
            this.maxTokens = maxTokens;
            this.commandName = commandName;
            this.predicate1 = predicate1;
            this.predicate2 = predicate2;
            this.validity = validity;
            this.prep1Value = new Preposition();
            this.prep2Value = new Preposition();
            this.predicate1Value = predicate2Value = null;
            this.comType = comType;
        }// Constructor

        public virtual string execute(ArrayList commandQueue, Mob mob, CommandExecuter commandExecutioner)
        {
            return "huh?";
        }
    }// class CommandClass

}// namespace _8th_Circle_Server
