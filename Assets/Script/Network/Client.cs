using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Google.Protobuf;

using LitJson;

public class Client : MonoBehaviour {
    public static Client Instance = null;

    //连接标识符
    private static int connectID = 0;
    //发送数据的数量
    private static int SendCount = 1;
    //数据发送队列
    private Queue<byte[]> SendQueue = new Queue<byte[]>();
    //回调函数字典
    private Dictionary<int, System.Action<string>> CallbackDict = new Dictionary<int, System.Action<string>>();


    //发送数据间隔(帧)
    private static int SendInterval = 1;
    //接收数据间隔(帧)
    private static int RecvInterval = 1;
    //包头长度
    private static int LENGTH_HEAD = 4;
    //单次接收最大长度
    private static int DATA_MAX_LENGTH = 4096;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ConnectNetwork();
    }

    private void Update()
    {
        if (TCPSocket.IsConnect())
        {
            //发送数据
            if (0 == Time.frameCount % SendInterval && 0 < SendQueue.Count)
            {
                TCPSocket.Send(SendQueue.Dequeue());
            }
            if (0 == Time.frameCount % RecvInterval)
            {
                onReceive();
            }
        }
    }
    private void OnApplicationQuit()
    {
        TCPSocket.Close();
    }


    public void ConnectNetwork(System.Action callback = null)
    {
        //获取服务器配置
        JsonData server = GameData.GetServerConfig();
        string ip = (string)server["ip"];
        string port = (string)server["port"];
        Debug.Log("Client.ConnectNetwork Start Connect Server ip = " + ip + " port = " + port);

        //连接
        TCPSocket.Connect(ip, port);
    }

    public void Request(IMessage msg, System.Action<string> callback = null) 
    {
        root_proto proto = new root_proto();
        //序列化
        byte[] data = Serialize(msg);
        proto.ConnectID = connectID;
        proto.MessageID = SendCount++;
        proto.MessageName = msg.GetType().ToString();
        proto.MessageData = ByteString.CopyFrom(data);
        //本地序列化
        byte[] protoByte = Function.Serialization(Serialize(proto));
        //保存回调
        CallbackDict.Add(proto.MessageID, callback);

        //加入发送队列
        SendQueue.Enqueue(protoByte);
        //菊花
        LoadLayerManager.Instance.AddLoad();
    }

    //当前接收的状态
    enum RecvState
    {
        idle = 0, //空闲状态
        head = 1, //接收包头
        body = 2, //接收数据体
        die = 3   //接收失败
    };
    delegate int FuncReceive(byte[] buffer, int len);
    //接收的数据
    private byte[] lastBuffer;
    //下次接受数据的长度
    private int receiveLen = 0;
    //当前接收状态
    private RecvState status = RecvState.idle;
    private void onReceive()
    {

        //接受数据的函数
        FuncReceive receive = delegate (byte[] buffer, int len) {
            if (DATA_MAX_LENGTH < len)
            {
                len = DATA_MAX_LENGTH;
            }
            int ret = TCPSocket.Receive(buffer, len);
            return ret;
        };
        //状态切换函数
        System.Action switch_idle = delegate() {
            status = RecvState.idle;
            receiveLen = 0;
        };
        System.Action switch_head = delegate() {
            status = RecvState.head;
            receiveLen = LENGTH_HEAD;
        };
        System.Action switch_die = delegate() {
            status = RecvState.die;
            receiveLen = 0;
        };
        System.Action<int> switch_body = delegate(int len) {
            status = RecvState.body;
            receiveLen = len;
        };

        //状态操作函数
        System.Action idle_func = delegate() {
            switch_head();
        };
        //接收包头
        System.Action head_func = delegate() {
            byte[] buffer =new byte[receiveLen]; ;
            int ret = receive(buffer, receiveLen);
            if (-1 == ret)
            {
                switch_die();
                return;
            }
            if (ret <= 0)
            {
                return;
            }
            int bodyLen = Function.Byte4ToInt(buffer);
            if (DATA_MAX_LENGTH < bodyLen)
            {
                Debug.LogError("Client.onReceive body data is too long!");
            }
            switch_body(bodyLen);
        };
        //接受数据
        System.Action body_func = delegate() {
            byte[] buffer = new byte[receiveLen];
            int ret = receive(buffer, receiveLen);
            if (-1 == ret)
            {
                switch_die();
                return;
            }
            if (ret <= 0)
            {
                return;
            }
            if (null != lastBuffer && 0 < lastBuffer.Length)
            {
                //需要拼接数据
                lastBuffer = Function.MergeArray<byte>(lastBuffer, buffer);
            }
            else
            {
                lastBuffer = buffer;
            }
            if (ret < receiveLen)
            {
                //数据没有接收完
                switch_body(receiveLen - ret);
            }
            else
            {
                //数据接收完成
                try
                {
                    receiveData(lastBuffer);
                }
                finally
                {
                    lastBuffer = null;
                    switch_idle();
                }
            }
        };
        System.Action die_func = delegate() {
            //pass
        };

        //start
        switch (status)
        {
            case RecvState.idle:
                idle_func(); break;
            case RecvState.head:
                head_func(); break;
            case RecvState.body:
                body_func(); break;
            case RecvState.die:
                head_func(); break;
            default:
                idle_func(); break;
        }
    }
    private void receiveData(byte[] data)
    {

        root_proto proto = new root_proto();
        proto = Deserialize(root_proto.Parser, data) as root_proto;

        //校正服务器时间
        Function.SetServerTime((long)proto.ServerTime);
        //数据
        string strData = proto.MessageData.ToStringUtf8();
        //取消菊花
        LoadLayerManager.Instance.RemoveLoad();
        //事件
        UserEventManager.TriggerEvent(proto.MessageName, strData);

        //激活回调函数
        try
        {
            System.Action<string> callback = CallbackDict[proto.MessageID];
            if (null != callback)
            {
                callback(strData);
            }
            CallbackDict.Remove(proto.MessageID);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    //序列化
    public static byte[] Serialize(IMessage data)
    {
        MemoryStream stream = new MemoryStream();
        if (null != stream)
        {
            data.WriteTo(stream);
            byte[] bytes = stream.ToArray();
            stream.Close();
            return bytes;
        }
        stream.Close();
        return null;
    }
    //反序列化
    public static IMessage Deserialize(MessageParser parser, byte[] byteData)
    {
        Stream stream = new MemoryStream(byteData);
        if (null != stream)
        {
            IMessage msg = parser.ParseFrom(stream);
            stream.Close();
            return msg;
        }
        stream.Close();
        return default(IMessage);
    }
    public static IMessage Deserialize(MessageParser parser, string data)
    {
        byte[] byteData = Encoding.UTF8.GetBytes(data);
        return Deserialize(parser, byteData);
    }

}
