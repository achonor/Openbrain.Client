using System.Text;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Google.Protobuf;

using LitJson;

public class Client : MonoBehaviour {
    public static Client Instance = null;

    //发送数据的数量
    private static int SendCount = 0;
    //数据发送队列
    private Queue<IMessage> SendQueue = new Queue<IMessage>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ConnectNetwork();
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

    public void Request<T>(T msg, System.Action<IMessage> callback)
    {
        MainProto proto = new MainProto();
    }

}
