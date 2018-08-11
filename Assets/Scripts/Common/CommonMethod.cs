using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMethod {
    //初始化数据
    public static void InitGame(System.Action callback)
    {
        //异步操作数量
        int async_count = 0;
        //异步操作完成后调用
        System.Action async_end = delegate ()
        {
            async_count--;
            if (0 == async_count)
            {
                callback();
            }
        };

        //初始化配置表
        async_count++;
        PlayDataConfig.Instance.LoadConfig(async_end);
        async_count++;
        EmojiDataConfig.Instance.LoadConfig(async_end);

        //连接网络
        async_count++;
        Client.Instance.ConnectNetwork(false, (isOK) =>
        {
            if (false == isOK)
            {
                Debug.LogError("ConnectNetwork Faild!");
            }
            async_end();
        });

        //保留，方便提升性能
        async_count++;
        Scheduler.Instance.CreateScheduler("CommonMethod.InitGame", 2, 1, 0, (param) =>
        {
            async_end();
        });
    }


    public static void EnterGame()
    {
        //加载TableUI
        UIManager.OpenUI("Prefabs/TableUI", UIManager.Instance.BaseUIRoot, (GameObject obj) =>
        {
            UserEventManager.TriggerEvent("LoginGame");
            Debug.Log("CommonMethod.EnterGame Open TableUI Success!");
        });
    }



    public static string DataSize2String(float dataSize)
    {
        if (dataSize < 1024)
        {
            return string.Format("{0:N2}Byte", dataSize);
        }else if(dataSize < 1048576)
        {
            return string.Format("{0:N2}KB", dataSize / 1024);
        }
        else
        {
            return string.Format("{0:N2}MB", dataSize / 1048576);
        }
    }
}
