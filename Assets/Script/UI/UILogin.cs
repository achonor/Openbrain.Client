using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogin : MonoBehaviour {
    private void Awake()
    {
        GameObject weChatBtn = transform.Find("WechatButton").gameObject;
        EventTrigger.Get(weChatBtn).onClick = ClickWeChat;

        UserEventManager.RegisterEvent("rep_message_login_game", (data) =>
        {
            rep_message_login_game repMsg = Client.Deserialize(rep_message_login_game.Parser, (string)data) as rep_message_login_game;

            Debug.Log("UILogin.Awake Login rep_message_login_game");
        });
    }

    public void ClickWeChat(GameObject go)
    {
        Debug.Log("UILogin.ClickWeChat");
        req_message_login_game reqMsg = new req_message_login_game();
        reqMsg.UserName = "lilincong";
        Client.Instance.Request(reqMsg, (string data) =>
        {
            rep_message_login_game repMsg = Client.Deserialize(rep_message_login_game.Parser, data) as rep_message_login_game;

            Debug.Log("UILogin.ClickWeChat Login Success isOK = " + repMsg.IsOK + " testString = " + repMsg.TestString);
        });
    }
}
