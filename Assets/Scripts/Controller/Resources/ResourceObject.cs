using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
 * 资源对象
 */

public class ResourceObject
{
    public string name;
    public bool inBundle;
    public bool isLoading;
    public GameObject resObject;
    public ResourceCallback loadedCallback;
    //释放时间戳
    public double freeTime;

    public ResourceObject(string path, bool _inBundle = false, ResourceCallback callback = null)
    {
        name = path;
        isLoading = true;
        resObject = null;
        inBundle = _inBundle;
        loadedCallback = callback;
    }

    public void CallLoaded()
    {
        isLoading = false;
        if (null != loadedCallback)
        {
            loadedCallback(this);
        }
    }
}