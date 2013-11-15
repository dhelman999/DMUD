using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

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
        public ArrayList mCommandList;
        
        public ArrayList mNounList;
        public ArrayList mVerbList;
        public ArrayList mPrepList;
        private Thread mSpinWorkThread;
        static string sCommandString = string.Empty;
        static Queue sCommandQueue;
        static object QueueLock = new object();

        public CommandHandler()
        {
            mCommandList = new ArrayList();
            sCommandQueue = new Queue();
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
        }// Constructor

        public void start()
        {
            mSpinWorkThread = new Thread(new ThreadStart(spinWork));
            mSpinWorkThread.Start();
        }// start

        public static void spinWork()
        {  
            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }// try
                catch
                {
                    while (sCommandQueue.Count > 0)
                    {
                        Console.WriteLine(sCommandQueue.Dequeue());
                    }// while
                }// catch
            }// while
        }// spinWork

        public void enQueueCommand(string commandString)
        {
            lock (QueueLock)
            {
                sCommandQueue.Enqueue(commandString);
            }// lock
            mSpinWorkThread.Interrupt();
        }// enQueueCommand
    }// Class CommandHandler
}// Namespace _8th_Circle_Server
