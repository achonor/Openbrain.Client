using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoad : UIBase {

    private int[] LoadTextKey = { 10401, 10402, 10403, 10404 };
    public override void OnOpen()
    {
        base.OnOpen();

        //开始初始化数据
        CommonMethod.InitGame(() =>
        {
            //关闭界面
            this.Close();
        });
        //定时器显示文字
        int runCount = 0;
        Text loadText = transform.Find("InitText").GetComponent<Text>();
        Scheduler.Instance.CreateScheduler("UILoad.LoadData", 0, 0, 0.3f, () =>
        {
            loadText.text = Language.GetTextByKey(LoadTextKey[runCount % LoadTextKey.Length]);
            runCount++;
        });
    }

    public override void OnClose()
    {
        base.OnClose();
        //关闭定时器
        Scheduler.Instance.Stop("UILoad.LoadData");
        //打开登陆UI
        UIManager.OpenUI("Prefabs/LoginUI", UIManager.Instance.GameUIRoot, (GameObject obj) =>
        {
            Debug.Log("GameController.Start Open LoginUI Success!");
        });
    }

}
