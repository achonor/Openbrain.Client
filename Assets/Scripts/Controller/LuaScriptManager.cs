using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using System.IO;

public class LuaScriptManager {
    private static LuaState _luaState;
    public static LuaState luaState {
        get
        {
            if (null == _luaState)
            {
                Init();
            }
            return _luaState;
        }
    }
    static string ResourcesPath = Application.dataPath + "\\Resources/";
    static string[] SearchPath = new string[]
    {
        "LuaScripts",
        "LuaScripts/UIModule"
    };

    public static void Init()
    {
        new LuaResLoader();
        _luaState = new LuaState();
        _luaState.LogGC = true;
        _luaState.Start();
        LuaBinder.Bind(_luaState);
        DelegateFactory.Init();
        //添加搜索路径
        foreach(var path in SearchPath)
        {
            _luaState.AddSearchPath(ResourcesPath + path);
        }
    }

    //获取lua在ResourcesPath下的路径
    public static string GetLuaPath(string name)
    {
        foreach (var path in SearchPath)
        {
            string fullPath = string.Format("{0}{1}/{2}.bytes", ResourcesPath, path, name);
            if (File.Exists(fullPath))
            {
                return string.Format("{0}/{1}", path, name);
            }
        }
        return null;
    }

    public static object RunLuaFile(string fileName)
    {
        TextAsset textAsset = null;
#if UNITY_EDITOR
        //先尝试从ab中加载
        textAsset = AssetBundleLoader.LoadFileFromAssetBundle<TextAsset>("luaassetbundle", fileName);
        Debug.Log("luaassetbundle." + fileName + "load success!");
#endif
        if (null == textAsset)
        {
            //如果找不到，从文件加载
            string filePath = GetLuaPath(fileName);
            textAsset = Resources.Load<TextAsset>(filePath);
        }
        return RunLuaString(textAsset.text);
    }

    public static object RunLuaString(string luaStr)
    {
        return luaState.DoString<object>(luaStr);
    }
}
