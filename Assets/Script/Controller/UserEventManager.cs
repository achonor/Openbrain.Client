using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//事件回调声明
public delegate void EventCallback(object param);

public class UserEventManager : MonoBehaviour {

    //保存回调的字典
    private static Dictionary<string, EventCallback> CallbackDict = new Dictionary<string, EventCallback>();

    //注册事件
    public static void RegisterEvent(string name, EventCallback callback)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("UserEventManager.RegisterEvent name is null");
            return;
        }
        if (CallbackDict.ContainsKey(name))
        {
            EventCallback call = CallbackDict[name];
            call += callback;
        }
        else
        {
            EventCallback call = new EventCallback(callback);
            CallbackDict.Add(name, call);
        }
    }
    //注销事件
    public static void UnregisterEvent(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("UserEventManager.UnregisterEvent name is null");
            return;
        }
        if (!CallbackDict.ContainsKey(name))
        {
            Debug.LogError("UserEventManager.UnregisterEvent not found callback name = " + name);
            return;
        }
        CallbackDict.Remove(name);
    }
    //触发事件
    public static void TriggerEvent(string name, object param = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("UserEventManager.TriggerEvent name is null");
            return;
        }
        if (CallbackDict.ContainsKey(name))
        {
            EventCallback call = CallbackDict[name];
            call(param);
        }
    }
}
