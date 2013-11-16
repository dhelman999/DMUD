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
    struct commandData
    {
        public string command;
        public ClientHandler clientHandler;

        public commandData(string command, ClientHandler ch)
        {
            this.command = command;
            this.clientHandler = ch;
        }
    }// commandData

    class CommandHandler
    {
        // Debug
        internal const bool DEBUG = true;     

        // Member Variables
        public Queue mCommandQueue;
        public CommandExecuter mCommandExecuter;
        public World mWorld;

        private object mQueueLock;
        private Thread mSpinWorkThread; 
  
        public CommandHandler(World world)
        {  
            mCommandQueue = new Queue();
            mCommandExecuter = new CommandExecuter();
            mWorld = world;
            mQueueLock = new object();
        }// Constructor

        public void start()
        {
            mSpinWorkThread = new Thread(() => spinWork(this, mCommandExecuter));
            mSpinWorkThread.Start();
        }// start

        public static void spinWork(CommandHandler commandHandler, CommandExecuter commandExecuter)
        {
            commandData comData;

            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }// try
                catch
                {
                    while (commandHandler.mCommandQueue.Count > 0)
                    {
                        comData = ((commandData)commandHandler.mCommandQueue.Dequeue());
                        if (commandExecuter.process(comData.command, comData.clientHandler) == errorCode.E_OK)
                        {
                        }
                        else
                        {
                            comData.clientHandler.safeWrite(comData.command + "is invalid");
                        }
                    }// while
                }// catch
            }// while
        }// spinWork

        public void enQueueCommand(commandData cd)
        {
            lock (mQueueLock)
            {
                mCommandQueue.Enqueue(cd);
            }// lock
            mSpinWorkThread.Interrupt();
        }// enQueueCommand

    }// Class CommandHandler

}// Namespace _8th_Circle_Server
