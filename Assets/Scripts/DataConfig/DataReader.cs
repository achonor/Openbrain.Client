using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//配置表数据基类

public abstract class DataReader{
    protected string GetDataConfigPath()
    {
        if (GameConst.UsePersistent)
        {
            return "file://" + GameConst.persistentPath + "/DataConfig/" + GetDataConfigName();
        }
        else
        {
            return GameConst.streamingUrl + "/DataConfig/" + GetDataConfigName();
        }
    }
    protected abstract string GetDataConfigName();
    protected abstract MessageParser GetMessageParser();
}
