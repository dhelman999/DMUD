using System.Collections.Generic;
using System.Threading;

namespace _8th_Circle_Server
{
    public class CommandHandler
    {
        // Debug
        internal const bool DEBUG = true;     

        // Member Variables
        public Queue<commandData> mCommandQueue;
        public CommandExecuter mCommandExecuter;
        public World mWorld;

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

                        if (comData.mob.mActionTimer <= 0)
                            commandExecuter.process(comData.command, comData.mob);
                        else if (comData.mob.mFlagList.Contains(MobFlags.FLAG_SEARCHING))
                        {
                            if (comData.mob.mResType == ResType.PLAYER)
                                ((CombatMob)comData.mob).mClientHandler.safeWrite("you can't do that while searching");
                        }
                        else
                            comData.mob.mQueuedCommand = comData.command;
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
