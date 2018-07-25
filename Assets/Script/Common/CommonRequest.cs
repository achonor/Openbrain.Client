using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonRequest {
    //请求登陆游戏
    public static void ReqLoginGame(string userID, string userName, string userIcon = null)
    {
        Debug.Log("CommonRequest.ReqLoginGame userID = " + userID + " userName " + userName);
        req_message_login_game reqMsg = new req_message_login_game();
        reqMsg.UserID = userID;
        reqMsg.UserName = userName;
        reqMsg.UserIcon = null != userIcon ? userIcon : PlayerData.userIcon;
        Client.Instance.Request(reqMsg, (byte[] data) =>
        {
            rep_message_login_game repMsg = Client.Deserialize(rep_message_login_game.Parser, data) as rep_message_login_game;
            Debug.Log("CommonRequest.ReqLoginGame Login Success isOK = " + repMsg.IsOK);
            if (0 == repMsg.IsOK)
            {
                //登陆成功
                PlayerData.UpdatePlayerData(repMsg.PlayerInfo);
                CommonMethod.EnterGame();
            }

        });
    }
    //请求开始匹配
    public static void ReqSatrtMatch()
    {
        Debug.Log("CommonRequest.ReqSatrtMatch");
        req_message_start_match reqMsg = new req_message_start_match();
        Client.Instance.Request(reqMsg, (byte[] data) => {
            rep_message_start_match repMsg = Client.Deserialize(rep_message_start_match.Parser, data) as rep_message_start_match;
            Debug.Log("CommonRequest.ReqSatrtMatch isOK = " + repMsg.IsOK);
            if (0 == repMsg.IsOK)
            {
                //已经开始匹配
            }
            else
            {
                Debug.LogError("CommonRequest.ReqSatrtMatch Request Match Error!");
            }
        });
    }
    //请求开始准备
    public static void ReqSatrtReady()
    {
        Debug.Log("CommonRequest.ReqSatrtReady");
        req_message_start_ready reqMsg = new req_message_start_ready();
        Client.Instance.Request(reqMsg, (byte[] data) =>
        {
            rep_message_start_ready repMsg = Client.Deserialize(rep_message_start_ready.Parser, data) as rep_message_start_ready;
            Debug.Log("CommonRequest.ReqSatrtReady isOK = " + repMsg.IsOK);
            if (0 == repMsg.IsOK)
            {
                //打开准备UI
                UIManager.OpenUI("Prefabs/ReadyUI", UIManager.Instance.GameUIRoot, (uiObj)=> {
                    UIReady uiReady = uiObj.transform.GetComponent<UIReady>();
                    if (null != uiReady)
                    {
                        uiReady.RefreshUI(repMsg);
                    }
                    //关闭匹配UI
                    UIManager.CloseUI("Prefabs/MatchUI");
                    //关闭上一局UI
                    UIManager.CloseUI("Prefabs/PlayUI");
                });
            }
            else
            {
                Debug.LogError("CommonRequest.ReqSatrtReady Request Match Error!");
            }
        });
    }
    //请求开始游戏
    public static void ReqSatrtGame(int innings, System.Action<rep_message_start_game> callback = null)
    {
        Debug.Log("CommonRequest.ReqSatrtGame");
        req_message_start_game reqMsg = new req_message_start_game();
        reqMsg.Innings = innings;
        Client.Instance.Request(reqMsg, (byte[] data) =>
        {
            rep_message_start_game repMsg = Client.Deserialize(rep_message_start_game.Parser, data) as rep_message_start_game;
            Debug.Log("CommonRequest.ReqSatrtGame isOK = " + repMsg.IsOK);
            if (0 == repMsg.IsOK)
            {
                //开始游戏
                Debug.Log("end_time = " + repMsg.EndTime);
                if (null != callback)
                {
                    callback(repMsg);
                }
                //打开PlayUI
                UIManager.OpenUI("Prefabs/PlayUI", UIManager.Instance.GameUIRoot, (uiObj) =>
                {
                    UIPlay uiPlay = uiObj.transform.GetComponent<UIPlay>();
                    if (null != uiPlay)
                    {
                        uiPlay.RefreshUI(innings, repMsg);
                    }
                    //关闭准备UI
                    UIManager.CloseUI("Prefabs/ReadyUI");
                });
            }
            else
            {
                Debug.LogError("CommonRequest.ReqSatrtGame Request Start Error!");
            }
        }, false);
    }
}
