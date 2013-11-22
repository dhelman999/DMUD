using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace _8th_Circle_Server
{
    class ClientHandler
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
        public Mob mPlayer;
        public string mCmdString;
        public World mWorld;
        public CommandHandler mCommandHandler;
        
        private object PlayerLock = new object();  

        public ClientHandler(TcpListener tcpListener, World world)
        {
            mPlayer = new Player(this);
            mTcpListener = tcpListener;
            mCommandHandler = new CommandHandler(world);
            mWorld = world;      
        }// Constructor

        public void start()
        {
            mCommandHandler.start();

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
                            Room curRoom = mWorld.getRoom(1, 1, 1);
                            curRoom.mCurrentArea.mPlayerList.Add(mPlayer);
                            mPlayer.mCurrentArea = curRoom.mCurrentArea;
                            mPlayer.mWorldLoc[0] = 1;
                            mPlayer.mWorldLoc[1] = 1;
                            mPlayer.mWorldLoc[2] = 1;
                            mPlayer.mCurrentRoom = curRoom;
                            mWorld.mPlayerList.Add(mPlayer);
                            curRoom.mPlayerList.Add(mPlayer);   
                        }// lock

                        foreach (Player player in mPlayer.mWorld.mPlayerList)
                        {
                            player.mClientHandler.safeWrite(mPlayer.mName + " has joined the World");
                        }// foreach

                        mResponderThread.Interrupt();

                        do
                        {
                            mCmdString = mStreamReader.ReadLine();
                            mResponderThread.Interrupt();
                        } while (!mCmdString.Equals("exit"));

                        if (DEBUG)
                            Console.WriteLine("Client: " + mSocketForClient.RemoteEndPoint +
                                " is now exitting.");

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
            cmdData.clientHandler = clientHandler;
            clientHandler.mCmdString = "Please enter your player's name.";
            clientHandler.safeWrite(clientHandler.mCmdString);

            try
            {
                Thread.Sleep(Timeout.Infinite);
            }// try
            catch
            {
                if (clientHandler.mCmdString.Equals("exit"))
                    return;

                clientHandler.safeWrite("Welcome to the 8th Circle!");
                clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.mDescription +
                             "\n" + clientHandler.mPlayer.mCurrentRoom.exitString());
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
                    clientHandler.mCommandHandler.enQueueCommand(cmdData);
                    clientHandler.safeWrite(clientHandler.mCmdString);
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
            }
        }// safeWrite

        private void playerLeft()
        {
            foreach (Player player in mPlayer.mWorld.mPlayerList)
            {
                player.mClientHandler.safeWrite(mPlayer.mName + " has left the world");
            }// foreach

            mPlayer.mCurrentRoom.mPlayerList.Remove(mPlayer);
            mWorld.mPlayerList.Remove(mPlayer);
            mStreamReader.Close();
            mStreamWriter.Close();
            mNetworkStream.Close();
            mResponderThread.Abort();
        }// playerLeft

    }// Class ClientHandler

}// Namespace _8th_Circle_Server
