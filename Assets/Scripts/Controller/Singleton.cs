using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//单列基类
public class Singleton<T> where T : new()
{
    private static T _Instance = default(T);
    public static T Instance
    {
        get
        {
            if (null == _Instance)
            {
                _Instance = new T();
            }
            return _Instance;
        }
    }
}
