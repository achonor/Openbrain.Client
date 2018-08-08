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
    static string ResourcesPath = Application.dataPath + "/";
    static string[] SearchPath = new string[]
    {
        "Lua/LuaScripts",
        "Lua/LuaScripts/Base",
        "Lua/LuaScripts/UIModule"
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
        //加载class文件
        RunLuaFile("class.lua");
    }

    //获取lua在ResourcesPath下的路径
    public static string GetLuaPath(string name)
    {
        foreach (var path in SearchPath)
        {
            string fullPath = string.Format("{0}{1}/{2}", ResourcesPath, path, name);
            if (File.Exists(fullPath))
            {
                return fullPath;
                //return string.Format("{0}/{1}", path, name);
            }
        }
        return null;
    }

    public static object RunLuaFile(string fileName)
    {
        TextAsset textAsset = null;
#if !UNITY_EDITOR
        //先尝试从ab中加载
        textAsset = AssetBundleLoader.LoadFileFromAssetBundle<TextAsset>("luascripts", fileName);
        if (null == textAsset)
        {
            Debug.Log("from luascripts load " + fileName + " faild!");
        }
#endif
#if UNITY_EDITOR
        if (null == textAsset)
        {
            string filePath = GetLuaPath(fileName);
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            StreamReader srcFile = new StreamReader(filePath, utf8WithoutBom);
            string srcText = srcFile.ReadToEnd();
            textAsset = new TextAsset(srcText);
        }
#endif
        return RunLuaString(textAsset.text);
    }

    public static object RunLuaString(string luaStr)
    {
        return luaState.DoString<object>(luaStr);
    }
}
