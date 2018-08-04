using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 菊花层管理
 */

public class LoadLayerManager : MonoBehaviour {
    public static LoadLayerManager Instance;

    private GameObject RootObjec;
    //菊花层数
    private int loadingCount = 0;

    private void Awake()
    {
        Instance = this;
        RootObjec = transform.Find("Root").gameObject;
    }

    public void AddLoad()
    {
        loadingCount++;
        if (0 < loadingCount)
        {
            RootObjec.SetActive(true);
        }
    }

    public void RemoveLoad()
    {
        loadingCount--;
        if (loadingCount < 0)
        {
            Debug.LogError("LoadLayerManager.RemoveLoad loadingCount = " + loadingCount);
            loadingCount = 0;
        }
        if (0 == loadingCount)
        {
            RootObjec.SetActive(false);
        }
    }

}
