using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Flashing : TweenBase
{
    public float endAlpha = 0;
    public Vector3 endScale;

    public Image image;
    private Vector3 initScale;
    private float initAlpha;
    private bool played = false;

    protected override void Reset(){
        image = transform.GetComponent<Image>();
    }

    public override void Init(){
        if (!played)
        {
            return;
        }
        //transform.localScale = initScale;
        //image.color = new Color(image.color.r, image.color.b, image.color.g, initAlpha);
    }

    public override void Play(System.Action callback = null)
    {
        Image image = transform.GetComponent<Image>();
        initScale = transform.localScale;
        initAlpha = image.color.a;
        played = true;

        image.DOFade(endAlpha, duration);
        var tween = transform.DOScale(endScale, duration);
        if (null != callback)
        {
            tween.OnComplete(() =>
            {
                callback();
            });
        }
    }
    public void ReversePlay(System.Action callback = null)
    {
        Image image = transform.GetComponent<Image>();
        initScale = transform.localScale;
        initAlpha = image.color.a;
        played = true;

        image.color = new Color(image.color.r, image.color.g, image.color.b, endAlpha);
        transform.localScale = endScale;

        image.DOFade(initAlpha, duration);
        var tween = transform.DOScale(initScale, duration);
        if (null != callback)
        {
            tween.OnComplete(() =>
            {
                callback();
            });
        }
    }
}
