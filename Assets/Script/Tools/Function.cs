using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;

public class Function{
    //1970
    private static DateTime time1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    //服务器和本地时间差
    private static long offsetTime;

    //获取本地时间戳
    public static long GetLocaLTime()
    {
        return Convert.ToInt64((DateTime.UtcNow - time1970).TotalSeconds);
    }
    public static long GetServerTime()
    {
        return GetLocaLTime() + offsetTime;
    }
    public static void SetServerTime(long serverTime)
    {
        TimeSpan ts = DateTime.UtcNow - time1970;
        offsetTime = serverTime - GetLocaLTime();
    }

    public static byte[] Serialization(byte[] data)
    {
        //数据长度
        int protoLen = data.Length;
        byte[] head = new byte[4];
        IntToByte4(head, protoLen);
        //拼接
        return MergeArray<byte>(head, data);
    }

    public static int Byte4ToInt(byte[] data) {
        int ret = 0;
        for (int i = 0; i < 4; i++)
        {
            int tmpInt = data[i];
            if (tmpInt < 0)
            {
                tmpInt = tmpInt + 256;
            }
            ret = (ret << 8) | tmpInt;
        }
        return ret;
    }

    public static void IntToByte4(byte[] data, int num)
    {
        for (int i = 3; i >= 0; i--)
        {
            int tmpInt = ((255 << (8 * i))) & num;
            data[3 - i] = (byte)tmpInt;
        }
    }

    public static T[] MergeArray<T>(T[] first, T[] second)
    {
        T[] result = new T[first.Length + second.Length];
        first.CopyTo(result, 0);
        second.CopyTo(result, first.Length);
        return result;
    }


    //加载url图
    public static IEnumerator DownloadImage(Image image, string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
        }else
        {
            Texture2D tex = www.texture;
            Sprite temp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
            image.sprite = temp;
        }
    }
    //通过枚举获取等级文字
    public static string LevelToString(enum_player_level level)
    {
        switch (level)
        {
            case enum_player_level.Copper:
                return Language.GetTextByKey(10001);
            case enum_player_level.Silver:
                return Language.GetTextByKey(10002);
            case enum_player_level.Gold:
                return Language.GetTextByKey(10003);
            case enum_player_level.Platinum:
                return Language.GetTextByKey(10004);
            case enum_player_level.Diamond:
                return Language.GetTextByKey(10005);
            default:
                return "NULL";
        }
    }

    //通过枚举获取熟练度文字
    public static string ProficiencyToString(enum_player_proficiency proficiency)
    {
        switch (proficiency)
        {
            case enum_player_proficiency.Toe:
                return Language.GetTextByKey(10101);
            case enum_player_proficiency.Calf:
                return Language.GetTextByKey(10102);
            case enum_player_proficiency.Knee:
                return Language.GetTextByKey(10103);
            case enum_player_proficiency.Thigh:
                return Language.GetTextByKey(10104);
            case enum_player_proficiency.Butt:
                return Language.GetTextByKey(10105);
            case enum_player_proficiency.Belly:
                return Language.GetTextByKey(10106);
            case enum_player_proficiency.Chest:
                return Language.GetTextByKey(10107);
            case enum_player_proficiency.Neck:
                return Language.GetTextByKey(10108);
            case enum_player_proficiency.Brain:
                return Language.GetTextByKey(10109);
            default:
                return "NULL";
        }
    }

}
