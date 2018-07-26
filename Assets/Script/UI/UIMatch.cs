using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMatch : UIBase {


    private Transform topInfo;
    private Transform bottomInfo;
    private Transform matching;
    private Text matchingText;

    private int[] matchingTextKey = {10202, 10203, 10204, 10205};

    private void Awake() {
        topInfo = transform.Find("TopInfo");
        bottomInfo = transform.Find("BottomInfo");
        matching = transform.Find("Matching");
        matchingText = matching.Find("Text").GetComponent<Text>();
    }
	
    public override void OnOpen()
    {
        base.OnOpen();
        //注册匹配成功推送
        UserEventManager.RegisterEvent("rep_message_match_success", (param) => {
            rep_message_match_success repMsg = Client.Deserialize(rep_message_match_success.Parser, (byte[])param) as rep_message_match_success;
            //设置对手信息
            SetPlayerInfo(repMsg.PlayerInfo, bottomInfo);
            //六边形
            SetHexagon(repMsg.PlayerInfo, false);
            //状态切换
            SetMatchState(1);
        });
        //设置自己的信息
        SetPlayerInfo(PlayerData.playerInfo, topInfo);
        //六边形
        SetHexagon(PlayerData.playerInfo, true);

        //设置状态
        SetMatchState(0);
    }
    //设置玩家信息
    void SetPlayerInfo(struct_player_info playerInfo, Transform rootObj)
    {
        //头像
        if (!string.IsNullOrEmpty(playerInfo.UserIcon))
        {
            var headIcon = rootObj.Find("BaseImage/HeadImage/Mask/Image").GetComponent<Image>();
            StartCoroutine(Function.DownloadImage(headIcon, playerInfo.UserIcon, (sp)=> {
                //图片加载完成, 播放动画
                topInfo.GetComponent<MoveBy>().Play();
            }));
        }
        //名字
        rootObj.Find("Name").GetComponent<Text>().text = playerInfo.UserName;
        //等级
        rootObj.Find("LevelInfo/Level/Text").GetComponent<Text>().text = Function.LevelToString(playerInfo.Level);
        //排名
        rootObj.Find("LevelInfo/Ranking").GetComponent<Text>().text = string.Format(Language.GetTextByKey(10201), playerInfo.Ranking);
    }
    //设置六边形
    void SetHexagon(struct_player_info playerInfo, bool isSelf)
    {
        Polygon polygon = null;
        if (true == isSelf)
        {
            polygon = transform.Find("Hexagon/Polygon1").GetComponent<Polygon>();
        }
        else
        {
            polygon = transform.Find("Hexagon/Polygon2").GetComponent<Polygon>();
        }
        polygon.SetValue(new float[] { playerInfo.Speed, playerInfo.Judgment, playerInfo.Calculate, playerInfo.Accuracy, playerInfo.Observation, playerInfo.Memory });
    }

    //设置状态
    /// <summary>
    /// 设置匹配状态，控制UI显示
    /// </summary>
    /// <param name="state">0.匹配中，1.匹配成功</param>
    private void SetMatchState(int state)
    {
        //对手信息
        bottomInfo.gameObject.SetActive(1 == state);
        //匹配中
        matching.gameObject.SetActive(0 == state);
        if (0 == state) {
            //定时替换匹配中文字
            int matchingCount = 0;
            Scheduler.Instance.CreateScheduler("ChangeMatchingText", 0, 0, 0.3f, (param) =>
            {
                string curText = Language.GetTextByKey(matchingTextKey[matchingCount % matchingTextKey.Length]);
                if (!string.IsNullOrEmpty(curText))
                {
                    matchingText.text = curText;
                }
                matchingCount++;
            });
        }
        else{
            //播放动画
            bottomInfo.GetComponent<MoveBy>().Play();
            //停止定时器
            Scheduler.Instance.Stop("ChangeMatchingText");
            //一秒钟后发送准备游戏协议
            Scheduler.Instance.CreateScheduler("ReadyStartGame", 1.0f, 1, 0, (param) =>
            {
                CommonRequest.ReqSatrtReady();
            });
        }
    }

    public override void OnClose()
    {
        base.OnClose();
        Scheduler.Instance.Stop("ChangeMatchingText");

        //注销事件
        UserEventManager.UnRegisterEvent("rep_message_match_success");
    }
}
