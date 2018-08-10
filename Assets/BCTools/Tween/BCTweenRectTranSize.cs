using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BCTweenRectTranSize : BCUITweener
{
	[HideInInspector]
	public int sizeMode = 0;//0:height,1:width
	[HideInInspector]
	public float from;
	[HideInInspector]
	public float to;
	[HideInInspector]
	public RectTransform mTran;
	public RectTransform cachedTransform { get { if (mTran == null) mTran = transform as RectTransform; return mTran; } }
	public float value
	{
		get
		{
			if (cachedTransform != null)
			{
				if (sizeMode == 0)
				{
					return cachedTransform.rect.height;
				}
				else if (sizeMode == 1)
				{
					return cachedTransform.rect.width;
				}
			}
			return 0f;
		}
		set
		{
			if (cachedTransform != null)
			{
				if (sizeMode == 0)
				{
					cachedTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
				}
				else if (sizeMode == 1)
				{
					cachedTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
				}
			}
		}
	}

	/// <summary>
	/// Tween the value.
	/// </summary>
	protected override void OnUpdate (float factor, bool isFinished) 
    {
		value = (from * (1f-factor)) + (to * factor);
    }

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue() { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue() { to = value; }

	[ContextMenu("Assume value of 'From'")]
	public override void SetCurrentValueToStart() { value = from; }

	[ContextMenu("Assume value of 'To'")]
	public override void SetCurrentValueToEnd() { value = to; }
}
