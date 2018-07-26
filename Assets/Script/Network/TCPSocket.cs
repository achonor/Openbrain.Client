using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;


using UnityEngine;
using System.Collections;

public class RecvStream
{
    //数据缓存池初始大小
    private static int BUFFER_SIZE = 4096;

    //当前缓存起始下标
    private int startIdx = 0;
    //当前缓存长度
    private int bufferLength = 0;
    //缓存
    private byte[] buffer = new byte[BUFFER_SIZE];

    //添加数据到缓存
    public void AddBuffer(byte[] data, int dataLen)
    {
        if (dataLen > buffer.Length - startIdx - bufferLength)
        {
            //超过当前缓存
            ResetBuffer();
            if (dataLen > buffer.Length - startIdx - bufferLength)
            {
                //缓存长度不够，重新分配
                byte[] tmpBuffer = new byte[startIdx + bufferLength + dataLen];
                Array.Copy(buffer, startIdx, tmpBuffer, startIdx, bufferLength);
                buffer = tmpBuffer;
                Debug.LogError("TCPSocket.AddBuffer buffer length = " + buffer.Length);
            }
        }

        Array.Copy(data, 0, buffer, startIdx + bufferLength, dataLen);;
        bufferLength += dataLen;
    }


    /// <summary>
    /// 获取buff中的数据
    /// </summary>
    /// <param name="data">返回获取数据</param>
    /// <param name="getLen">需要的长度</param>
    /// <returns>返回实际获取的长度</returns>
    public int GetBuffer(byte[] data, int getLen)
    {
        if (bufferLength <= 0)
        {
            return 0;
        }

        if(bufferLength < getLen)
        {
            getLen = bufferLength;
        }
        Array.Copy(buffer, startIdx, data, 0, getLen);
        startIdx += getLen;
        bufferLength -= getLen;
        return getLen;
    }


    //重设缓存
    private void ResetBuffer()
    {
        byte[] tmpBuffer = new byte[bufferLength];
        Array.Copy(buffer, startIdx, tmpBuffer, 0, bufferLength);
        Array.Copy(tmpBuffer, 0, buffer, 0, bufferLength);
        startIdx = 0;
    }

}


public class TCPSocket
{
    private static Socket tcpSocket = null;

    //是否连接服务器
    private static bool IsConnected = false;
    //数据接收线程
    private static Thread receiveThread = null;
    //数据缓存池
    private static RecvStream BufferPool = new RecvStream();

    //单次接收数据的长度
    private static int OneReceiveLength = 1024;
    //连接超时时间
    private static int ConnectTimeOut = 5;


    public static void Connect(string ip, string port, System.Action<bool> callback = null)
    {
        //创建套接字
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //连接状态
        int linkState = -1;
        //异步连接
        tcpSocket.BeginConnect(IPAddress.Parse(ip), Convert.ToInt32(port), (IAsyncResult ar) => {
            //结束挂起的连接请求
            try{
                Socket handler = (Socket)ar.AsyncState;
                handler.EndConnect(ar);
                linkState = 1;
            }
            catch (SocketException ex)
            {
                Debug.LogError(ex);
                linkState = 0;
            }
        }, tcpSocket);
        //超时时间
        double outTime = Function.GetServerTime() + ConnectTimeOut;
        Scheduler.Instance.CreateScheduler("CHECK_BEGINCONNECT", 0, 0, 0.2f, (param) =>
        {
            Debug.Log("linkState = " + linkState + " Function.GetServerTime() = " + Function.GetServerTime() + " outTime = " + outTime);
            if (-1 == linkState && Function.GetServerTime() < outTime)
            {
                //还在连接中
                return ;
            }
            Scheduler.Instance.Stop("CHECK_BEGINCONNECT");
            if (0 == linkState || -1 == linkState)
            {
                //失败
                tcpSocket.Close();
                IsConnected = false;
                if (null != callback)
                {
                    callback(false);
                }
            }
            else if(1 == linkState)
            {
                //连接成功
                IsConnected = true;
                //创建接收线程
                receiveThread = new Thread(new ThreadStart(OnReceive));
                receiveThread.IsBackground = true;
                receiveThread.Start();
                if (null != callback)
                {
                    callback(true);
                }
            }
        });
    }

    //临时接收缓存
    private static byte[] tempBuffer = new byte[OneReceiveLength];
    public static void OnReceive()
    {
        while (true)
        {
            if (null == tcpSocket || !tcpSocket.Connected)
            {
                IsConnected = false;
                break;
            }
            try
            {
                int size = tcpSocket.Receive(tempBuffer, tempBuffer.Length, SocketFlags.None);
                lock (BufferPool)
                {
                    BufferPool.AddBuffer(tempBuffer, size);
                }
            }catch (ThreadAbortException){}
            catch (Exception e)
            {
                //tcpSocket.Disconnect(true);
                //tcpSocket.Shutdown(SocketShutdown.Both);
                Debug.LogError(e);
            }

        }
    }
    public static int Receive(byte[] data, int getLen)
    {
        lock (BufferPool)
        {
            return BufferPool.GetBuffer(data, getLen);
        }
    }

    public static void Send(byte[] buffer) {
        if (null == tcpSocket || !IsConnect())
        {
            Debug.LogError("Network not connected!!!");
            return;
        }
        tcpSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, (IAsyncResult ar) => {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }, tcpSocket);
    }

    public static void Disconnect()
    {
        IsConnected = false;
        if (null != receiveThread)
        {
            receiveThread.Abort();
            receiveThread = null;
        }
        if (null != tcpSocket && tcpSocket.Connected)
        {
            tcpSocket.Close();
            tcpSocket = null;
        }

    }


    public static bool IsConnect()
    {
        return (null != tcpSocket && true == IsConnected);
    }

    public static void Close()
    {
        Disconnect();
    }
}