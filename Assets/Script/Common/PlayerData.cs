using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {
    //玩家id
    public static string userID;
    //玩家昵称
    public static string userName;
    //玩家icon url
    public static string userIcon;


    public static void UpdatePlayerData(struct_player_info info)
    {
        userID = info.UserID;
        userName = info.UserName;
        userIcon = info.UserIcon;
    }

}
