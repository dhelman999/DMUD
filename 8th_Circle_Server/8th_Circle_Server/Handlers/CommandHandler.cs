﻿using System.Collections.Generic;
using System.Threading;

namespace _8th_Circle_Server
{
    public class CommandHandler
    {   
        private Queue<commandData> mCommandQueue;
        private CommandExecuter mCommandExecuter;
        private World mWorld;
        private object mQueueLock;
        private Thread mSpinWorkThread; 
  
        public CommandHandler(World world)
        {  
            mCommandQueue = new Queue<commandData>();
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
                        comData = (commandHandler.mCommandQueue.Dequeue());

                        if (comData.mob.GetActionTimer() <= 0)
                            commandExecuter.process(comData.command, comData.mob);
                        else if (comData.mob.GetFlagList().Contains(MobFlags.FLAG_SEARCHING))
                             comData.mob.safeWrite("you can't do that while searching");
                        else
                            comData.mob.SetQueuedCommand(comData.command);
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
