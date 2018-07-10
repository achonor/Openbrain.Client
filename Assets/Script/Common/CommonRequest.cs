using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonRequest {
    //请求登陆游戏
    public static void ReqLoginGame(string userID, string userName)
    {
        Debug.Log("UILogin.ReqLoginGame userID = " + userID + " userName " + userName);
        req_message_login_game reqMsg = new req_message_login_game();
        reqMsg.UserID = userID;
        reqMsg.UserName = userName;
        Client.Instance.Request(reqMsg, (string data) =>
        {
            rep_message_login_game repMsg = Client.Deserialize(rep_message_login_game.Parser, data) as rep_message_login_game;
            Debug.Log("UILogin.ClickWeChat Login Success isOK = " + repMsg.IsOK);
            if (0 == repMsg.IsOK)
            {
                //登陆成功
                PlayerData.UpdatePlayerData(repMsg.PlayerInfo);
                CommonMethod.EnterGame();
            }

        });
    }
}
