using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsData
{
    public TipsData(string _content, Color _color, float _stayTime = 2.0f)
    {
        content = _content;
        color = _color;
        stayTime = _stayTime;
    }

    public Color color;
    public string content;
    public float stayTime;
}

public class TipsManager {
    public static bool showing = false;
    private static Queue<TipsData> tipsQueue = new Queue<TipsData>();

    public static void ShowTips(string text, object color = null, float stayTime = 2.0f)
    {
        if (null == color)
        {
            color = Color.white;
        }
        tipsQueue.Enqueue(new TipsData(text, (Color)color, stayTime));
        if (false == showing)
        {
            _ShowTips();
        }
    }

    private static void _ShowTips()
    {
        if (0 == tipsQueue.Count)
        {
            showing = false;
            return;
        }
        showing = true;
        TipsData tips = tipsQueue.Dequeue();
        UIManager.OpenUI("Prefabs/TipsUI", UIManager.Instance.TipsUIRoot, (GameObject obj) =>
        {
            UITips uiTips = obj.GetComponent<UITips>();
            uiTips.StartShow(tips);

            Scheduler.Instance.CreateScheduler("tips" + Function.GetServerTime(), tips.stayTime, 1, 0, () => {
                UIManager.CloseUI("Prefabs/TipsUI");
                _ShowTips();
            });
        });
    }
}
