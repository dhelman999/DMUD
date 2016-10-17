using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace _8th_Circle_Server
{
    public class ClientHandler
    {
        private TcpListener mTcpListener;
        private Socket mSocketForClient;
        private NetworkStream mNetworkStream;
        private StreamReader mStreamReader;
        private StreamWriter mStreamWriter;
        private Thread mResponderThread;
        private CombatMob mPlayer;
        private String mCmdString;
        private World mWorld;
        private CommandHandler mCommandHandler;
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

                    if(mSocketForClient.Connected)
                    {
                        Console.WriteLine("Client:" + mSocketForClient.RemoteEndPoint + " now connected to server.");
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
                            }
                        }// while
                        mPlayer.SetAreaLoc(0, 1);
                        mPlayer.SetAreaLoc(1, 1);
                        mPlayer.SetAreaLoc(2, 1);

                        mPlayer.SetDesc(mPlayer.GetName() + " is an 8th Circle Adventurer!");
                        mPlayer.SetWorld(mWorld);
                        mWorld.addRes(mPlayer);
                        Room currentRoom = mWorld.getRoom(100 + 1, 100 + 1, 100 + 1, AreaID.AID_PROTOAREA);

                        String clientString = String.Empty;
                        mPlayer.changeRoom(currentRoom, ref clientString);

                        Utils.broadcast(mWorld, mPlayer, mPlayer.GetName() + " has joined the World", "You enter the 8th Circle...");

                        do
                        {     
                            mResponderThread.Interrupt();
                            mCmdString = mStreamReader.ReadLine();
                        }
                        while (!mCmdString.Equals("exit"));

                        playerLeft();
                    }// if mSocketForClient
                }// try
                catch
                {
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
            commandData cmdData = new commandData();
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
                if (mCmdString.Equals("exit"))
                    return;

                if(mPlayer != null && mPlayer.GetCurrentRoom() != null)
                   safeWrite(mPlayer.GetCurrentRoom().exitString() + mPlayer.playerString());
            }// catch
            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }// try
                catch
                {
                    if (mCmdString.Equals("exit"))
                        break;

                    cmdData.command = mCmdString;
                    cmdData.mob = mPlayer;
                    mCommandHandler.enQueueCommand(cmdData);
                    safeWrite(mCmdString + "\n");
                }// catch
            }// while
        }// ClientResponder

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

        private void playerLeft()
        {
            if (mPlayer != null)
            {
                Utils.broadcast(mWorld, mPlayer, mPlayer.GetName() + " has left the world");
                mWorld.totallyRemoveRes(mPlayer);
            }// if

            mStreamReader.Close();
            mStreamWriter.Close();
            mNetworkStream.Close();
            mResponderThread.Abort();
        }// playerLeft

    }// Class ClientHandler

}// Namespace _8th_Circle_Server
