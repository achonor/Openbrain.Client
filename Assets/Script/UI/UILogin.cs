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
        req_message_login_game reqMsg = new req_message_login_game();
        reqMsg.UserName = "lilincong";
        Client.Instance.Request(reqMsg, (string data) =>
        {
            rep_message_login_game repMsg = new rep_message_login_game();
            Client.Deserialize(rep_message_login_game.Parser, data);
            Debug.LogError("UILogin.ClickWeChat Login Success");
        });
    }
}
