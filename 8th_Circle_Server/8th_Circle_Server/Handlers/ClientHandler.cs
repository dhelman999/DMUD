using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace _8th_Circle_Server
{
    public class ClientHandler
    {
        // Member Variables
        public TcpListener mTcpListener;
        public Socket mSocketForClient;
        public NetworkStream mNetworkStream;
        public StreamReader mStreamReader;
        public StreamWriter mStreamWriter;
        public Thread mResponderThread;
        public CombatMob mPlayer;
        public string mCmdString;
        public World mWorld;
        public CommandHandler mCommandHandler;

        private object PlayerLock = new object();  

        public ClientHandler(TcpListener tcpListener, World world)
        {
            mPlayer = new CombatMob();
            mPlayer.mClientHandler = this;
            mPlayer.mResType = ResType.PLAYER;
            mTcpListener = tcpListener;
            mCommandHandler = world.mCommandHandler;
            mWorld = world;      
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
                            mPlayer.mName = mStreamReader.ReadLine();
                            mPlayer.mWorld = mWorld;
                            mPlayer.mDescription = mPlayer.mName + " is an 8th Circle Adventurer!";
                            Room curRoom = mWorld.getRoom(100 + 1, 100 + 1, 100 + 1, AreaID.AID_PROTOAREA);
                            curRoom.mCurrentArea.addRes(mPlayer);
                            mPlayer.mCurrentArea = curRoom.mCurrentArea;
                            mPlayer.mAreaLoc[0] = 1;
                            mPlayer.mAreaLoc[1] = 1;
                            mPlayer.mAreaLoc[2] = 1;
                            mPlayer.mCurrentRoom = curRoom;
                        }// lock

                        mCmdString = string.Empty;
                        mResponderThread.Interrupt();

                        while (mCmdString != "warrior" &&
                               mCmdString != "rogue"   &&
                               mCmdString != "cleric"  &&
                               mCmdString != "wizard")
                        {
                            mCmdString = string.Empty;
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
                                        mPlayer.mMobType = MobType.NONHEROIC;
                                        break;
                                }// switch
                            }
                        }// while

                        mPlayer.fillResistances();
                        mWorld.addRes(mPlayer);
                        mPlayer.mCurrentRoom.addRes(mPlayer);

                        foreach (CombatMob player in mPlayer.mWorld.getRes(ResType.PLAYER))
                            player.mClientHandler.safeWrite(mPlayer.mName + " has joined the World");

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

                safeWrite("Welcome to the 8th Circle!");

                if(mPlayer != null && mPlayer.mCurrentRoom != null)
                   safeWrite(mPlayer.mCurrentRoom.exitString() + mPlayer.playerString());
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

        public void safeWrite(string response)
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
                if (mWorld.getRes(ResType.PLAYER).Count > 0)
                {
                    foreach (CombatMob player in mWorld.getRes(ResType.PLAYER))
                        player.mClientHandler.safeWrite(mPlayer.mName + " has left the world");

                    mPlayer.mCurrentRoom.removeRes(mPlayer);
                    mPlayer.mCurrentArea.removeRes(mPlayer);
                    mWorld.removeRes(mPlayer);
                }// if
            }// if

            mStreamReader.Close();
            mStreamWriter.Close();
            mNetworkStream.Close();
            mResponderThread.Abort();
        }// playerLeft

    }// Class ClientHandler

}// Namespace _8th_Circle_Server
