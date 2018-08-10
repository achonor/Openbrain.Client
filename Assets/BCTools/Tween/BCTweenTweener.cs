using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BCTweenTweener : BCUITweener
{
	public List<BCUITweener> tweenlist = new List<BCUITweener>();
	protected override void OnUpdate (float factor, bool isFinished)
	{
		for (int i = 0; i < tweenlist.Count; i++)
		{
			if (tweenlist[i] == null) continue;
			tweenlist[i].DoOnUpdate(factor,isFinished);
		}
	}

	new public void Start()
	{
		for (int i = 0; i < tweenlist.Count; i++)
		{
			if (tweenlist[i] == null) continue;
			if(tweenlist[i].enabled)
				tweenlist[i].enabled = false;
		}
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue() 
	{
		for (int i = 0; i < tweenlist.Count; i++)
		{
			if (tweenlist[i] == null) continue;
			tweenlist[i].SetStartToCurrentValue();
		}
	}

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		for (int i = 0; i < tweenlist.Count; i++)
		{
			if (tweenlist[i] == null) continue;
			tweenlist[i].SetEndToCurrentValue();
		}
	}

	[ContextMenu("Assume value of 'From'")]
	public override void SetCurrentValueToStart()
	{
		for (int i = 0; i < tweenlist.Count; i++)
		{
			if (tweenlist[i] == null) continue;
			tweenlist[i].SetCurrentValueToStart();
		}
	}

	[ContextMenu("Assume value of 'To'")]
	public override void SetCurrentValueToEnd()
	{
		for (int i = 0; i < tweenlist.Count; i++)
		{
			if (tweenlist[i] == null) continue;
			tweenlist[i].SetCurrentValueToEnd();
		}
	}
}
