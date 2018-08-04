using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩法基类

public class PlayBase : UIBase {

    //回答完成回调
    public System.Action<bool, int> answerFinish;

    /// <summary>
    /// 开始玩？
    /// </summary>
    public virtual void StartPlay() {
        CreateProblem();
    }

    /// <summary>
    /// 创建一个问题
    /// </summary>
    protected virtual void CreateProblem()
    {
    }
    /// <summary>
    /// 获取玩法的id
    /// </summary>
    /// <returns>ID</returns>
    public virtual int GetPlayID()
    {
        return -1;
    }
}
