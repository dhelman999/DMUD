﻿using System;
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
        public Mob mPlayer;
        public string mCmdString;
        public World mWorld;
        public CommandHandler mCommandHandler;
        public EventHandler mEventHandler;

        private object PlayerLock = new object();  

        public ClientHandler(TcpListener tcpListener, World world)
        {
            mPlayer = new Player(this);
            mTcpListener = tcpListener;
            mCommandHandler = world.mCommandHandler;
            mEventHandler = world.mEventHandler;
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
                            mWorld.addRes(mPlayer);
                            curRoom.addRes(mPlayer);
                        }// lock

                        foreach (Player player in mPlayer.mWorld.getRes(ResType.PLAYER))
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
                        {
                            
                            if(mPlayer != null &&
                               mPlayer.mName != string.Empty)
                                Console.WriteLine("Player: " + mPlayer.mName + " Client: " + 
                                    mSocketForClient.RemoteEndPoint + " is now exitting.");
                            else
                                Console.WriteLine("Client: " + mSocketForClient.RemoteEndPoint +
                                " is now exitting.");
                        }


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
                if (clientHandler.mCmdString.Equals("exit"))
                    return;

                clientHandler.safeWrite("Welcome to the 8th Circle!");
                if(clientHandler.mPlayer != null &&
                   clientHandler.mPlayer.mCurrentRoom != null)
                   clientHandler.safeWrite(clientHandler.mPlayer.mCurrentRoom.exitString());
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
            }// catch
        }// safeWrite

        private void playerLeft()
        {
            if (mWorld.getRes(ResType.PLAYER).Count > 0)
            {
                foreach (Player player in mWorld.getRes(ResType.PLAYER))
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
