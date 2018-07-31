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
