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
        Debug.Log("UIReady.RefreshUI readyInfo = " + readyInfo);
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
        Scheduler.Instance.CreateScheduler("UIReady.Countdown", 0, 0, 0.01f, (param) =>
        {
            float lastTime = (float)(readyInfo.StartTime - Function.GetServerTime());
            countdownImage.fillAmount = lastTime / totalTime;
            Debug.Log("UIReady.Countdown lastTime = " + lastTime + " totalTime = " + totalTime);
            if (countdownImage.fillAmount <= 0)
            {
                Scheduler.Instance.Stop("UIReady.Countdown");
                //请求开始
                CommonRequest.ReqSatrtGame(readyInfo.Innings, (repMsg)=> {
                    //关闭UI
                    //this.Close();
                });
            }
        });
        //第几回合
        transform.Find("Countdown/Text").GetComponent<Text>().text = Language.GetTextByKey(InningsTextKey[readyInfo.Innings]);
        //展示要随机的玩法
        playViewList.RegisterInitCallback((Transform obj, int index) =>
        {
            int playID = readyInfo.RandPlayId[index];
            //获取配置
            play_data playData = PlayDataConfig.Instance.GetDataByID(playID);
            //icon
            Function.SetImageSprite(obj.Find("Icon/Image").GetComponent<Image>(), playData.Icon);
            //名字
            obj.Find("Name").GetComponent<Text>().text = playData.Name;
        });
        //刷新列表
        playViewList.totalCount = readyInfo.RandPlayId.Count;
        playViewList.RefillCells();
        //随机选中玩法
        int randCount = 0;
        Transform randRoot = transform.Find("RandPlay/Viewport/Content");
        Scheduler.Instance.CreateScheduler("UIReady.RandPlay", 0, 0, 0.3f, (param) => {
            for (int idx = 0; idx < randRoot.childCount; idx++)
            {
                randRoot.GetChild(idx).Find("Icon/Choose").gameObject.SetActive(idx == randCount);
            }
            if (readyInfo.RandPlayId[randCount] == readyInfo.PlayId && readyInfo.StartTime - Function.GetServerTime() < 1.0)
            {
                //结束定时器
                Scheduler.Instance.Stop("UIReady.RandPlay");
                //播放一个放大效果
                ScaleTo scaleTo = randRoot.GetChild(randCount).Find("Icon").GetComponent<ScaleTo>();
                scaleTo.Play();

            }
            randCount = (randCount + 1) % readyInfo.RandPlayId.Count;
        });
    }

    public override void OnClose()
    {
        base.OnClose();
        Scheduler.Instance.Stop("UIReady.Countdown");
        Scheduler.Instance.Stop("UIReady.RandPlay");
    }
}
