using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public System.IO.StreamReader mStreamReader;
        public System.IO.StreamWriter mStreamWriter;
        public Thread mResponderThread;
        public Mob mPlayer;
        object PlayerLock = new object();

        // Static Variables
        static string sCmdString;
        static CommandHandler sCommandHandler;
        static World sWorld;

        public ClientHandler(TcpListener tcpListener, CommandHandler commandHandler, World world)
        {
            mPlayer = new Mob();
            mTcpListener = tcpListener;
            sCommandHandler = commandHandler;
            sWorld = world;
        }

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
                        mStreamReader = new System.IO.StreamReader(mNetworkStream);
                        mStreamWriter = new System.IO.StreamWriter(mNetworkStream);
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
                        }
                        mResponderThread.Interrupt();

                        do
                        {
                            sCmdString = mStreamReader.ReadLine();
                            mResponderThread.Interrupt();
                        } while (!sCmdString.Equals("exit"));

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
                    }
                }// catch
            }// while
        }// ClientListener

        static void ClientResponder(ClientHandler ch)
        {
            sCmdString = "Please enter your player's name.";
            safeWrite(ch.mStreamWriter, sCmdString);
            try
            {
                Thread.Sleep(Timeout.Infinite);
            }
            catch
            {
                if (sCmdString.Equals("exit"))
                    return;

                safeWrite(ch.mStreamWriter, "Welcome to the 8th Circle!");
                safeWrite(ch.mStreamWriter, ch.mPlayer.mCurrentRoom.mDescription +
                    "\n" + ch.mPlayer.mCurrentRoom.exitString());
            }
            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }// try
                catch
                {
                    if (sCmdString.Equals("exit"))
                        break;

                    sCommandHandler.enQueueCommand(sCmdString);
                    safeWrite(ch.mStreamWriter, sCmdString);
                }// catch
            }// while
        }// ClientResponder

        private static void safeWrite(System.IO.StreamWriter sw, string response)
        {
            if (sw.BaseStream != null)
            {
                sw.WriteLine(response);
                sw.Flush();
            }// if
            else
            {
                sw.Dispose();
            }// else
        }// safeWrite
    }// Class ClientHandler
}// Namespace _8th_Circle_Server
