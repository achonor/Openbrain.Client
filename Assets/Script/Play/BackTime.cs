using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//逆转时光
public class BackTime : PlayBase {

    //红色
    private Color RedBlock = new Color(247, 73, 74, 255) / 255f;
    //蓝色
    private Color BlueBlock = new Color(8, 69, 99, 255) / 255f;

    private GameObject mMask;
    private Transform mContent;
    private List<Transform> blockList = new List<Transform>();

    //配置表数据
    private play_data playData = null;


    //当前是第几个问题
    private int problemIdx = -1;
    //当前问题的答案
    private List<int> problemResult;

    private void Awake()
    {
        mMask = transform.Find("Mask").gameObject;
        mContent = transform.Find("Content");

        //获取所有色块
        for (int idx = 0; idx < mContent.childCount; idx++)
        {
            Transform tmpBlock = mContent.Find(idx.ToString());
            //先隐藏吧
            tmpBlock.gameObject.SetActive(false);
            blockList.Add(tmpBlock);
            //注册回调函数
            EventTrigger.Get(tmpBlock.gameObject).onClick = (obj) =>
            {
                ClickBlock(obj, blockList.IndexOf(obj.transform));
            };
        }
        //读取配置表数据
        playData = PlayDataConfig.Instance.GetDataByID(GetPlayID());
    }

    public override void OnOpen()
    {
        base.OnOpen();

        problemIdx = -1;
    }

    protected override void CreateProblem()
    {
        //问题下标加一
        problemIdx = Mathf.Clamp(problemIdx + 1, 0, playData.Param1.Count - 1);
        //问题难度
        int problemLevel = (int)playData.Param1[problemIdx];
        //随机多个数字
        problemResult = new List<int>(Function.RandInRange(0, 8, problemLevel));
        //打开遮罩防止点击
        mMask.SetActive(true);
        //隐藏所有色块
        foreach(var block in blockList)
        {
            block.gameObject.SetActive(false);
        }
        //按顺序出现色块
        int tmpCount = 0;
        Scheduler.Instance.CreateScheduler("BackTime.CreateProblem1", 0, 0, 0.3f, (param1) =>
        {
            Transform tmpBlock = blockList[problemResult[tmpCount]];
            //蓝色
            tmpBlock.GetComponent<Image>().color = BlueBlock;
            tmpBlock.gameObject.SetActive(true);
            tmpCount++;
            if (tmpCount == problemResult.Count)
            {
                //所有色块都显示了
                Scheduler.Instance.Stop("BackTime.CreateProblem1");
                //过一会让色块变成红色
                Scheduler.Instance.CreateScheduler("BackTime.CreateProblem2", 0, 1, 0.5f, (param2) =>
                {
                    foreach (var idx in problemResult)
                    {
                        blockList[idx].GetComponent<Image>().color = RedBlock;
                    }
                    //取消遮罩
                    mMask.SetActive(false);
                });
            }

        });
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
        //统计分数
        int addGrade = (int)playData.Param3[problemIdx];
        if (null != answerFinish)
        {
            answerFinish(false, -addGrade);
        }

        //重新创建问题
        CreateProblem();
    }

    /// <summary>
    /// 点击色块的回调
    /// </summary>
    /// <param name="objBlock">色块的GameObject</param>
    /// <param name="index">这是第几个色块</param>
    private void ClickBlock(GameObject objBlock, int index)
    {
        if (index == problemResult[problemResult.Count - 1])
        {
            //点击正确
            objBlock.SetActive(false);
            problemResult.RemoveAt(problemResult.Count - 1);
            if (0 == problemResult.Count)
            {
                //回答正确
                AnswerSuccess();
            }
        }
        else
        {
            //点击错误, 直接调用回答错误
            AnswerFaild();
        }
    }


    public override void OnClose()
    {
        base.OnClose();
        Scheduler.Instance.Stop("BackTime.CreateProblem1");
        Scheduler.Instance.Stop("BackTime.CreateProblem2");
    }

    public override int GetPlayID()
    {
        return 0;
    }
}
