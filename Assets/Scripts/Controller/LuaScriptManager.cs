using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

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

    public static void Init()
    {
        new LuaResLoader();
        _luaState = new LuaState();
        _luaState.LogGC = true;
        _luaState.Start();
        LuaBinder.Bind(_luaState);

        //添加搜索路径
        _luaState.AddSearchPath(Application.dataPath + "\\Resources/Lua");
        _luaState.AddSearchPath(Application.dataPath + "\\Resources/LuaScripts");
        _luaState.AddSearchPath(Application.dataPath + "\\Resources/LuaScripts/UIModule");
    }

    public static object RunLuaFile(string filePath)
    {
        return luaState.DoFile<object>(filePath);
    }

    public static object RunLuaString(string luaStr)
    {
        return luaState.DoString<object>(luaStr);
    }
}
