using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * UI类的基类
*/

public class UIBase : MonoBehaviour {
    [HideInInspector]
    public string name;
    //当UI打开调用
    public virtual void OnOpen()
    {
        Debug.Log("UIBase.OnOpen");
    }
    //当UI关闭调用
    public virtual void OnClose()
    {
        Debug.Log("UIBase.OnClose");
    }
    public virtual void Close()
    {
        UIManager.CloseUI(name);
    }
}
