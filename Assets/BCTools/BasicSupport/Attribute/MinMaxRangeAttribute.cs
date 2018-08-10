using UnityEngine;
using System.Collections;
/// <summary>
/// My range attribute:use [MyRange(0f,5f)] or [MyRangeAttribute(0f,5f)] will work
/// </summary>
public class MinMaxRangeAttribute : PropertyAttribute 
{
	public float minLimit;
	public float maxLimit;
	public bool useMiddle = false;
	public float middle;
	public MinMaxRangeAttribute(float minLimit,float maxLimit)
	{
		if(maxLimit >= minLimit)
		{
			this.minLimit = minLimit;
			this.maxLimit = maxLimit;
		}
		else
		{
			this.minLimit = maxLimit;
			this.maxLimit = minLimit;
		}
	}
	public MinMaxRangeAttribute(float minLimit,float maxLimit,float middle)
	{
		if(maxLimit >= minLimit)
		{
			this.minLimit = minLimit;
			this.maxLimit = maxLimit;
		}
		else
		{
			this.minLimit = maxLimit;
			this.maxLimit = minLimit;
		}
		if(middle > this.maxLimit || middle < this.minLimit)
		{
			middle = (this.minLimit+this.maxLimit)/2f;
		}
		this.middle = middle;
		this.useMiddle = true;
	}
}
