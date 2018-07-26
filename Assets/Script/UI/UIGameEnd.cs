using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIGameEnd : UIBase {


    private rep_message_game_end endInfo;

    public override void OnOpen()
    {
        base.OnOpen();
        //显示tableUI
        GameObject uiTable = UIManager.GetUIByName("Prefabs/TableUI");
        if (null != uiTable)
        {
            uiTable.SetActive(true);
        }
        else
        {
            UIManager.OpenUI("Prefabs/TableUI", UIManager.Instance.BaseUIRoot);
        }
    }

    //刷新界面
    public void RefreshUI(rep_message_game_end _endInfo)
    {
        this.endInfo = _endInfo;
        //我的头像
        if (!string.IsNullOrEmpty(PlayerData.userIcon))
        {
            var headIcon = transform.Find("PlayerInfo/LeftHeadImage/Mask/Image").GetComponent<Image>();
            StartCoroutine(Function.DownloadImage(headIcon, PlayerData.userIcon));
        }
        //对手头像
        struct_player_info opponentInfo = this.endInfo.PlayerInfo;
        if (!string.IsNullOrEmpty(opponentInfo.UserIcon))
        {
            var headIcon = transform.Find("PlayerInfo/RightHeadImage/Mask/Image").GetComponent<Image>();
            StartCoroutine(Function.DownloadImage(headIcon, opponentInfo.UserIcon));
        }
        //名字
        transform.Find("PlayerInfo/LeftHeadImage/Name").GetComponent<Text>().text = PlayerData.userName;
        transform.Find("PlayerInfo/RightHeadImage/Name").GetComponent<Text>().text = endInfo.PlayerInfo.UserName;

        //定时器依次展示各局分数
        float leftSum = 0;
        float rightSum = 0;
        for (int i = 0; i < endInfo.LeftGrade.Count; i++)
        {
            leftSum += endInfo.LeftGrade[i];
            rightSum += endInfo.RightGrade[i];
            transform.Find(string.Format("GradeList/{0}/LeftText", i)).GetComponent<Text>().text = endInfo.LeftGrade[i].ToString();
            transform.Find(string.Format("GradeList/{0}/RightText", i)).GetComponent<Text>().text = endInfo.RightGrade[i].ToString();
            Scheduler.Instance.CreateScheduler("UIGameEnd.RefreshUI.ShowGrade." + i, i * 0.5f, 1, 0, (param) =>
            {
                int index = (int)param;
                float inningsSum = endInfo.LeftGrade[index] + endInfo.RightGrade[index];
                //Image
                Image leftIamge = transform.Find(string.Format("GradeList/{0}/Mask/LeftImage", index)).GetComponent<Image>();
                Image rightIamge = transform.Find(string.Format("GradeList/{0}/Mask/RightImage", index)).GetComponent<Image>();
                leftIamge.fillAmount = 0;
                rightIamge.fillAmount = 0;
                DOTween.To(() => leftIamge.fillAmount, (value) => leftIamge.fillAmount = value, (0 == inningsSum) ? 0.5f : endInfo.LeftGrade[index] / inningsSum, 1f);
                DOTween.To(() => rightIamge.fillAmount, (value) => rightIamge.fillAmount = value, (0 == inningsSum) ? 0.5f : endInfo.RightGrade[index] / inningsSum, 1f);
            }, i);
        }
        //总分
        transform.Find(string.Format("GradeList/Sum/LeftText")).GetComponent<Text>().text = leftSum.ToString();
        transform.Find(string.Format("GradeList/Sum/RightText")).GetComponent<Text>().text = rightSum.ToString();
        Image sumLeftIamge = transform.Find(string.Format("GradeList/Sum/Mask/LeftImage")).GetComponent<Image>();
        Image sumRightIamge = transform.Find(string.Format("GradeList/Sum/Mask/RightImage")).GetComponent<Image>();
        sumLeftIamge.fillAmount = 0;
        sumRightIamge.fillAmount = 0;
        float allSum = leftSum + rightSum;
        Scheduler.Instance.CreateScheduler("UIGameEnd.RefreshUI.ShowGrade.Sum", endInfo.LeftGrade.Count * 0.5f, 1, 0, (param) =>
        {
            DOTween.To(() => sumLeftIamge.fillAmount, (value) => sumLeftIamge.fillAmount = value, (0 == allSum) ? 0.5f : leftSum / allSum, 1f);
            DOTween.To(() => sumRightIamge.fillAmount, (value) => sumRightIamge.fillAmount = value, (0 == allSum) ? 0.5f : rightSum / allSum, 1f);
        });
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
