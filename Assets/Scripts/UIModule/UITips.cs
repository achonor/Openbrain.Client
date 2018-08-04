using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITips : UIBase {

    private Transform imgTran;
    private Vector3 imageInitPosition;
    public override void OnOpen()
    {
        imgTran = base.transform.Find("Image");
        imageInitPosition = imgTran.localPosition;
    }

    public void StartShow(TipsData tips)
    {
        Text objText = base.transform.Find("Image/Text").GetComponent<Text>();
        objText.text = tips.content;
        objText.color = tips.color;

        //move
        var move = DOTween.To(() => imgTran.localPosition, (value) => imgTran.localPosition = value, imgTran.localPosition + new Vector3(0, 100, 0), tips.stayTime);
        //image alpha
        Image objImage = imgTran.GetComponent<Image>();
        var imageAlpha1 = DOTween.ToAlpha(() => objImage.color, (value) => objImage.color = value, 1, (tips.stayTime * 0.7f));
        var imageAlpha2 = DOTween.ToAlpha(() => objImage.color, (value) => objImage.color = value, 0, (tips.stayTime * 0.3f));
        var imageAlphaSequence = DOTween.Sequence();
        imageAlphaSequence.Append(imageAlpha1);
        imageAlphaSequence.Append(imageAlpha2);

        //text alpha
        var textAlpha1 = DOTween.ToAlpha(() => objText.color, (value) => objText.color = value, 1, (tips.stayTime * 0.7f));
        var textAlpha2 = DOTween.ToAlpha(() => objText.color, (value) => objText.color = value, 0, (tips.stayTime * 0.3f));
        var textAlphaSequence = DOTween.Sequence();
        textAlphaSequence.Append(textAlpha1);
        textAlphaSequence.Append(textAlpha2);
    }
    public override void OnClose()
    {
        imgTran.localPosition = imageInitPosition;
    }
}
