﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class LuaScriptManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(LuaScriptManager), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("ReconnectionLuaDebug", ReconnectionLuaDebug);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("Instance", get_Instance, set_Instance);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReconnectionLuaDebug(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			LuaScriptManager obj = (LuaScriptManager)ToLua.CheckObject<LuaScriptManager>(L, 1);
			obj.ReconnectionLuaDebug();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		try
		{
			ToLua.Push(L, LuaScriptManager.Instance);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Instance(IntPtr L)
	{
		try
		{
			LuaScriptManager arg0 = (LuaScriptManager)ToLua.CheckObject<LuaScriptManager>(L, 2);
			LuaScriptManager.Instance = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

