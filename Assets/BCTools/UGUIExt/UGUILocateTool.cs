using UnityEngine;
using UnityEngine.UI;
using System;
/// <summary>
/// 用于定位ScrollRect中的某物到指定位置,该脚本挂在ScrollRect 的 content上
/// </summary>
public class UGUILocateTool : MonoBehaviour
{
	public GridLayoutGroup.Axis m_Mode = GridLayoutGroup.Axis.Horizontal;
	public Transform m_LocateTarget;
	public Transform m_Child;

	//定位
	public void Locate(Transform childtran)
	{
		if (childtran == null || !childtran.IsChildOf(transform))
		{
			return;
		}
		Vector3 vec = m_LocateTarget.position - childtran.position;
		Vector3 tempvec = transform.position + vec;
		if (m_Mode == GridLayoutGroup.Axis.Horizontal)
		{
			tempvec.y = transform.position.y;//keep vitical
		}
		else
		{
			tempvec.x = transform.position.x;//keep horizontal
		}
		transform.position = tempvec;
	}

	[ContextMenu("Locate Test")]
	public void LocateSimple()
	{
		Locate(m_Child);
	}

	public void sortChildSibling(bool small2Big)
	{
		BCTools.SortChildSibling(this.transform, small2Big);
	}

	[ContextMenu("sortChildSiblingSmall2Big")]
	public void sortChildSiblingSmall2Big()
	{
		sortChildSibling(true);
	}
	[ContextMenu("sortChildSiblingBig2Small")]
	public void sortChildSiblingBig2Small()
	{
		sortChildSibling(false);
	}
}
