using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;

public class EmojiDataConfig : DataReader {
    //单例
    private static EmojiDataConfig _Instance;
    public static EmojiDataConfig Instance
    {
        get
        {
            if (null == _Instance)
            {
                _Instance = new EmojiDataConfig();
            }
            return _Instance;
        }
    }
    //原始数据
    emoji_data_ARRAY _srcData = null;
    //数据字典
    private Dictionary<int, emoji_data> _dataDict = null;
    private EmojiDataConfig()
    {
        _dataDict = new Dictionary<int, emoji_data>();
        _srcData = ReadData() as emoji_data_ARRAY;
        foreach (var data in _srcData.Items)
        {
            _dataDict.Add((int)data.Id, data);
        }
    }

    public emoji_data GetDataByID(int id)
    {
        emoji_data retData = null;
        if (_dataDict.TryGetValue(id, out retData)){
            return retData;
        }
        return null;
    }

    public emoji_data[] GetAllData()
    {
        emoji_data[] retData = new emoji_data[_srcData.Items.Count];
        _srcData.Items.CopyTo(retData, 0);
        return retData;
    }

    protected override string GetDataConfigName()
    {
        return "emoji_data.bin";
    }
    protected override MessageParser GetMessageParser()
    {
        return emoji_data_ARRAY.Parser;
    }
}
