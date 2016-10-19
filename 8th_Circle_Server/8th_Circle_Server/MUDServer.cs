using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace _8th_Circle_Server
{
    class MUDServer
    {
        internal const int MUD_SERVER_PORT = 8888;

        static TcpListener sTcpListener;
        static List<Thread> sListenerThreadList;
        static List<ClientHandler> sClientHandlerList;
        static World sWorld;

        static void Main(String[] args)
        {
            sWorld = new World();
            sListenerThreadList  = new List<Thread>();
            sClientHandlerList = new List<ClientHandler>();     

            try
            {
                Console.WriteLine("Welcome to the 8th Circle, the Server Application\n\n");
                //Console.WriteLine("Please type the IP address where you want to host the MUD: ");
                //String ipAddr = Console.ReadLine();
                //IPAddress MUDAddress = IPAddress.Parse(ipAddr);
                //sTcpListener = new TcpListener(MUDAddress, MUD_SERVER_PORT);
                sTcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), MUD_SERVER_PORT);
                sTcpListener.Start();
                //Console.WriteLine("Enter the maximum number of players to support: ");
                //int maxPlayers = int.Parse(Console.ReadLine());
                int maxPlayers = 8;

                for (int i = 0; i < maxPlayers; ++i)
                {
                    Thread listenerThread = new Thread(() => ClientListener(sWorld));
                    sListenerThreadList.Add(listenerThread);
                    listenerThread.Start();      
                }// for

                //Console.WriteLine("The 8th Circle has been started on " + ipAddr + "::" + MUD_SERVER_PORT);
                Console.WriteLine("The 8th Circle has been started on 127.0.0.1" + "::" + MUD_SERVER_PORT);
            }// try
            catch
            {
                Console.WriteLine("Error starting the Server");
                sTcpListener.Stop();
            }// catch
        }// Main

        static void ClientListener(World world)
        {
            ClientHandler clientHandler = new ClientHandler(sTcpListener, world);
            sClientHandlerList.Add(clientHandler);
            clientHandler.start();
        }// ClientListener  
     
    }// Class MUDServer

}// Namespace _8th_Circle_Server
