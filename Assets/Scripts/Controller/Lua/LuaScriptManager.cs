using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using System.IO;

public class LuaScriptManager : LuaClient
{
    public new static LuaScriptManager Instance;
    
    static string[] SearchPath = new string[]
    {
        "Lua",
        "Lua/UIModule"
    };


    new void Awake()
    {
        Debug.Log("LuaScriptManager.Awake1");
        base.Awake();
        Instance = this;
        Debug.Log("LuaScriptManager.Awake2");
    }
    protected override LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }

    protected override void OnLoadFinished()
    {
        base.OnLoadFinished();
        Debug.Log(Application.persistentDataPath);
        //添加搜索路径
        foreach (var path in SearchPath)
        {
            if (GameConst.UsePersistent)
            {
                string tmpPath = string.Format("{0}/{1}/{2}", Application.persistentDataPath, GameConst.osDir, path);
                Debug.Log("LuaScriptManager.OnLoadFinished AddSearchPath = " + tmpPath);
                luaState.AddSearchPath(tmpPath);
            }
            else
            {
                luaState.AddSearchPath(Application.dataPath + "/" + path);
            }
        }

    }

    public T RunLuaFile<T>(string fileName)
    {
        return luaState.DoFile<T>(fileName);
    }

    public T RunLuaString<T>(string luaStr)
    {
        return luaState.DoString<T>(luaStr);
    }

    new void OnApplicationQuit()
    {
        base.Destroy();
    }
}