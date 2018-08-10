using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class UGUICountDown : MonoBehaviour
{
    public string FromTime = "3:0:0";
    public string LimitTime = "0:0:0";
    public bool ShowHour = true;
    [SerializeField] private Text timeTxt;
    #region FillImage
    [SerializeField] private Image FillImage;
    public bool FromIsFull = true;
    #endregion
    public UnityEvent onReachLimit;
    private int fromTime;
    private int limitTime;
    private int currentTime;
    private float secondCounter = 0f;
    void timeStrToInt()
    {
        string[] strFromArr = FromTime.Split(':');
        string[] strLimitArr = LimitTime.Split(':');
        fromTime = 0;
        limitTime = 0;
        if (strFromArr == null || strLimitArr == null || strFromArr.Length != 3 || strLimitArr.Length != 3)
        {
            return;//error
        }
        for (int i = 0; i < 3 ; i++)
        {
            fromTime += int.Parse(strFromArr[2-i]) * (int)(Mathf.Pow(60, i));
            limitTime += int.Parse(strLimitArr[2 - i])*(int) (Mathf.Pow(60, i));
        }
        currentTime = fromTime;
    }

    void refreshTimeStr()
    {
        if (ShowHour)
        {
            timeTxt.text = string.Format("{0:00}:{1:00}:{2:00}", (int) (currentTime/3600), (int) (currentTime%3600)/60,
                currentTime%60);
        }
        else
        {
            timeTxt.text = string.Format("{0:00}:{1:00}", (int) (currentTime/60), currentTime%60);
        }
    }
    public void SetAndPlay(string from, string limit)
    {
        this.FromTime = from;
        this.LimitTime = limit;
        Play();
    }

    public void Play()
    {
        if (!this.enabled)
        {
            this.enabled = true;
        }
        else
        {
            timeStrToInt();
            secondCounter = 0;
            refreshTimeStr();
        }
    }

    void OnEnable()
    {
        timeStrToInt();
        secondCounter = 0;
        refreshTimeStr();
    }

	// Update is called once per frame
	void Update ()
	{
	    secondCounter += Time.deltaTime;
	    if(secondCounter >= 1)
	    {
	        secondCounter = 0;
	        if (fromTime > limitTime)
	        {
	            currentTime--;
	        }
	        else
	        {
	            currentTime++;
	        }
	        refreshTimeStr();
	        if (currentTime == limitTime)
	        {
	            onReachLimit.Invoke();
	            this.enabled = false;
	        }
	    }
	    if (FillImage != null && FillImage.type == Image.Type.Filled)
	    {
	        FillImage.fillAmount = GetPercent();
	    }
	}

    float GetPercent()
    {
        float percent = 0;
        if (fromTime == limitTime)
        {
            return percent;
        }
        else if (fromTime > limitTime) //倒计时
        {
            percent = (fromTime - currentTime + secondCounter)/(fromTime - limitTime);
        }
        else //正计时
        {
            percent = (currentTime + secondCounter - fromTime)/(limitTime - fromTime);
        }

        if (FromIsFull)
        {
            percent = 1 - percent;
        }

        if (percent < 0)
        {
            percent = 0;
        }
        else if (percent > 1)
        {
            percent = 1;
        }

        return percent;
    }
}
