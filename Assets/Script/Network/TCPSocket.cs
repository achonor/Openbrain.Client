using System;
using System.Net;
using System.Text;
using System.Net.Sockets;


using UnityEngine;
using System.Collections;

public class TCPSocket
{
    private static Socket tcpSocket = null;

    public static void Connect(string ip, string port)
    {
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        tcpSocket.Connect(IPAddress.Parse(ip), Convert.ToInt32(port));
    }

    public static int Receive(byte[] buffer, int size)
    {
        if (null == tcpSocket)
        {
            Debug.LogError("Network not connected!!!");
            return 0;
        }
        return tcpSocket.Receive(buffer, size, SocketFlags.None);
    }


    public static void Send(byte[] buffer){
        if (null == tcpSocket)
        {
            Debug.LogError("Network not connected!!!");
            return;
        }
        tcpSocket.Send(buffer);
    }
}