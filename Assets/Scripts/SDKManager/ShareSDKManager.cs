using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;

public class ShareSDKManager : MonoBehaviour
{
    //单例
    public static ShareSDKManager Instance = null;

    public ShareSDK ssdk;
    public delegate void AuthCallback(PlatformType type, ResponseState state, Hashtable result);
    //授权结果回调
    protected AuthCallback authCallback;
    //获取信息获取回调
    protected AuthCallback infoCallback;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        ssdk = gameObject.GetComponent<ShareSDK>();
        ssdk.authHandler = OnAuthResultHandler;
        ssdk.showUserHandler = OnGetUserInfoResultHandler;
    }
    public void RegisterAuthCallback(AuthCallback callback)
    {
        if (null == authCallback)
        {
            authCallback = new AuthCallback(callback);
        }
        else
        {
            authCallback += callback;
        }
    }
    public void RegisterInfoCallback(AuthCallback callback)
    {
        if (null == infoCallback)
        {
            infoCallback = new AuthCallback(callback);
        }
        else
        {
            infoCallback += callback;
        }
    }
    //是否授权
    public bool IsAuthorized(PlatformType type)
    {
        return ssdk.IsAuthorized(type);
    }
    //获取用户数据
    public void GetUserInfo(PlatformType type)
    {
        ssdk.GetUserInfo(type);
    }

    public void WeChatAuthorize()
    {
        ssdk.Authorize(PlatformType.WeChat);
    }
    public void QQAuthorize()
    {
        ssdk.Authorize(PlatformType.QQ);
    }
    void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        Debug.Log("ShareSDKManager.OnAuthResultHandler = state" + state);
        if (state == ResponseState.Success)
        {
            if (result != null && result.Count > 0)
            {
                print("authorize success !" + "Platform :" + type + "result:" + MiniJSON.jsonEncode(result));
            }
            else
            {
                print("authorize success !" + "Platform :" + type);
            }
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
        if (null != authCallback)
        {
            authCallback(type, state, result);
        }
    }

    void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        Debug.Log("ShareSDKManager.OnGetUserInfoResultHandler = state" + state);
        if (state == ResponseState.Success)
        {
            print("get user info result :");
            print(MiniJSON.jsonEncode(result));
            print("AuthInfo:" + MiniJSON.jsonEncode(ssdk.GetAuthInfo(PlatformType.QQ)));
            print("Get userInfo success !Platform :" + type);
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
        if (null != infoCallback)
        {
            infoCallback(type, state, result);
        }
    }
}
