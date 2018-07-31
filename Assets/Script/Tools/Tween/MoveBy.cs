using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveBy : TweenBase
{
    //需要Move的值
    public Vector2 moveValue;
    private Vector2 initPosition;
    private Vector2 endPosition
    {
        get
        {
            return initPosition + moveValue;
        }
    }
    protected Vector2 anchorPosition;

    void Awake () {
        initPosition = rectTransform.anchoredPosition;
    }

    protected override void Reset(){}

    public override void Init()
    {
        rectTransform.anchoredPosition = initPosition;
    }
    public override Tweener Play(System.Action callback = null)
    {
        var tweenTo = DOTween.To(() => rectTransform.anchoredPosition, (value) => rectTransform.anchoredPosition = value, endPosition, duration);
        if (null != callback)
        {
            tweenTo.OnComplete(()=>{
                callback();
            });
        }
        return tweenTo;
    }
}
