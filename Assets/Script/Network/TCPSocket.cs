using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;


using UnityEngine;
using System.Collections;

public class TCPSocket
{
    private static Socket tcpSocket = null;

    //是否连接服务器
    private static bool IsConnected = false;
    //连接超时时间
    private static int ConnectTimeOut = 5;
    public static void Connect(string ip, string port, System.Action<bool> callback = null)
    {
        //创建套接字
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //非阻塞
        tcpSocket.Blocking = false;
        //连接状态
        int linkState = -1;
        //异步连接
        LoadLayerManager.Instance.AddLoad();
        tcpSocket.BeginConnect(IPAddress.Parse(ip), Convert.ToInt32(port), (IAsyncResult ar) => {
            //结束挂起的连接请求
            try{
                Socket handler = (Socket)ar.AsyncState;
                handler.EndConnect(ar);
                linkState = 1;
                IsConnected = true;
            }
            catch (SocketException ex)
            {
                Debug.LogError(ex);
                linkState = 0;
                IsConnected = false;
            }
        }, tcpSocket);
        //超时时间
        long outTime = Function.GetServerTime() + ConnectTimeOut;
        Scheduler.Instance.CreateScheduler("CHECK_BEGINCONNECT", 0, 0, 0.2f, () =>
        {
            Debug.Log("linkState = " + linkState + " Function.GetServerTime() = " + Function.GetServerTime() + " outTime = " + outTime);
            if (-1 == linkState && Function.GetServerTime() < outTime)
            {
                //还在连接中
                return ;
            }
            //移除菊花
            LoadLayerManager.Instance.RemoveLoad();
            Scheduler.Instance.Stop("CHECK_BEGINCONNECT");
            if (0 == linkState || -1 == linkState)
            {
                //失败
                tcpSocket.Close();
                IsConnected = false;
            }
            else if(1 == linkState)
            {
                //连接成功
                IsConnected = true;
            }
        });
    }

    public static int Receive(byte[] buffer, int size)
    {
        if (null == tcpSocket)
        {
            Debug.LogError("Network not connected!!!");
            return 0;
        }
        try
        {
            return tcpSocket.Receive(buffer, size, SocketFlags.None);
        }catch(Exception e)
        {
            Debug.LogError(e);
        }
        return -1;
    }


    public static void Send(byte[] buffer){
        if (null == tcpSocket)
        {
            Debug.LogError("Network not connected!!!");
            return;
        }
        tcpSocket.Send(buffer);
    }

    public static bool IsConnect()
    {
        return (null != tcpSocket && true == IsConnected);
    }
}