using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
 * 资源加载，释放管理器
 */

public delegate void ResourceCallback(ResourceObject resObj);

public class ResourceManager : MonoBehaviour {
    public static ResourceManager Instance;

    //资源容器
    static Dictionary<string, ResourceObject> ResDict = new Dictionary<string, ResourceObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void LoadResource<T>(string path, bool isSingle = true, bool _inBundle = false, System.Action<T> callback = null) where T : Object
    {
        ResourceObject resObject;
        ResourceCallback loadCallback = CreateCallback<T>(isSingle, callback);
        if (ResDict.TryGetValue(path, out resObject))
        {
            if (resObject.isLoading)
            {
                //加载中
                resObject.loadedCallback += loadCallback;
            }
            else
            {
                //已经加载了
                resObject.loadedCallback = loadCallback;
                resObject.CallLoaded();
            }
        }
        else if(ResourcePool.Instance.TryGetValue(path, out resObject))
        {
            //从缓冲池中拿
            ResourcePool.Instance.Remove(path);
            ResDict.Add(path, resObject);
            //回调
            resObject.loadedCallback = loadCallback;
            resObject.CallLoaded();
        }
        else
        {
            resObject = ResourceLoad.Instance.LoadResource(path, _inBundle, loadCallback);
            //放入字典
            ResDict.Add(path, resObject);
        }
    }

    //资源放入缓存，准备释放
    public void DestroyResource(string path)
    {
        if (!ResDict.ContainsKey(path))
        {
            Debug.LogWarning("ResourceManager.DestroyResource not found " + path);
            return;
        }
        ResourceObject resObject = ResDict[path];
        DestroyResource(resObject);
    }
    public void DestroyResource(ResourceObject resObject)
    {
        ResourcePool.Instance.DestroyResource(resObject);
        ResDict.Remove(resObject.name);
    }

    private ResourceCallback CreateCallback<T>(bool isSingle, System.Action<T> callback) where T : Object
    {
        return (ResourceObject resObj) => {
            T resObject = resObj.resObject as T;
            if (isSingle)
            {
                callback(resObject);
            }
            else
            {
                //不是单例，直接把源数据丢进Pool
                DestroyResource(resObj);
                callback(Instantiate(resObject));
            }
        };
    }


    //异步加载二进制数据
    private IEnumerator _WWWLoad(string filePath, System.Action<byte[]> callback)
    {
#if UNITY_EDITOR || (!UNITY_ANDROID)
        filePath = "file://" + filePath;
#endif
        WWW www = new WWW(filePath);
        yield return www;
        callback(www.bytes);
    }
    public void WWWLoad(string filePath, System.Action<byte[]> callback)
    {
        StartCoroutine(_WWWLoad(filePath, callback));
    }
}
