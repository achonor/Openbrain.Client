using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * 菊花层管理
 */

public class LoadLayerManager : MonoBehaviour {
    public static LoadLayerManager Instance;

    //菊花层数
    private int loadingCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddLoad()
    {
        loadingCount++;
        if (0 < loadingCount)
        {
            gameObject.SetActive(true);
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
            gameObject.SetActive(false);
        }
    }

}
