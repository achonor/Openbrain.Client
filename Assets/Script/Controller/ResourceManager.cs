using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 资源加载，释放管理器
 */

public class ResourceObject
{
    public string name;
    public bool isLoading;
    public GameObject resObject;
    public System.Action<GameObject> loadedCallback;
    //释放时间戳
    public long freeTime;

    public ResourceObject(string path, System.Action<GameObject> callback = null)
    {
        name = path;
        isLoading = true;
        resObject = null;
        loadedCallback = callback;
    }

    public void CallLoaded()
    {
        isLoading = false;
        if (null != loadedCallback)
        {
            loadedCallback(resObject);
        }
    }
}

public class ResourceManager : MonoBehaviour{
    public static ResourceManager Instance;
    public Transform poolNode = null;

    //协程句柄
    private static IEnumerator CoroutineHandle = null;

    //资源容器
    static Dictionary<string, ResourceObject> ResDict = new Dictionary<string, ResourceObject>();
    //缓冲池
    static Dictionary<string, ResourceObject> CachePool = new Dictionary<string, ResourceObject>();

    //加载队列
    static Queue<ResourceObject> LoadQueue = new Queue<ResourceObject>();

    //资源在缓冲池等待释放的时间
    static int WAIT_FREE_TIME = 30;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //注册资源释放定时器
        Scheduler.Instance.CreateScheduler("FREE_RESOURCE", 0, 0, 1.0f, () =>
        {
            LoopCachePool();
        });
    }

    private void LoopCachePool()
    {
        //需要释放的资源列表
        List<ResourceObject> freeList = new List<ResourceObject>();
        //获取当前时间
        long curTime = Function.GetServerTime();
        foreach(var res in CachePool.Values)
        {
            if (res.freeTime < curTime)
            {
                freeList.Add(res);
            }
        }
        //释放
        foreach(var res in freeList)
        {
            //从缓存移除
            CachePool.Remove(res.name);
            //释放
            Destroy(res.resObject);
        }
    }
    public void LoadResource(string path, System.Action<GameObject> callback = null)
    {
        ResourceObject resObject;
        if (ResDict.TryGetValue(path, out resObject))
        {
            if (resObject.isLoading)
            {
                //加载中
                resObject.loadedCallback += callback;
            }
            else
            {
                //已经加载了
                resObject.loadedCallback = callback;
                resObject.CallLoaded();
            }
        }
        else if(CachePool.TryGetValue(path, out resObject))
        {
            //从缓冲池中拿
            CachePool.Remove(path);
            ResDict.Add(path, resObject);
            //回调
            resObject.loadedCallback = callback;
            resObject.CallLoaded();
        }
        else
        {
            //需要加载
            resObject = new ResourceObject(path, callback);
            //加入load队列
            LoadQueue.Enqueue(resObject);
            //放入字典
            ResDict.Add(path, resObject);

            if (null == CoroutineHandle)
            {
                //协程还没启动
                CoroutineHandle = _Load();
                StartCoroutine(CoroutineHandle);
            }
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
        if (resObject.isLoading)
        {
            //资源还在加载，回调中释放
            resObject.loadedCallback += (GameObject obj) =>
            {
                _DestroyResource(resObject);
            };
        }
        else
        {
            //直接释放
            _DestroyResource(resObject);
        }
    }
    private void _DestroyResource(ResourceObject res)
    {
        //从字典移除
        ResDict.Remove(res.name);
        //添加到缓存池准备释放
        CachePool.Add(res.name, res);
        //移动到poolNode下
        res.resObject.transform.SetParent(poolNode, false);
        //释放时间戳
        res.freeTime = Function.GetServerTime() + WAIT_FREE_TIME;
    }

    //加载
    private IEnumerator _Load()
    {
        while(0 < LoadQueue.Count)
        {
            ResourceObject resObject = LoadQueue.Dequeue();
            //加载
            ResourceRequest request = Resources.LoadAsync(resObject.name);
            yield return request;
            //实例化
            resObject.resObject = Instantiate(request.asset) as GameObject;
            if (null == resObject.resObject)
            {
                Debug.LogError("ResourceManager._Load Load Faild name = " + resObject.name);
            }
            //回调
            resObject.CallLoaded();
        }
        //协程结束
        CoroutineHandle = null;
    }

}
