using UnityEngine;
using System;
using System.Collections.Generic;
/// <summary>
/// These curves maybe useful when you create tweener dynamically
/// </summary>

public class BCDicOfCurve : MonoBehaviour
{
	[Serializable]
	public class CurveData
	{
		public string tag;
		public string Desc;
		public AnimationCurve curve = new AnimationCurve();
	}
	public List<CurveData> CurveList = new List<CurveData>();
	public AnimationCurve GetCurveByTag(string tag)
	{
		for (int i = 0; i < CurveList.Count; i++)
		{
			if (tag.Equals(CurveList[i].tag))
			{
				return CurveList[i].curve;
			}
		}
		return null;
	}
}
