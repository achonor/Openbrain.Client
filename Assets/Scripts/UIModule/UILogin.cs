using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cn.sharesdk.unity3d;

public class UILogin : UIBase {

    //防止重复点击
    private bool isLogining = false;

    private GameObject weChatBtn;
    private GameObject QQBtn;
    private void Awake()
    {
        weChatBtn = transform.Find("WechatButton").gameObject;
        QQBtn = transform.Find("QQButton").gameObject;
        EventTrigger.Get(weChatBtn).onClick = ClickWeChat;
        EventTrigger.Get(QQBtn).onClick = ClickQQ;
    }

    private void Start()
    {
        //注册授权成功回调
        ShareSDKManager.Instance.RegisterAuthCallback(AuthorizeCallback);
        //注册获取信息成功的回调
        ShareSDKManager.Instance.RegisterInfoCallback(GetUserInfoCallback);

        if (true == ShareSDKManager.Instance.IsAuthorized(PlatformType.WeChat))
        {
            //是否微信授权过，直接获取用户信息
            ShareSDKManager.Instance.GetUserInfo(PlatformType.WeChat);
        }
        else if(true == ShareSDKManager.Instance.IsAuthorized(PlatformType.QQ))
        {
            //是否QQ授权过，直接获取用户信息
            ShareSDKManager.Instance.GetUserInfo(PlatformType.QQ);
        }
        else
        {
            //没有授权，重新等待用户操作
        }
    }
    public void ClickWeChat(GameObject go)
    {
        if (true)
        {
            TipsManager.ShowTips(Language.GetTextByKey(1));
            return;
        }
        Debug.Log("UILogin.ClickWeChat");
#if UNITY_EDITOR
        CommonRequest.ReqLoginGame("achonor", "我要玩女号", @"http://thirdqq.qlogo.cn/qqapp/1107034140/D532B5446BA9E5AC90AB5138D1BD19BC/100");
#elif UNITY_IOS || UNITY_ANDROID
        if (true == isLogining)
        {
            return;
        }
        isLogining = true;
        ShareSDKManager.Instance.WeChatAuthorize();
#endif
    }
    public void ClickQQ(GameObject go)
    {
        Debug.Log("UILogin.ClickQQ");
#if UNITY_EDITOR
        CommonRequest.ReqLoginGame("achonor", "我要玩女号", @"http://thirdqq.qlogo.cn/qqapp/1107034140/D532B5446BA9E5AC90AB5138D1BD19BC/100");
#elif UNITY_IOS || UNITY_ANDROID
        if (true == isLogining)
        {
            return;
        }
        isLogining = true;
        ShareSDKManager.Instance.QQAuthorize();
#endif
    }

    //授权结果回调
    public void AuthorizeCallback(PlatformType type, ResponseState state, Hashtable result)
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
            Debug.Log("UILogin.AuthorizeCallback Authorize faild!");
        }
    }
    //获取用户信息后调用
    public void GetUserInfoCallback(PlatformType type, ResponseState state, Hashtable result)
    {
        if (ResponseState.Success == state)
        {
            Hashtable user = ShareSDKManager.Instance.ssdk.GetAuthInfo(type);
            //ID
            Debug.Log("userID = " + (string)user["userID"]);
            //名字
            Debug.Log("userName = " + (string)user["userName"]);
            //头像
            Debug.Log("userIcon = " + (string)user["userIcon"]);
            //请求登陆
            CommonRequest.ReqLoginGame((string)user["userID"], (string)user["userName"], (string)user["userIcon"]);
        }
        else
        {
            Debug.Log("UILogin.GetUserInfoCallback Get user info faild!");
        }
    }


}
