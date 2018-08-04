using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleTo : TweenBase {

    //ScaleTo的值
    public Vector3 endScale = Vector3.zero;
    private Vector3 initScale;
    
    void Awake()
    {
        initScale = transform.localScale;
    }

    protected override void Reset()
    {
        endScale = transform.localScale;
    }
    public override void Init()
    {
        transform.localScale = initScale;
    }
    public override Tweener Play(System.Action callback = null)
    {
        var tweenTo = DOTween.To(() => transform.localScale, (value) => transform.localScale = value, endScale, duration);
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
