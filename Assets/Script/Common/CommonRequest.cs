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
}
