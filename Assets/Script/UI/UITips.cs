using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITips : UIBase {
    

    public override void OnOpen()
    {

    }

    public void StartShow(TipsData tips)
    {
        Text objText = base.transform.Find("Image/Text").GetComponent<Text>();
        objText.text = tips.content;
        objText.color = tips.color;


        //tween
        RectTransform imgTran = base.transform.Find("Image").GetComponent<RectTransform>();
        var move1 = imgTran.DOLocalMoveY(-200, 1.0f);
    }
}
