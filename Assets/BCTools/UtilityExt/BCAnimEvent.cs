using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
public class BCAnimEvent : MonoBehaviour
{
	[System.Serializable]
	public class BCAnimData
	{
		public string tag;
		public UnityEvent unievent = new UnityEvent(); 
	}
	[SerializeField]
	List<BCAnimData> animEveList = new List<BCAnimData>();
	public void DoEventByTag(string tag)
	{
		for(int i=0;i<animEveList.Count;i++)
		{
			if(animEveList[i] == null)continue;
			if(string.Equals(animEveList[i].tag,tag))
			{
				animEveList[i].unievent.Invoke();
				break;
			}
		}
	}
}
