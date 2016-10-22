using System.Collections.Generic;
using System.Threading;

namespace _8th_Circle_Server
{
    // The CommandHandler gets queued commands that the client sends the server via the ClientHandler command queue.  
    // Each mob has an action timer, which is essentially its cooldown between actions.  If the mob is able to
    // take another command, we will process the command by passing it to the CommandExecuter, otherwise it will
    // get queued up while we wait for the action timer.
    public class CommandHandler
    {
        private World mWorld;

        // The shared queue between the CommandHandler and the ClientHandler
        private Queue<CommandData> mCommandQueue;

        // Reference to the executer to process the commands
        private CommandExecuter mCommandExecuter;
        
        // Main worker thread to process commands
        private Thread mSpinWorkThread;

        // Primitive lock for the command queue
        private object mQueueLock;

        public CommandHandler(World world)
        {  
            mCommandQueue = new Queue<CommandData>();
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
            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }// try
                catch
                {
                    // Wake up and process commands
                    commandHandler.processCommands(commandExecuter);
                }// catch
            }// while (true)
        }// spinWork

        public void processCommands(CommandExecuter commandExecuter)
        {
            while (true)
            {
                CommandData comData;

                // Protect the queue
                lock (mQueueLock)
                {
                    if (mCommandQueue.Count > 0)
                        comData = mCommandQueue.Dequeue();
                    else
                        break;  
                }

                // Only process the command if we are off cooldown, otherwise queue up the command for next time
                if (comData.mob.GetActionTimer() <= 0)
                    commandExecuter.process(comData.command, comData.mob);
                else if (comData.mob.HasFlag(MobFlags.SEARCHING)) // TODO this really shouldn't be handled here
                    comData.mob.safeWrite("you can't do that while searching");
                else
                    comData.mob.SetQueuedCommand(comData.command);
            }// While (true)
        }// processCommands

        // Accessor for other handlers to add commands to the queue
        public void enQueueCommand(CommandData cd)
        {
            lock (mQueueLock)
            {
                mCommandQueue.Enqueue(cd);
            }// lock

            mSpinWorkThread.Interrupt();
        }// enQueueCommand

    }// Class CommandHandler

}// Namespace _8th_Circle_Server
