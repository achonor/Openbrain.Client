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
    //体力
    public static int energy;
    //钻石
    public static int gems;
    //等级
    public static enum_player_level level;
    //熟练度
    public static enum_player_proficiency proficiency;


    public static void UpdatePlayerData(struct_player_info info)
    {
        if (!string.IsNullOrEmpty(info.UserName))
        {
            userName = info.UserName;
        }
        if (!string.IsNullOrEmpty(info.UserIcon))
        {
            userIcon = info.UserIcon;
        }
        energy = info.Energy;
        gems = info.Gems;
        level = info.Level;
        proficiency = info.Proficiency;
    }
}
