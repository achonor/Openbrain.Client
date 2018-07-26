using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UITable : UIBase {

    protected GameObject homeUI = null;
    protected GameObject levelUI = null;
    protected GameObject friendUI = null;
    protected GameObject taskUI = null;
    protected GameObject moreUI = null;

    private void Awake()
    {
        //按钮事件
        EventTrigger.Get(transform.Find("ButtonList/Home").gameObject).onClick = ClickHome;
        EventTrigger.Get(transform.Find("ButtonList/Level").gameObject).onClick = ClickLevel;
        EventTrigger.Get(transform.Find("ButtonList/Friend").gameObject).onClick = ClickFriend;
        EventTrigger.Get(transform.Find("ButtonList/Task").gameObject).onClick = ClickTask;
        EventTrigger.Get(transform.Find("ButtonList/More").gameObject).onClick = ClickMore;
    }

    public override void OnOpen()
    {
        //默认打开主UI
        ClickHome();
    }

    public void ClickHome(GameObject go = null)
    {
        HideAllUI();
        if (null != this.homeUI)
        {
            this.homeUI.SetActive(true);
            return;
        }
        UIManager.OpenUI("Prefabs/MainUI", UIManager.Instance.GameUIRoot, (GameObject obj) =>
        {
            this.homeUI = obj;
            Debug.Log("UITable.OnOpen Open MainUI Success!");
        });
    }
    public void ClickLevel(GameObject go)
    {
        TipsManager.ShowTips(Language.GetTextByKey(1));
    }
    public void ClickFriend(GameObject go)
    {
        TipsManager.ShowTips(Language.GetTextByKey(1));
    }
    public void ClickTask(GameObject go)
    {
        TipsManager.ShowTips(Language.GetTextByKey(1));
    }
    public void ClickMore(GameObject go)
    {
        TipsManager.ShowTips(Language.GetTextByKey(1));
    }


    //隐藏所有UI
    protected void HideAllUI()
    {
        if(null != this.homeUI)
        {
            this.homeUI.SetActive(false);
        }
        if (null != this.levelUI)
        {
            this.levelUI.SetActive(false);
        }
        if (null != this.friendUI)
        {
            this.friendUI.SetActive(false);
        }
        if (null != this.taskUI)
        {
            this.taskUI.SetActive(false);
        }
        if (null != this.moreUI)
        {
            this.moreUI.SetActive(false);
        }
    }
}
