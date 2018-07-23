using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class PlayDataConfig : DataReader {
    //单例
    private static PlayDataConfig _Instance;
    public static PlayDataConfig Instance
    {
        get
        {
            if (null == _Instance)
            {
                _Instance = new PlayDataConfig();
            }
            return _Instance;
        }
    }
    //数据字典
    private Dictionary<int, play_data> _dataDict = null;
    private PlayDataConfig()
    {
        _dataDict = new Dictionary<int, play_data>();
        play_data_ARRAY allData = ReadData() as play_data_ARRAY;
        foreach (var data in allData.Items)
        {
            _dataDict.Add((int)data.Id, data);
        }
    }

    public play_data GetDataByID(int id)
    {
        play_data retData = null;
        if (_dataDict.TryGetValue(id, out retData)){
            return retData;
        }
        return null;
    }

    protected override string GetDataConfigName()
    {
        return "play_data.bin";
    }
    protected override MessageParser GetMessageParser()
    {
        return play_data_ARRAY.Parser;
    }
}
