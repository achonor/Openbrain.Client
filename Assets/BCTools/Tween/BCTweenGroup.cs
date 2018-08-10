using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Tween some tweener together.
/// </summary>

public class BCTweenGroup : MonoBehaviour
{
	public List<BCUITweener> tweenGroup = new List<BCUITweener>();
	public void PlayForce(bool forward)
	{
		for (int i = 0; i < tweenGroup.Count; i++)
		{
			if (tweenGroup[i] != null && tweenGroup[i].gameObject.activeSelf)
			{
				if (forward)
					tweenGroup[i].PlayForwardForce();
				else
					tweenGroup[i].PlayReverseForce();
			}
		}
	}

    public void PlayForceNextFrame(bool forward)
    {
        for (int i = 0; i < tweenGroup.Count; i++)
        {
            if (tweenGroup[i] != null && tweenGroup[i].gameObject.activeSelf)
            {
                if (forward)
                    tweenGroup[i].PlayForwardForceNextFrame();
                else
                    tweenGroup[i].PlayReverseForceNextFrame();
            }
        }
    }

	public void StopAll()
	{
		for (int i = 0; i < tweenGroup.Count; i++)
		{
			if (tweenGroup[i] != null)
			{
				tweenGroup[i].Stop();
			}
		}
	}
    public void StopAllAndReset()
    {
        for (int i = 0; i < tweenGroup.Count; i++)
        {
            if (tweenGroup[i] != null)
            {
                tweenGroup[i].Stop();
                tweenGroup[i].SetCurrentValueToStart();
            }
        }
    }
	public void PlayForwardForce()
	{
		PlayForce(true);
	}
	public void PlayReverseForce()
	{
		PlayForce(false);
	}
    public void PlayForwardNextFrame()
    {
        PlayForceNextFrame(true);
    }
    public void PlayReverseNextFrame()
    {
        PlayForceNextFrame(false);
    }

	[ContextMenu("Assume value of 'From'")]
	public void SetCurrentValueToStart()
	{
		for (int i = 0; i < tweenGroup.Count; i++)
		{
			if (tweenGroup[i] == null) continue;
			tweenGroup[i].SetCurrentValueToStart();
		}
	}

	[ContextMenu("Assume value of 'To'")]
	public void SetCurrentValueToEnd()
	{
		for (int i = 0; i < tweenGroup.Count; i++)
		{
			if (tweenGroup[i] == null) continue;
			tweenGroup[i].SetCurrentValueToEnd();
		}
	}
}
