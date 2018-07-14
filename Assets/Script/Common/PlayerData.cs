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
    //速度
    public static float speed;
    //判断力
    public static float judgment;
    //计算力
    public static float calculate;
    //精确度
    public static float accuracy;
    //观察力
    public static float observation;
    //记忆力
    public static float memory;


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
        speed = info.Speed;
        judgment = info.Judgment;
        calculate = info.Calculate;
        accuracy = info.Accuracy;
        observation = info.Observation;
        memory = info.Memory;
    }
}
