using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveTo : MonoBehaviour {

    //是否需要显示的时候播放
    public bool enablePlay = false;
    //MoveTo的位置
    public Vector3 endPosition = Vector3.zero;
    //持续时间
    public float duration = 0;

    private Vector3 initPosition;

	// Use this for initialization
	void Start () {
        initPosition = transform.localPosition;
    }

    void OnEnable()
    {
        if (true == enablePlay)
        {
            Init();
            Play();
        }
    }
    private void Reset()
    {
        endPosition = transform.localPosition;
    }

    public void Init()
    {
        transform.localPosition = initPosition;
    }

    public void Play()
    {
        DOTween.To(() => transform.localPosition, (value) => transform.localPosition = value, endPosition, duration);
    }
}
