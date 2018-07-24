using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//配置表数据基类

public abstract class DataReader{
    protected string GetDataConfigPath()
    {
        return Application.streamingAssetsPath + "/DataConfig/" + GetDataConfigName();
    }
    protected abstract string GetDataConfigName();
    protected abstract MessageParser GetMessageParser();
}
