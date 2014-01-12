using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace _8th_Circle_Server
{
    public class ClientHandler
    {
        // Debug
        internal const bool DEBUG = true;

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
                        Console.WriteLine("Client:" + mSocketForClient.RemoteEndPoint +
                            " now connected to server.");
                        mNetworkStream = new NetworkStream(mSocketForClient);
                        mStreamReader = new StreamReader(mNetworkStream);
                        mStreamWriter = new StreamWriter(mNetworkStream);
                        mResponderThread = new Thread(() => ClientResponder(this));
                        mResponderThread.Start();

                        lock (PlayerLock)
                        {
                            mPlayer.mName = mStreamReader.ReadLine();
                            mPlayer.mWorld = mWorld;
                            mPlayer.mDescription = mPlayer.mName + " is an 8th Circle Adventurer!";
                            Room curRoom = mWorld.getRoom(100 + 1,
                                100 + 1, 100 + 1, AreaID.AID_PROTOAREA);
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
                                safeWrite("please enter a valid class\n");
                            else
                            {
                                switch (mCmdString)
                                {
                                    // TODO
                                    // Make this more generic
                                    case "warrior":
                                        mPlayer = new Warrior(mPlayer);
                                        mPlayer.mMobType = MobType.WARRIOR;
                                        mPlayer.mStats.mBasePhysRes = 250;
                                        mPlayer.mStats.mBaseEvade -= 15;
                                        mPlayer.mStats.mBaseDamBonus = 1;
                                        mPlayer.mStats.mBaseHit -= 5;
                                        mPlayer.mStats.mBaseMaxHp += 10;
                                        mPlayer.mStats.mCurrentHp += 10;
                                        break;

                                    case "rogue":
                                        mPlayer = new Rogue(mPlayer);
                                        mPlayer.mMobType = MobType.ROGUE;
                                        mPlayer.mStats.mBaseHit += 5;
                                        mPlayer.mStats.mBaseDamBonus = 1;
                                        mPlayer.mStats.mBaseEvade += 5;
                                        mPlayer.mStats.mBaseMaxHp -= 5;
                                        mPlayer.mStats.mCurrentHp -= 5;
                                        break;

                                    case "cleric":
                                        mPlayer = new Cleric(mPlayer);
                                        mPlayer.mMobType = MobType.CLERIC;
                                        mPlayer.mStats.mBaseMaxMana = 30;
                                        mPlayer.mStats.mCurrentMana = 30;
                                        mPlayer.mStats.mBaseEvade -= 5;
                                        break;

                                    case "wizard":
                                        mPlayer = new Wizard(mPlayer);
                                        mPlayer.mMobType = MobType.WIZARD;
                                        mPlayer.mStats.mBaseHit -= 10;
                                        mPlayer.mStats.mBaseMaxHp -= 10;
                                        mPlayer.mStats.mCurrentHp -= 10;
                                        mPlayer.mStats.mBaseMaxMana = 40;
                                        mPlayer.mStats.mCurrentMana = 40;
                                        break;

                                    default:
                                        mPlayer.mMobType = MobType.NONHEROIC;
                                        break;
                                }// switch
                            }
                        }// while

                        mWorld.addRes(mPlayer);
                        mPlayer.mCurrentRoom.addRes(mPlayer);

                        foreach (CombatMob player in mPlayer.mWorld.getRes(ResType.PLAYER))
                        {
                            player.mClientHandler.safeWrite(mPlayer.mName + " has joined the World");
                        }// foreach

                        do
                        {     
                            mResponderThread.Interrupt();
                            mCmdString = mStreamReader.ReadLine();
                        } while (!mCmdString.Equals("exit"));

                        if (DEBUG)
                        {           
                            if(mPlayer != null &&
                               mPlayer.mName != string.Empty)
                                Console.WriteLine("CombatMob: " + mPlayer.mName + " Client: " + 
                                    mSocketForClient.RemoteEndPoint + " is now exitting.");
                            else
                                Console.WriteLine("Client: " + mSocketForClient.RemoteEndPoint +
                                " is now exitting.");
                        }// if

                        playerLeft();
                    }// if mSocketForClient
                }// try
                catch
                {
                    Console.WriteLine("Exception caught while listening to " + mSocketForClient.RemoteEndPoint);
                    if (mStreamReader != null &&
                        mStreamWriter != null &&
                        mNetworkStream != null &&
                        mResponderThread != null)
                    {
                        playerLeft();
                    }// if
                }// catch
            }// while
        }// ClientListener

        static void ClientResponder(ClientHandler clientHandler)
        {
            commandData cmdData = new commandData();
            cmdData.mob = clientHandler.mPlayer;
            clientHandler.mCmdString = "Please enter your player's name.";
            clientHandler.safeWrite(clientHandler.mCmdString);

            try
            {
                Thread.Sleep(Timeout.Infinite);
            }// try
            catch
            {
                clientHandler.mCmdString = "\nPlease enter your character class, choose one from the following:\n\n" +
                    "warrior\nrogue\ncleric\nwizard\n";
                clientHandler.safeWrite(clientHandler.mCmdString);
            }// catch

            try
            {
                Thread.Sleep(Timeout.Infinite);
            }// try
            catch
            {
                if (clientHandler.mCmdString.Equals("exit"))
                    return;

                clientHandler.safeWrite("Welcome to the 8th Circle!");
                if(clientHandler.mPlayer != null &&
                   clientHandler.mPlayer.mCurrentRoom != null)
                   clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.exitString() +
                       ((CombatMob)clientHandler.mPlayer).playerString());
            }// catch
            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }// try
                catch
                {
                    if (clientHandler.mCmdString.Equals("exit"))
                        break;

                    cmdData.command = clientHandler.mCmdString;
                    cmdData.mob = clientHandler.mPlayer;
                    clientHandler.mCommandHandler.enQueueCommand(cmdData);
                    clientHandler.safeWrite(clientHandler.mCmdString + "\n");
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
            if (mWorld.getRes(ResType.PLAYER).Count > 0)
            {
                foreach (CombatMob player in mWorld.getRes(ResType.PLAYER))
                {
                    player.mClientHandler.safeWrite(mPlayer.mName + " has left the world");
                }// foreach

                mPlayer.mCurrentRoom.removeRes(mPlayer);
                mPlayer.mCurrentArea.removeRes(mPlayer);
                mWorld.removeRes(mPlayer);
            }// if

            mStreamReader.Close();
            mStreamWriter.Close();
            mNetworkStream.Close();
            mResponderThread.Abort();
        }// playerLeft

    }// Class ClientHandler

}// Namespace _8th_Circle_Server
