using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlay : UIBase {

    private rep_message_start_game playInfo;
    /// <summary>
    /// 当前界面状态 0.介绍阶段，1.游戏阶段
    /// </summary>
    private int state = 0;
    /// <summary>
    /// 当前倒计时时间
    /// </summary>
    private double countdownTime = 0;

    private Transform introUI;
    private Transform playUI;

    private void Awake()
    {
        introUI = transform.Find("Intro");
        playUI = transform.Find("Play");

        //注册按钮事件
    }


    public override void OnOpen()
    {
        base.OnOpen();
    }

    public void RefreshUI(rep_message_start_game _playInfo)
    {
        this.playInfo = _playInfo;
        //初始界面状态
        SetUIState(0);
        //我的头像
        if (!string.IsNullOrEmpty(PlayerData.userIcon))
        {
            var headIcon = transform.Find("PlayerInfo/LeftHeadImage/Mask/Image").GetComponent<Image>();
            StartCoroutine(Function.DownloadImage(headIcon, PlayerData.userIcon));
        }
        //对手头像
        struct_player_info opponentInfo = this.playInfo.PlayerInfo;
        if (!string.IsNullOrEmpty(opponentInfo.UserIcon))
        {
            var headIcon = transform.Find("PlayerInfo/RightHeadImage/Mask/Image").GetComponent<Image>();
            StartCoroutine(Function.DownloadImage(headIcon, opponentInfo.UserIcon));
        }
        //加载玩法prefab

    }

    /// <summary>
    /// 设置界面状态
    /// </summary>
    /// <param name="_state">0.介绍阶段，1.游戏阶段</param>
    private void SetUIState(int _state)
    {
        this.state = _state;
        introUI.gameObject.SetActive(0 == this.state);
        playUI.gameObject.SetActive(1 == this.state);
        if (0 == this.state)
        {
            SetCountdownTime(this.playInfo.IntroEndTime);
            //设置玩法介绍image
            play_data playData = PlayDataConfig.Instance.GetDataByID(this.playInfo.PlayId);
            Function.SetImageSprite(introUI.Find("PlayIntro/Image").GetComponent<Image>(), playData.IntroIcon);
        }
        else
        {
            SetCountdownTime(this.playInfo.EndTime);
            //开始玩
        }
    }
    /// <summary>
    /// 设置倒计时
    /// </summary>
    /// <param name="endTime">倒计时结束时间</param>
    private void SetCountdownTime(double endTime)
    {
        countdownTime = endTime;
        //Text控件
        Text countdownText = transform.Find("PlayerInfo/Countdown").GetComponent<Text>();
        //创建定时器
        Scheduler.Instance.CreateScheduler("UIPlay.SetCountdownTime", 0, 0, 1.0f, () =>
        {
            double lastTime = countdownTime - Function.GetServerTime();
            if (lastTime < 0)
            {
                lastTime = 0;
            }
            countdownText.text = ((int)lastTime).ToString();
            if (lastTime <= 0)
            {
                Scheduler.Instance.Stop("UIPlay.SetCountdownTime");
                if (0 == this.state)
                {
                    SetUIState(1);
                }
                else
                {
                    //结束
                }
            }
        });
    }

    public override void OnClose()
    {
        base.OnClose();
        Scheduler.Instance.Stop("UIPlay.SetCountdownTime");
    }
}
