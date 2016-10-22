using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace _8th_Circle_Server
{
    // The client handler, like all other handlers uses a seperate thread to listen to clients commands and process them.
    // It contains all the networking elements needed to send data to the client.
    public class ClientHandler
    {
        private World mWorld;
        private Thread mResponderThread;

        // Based on TCP, I didn't feel like dealing with lost packets with UDP
        // Other networking elements needed, like readers, writers, streams ect
        private TcpListener mTcpListener;
        private Socket mSocketForClient;
        private NetworkStream mNetworkStream;
        private StreamReader mStreamReader;
        private StreamWriter mStreamWriter;
        
        // Doesn't necessarily have to be a player, although it currently only is supported by players
        private CombatMob mPlayer;

        // Holds the last command being processed
        private String mCmdString;
        
        // Has to know how to handle commands
        private CommandHandler mCommandHandler;

        // Primitive thread safety, of course isn't really used.
        private object PlayerLock;  

        public ClientHandler(TcpListener tcpListener, World world)
        {
            mPlayer = new CombatMob();
            mPlayer.SetClientHandler(this);
            mPlayer.SetResType(ResType.PLAYER);
            mTcpListener = tcpListener;
            mCommandHandler = world.GetCommandHandler();
            mWorld = world;
            PlayerLock = new object();
        }// Constructor

        public void start()
        {
            while (true)
            {
                try
                {
                    mSocketForClient = mTcpListener.AcceptSocket();

                    // Player first connects to the MUD
                    if(mSocketForClient.Connected)
                    {
                        Console.WriteLine("Client:" + mSocketForClient.RemoteEndPoint + " now connected to server.");

                        // Create networking elements
                        mNetworkStream = new NetworkStream(mSocketForClient);
                        mStreamReader = new StreamReader(mNetworkStream);
                        mStreamWriter = new StreamWriter(mNetworkStream);
                        mResponderThread = new Thread(ClientResponder);
                        mResponderThread.Start();

                        lock (PlayerLock)
                        {
                            mPlayer.SetName(mStreamReader.ReadLine());
                        }// lock

                        mCmdString = String.Empty;
                        mResponderThread.Interrupt();

                        // Let them choose a class
                        while (mCmdString != "warrior" &&
                               mCmdString != "rogue"   &&
                               mCmdString != "cleric"  &&
                               mCmdString != "wizard")
                        {
                            mCmdString = String.Empty;
                            mCmdString = mStreamReader.ReadLine();

                            if (mCmdString != "warrior" &&
                                mCmdString != "rogue"   &&
                                mCmdString != "cleric"  &&
                                mCmdString != "wizard")
                            {
                                safeWrite("please enter a valid class\n");
                            }
                            else
                            {
                                switch (mCmdString)
                                {
                                    case "warrior":
                                        mPlayer = new Warrior(mPlayer);
                                        break;

                                    case "rogue":
                                        mPlayer = new Rogue(mPlayer);
                                        break;

                                    case "cleric":
                                        mPlayer = new Cleric(mPlayer);
                                        break;

                                    case "wizard":
                                        mPlayer = new Wizard(mPlayer);
                                        break;

                                    default:
                                        mPlayer.SetMobType(MobType.NONHEROIC);
                                        break;
                                }// switch
                            }// else
                        }// while

                        // Set their starting properties
                        mPlayer.SetAreaLoc(0, 1);
                        mPlayer.SetAreaLoc(1, 1);
                        mPlayer.SetAreaLoc(2, 1);

                        mPlayer.SetDesc(mPlayer.GetName() + " is an 8th Circle Adventurer!");
                        mPlayer.SetWorld(mWorld);
                        mWorld.addRes(mPlayer);
                        Room currentRoom = mWorld.getRoom(1,1,1, AreaID.AID_PROTOAREA);

                        String clientString = String.Empty;
                        mPlayer.changeRoom(currentRoom, ref clientString);

                        Utils.Broadcast(mWorld, mPlayer, mPlayer.GetName() + " has joined the World", "You enter the 8th Circle...");

                        // Read commands the players send, when they do, wake up the responder thread to process the command and
                        // actually send back the results to the client.
                        do
                        {     
                            mResponderThread.Interrupt();
                            mCmdString = mStreamReader.ReadLine();
                        }
                        while (!mCmdString.Equals("exit"));

                        // They typed 'exit' leave the game
                        playerLeft();
                    }// if mSocketForClient
                }// try
                catch
                {
                    // Exceptions happen if something bad happens, or they just close the client, need to leave the game
                    Console.WriteLine("Exception caught while listening to " + mSocketForClient.RemoteEndPoint);

                    if (mStreamReader != null  &&
                        mStreamWriter != null  &&
                        mNetworkStream != null &&
                        mResponderThread != null)
                    {
                        playerLeft();
                    }// if
                }// catch
            }// while
        }// ClientListener

        void ClientResponder()
        {
            CommandData cmdData = new CommandData();
            cmdData.mob = mPlayer;
            mCmdString = "Please enter your player's name.";
            safeWrite(mCmdString);

            try
            {
                Thread.Sleep(Timeout.Infinite);
            }// try
            catch
            {
                mCmdString = "\nPlease enter your character class, choose one from the following:\n\n" + "warrior\nrogue\ncleric\nwizard\n";
                safeWrite(mCmdString);
            }// catch

            try
            {
                Thread.Sleep(Timeout.Infinite);
            }// try
            catch
            {
                // Show them the initial room they start in
                if (mCmdString.Equals("exit"))
                    return;

                Room currentRoom = mPlayer.GetCurrentRoom();

                if(mPlayer != null && mPlayer.GetCurrentRoom() != null)
                   safeWrite(currentRoom.exitString() + mPlayer.playerString());
            }// catch

            // The main processing of their sent command
            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }// try
                catch
                {
                    // Wake up and process the clients command
                    if (mCmdString.Equals("exit"))
                        break;

                    // Queue up the command for processing and send them a copy of what they typed back
                    cmdData.command = mCmdString;
                    cmdData.mob = mPlayer;
                    mCommandHandler.enQueueCommand(cmdData);
                    safeWrite(mCmdString + "\n");
                }// catch
            }// while
        }// ClientResponder

        // Safely write back to the client, make sure to dispose of resource responsibly if something goes wrong.
        public void safeWrite(String response)
        {
            try
            {
                if (mStreamWriter.BaseStream != null)
                {
                    mStreamWriter.WriteLine(response);
                    mStreamWriter.Flush();
                }// if
                else
                {
                    mStreamWriter.Dispose();
                }// else
            }// try
            catch
            {
                mStreamReader.Close();
            }// catch
        }// safeWrite

        // Remove the player from the world and close networking elements
        private void playerLeft()
        {
            if (mPlayer != null)
            {
                mWorld.totallyRemoveRes(mPlayer);
                Utils.Broadcast(mWorld, mPlayer, mPlayer.GetName() + " has left the world");
            }// if

            mStreamReader.Close();
            mStreamWriter.Close();
            mNetworkStream.Close();
            mResponderThread.Abort();
        }// playerLeft

    }// Class ClientHandler

}// Namespace _8th_Circle_Server
