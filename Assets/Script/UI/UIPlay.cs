using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlay : UIBase {

    private rep_message_start_game playInfo;
    /// <summary>
    /// 这是第几局
    /// </summary>
    private int innings = 0;
    /// <summary>
    /// 当前界面状态 0.介绍阶段，1.游戏阶段
    /// </summary>
    private int state = 0;
    /// <summary>
    /// 当前倒计时时间
    /// </summary>
    private double countdownTime = 0;
    /// <summary>
    /// 自己的分数
    /// </summary>
    private int leftGrade = 0;
    /// <summary>
    /// 对手的分数
    /// </summary>
    private int rightGrade = 0;

    private Transform introUI;
    private Transform playUI;
    private PlayBase playBase;
    private Text leftGradeText;
    private Text rightGradeText;
    private Image sliderImage;

    private void Awake()
    {
        introUI = transform.Find("Intro");
        playUI = transform.Find("Play");
        leftGradeText = transform.Find("Grade/LeftText").GetComponent<Text>();
        rightGradeText = transform.Find("Grade/RightText").GetComponent<Text>();
        sliderImage = transform.Find("Grade/Mask/Image").GetComponent<Image>();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        //重设分数
        leftGrade = 0;
        rightGrade = 0;
        SetGrade(0, 0);
        //注册对手分数变更事件
        UserEventManager.RegisterEvent("rep_message_updata_opponent_grade", (param) =>
        {
            rep_message_updata_opponent_grade repMsg = Client.Deserialize(rep_message_updata_opponent_grade.Parser, (byte[])param) as rep_message_updata_opponent_grade;
            SetGrade(0, repMsg.Grade - rightGrade);
        });

        //注册局结束事件
        UserEventManager.RegisterEvent("rep_message_innings_end", (param) =>
        {
            rep_message_innings_end repMsg = Client.Deserialize(rep_message_innings_end.Parser, (byte[])param) as rep_message_innings_end;
            if (repMsg.HasInnings)
            {
                CommonRequest.ReqSatrtReady();
            }
        });
        //注册游戏结束事件
        UserEventManager.RegisterEvent("rep_message_game_end", (param) =>
        {
            rep_message_game_end repMsg = Client.Deserialize(rep_message_game_end.Parser, (byte[])param) as rep_message_game_end;
            UIManager.OpenUI("Prefabs/GameEndUI", UIManager.Instance.GameUIRoot, (uiObj) =>
            {
                UIGameEnd uiGameEnd = uiObj.transform.GetComponent<UIGameEnd>();
                uiGameEnd.RefreshUI(repMsg);
                //关闭UIPlay
                this.Close();
            });
        });
    }

    public void RefreshUI(int _innings, rep_message_start_game _playInfo)
    {
        this.innings = _innings;
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
        play_data playData = PlayDataConfig.Instance.GetDataByID(this.playInfo.PlayId);
        UIManager.OpenUI(playData.PrefabPath, playUI, (uiObj) =>
        {
            playBase = uiObj.transform.GetComponent<PlayBase>();
            playBase.answerFinish = AnswerFinish;
        });
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
            playBase.StartPlay();
        }
    }

    private void SetGrade(int _addLeftGrade, int _addRightGrade)
    {
        leftGrade += _addLeftGrade;
        if (leftGrade < 0) leftGrade = 0;
        rightGrade += _addRightGrade;
        if (rightGrade < 0) rightGrade = 0;
        leftGradeText.text = leftGrade.ToString();
        rightGradeText.text = rightGrade.ToString();

        float sumGrade = leftGrade + rightGrade;
        //设置UI
        if (0 == sumGrade)
        {
            sliderImage.fillAmount = 0.5f;
        }
        else
        {
            sliderImage.fillAmount = leftGrade / sumGrade;
        }
    }
    
    /// <summary>
    /// 每次回答完成回调
    /// </summary>
    /// <param name="isOK">是否正确</param>
    /// <param name="addGrade">增加的分数</param>
    public void AnswerFinish(bool isOK, int addGrade)
    {
        //同步分数
        req_message_updata_grade rProto = new req_message_updata_grade();
        rProto.Innings = innings;
        rProto.AddValue = addGrade;
        Client.Instance.Request(rProto, null, false);
        //更新界面分数
        SetGrade(addGrade, 0);
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
        Scheduler.Instance.CreateScheduler("UIPlay.SetCountdownTime", 0, 0, 1.0f, (param) =>
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
                    playBase.Close();
                }
            }
        });
    }

    public override void OnClose()
    {
        base.OnClose();
        if (null != playBase)
        {
            playBase.Close();
        }
        Scheduler.Instance.Stop("UIPlay.SetCountdownTime");
        UserEventManager.UnRegisterEvent("rep_message_updata_opponent_grade");
        UserEventManager.UnRegisterEvent("rep_message_innings_end");
    }
}
