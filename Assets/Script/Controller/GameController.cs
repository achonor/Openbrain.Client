using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Awake()
    {
        //加载配置
        GameData.LoadConfigs();

        //注册监听
        RegisterEvent();
    }

    void Start()
    {
        //游戏开始，打开登陆界面
        UIManager.OpenUI("Prefabs/LoginUI", UIManager.Instance.GameUIRoot, (GameObject obj) =>
        {
            Debug.Log("GameController.Start Open LoginUI Success!");
        });
    }

    //注册监听
    void RegisterEvent()
    {
        UserEventManager.RegisterEvent("rep_message_player_info", (param) =>
        {
            rep_message_player_info repMsg = Client.Deserialize(rep_message_player_info.Parser, (string)param) as rep_message_player_info;
            PlayerData.UpdatePlayerData(repMsg.PlayerInfo);
        });
    }

}