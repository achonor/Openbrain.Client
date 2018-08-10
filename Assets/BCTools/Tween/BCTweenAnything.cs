using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class BCTweenAnything : BCUITweener
{
	[HideInInspector]
	public Vector4 from;
	[HideInInspector]
	public Vector4 to;
	[HideInInspector]
	//public List<BCEventDelegate> onUpdateEventList = new List<BCEventDelegate>();
	public UnityEvent onUpdateEvents = new UnityEvent();
	Vector4 mValue = Vector4.zero;
	public bool useUpdate = false;
	public Vector4 value
	{
		get
		{
			return mValue;
		}
		set
		{
			mValue = value;
		}
	}

	/// <summary>
	/// Tween the value.
	/// </summary>
	protected override void OnUpdate (float factor, bool isFinished) 
    {
		value = (from * (1f-factor)) + (to * factor);
		if (useUpdate)
		{
			excuteEvent();
		}
    }

	void excuteEvent()
	{
		onUpdateEvents.Invoke();
		return;
		/*
		if(onUpdateEventList.Count < 1)return;
		List<BCEventDelegate> mTemp = onUpdateEventList;
		onUpdateEventList = new List<BCEventDelegate>();
		// Notify the listener delegates
		BCEventDelegate.Execute(mTemp);
		
		// Re-add the previous persistent delegates
		for (int i = 0; i < mTemp.Count; ++i)
		{
			BCEventDelegate ed = mTemp[i];
			if (ed != null && !ed.oneShot) BCEventDelegate.Add(onUpdateEventList, ed, ed.oneShot);
		}*/
	}
}
