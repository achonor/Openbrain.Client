using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Tween动画基类

public class TweenBase : MonoBehaviour {

    //是否需要显示的时候播放
    public bool enablePlay = false;
    //持续时间
    public float duration = 0;

    protected RectTransform _rectTransform = null;
    protected RectTransform rectTransform
    {
        get
        {
            if (null == _rectTransform)
            {
                _rectTransform = transform.GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    void OnEnable()
    {
        if (true == enablePlay)
        {
            Init();
            Play();
        }
    }
    protected virtual void Reset() { }
    public virtual void Init() { }
    public virtual Tweener Play(System.Action callback = null) { return null; }
    public virtual Tweener ReversePlay(System.Action callback = null) { return null; }
}
