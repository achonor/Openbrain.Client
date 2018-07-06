using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogin : MonoBehaviour {


    private void Awake()
    {
        GameObject weChatBtn = transform.Find("WechatButton").gameObject;
        EventTrigger.Get(weChatBtn).onClick = ClickWeChat;
    }

    public void ClickWeChat(GameObject go)
    {
        Debug.Log("UILogin.ClickWeChat");
    }
}
