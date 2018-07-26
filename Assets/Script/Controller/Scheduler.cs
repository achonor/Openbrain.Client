using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//定时器回调函数
public delegate void SchedulerCallback(object param);

public class SchedulerData
{
    private string name;
    private float startInterval;        //多少秒后启动
    private int runCount;               //运行次数
    private float timeInterval;         //每两次运行的时间间隔
    private SchedulerCallback callback; //回调
    private object param;               //回调参数

    private bool isPause = false;
    public IEnumerator handle;          //用于关闭定时器

    public SchedulerData(string _name, float _startInterval = 0, int _runCount = 0, float _timeInterval = 1.0f, SchedulerCallback _callback = null, object _param = null)
    {
        name = _name;
        startInterval = _startInterval;
        runCount = _runCount;
        timeInterval = _timeInterval;
        callback = _callback;
        param = _param;
    }
    public IEnumerator RunFunction()
    {
        yield return new WaitForSeconds(startInterval);
        int count = 0;
        while (true)
        {
            if (0 < runCount && runCount <= count)
            {
                break;
            }
            if (isPause)
            {
                continue;
            }
            if (null != callback)
            {
                try
                {
                    callback(param);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            count++;
            yield return new WaitForSeconds(timeInterval);
        }
        Scheduler.Instance.Stop(name);
    }
}

public class Scheduler : MonoBehaviour
{
    public static Scheduler Instance;
    private Dictionary<string, SchedulerData> schedulers = new Dictionary<string, SchedulerData>();

    private void Awake()
    {
        Instance = this;
    }
    //关闭定时器
    public void Stop(string name)
    {
        SchedulerData scheduler;
        if (schedulers.TryGetValue(name, out scheduler))
        {
            if (null != scheduler.handle)
            {
                StopCoroutine(scheduler.handle);
            }
            //移除
            schedulers.Remove(name);
        }
    }

    public SchedulerData CreateScheduler(string name, float _startInterval = 0, int _runCount = 0, float _timeInterval = 1.0f, SchedulerCallback _callback = null, object _param = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("The name is illegal");
            return null;
        }
        if (schedulers.ContainsKey(name))
        {
            //关闭同名定时器
            Stop(name);
            Debug.LogError("Create Same name Scheduler name = " + name);
        }
        SchedulerData scheduler = new SchedulerData(name, _startInterval, _runCount, _timeInterval, _callback, _param);
        scheduler.handle = scheduler.RunFunction();
        StartCoroutine(scheduler.handle);
        //保存
        schedulers.Add(name, scheduler);
        return scheduler;
    }
}