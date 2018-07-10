using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * UI管理器
 */

public class UIManager : MonoBehaviour {
    public static UIManager Instance = null;

    //普通UI根节点
    public Transform GameUIRoot;
    //基础根节点
    public Transform BaseUIRoot;
    //MessageUI根节点
    public Transform MessageUIRoot;

    //打开的UI
    private static Dictionary<string, GameObject> UIDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public static void OpenUI(string prePath, Transform parent, System.Action<GameObject> callback = null)
    {
        //加载资源
        UIDict.Add(prePath, null);
        ResourceManager.Instance.LoadResource(prePath, (GameObject obj) =>
        {
            if (null == obj)
            {
                Debug.LogError("UIManager.OpenUI faild prePath = " + prePath);
                return;
            }
            if (!UIDict.ContainsKey(prePath))
            {
                //还没加载完成就被关闭了
                return;
            }
            _OpenUI(prePath, obj, parent);
            //打开后回调
            if(null != callback)
            {
                callback(obj);
            }
        });
    }

    public static void CloseUI(string prePath)
    {
        if (!UIDict.ContainsKey(prePath))
        {
            Debug.LogError("UIManager.CloseUI faild: not found!");
            return;
        }
        GameObject obj = UIDict[prePath];
        _CloseUI(prePath, obj);
    }
    private static void _CloseUI(string prePath, GameObject obj)
    {
        obj.SetActive(false);
        //获取UI脚本
        UIBase ui = obj.GetComponent<UIBase>();
        if (null != ui)
        {
            ui.OnClose();
        }
        //释放
        ResourceManager.Instance.DestroyResource(prePath);
        UIDict.Remove(prePath);
    }

    private static void _OpenUI(string prePath, GameObject obj, Transform parent)
    {
        UIDict[prePath] = obj;
        if (null != parent)
        {
            obj.transform.SetParent(parent, false);
        }
        obj.SetActive(true);
        //获取UI脚本
        UIBase ui = obj.GetComponent<UIBase>();
        if (null != ui)
        {
            ui.name = prePath;
            ui.OnOpen();
        }
    }
}
