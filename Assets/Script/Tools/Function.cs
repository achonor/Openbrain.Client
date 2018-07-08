using System;
using System.Text;
using UnityEngine;
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
}
