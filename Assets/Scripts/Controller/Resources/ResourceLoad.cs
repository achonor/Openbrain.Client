using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
 * 资源加载
 */


public class ResourceLoad : MonoBehaviour {
    public static ResourceLoad Instance;

    //协程句柄
    private static IEnumerator CoroutineHandle = null;
    //加载队列
    static Queue<ResourceObject> LoadQueue = new Queue<ResourceObject>();

    private void Awake()
    {
        Instance = this;
    }

    public ResourceObject LoadResource(string path, bool _inBundle = false, ResourceCallback callback = null)
    {
        //需要加载
        ResourceObject resObject = new ResourceObject(path, _inBundle, callback);
        //加入load队列
        LoadQueue.Enqueue(resObject);

        if (null == CoroutineHandle)
        {
            //协程还没启动
            CoroutineHandle = _Load();
            StartCoroutine(CoroutineHandle);
        }
        return resObject;
    }

    //加载
    private IEnumerator _Load()
    {
        while(0 < LoadQueue.Count)
        {
            ResourceObject resObject = LoadQueue.Dequeue();
            //加载
            if (!resObject.inBundle)
            {
                //Resources中加载
                ResourceRequest request = Resources.LoadAsync(resObject.name);
                yield return request;
                //实例化
                resObject.resObject = Instantiate(request.asset) as GameObject;
            }
            else
            {
                //AssetBundle中加载
                string fileName = Path.GetFileName(resObject.name);
                AssetBundleRequest request = AssetBundleLoader.LoadFileFromAssetBundleAsync(fileName.ToLower(), fileName);
                yield return request;
                //实例化
                resObject.resObject = Instantiate(request.asset) as GameObject;
            }
            if (null == resObject.resObject)
            {
                Debug.LogError("ResourceLoad._Load Load Faild name = " + resObject.name);
            }
            //回调
            resObject.CallLoaded();
        }
        //协程结束
        CoroutineHandle = null;
    }
}
