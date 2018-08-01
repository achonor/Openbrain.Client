using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//平分秋色
public class EqualColor : PlayBase {
    private Transform mContent3x3;
    private Transform mContent4x4;
    private List<Transform> blockList3x3 = new List<Transform>();
    private List<Transform> blockList4x4 = new List<Transform>();
    private int[] blockID3x3 = new int[9];
    private int[] blockID4x4 = new int[16];

    //配置表数据
    private play_data playData = null;
    //当前是第几个问题
    private int problemIdx = -1;
    //当前难度
    private int problemLevel = 3;
    //所有颜色
    private Color[] allColor;
    //当前问题的答案
    private Color problemResult;

    private void Awake()
    {
        mContent3x3 = transform.Find("Content3x3");
        mContent4x4 = transform.Find("Content4x4");

        //获取所有色块
        for (int idx = 0; idx < mContent3x3.childCount; idx++)
        {
            blockID3x3[idx] = idx;
            Transform tmpBlock = mContent3x3.Find(idx.ToString());
            //先隐藏吧
            blockList3x3.Add(tmpBlock);
            //注册回调函数
            EventTrigger.Get(tmpBlock.gameObject).onClick = (obj) =>
            {
                ClickBlock(3, obj, blockList3x3.IndexOf(obj.transform));
            };
        }
        for (int idx = 0; idx < mContent4x4.childCount; idx++)
        {
            blockID4x4[idx] = idx;
            Transform tmpBlock = mContent4x4.Find(idx.ToString());
            //先隐藏吧
            blockList4x4.Add(tmpBlock);
            //注册回调函数
            EventTrigger.Get(tmpBlock.gameObject).onClick = (obj) =>
            {
                ClickBlock(4, obj, blockList4x4.IndexOf(obj.transform));
            };
        }
        
        //读取配置表数据
        playData = PlayDataConfig.Instance.GetDataByID(GetPlayID());
        //读取所有颜色
        allColor = new Color[playData.Param3.Count / 4];
        for (int i = 0; i < playData.Param3.Count; i += 4)
        {
            allColor[i / 4] = (new Color(playData.Param3[i], playData.Param3[i + 1], playData.Param3[i + 2], playData.Param3[i + 3]) / 255f);
        }
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
        problemLevel = (int)playData.Param1[problemIdx];
        //色块列表
        int[] curBlockID = (3 == problemLevel) ? blockID3x3 : blockID4x4;
        List<Transform> curBlockList = (3 == problemLevel) ? blockList3x3 : blockList4x4;
        //需要多少颜色
        int colorNumber = UnityEngine.Random.Range(2, allColor.Length);
        //颜色划分
        int[] colorDivision = ColorDivision(curBlockList.Count, colorNumber);
        //打乱色块编号
        Function.UpsetArray<int>(curBlockID);
        //打乱颜色
        Function.UpsetArray<Color>(allColor);
        //写入颜色
        int tmpIndex = 0;
        int resultIndex = 0;
        for (int i = 0; i < colorDivision.Length; i++)
        {
            if (colorDivision[resultIndex] < colorDivision[i])
            {
                resultIndex = i;
            }
            for (int k = 0; k < colorDivision[i]; k++)
            {
                Transform tmpBlock = curBlockList[curBlockID[tmpIndex++]];
                tmpBlock.GetComponent<Image>().color = allColor[i];
            }
        }
        problemResult = allColor[resultIndex];
        mContent3x3.gameObject.SetActive(3 == problemLevel);
        mContent4x4.gameObject.SetActive(3 != problemLevel);
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
    private void ClickBlock(int level, GameObject objBlock, int index)
    {
        if (level != problemLevel)
        {
            Debug.LogError("EqualColor.ClickBlock level != problemLevel");
            return;
        }
        Color blockColor = objBlock.transform.GetComponent<Image>().color;
        if (blockColor == problemResult)
        {
            AnswerSuccess();
        }
        else
        {
            AnswerFaild();
        }
    }
    /// <summary>
    /// 颜色划分，保证有个唯一的最大值的前提下尽量平均分配
    /// </summary>
    /// <param name="number">总数</param>
    /// <param name="size">划分多少份</param>
    /// <returns></returns>
    private int[] ColorDivision(int number, int size)
    {
        int[] retResult = new int[size];
        for (int i = 0; i < retResult.Length; i++)
        {
            retResult[i] = number / size;
        }
        if (0 == number % size)
        {
            retResult[0]++;
            retResult[retResult.Length - 1]--;
            return retResult;
        }
        int[] tmpResult = ColorDivision(number % size, size - 1);
        //累加
        for (int i = 0; i < tmpResult.Length; i++)
        {
            retResult[i] += tmpResult[i];
        }
        return retResult;
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public override int GetPlayID()
    {
        return 3;
    }
}
