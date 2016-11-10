using System;
using System.Net.Sockets;

public static class GlobalData
{
    // Server IP Address that the client will attempt to connect to
    public static String sServerIPAddr;

    // Socket used to communicate with the server
    public static TcpClient sSocketForServer;
}// class GlobalData
