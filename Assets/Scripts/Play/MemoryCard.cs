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
    //所有色块状态
    private Dictionary<Transform, bool> blockState = new Dictionary<Transform, bool>();
    //配置表数据
    private play_data playData = null;
    //当前是第几个问题
    private int problemIdx = -1;


    //显示的色块数量
    private int showBlockNumber
    {
        get
        {
            int retCount = 0;
            foreach(var block in blockList)
            {
                if (block.gameObject.activeSelf)
                {
                    retCount++;
                }
            }
            return retCount;
        }
    }

    private void Awake()
    {
        mMask = transform.Find("Mask").gameObject;
        mContent = transform.Find("Content");

        //遮罩事件
        EventTrigger.Get(mMask).onClick = (obj) => {
            //那所有色块反过来
            SetAllBlockState(true);
            //隐藏遮罩
            mMask.SetActive(false);
        };

        //获取所有色块
        for (int idx = 0; idx < mContent.childCount; idx++)
        {
            Transform tmpBlock = mContent.Find(idx.ToString());
            //先隐藏吧
            //tmpBlock.gameObject.SetActive(false);
            blockList.Add(tmpBlock);
            blockState.Add(tmpBlock, false);
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
        //初始化数据
        problemIdx = -1;
    }

    protected override void CreateProblem()
    {
        //打开遮罩
        mMask.SetActive(true);
        //问题下标加一
        problemIdx = Mathf.Clamp(problemIdx + 1, 0, playData.Param1.Count - 1);
        //问题难度
        int problemLevel = (int)playData.Param1[problemIdx];
        //隐藏所有色块
        foreach(var block in blockList)
        {
            block.gameObject.SetActive(false);
            SetBlockState(block.gameObject, false);
        }
        //通过难度随机出要显示的色块
        int[] showIdx = Function.RandInRange(0, 8, problemLevel << 1);
        //随机要显示的几个符号
        string[] allText = new string[playData.Param4.Count];
        playData.Param4.CopyTo(allText, 0);
        string[] showText = Function.RandInRange<string>(allText, problemLevel);
        //填入符号
        for(int i = 0; i< showIdx.Length; i++)
        {
            Transform block = blockList[showIdx[i]];
            block.gameObject.SetActive(true);
            //填入符号
            block.Find("Text").GetComponent<Text>().text = showText[i >> 1];
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
        if (showBlockNumber < 2)
        {
            //重新创建问题
            CreateProblem();
        }
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
    private void ClickBlock(GameObject objBlock, int index)
    {
        SetBlockState(objBlock, !blockState[objBlock.transform], ()=> {
            //判断是否正确
            List<Transform> faceBlock = new List<Transform>();
            foreach(var block in blockList)
            {
                if (block.gameObject.activeSelf && false == blockState[block])
                {
                    faceBlock.Add(block);
                }
            }
            if (1 < faceBlock.Count)
            {
                //算一次回答
                bool isSuccess = true;
                string lastText = null;
                foreach(var block in faceBlock)
                {
                    string blockText = block.Find("Text").GetComponent<Text>().text;
                    if (null != lastText && lastText != blockText)
                    {

                        //回答错误
                        isSuccess = false;
                        break;
                    }
                    lastText = blockText;
                }
                foreach (var block in faceBlock)
                {
                    if(true == isSuccess)
                    {
                        block.gameObject.SetActive(false);
                    }
                    else
                    {
                        SetBlockState(block.gameObject, true);
                    }
                }
                if (true == isSuccess)
                {
                    AnswerSuccess();
                }
                else
                {
                    AnswerFaild();
                }
            }
        });
    }

    /// <summary>
    /// 设置色块状态
    /// </summary>
    /// <param name="objBlock"></param>
    /// <param name="value">true.显示背面，false.显示符号</param>
    public void SetBlockState(GameObject objBlock, bool value, System.Action callback = null)
    {
        System.Action _callback = () =>{
            blockState[objBlock.transform] = value;
            if (null != callback)
            {
                callback();
            }
        };
        GameObject objMask = objBlock.transform.Find("Mask").gameObject;
        Rotation rotation = objBlock.transform.GetComponent<Rotation>();
        if (true == value)
        {
            objMask.SetActive(false);
            Tweener tween = rotation.Play(() =>
            {
                rotation.Init();
                _callback();
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
            objMask.SetActive(true);
            Tweener tween = rotation.ReversePlay(() =>
            {
                _callback();
            });
            tween.OnUpdate(() =>
            {
                if (objBlock.transform.localRotation.eulerAngles.y <= 90)
                {
                    objMask.SetActive(false);
                }
            });
        }
    }

    //设置所有色块状态
    private void SetAllBlockState(bool value)
    {
        //所有色块反过来
        foreach (var block in blockList)
        {
            SetBlockState(block.gameObject, value);
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
