using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace _8th_Circle_Client
{
    class MUDClient
    {
        // Debug
        internal const bool DEBUG = false;

        // Constants
        internal const int MUD_SERVER_PORT = 8888;

        // Static Variables
        static TcpClient socketForServer;
        static NetworkStream networkStream;
        static System.IO.StreamReader streamReader;
        static System.IO.StreamWriter streamWriter;

        static void Main(string[] args)
        {
            string ipAddr = string.Empty;
            try
            {
                Console.Title = "8th Circle Client";
                Console.WriteLine("8th Circle MUD Client");
                Console.WriteLine("Enter IP address of the MUD Server");
                ipAddr = Console.ReadLine();
                socketForServer = new TcpClient(ipAddr, MUD_SERVER_PORT);
            }// try
            catch
            {
                Console.WriteLine("Failed to connect to " + ipAddr + "::" + 8888);
                Console.WriteLine("Exitting");
                return;
            }// catch

            networkStream = socketForServer.GetStream();
            Console.WriteLine("Connected to " + ipAddr + "::" + 8888 + "\n");

            streamReader = new System.IO.StreamReader(networkStream);
            streamWriter = new System.IO.StreamWriter(networkStream);
            Thread readerThread = new Thread(new ThreadStart(ServerListener));
            Thread writerThread = new Thread(new ThreadStart(ServerWriter));
            readerThread.Start();
            writerThread.Start();
        }// Main

        static void ServerListener()
        {
            string outputString = string.Empty;

            try
            {
                while (true)
                {
                    Thread.Sleep(10);
                    outputString = streamReader.ReadLine();
                    if (outputString.Contains("say") || outputString.Contains("says"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.WriteLine(outputString);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }// while
            }// try
            catch
            {
                if(DEBUG)
                    Console.WriteLine("Exception reading from Server");
                streamReader.Close();
                streamWriter.Close();
                networkStream.Close();
                socketForServer.Close();
            }// catch
        }// ServerReader

        static void ServerWriter()
        {
            string str = string.Empty;
            try
            {
                do
                {
                    str = Console.ReadLine();
                    streamWriter.WriteLine(str);
                    streamWriter.Flush();
                }while(!str.Equals("exit"));

                goto Cleanup;
            }// try
            catch
            {
                if(DEBUG)
                    Console.WriteLine("Exception reading from Server");
                goto Cleanup;
            }// catch

            Cleanup:
                streamReader.Close();
                streamWriter.Close();
                networkStream.Close();
                socketForServer.Close();
        }// ServerWriter
    }// Class MUDClient
}// namespace _8th_Circle_Client