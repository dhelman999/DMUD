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

        // Static Variables
        
        static CommandHandler sCommandHandler;
        static World sWorld;

        // Member Variables
        public TcpListener mTcpListener;
        public Socket mSocketForClient;
        public NetworkStream mNetworkStream;
        public StreamReader mStreamReader;
        public StreamWriter mStreamWriter;
        public Thread mResponderThread;
        public Mob mPlayer;
        public string mCmdString;
        object PlayerLock = new object();  

        public ClientHandler(TcpListener tcpListener, CommandHandler commandHandler, World world)
        {
            mPlayer = new Mob();
            mTcpListener = tcpListener;
            sCommandHandler = commandHandler;
            sWorld = world;
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

                        mPlayer.mName = mStreamReader.ReadLine();
                        mPlayer.mWorld = sWorld;
                        Room curRoom = sWorld.getRoom(1, 1, 1);
                        lock (PlayerLock)
                        {
                            mPlayer.mWorldLoc[0] = 1;
                            mPlayer.mWorldLoc[1] = 1;
                            mPlayer.mWorldLoc[2] = 1;
                            mPlayer.mCurrentRoom = curRoom;
                            sWorld.mPlayerList.Add(mPlayer);
                            curRoom.mPlayerList.Add(mPlayer); 
                        }// lock
                        mResponderThread.Interrupt();

                        do
                        {
                            mCmdString = mStreamReader.ReadLine();
                            mResponderThread.Interrupt();
                        } while (!mCmdString.Equals("exit"));

                        if (DEBUG)
                            Console.WriteLine("Client: " + mSocketForClient.RemoteEndPoint +
                                " is now exitting.");

                        mStreamReader.Close();
                        mStreamWriter.Close();
                        mNetworkStream.Close();
                        mResponderThread.Abort();
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
                        mStreamReader.Close();
                        mStreamWriter.Close();
                        mNetworkStream.Close();
                        mResponderThread.Abort();
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
                    sCommandHandler.enQueueCommand(cmdData);
                    clientHandler.safeWrite(clientHandler.mCmdString);
                }// catch
            }// while
        }// ClientResponder

        public void safeWrite(string response)
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
        }// safeWrite

    }// Class ClientHandler

}// Namespace _8th_Circle_Server
