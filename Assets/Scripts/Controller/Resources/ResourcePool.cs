using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
 * 释放管理器
 */
public class ResourcePool : MonoBehaviour
{
    public static ResourcePool Instance;
    public Transform poolNode = null;
    //缓冲池
    static Dictionary<string, ResourceObject> CachePool = new Dictionary<string, ResourceObject>();

    //资源在缓冲池等待释放的时间
    static int WAIT_FREE_TIME = 30;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //注册资源释放定时器
        Scheduler.Instance.CreateScheduler("FREE_RESOURCE", 0, 0, 1.0f, (param) =>
        {
            LoopCachePool();
        });
    }

    private void LoopCachePool()
    {
        //需要释放的资源列表
        List<ResourceObject> freeList = new List<ResourceObject>();
        //获取当前时间
        double curTime = Function.GetServerTime();
        foreach (var res in CachePool.Values)
        {
            if (res.freeTime < curTime)
            {
                freeList.Add(res);
            }
        }
        //释放
        foreach (var res in freeList)
        {
            //从缓存移除
            CachePool.Remove(res.name);
            //释放
            Destroy(res.resObject);
        }
    }

    //资源放入缓存，准备释放
    public void DestroyResource(ResourceObject resObject)
    {
        if (resObject.isLoading)
        {
            //资源还在加载，回调中释放
            resObject.loadedCallback += (ResourceObject obj) =>
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
        //添加到缓存池准备释放
        CachePool.Add(res.name, res);
        //移动到poolNode下
        res.resObject.transform.SetParent(poolNode, false);
        //释放时间戳
        res.freeTime = Function.GetServerTime() + WAIT_FREE_TIME;
    }

    //池中是否有对象
    public bool TryGetValue(string path, out ResourceObject resObject)
    {
        return CachePool.TryGetValue(path, out resObject);
    }
    //从缓存池移除对象
    public bool Remove(string path)
    {
        return CachePool.Remove(path);
    }
}
