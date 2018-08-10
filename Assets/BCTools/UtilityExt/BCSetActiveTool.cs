using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Control Several GameObjects Active or not,or reverse
/// </summary>

public class BCSetActiveTool : MonoBehaviour
{
	[Serializable]
	public class SetActiveData
	{
		public GameObject go;
		public bool setActive = true;
	}
	public List<SetActiveData> list = new List<SetActiveData>();
	public bool Reverse = false;

	public void SetReverseTrue()
	{
		Reverse = true;
	}

	public void SetReverseFalse()
	{
		Reverse = false;
	}

	[ContextMenu("Test DoSetActive()")]
	public void DoSetActive()
	{
		for(int i=0;i<list.Count;i++)
		{
			if(list[i].go != null)
			{
				list[i].go.SetActive(list[i].setActive ^ Reverse);
			}
		}
	}

	public void DoSetActiveForward()
	{
		SetReverseFalse();
		DoSetActive();
	}

	public void DoSetActiveReverse()
	{
		SetReverseTrue();
		DoSetActive();
	}

	//这个函数适合在代码中调用 ，不改变预设！！
	public void setOneActiveOnly(int index)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (i == index && list[i].setActive != null)
			{
				list[i].go.SetActive(true);
			}
			else
				list[i].go.SetActive(false);
		}
	}
}
