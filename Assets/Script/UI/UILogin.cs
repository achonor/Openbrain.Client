using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cn.sharesdk.unity3d;

public class UILogin : MonoBehaviour {

    //防止重复点击
    private bool isLogining = false;
    private void Awake()
    {
        GameObject weChatBtn = transform.Find("WechatButton").gameObject;
        EventTrigger.Get(weChatBtn).onClick = ClickWeChat;
    }

    private void Start()
    {
        //注册授权成功回调
        ShareSDKManager.Instance.RegisterAuthCallback(AuthorizeSuccess);
        //注册获取信息成功的回调
        ShareSDKManager.Instance.RegisterInfoCallback(GetUserInfoSuccess);

        if (true == ShareSDKManager.Instance.IsAuthorized(PlatformType.WeChat))
        {
            //是否微信授权过，直接获取用户信息
            ShareSDKManager.Instance.GetUserInfo(PlatformType.WeChat);
        }
        else
        {
            //没有授权，重新等待用户操作
        }
    }

    public void ClickWeChat(GameObject go)
    {
        Debug.Log("UILogin.ClickWeChat");
        if (true == isLogining)
        {
            return;
        }
        isLogining = true;
        ShareSDKManager.Instance.WeChatAuthorize();
    }


    //授权结果回调
    public void AuthorizeSuccess(PlatformType type, ResponseState state, Hashtable result)
    {
        //恢复按钮
        isLogining = false;
        if (ResponseState.Success == state)
        {
            //直接获取信息
            ShareSDKManager.Instance.GetUserInfo(type);
        }
        else
        {
            Debug.Log("UILogin.AuthorizeSuccess Authorize faild!");
        }
    }
    //获取用户信息后调用
    public void GetUserInfoSuccess(PlatformType type, ResponseState state, Hashtable result)
    {
        if (ResponseState.Success == state)
        {
            //头像
            Debug.Log("headimgurl = " + (string)result["headimgurl"]);
            //名字
            Debug.Log("nickname = " + (string)result["nickname"]);
            //请求登陆
            ReqLoginGame((string)result["nickname"]);
        }
        else
        {
            Debug.Log("UILogin.GetUserInfoSuccess Get user info faild!");
        }
    }

    public void ReqLoginGame(string userName)
    {
        req_message_login_game reqMsg = new req_message_login_game();
        reqMsg.UserName = userName;
        Client.Instance.Request(reqMsg, (string data) =>
        {
            rep_message_login_game repMsg = Client.Deserialize(rep_message_login_game.Parser, data) as rep_message_login_game;

            Debug.Log("UILogin.ClickWeChat Login Success isOK = " + repMsg.IsOK);
        });
    }
}
