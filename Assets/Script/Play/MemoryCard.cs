using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//记忆翻牌
public class MemoryCard : PlayBase {
    private GameObject mMask;
    private Transform mContent;
    private List<Transform> blockList = new List<Transform>();
    //配置表数据
    private play_data playData = null;
    //当前是第几个问题
    private int problemIdx = -1;

    private void Awake()
    {
        mMask = transform.Find("Mask").gameObject;
        mContent = transform.Find("Content");

        //获取所有色块
        for (int idx = 0; idx < mContent.childCount; idx++)
        {
            Transform tmpBlock = mContent.Find(idx.ToString());
            //先隐藏吧
            //tmpBlock.gameObject.SetActive(false);
            blockList.Add(tmpBlock);
            //注册回调函数
            EventTrigger.Get(tmpBlock.gameObject).onClick = (obj) =>
            {
                ClickBlock(obj, blockList.IndexOf(obj.transform));
            };
        }
        //读取配置表数据
        //playData = PlayDataConfig.Instance.GetDataByID(GetPlayID());
    }

    public override void OnOpen()
    {
        base.OnOpen();
        //初始化数据
        problemIdx = -1;
    }

    protected override void CreateProblem()
    {
        //问题下标加一
        problemIdx = Mathf.Clamp(problemIdx + 1, 0, playData.Param1.Count - 1);
        //问题难度
        int problemLevel = (int)playData.Param1[problemIdx];
        //打开遮罩防止点击
        mMask.SetActive(true);
        //隐藏所有色块
        foreach(var block in blockList)
        {
            block.gameObject.SetActive(false);
        }
    }
    

    //回答正确
    private void AnswerSuccess()
    {
        //统计分数
        int addGrade = (int)playData.Param2[problemIdx];
        if (null != answerFinish)
        {
            answerFinish(true, addGrade);
        }

        //重新创建问题
        CreateProblem();
    }

    //回答错误
    private void AnswerFaild()
    {
    }

    /// <summary>
    /// 点击色块的回调
    /// </summary>
    /// <param name="objBlock">色块的GameObject</param>
    /// <param name="index">这是第几个色块</param>
    int clickCount = 0;
    private void ClickBlock(GameObject objBlock, int index)
    {
        SetBlockState(objBlock, clickCount++ % 2 == 0);
    }

    public void SetBlockState(GameObject objBlock, bool value)
    {
        GameObject objMask = objBlock.transform.Find("Mask").gameObject;
        Rotation rotation = objBlock.transform.GetComponent<Rotation>();
        if (true == value)
        {
            Tweener tween = rotation.Play(() =>
            {
                rotation.Init();
            });
            tween.OnUpdate(() =>
            {
                if (90 <= objBlock.transform.localRotation.eulerAngles.y)
                {
                    objMask.SetActive(true);
                }
            });
        }
        else
        {
            Tweener tween = rotation.ReversePlay(() =>
            {});
            tween.OnUpdate(() =>
            {
                if (objBlock.transform.localRotation.eulerAngles.y <= 90)
                {
                    objMask.SetActive(false);
                }
            });
        }
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public override int GetPlayID()
    {
        return 2;
    }
}
