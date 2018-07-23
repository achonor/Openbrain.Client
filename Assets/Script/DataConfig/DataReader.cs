using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//配置表数据基类

public abstract class DataReader{
    protected IMessage ReadData()
    {
        //读文件
        FileStream file = new FileStream(GetDataConfigPath(), FileMode.Open);
        if (null == file) {
            Debug.LogError("DataReader.ReadData " + GetDataConfigPath() + " open faild");
            return null;
        }
        byte[] bytes = new byte[(int)file.Length];
        file.Read(bytes, 0, (int)file.Length);
        file.Close();
        //反序列化
        return GetMessageParser().ParseFrom(bytes);
    }

    private string GetDataConfigPath()
    {
        return Application.streamingAssetsPath + "/DataConfig/" + GetDataConfigName();
    }

    protected abstract string GetDataConfigName();
    protected abstract MessageParser GetMessageParser();
}
