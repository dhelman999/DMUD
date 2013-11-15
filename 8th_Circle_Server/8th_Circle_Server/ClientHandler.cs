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
        
        // Static Variables
        static string cmdString;

        public ClientHandler(TcpListener tcpListener)
        {
            this.mTcpListener = tcpListener;
        }

        public void start()
        {
            while (true)
            {
                try
                {
                    mSocketForClient = mTcpListener.AcceptSocket();
                    Thread.Sleep(10);
                    if(mSocketForClient.Connected)
                    {
                        Console.WriteLine("Client:" + mSocketForClient.RemoteEndPoint +
                            " now connected to server.");
                        mNetworkStream = new NetworkStream(mSocketForClient);
                        mStreamReader = new System.IO.StreamReader(mNetworkStream);
                        mStreamWriter = new System.IO.StreamWriter(mNetworkStream);
                        mResponderThread = new Thread(() => ClientResponder(mNetworkStream,
                                                                            mStreamWriter,
                                                                            mStreamReader));
                        mResponderThread.Start();

                        do
                        {
                            cmdString = mStreamReader.ReadLine();
                            mResponderThread.Interrupt();
                        } while (!cmdString.Equals("exit"));

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
                    Console.WriteLine("Exception caught while listening");
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

        static void ClientResponder(NetworkStream mNetworkStream,
                                    System.IO.StreamWriter mStreamWriter,
                                    System.IO.StreamReader mStreamReader)
        {
            string clientString = string.Empty;

            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }// try
                catch
                {
                    clientString = string.Empty;
                    clientString = cmdString;

                    if (clientString == "exit")
                        break;
                    else if (clientString == "broadcast")
                    {
                        //foreach (System.IO.StreamWriter sw in mStreamWriterList)
                        //{
                            //sw.WriteLine(clientString);
                            //sw.Flush();
                        //}
                        break;
                    }

                    if (mStreamWriter.BaseStream != null)
                    {
                        mStreamWriter.WriteLine(clientString);
                        mStreamWriter.Flush();
                    }
                    else
                    {
                        mStreamWriter.Dispose();
                    }
                }// catch
            }// while
        }// ClientResponder
    }// Class ClientHandler
}// Namespace _8th_Circle_Server
