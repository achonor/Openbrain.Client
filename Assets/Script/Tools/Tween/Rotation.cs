using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Rotation : TweenBase
{
    public Vector3 endAngle;
    
    private Quaternion initRotation;
    private bool played = false;

    public override void Init(){
        if (!played)
        {
            return;
        }
        transform.localRotation = initRotation;
    }
    public void SaveInit()
    {
        if (played)
        {
            return;
        }
        played = true;
        initRotation = transform.localRotation;
    }
    /// <summary>
    /// 旋转过程中的事件
    /// </summary>
    /// <param name="type">1.x轴，2.y轴，3.轴</param>
    /// <param name="angle">角度</param>
    /// <param name="callback">回调</param>
    private float lastOffset = 360;
    public void SetEvent(int type, float angle, System.Action callback = null)
    {
        
    }

    public override Tweener Play(System.Action callback = null)
    {
        SaveInit();
        var tween = transform.DORotate(endAngle, duration);
        if (null != callback)
        {
            tween.OnComplete(() =>
            {
                callback();
            });
        }
        return tween;
    }
    public override Tweener ReversePlay(System.Action callback = null)
    {
        SaveInit();
        transform.localRotation = Quaternion.Euler(endAngle);
        var tween = transform.DORotateQuaternion(initRotation, duration);
        if (null != callback)
        {
            tween.OnComplete(() =>
            {
                callback();
            });
        }
        return tween;
    }
}
