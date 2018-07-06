using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LitJson;

public class GameData{
    public static JsonData GameConfig;

    //GameConfig.json路径
    public static string GameConfigPath = "Configs/GameConfig";

    public static void LoadConfigs()
    {
        //加载GameConfig.json
        LoadGameConfig();
    }
    //加载GameConfig.json
    private static void LoadGameConfig()
    {
        TextAsset fileStr = Resources.Load(GameConfigPath) as TextAsset;
        if (null == fileStr)
        {
            Debug.LogError("GameData.LoadGameConfig Read " + GameConfigPath + " Faild!");
            return;
        }
        GameConfig = JsonMapper.ToObject(fileStr.text);
    }

    //获取当前服务器配置
    public static JsonData GetServerConfig()
    {
        JsonData serverConifg = GameConfig["Server"];
        int curIndex = (int)serverConifg["curIndex"];
        JsonData server = serverConifg["ServerList"][curIndex];
        return server;
    }
}
