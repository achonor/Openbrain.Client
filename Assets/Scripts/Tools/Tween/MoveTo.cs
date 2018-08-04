using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveTo : TweenBase
{
    //MoveTo的位置
    public Vector2 endPosition = Vector2.zero;
    private Vector2 initPosition;
    
	void Awake() {
        initPosition = rectTransform.anchoredPosition;
    }

    protected override void Reset()
    {
        endPosition = rectTransform.anchoredPosition;
    }
    public override void Init()
    {
        rectTransform.anchoredPosition = initPosition;
    }
    public override Tweener Play(System.Action callback = null)
    {
        var tweenTo = DOTween.To(() => rectTransform.anchoredPosition, (value) => rectTransform.anchoredPosition = value, endPosition, duration);
        if (null != callback)
        {
            tweenTo.OnComplete(() =>
            {
                callback();
            });
        }
        return tweenTo;
    }
}
