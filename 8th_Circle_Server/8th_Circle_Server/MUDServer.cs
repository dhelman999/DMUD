using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace _8th_Circle_Server
{
    class MUDServer
    {
        // Debug
        internal const bool DEBUG = true;

        // Constants
        internal const int MUD_SERVER_PORT = 8888;

        // Static Variables
        static TcpListener sTcpListener;
        static ArrayList sListenerThreadList;
        static ArrayList clientHandlerList;
        static CommandHandler sCommandHandler;

        static void Main(string[] args)
        {
            sListenerThreadList  = new ArrayList();
            clientHandlerList = new ArrayList();     
            sCommandHandler = new CommandHandler();
            sCommandHandler.start();

            try
            {
                Console.WriteLine("Welcome to the 8th Circle, the Server Application\n\n");
                Console.WriteLine("Please type the IP address where you want to host the MUD: ");
                string ipAddr = Console.ReadLine();
                IPAddress MUDAddress = IPAddress.Parse(ipAddr);
                sTcpListener = new TcpListener(MUDAddress, MUD_SERVER_PORT);
                sTcpListener.Start();
                Console.WriteLine("Enter the maximum number of players to support: ");
                int maxPlayers = int.Parse(Console.ReadLine());

                for (int i = 0; i < maxPlayers; i++)
                {
                    Thread listenerThread = new Thread(() => ClientListener(sCommandHandler));
                    sListenerThreadList.Add(listenerThread);
                    listenerThread.Start();      
                }// for

                

                Console.WriteLine("The 8th Circle has been started on " + ipAddr + 
                    "::" + MUD_SERVER_PORT);
            }// try
            catch
            {
                if (DEBUG)
                    Console.WriteLine("Error starting the Server");
                sTcpListener.Stop();
            }// catch
        }// Main

        static void ClientListener(CommandHandler commandHandler)
        {
            ClientHandler ch = new ClientHandler(sTcpListener, commandHandler);
            clientHandlerList.Add(ch);
            ch.start();
        }// ClientListener       
    }// Class MUDServer
}// Namespace _8th_Circle_Server
