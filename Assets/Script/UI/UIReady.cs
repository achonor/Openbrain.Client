using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReady : UIBase {
    private int[] InningsTextKey = {10301, 10302, 10303};


    private LoopHorizontalScrollRect playViewList;
    private LoopHorizontalScrollRect emojiViewList;
    private void Awake()
    {
        playViewList = transform.Find("RandPlay").GetComponent<LoopHorizontalScrollRect>();
        emojiViewList = transform.Find("Emoji/EmojiList").GetComponent<LoopHorizontalScrollRect>();
    }
    public override void OnOpen()
    {
        base.OnOpen();
        //表情包
        emoji_data[] emojiDatas = EmojiDataConfig.Instance.GetAllData();
        emojiViewList.totalCount = emojiDatas.Length;
        emojiViewList.RegisterInitCallback((Transform obj, int index) =>
        {
            Function.SetImageSprite(obj.Find("Image").GetComponent<Image>(), emojiDatas[index].Icon);
            EventTrigger.Get(obj.gameObject).onClick = (cObj) => {
                TipsManager.ShowTips(Language.GetTextByKey(1));
            };
        });
        emojiViewList.RefillCells();
    }

    public void RefreshUI(rep_message_start_ready readyInfo)
    {
        struct_player_info opponentInfo = readyInfo.PlayerInfo;
        //我的头像
        if (!string.IsNullOrEmpty(PlayerData.userIcon))
        {
            var headIcon = transform.Find("PlayerInfo/LeftHeadImage/Mask/Image").GetComponent<Image>();
            StartCoroutine(Function.DownloadImage(headIcon, PlayerData.userIcon));
        }
        //对手头像
        if (!string.IsNullOrEmpty(opponentInfo.UserIcon))
        {
            var headIcon = transform.Find("PlayerInfo/RightHeadImage/Mask/Image").GetComponent<Image>();
            StartCoroutine(Function.DownloadImage(headIcon, opponentInfo.UserIcon));
        }
        //倒计时
        float totalTime = (float)(readyInfo.StartTime - Function.GetServerTime());
        Image countdownImage = transform.Find("Countdown/Bar").GetComponent<Image>();
        Scheduler.Instance.CreateScheduler("UIReady.Countdown", 0, 0, 0.01f, () =>
        {
            float lastTime = (float)(readyInfo.StartTime - Function.GetServerTime());
            countdownImage.fillAmount = lastTime / totalTime;
            Debug.Log("UIReady.Countdown lastTime = " + lastTime + " totalTime = " + totalTime);
            if (countdownImage.fillAmount <= 0)
            {
                Scheduler.Instance.Stop("UIReady.Countdown");
            }
        });
        //第几回合
        transform.Find("Countdown/Text").GetComponent<Text>().text = Language.GetTextByKey(InningsTextKey[readyInfo.Innings]);
        //展示要随机的玩法
        playViewList.RegisterInitCallback((Transform obj, int index) =>
        {
            int playID = readyInfo.RandPlay[index];
            //获取配置
            play_data playData = PlayDataConfig.Instance.GetDataByID(playID);
            //icon
            Function.SetImageSprite(obj.Find("Image").GetComponent<Image>(), playData.Icon);
            //名字
            obj.Find("Name").GetComponent<Text>().text = playData.Name;
        });
        //刷新列表
        playViewList.totalCount = readyInfo.RandPlay.Count;
        playViewList.RefillCells();
    }

    public override void OnClose()
    {
        base.OnClose();
        Scheduler.Instance.Stop("UIReady.Countdown");
    }
}
