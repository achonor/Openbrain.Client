using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LitJson;

public class GameData{
    public static JsonData GameConfig;

    public static Color Gray        = new Color(125, 125, 125, 255) / 255f;
    public static Color Bule        = new Color( 66, 178, 231, 255) / 255f;
    public static Color LightRed    = new Color(138, 214, 230, 255) / 255f;
    public static Color LightBlue   = new Color(255, 153, 153, 255) / 255f;
    public static Color Yellow      = new Color(255, 207,  49, 255) / 255f;
    public static Color RedBlock    = new Color(247,  73,  74, 255) / 255f;
    public static Color BlueBlock   = new Color(  8,  69,  99, 255) / 255f;

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
