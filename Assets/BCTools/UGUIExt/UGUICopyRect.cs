using UnityEngine;
using UnityEngine.UI;
using System;
//复制自身的RectTransform的宽高信息给TargetTran，编辑状态实时可见
[ExecuteInEditMode]
public class UGUICopyRect : MonoBehaviour
{
	[HelpBox(2,"The target transform will be modified by copy from this transform")]
	public RectTransform targetTran;
	public bool fitHorizontal = true;
	public float minWidth = 0f;
	public bool fitVertical = true;
	public float minHeight = 0f;
	public bool fitPosition = false;
	[Int2Enum("localPosition,WorldPosition")]
	public int posMode = 0;
	RectTransform m_myTran;
	RectTransform myTran
	{
		get
		{
			if(m_myTran == null)
				m_myTran = this.transform as RectTransform;
			return m_myTran;
		}
	}

	void Update()
	{
		if (targetTran == null || myTran == null) return;
		Rect rect = myTran.rect;
		if (fitHorizontal)
			targetTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,Mathf.Max(rect.width,minWidth));
		if(fitVertical)
			targetTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(rect.height,minHeight));
		if (fitPosition)
		{
			if (posMode == 0)
			{
				targetTran.localPosition = transform.localPosition;
			}
			else
			{
				targetTran.position = transform.position;
			}
		}
	}
}
