using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
//这个脚本用来控制GridLayout、RectTransform保持预设的宽高比，使用时需配合基本的锚点设置
//Width_Height_Original 这个值，请务必使用点击齿轮Set Original Value来设置！！！
public class UGUIAspectRatio : MonoBehaviour
{
    public enum ChangeType
    {
        None,
        Horizontal,
        Vertical
    }
	[SerializeField]
	private Vector2 Width_Height_Original = Vector2.one;
    public ChangeType changeType;
	private RectTransform rtran;
	public RectTransform Rtran
	{
		get{
			if(rtran == null)
			{
				rtran = transform as RectTransform;
			}
			return rtran;
		}
	}
	[ContextMenu("Set Original Value")]
	void SetOriginalValue()
	{
		if(this.GetComponent<GridLayoutGroup>() != null)
		{
			Width_Height_Original = getCurrentScreenSize();
		}
		else
		{
			Width_Height_Original = new Vector2(Rtran.rect.width,rtran.rect.height);
		}

        if(Width_Height_Original.x == 0)Width_Height_Original.x = 0.1f; 
        if(Width_Height_Original.y == 0)Width_Height_Original.y = 0.1f;
	}
	[ContextMenu("Test/Set Aspect Ratio")]
	void Start()
	{
		GridLayoutGroup grid;
		Vector2 size = new Vector2(Rtran.rect.width,rtran.rect.height);
		if(changeType == ChangeType.None)
		{
			return;
		}
		else if((grid = this.GetComponent<GridLayoutGroup>()) != null)
		{
			float ratio = getCurrentScreenSize().x / Width_Height_Original.x;
			if(ratio != 1f)
			{
				grid.cellSize *= ratio;
				grid.spacing *= ratio;
			}
		}
		else if(changeType == ChangeType.Horizontal)
		{
			size.x = size.y * Width_Height_Original.x / Width_Height_Original.y;
			Rtran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size.x);
		}
		else if(changeType == ChangeType.Vertical)
		{
			size.y = size.x * Width_Height_Original.y / Width_Height_Original.x;
			Rtran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,size.y);
		}
	}

	Vector2 getCurrentScreenSize()
	{
		Vector2 rt = Vector2.one;
		Canvas cvs = this.GetComponentInParent<Canvas>()as Canvas;
		if(cvs != null)
		{
			Rect rct = (cvs.transform as RectTransform).rect;
			rt.x = rct.width;
			rt.y = rct.height;
		}
		return rt;
	}
}
