using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace _8th_Circle_Server
{
    enum commandType
    {
        VERB=1,
        NOUN,
        PREP
    };

    struct Command
    {
        public string command;
        public int matchNumber;
        public commandType type;

        public Command(string command, int matchNumber, commandType type)
        {
            this.command = command;
            this.matchNumber = matchNumber;
            this.type = type;
        }
    }

    class CommandHandler
    {
        ArrayList mCommandList;
        ArrayList mNounList;
        ArrayList mVerbList;
        ArrayList mPrepList;

        public CommandHandler()
        {
            mCommandList = new ArrayList();
            mVerbList = new ArrayList();
            mNounList = new ArrayList();
            mPrepList = new ArrayList();

            Command pt;

            pt = new Command("move", 1, commandType.VERB);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("look", 1, commandType.VERB);
            mCommandList.Add(pt);
            mVerbList.Add(pt);

            pt = new Command("exit", 1, commandType.VERB);
            mCommandList.Add(pt);
            mVerbList.Add(pt);
        }
    }
}
