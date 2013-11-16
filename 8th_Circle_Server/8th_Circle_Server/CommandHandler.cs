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
        public ClientHandler ch;

        public commandData(string command, ClientHandler ch)
        {
            this.command = command;
            this.ch = ch;
        }
    }// commandData

    class CommandHandler
    {
        // Debug
        internal const bool DEBUG = true;

        // Static Variables
        static object QueueLock = new object();
        public static World sWorld;

        // Member Variables
        public Queue mCommandQueue;

        private Thread mSpinWorkThread; 
  
        // Static Variables
        public static CommandExecuter sComExe;

        public CommandHandler(World world)
        {  
            mCommandQueue = new Queue();
            QueueLock = new object();
            sComExe = new CommandExecuter();
            sWorld = world;
        }// Constructor

        public void start()
        {
            //mSpinWorkThread = new Thread(new ThreadStart(spinWork));
            mSpinWorkThread = new Thread(() => spinWork(this));
            mSpinWorkThread.Start();
        }// start

        public static void spinWork(CommandHandler ch)
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
                    while (ch.mCommandQueue.Count > 0)
                    {
                        comData = ((commandData)ch.mCommandQueue.Dequeue());
                        if (sComExe.execute(comData.command, comData.ch) == errorCode.E_OK)
                        {
                        }
                        else
                        {
                            comData.ch.safeWrite(comData.command + "is invalid");
                        }
                    }// while
                }// catch
            }// while
        }// spinWork

        public void enQueueCommand(commandData cd)
        {
            lock (QueueLock)
            {
                mCommandQueue.Enqueue(cd);
            }// lock
            mSpinWorkThread.Interrupt();
        }// enQueueCommand

        private errorCode processRequest(string command, ClientHandler ch)
        {
            errorCode ret = errorCode.E_OK;
       
            return ret;
        }
    }// Class CommandHandler

}// Namespace _8th_Circle_Server
