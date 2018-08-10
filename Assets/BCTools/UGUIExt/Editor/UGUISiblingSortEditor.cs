using UnityEngine;
using System.Collections;
using UnityEditor;

public class UGUISiblingSortEditor : Editor
{
	[MenuItem("BCTool/Sort Sibling/small to big")]
	public static void SortSiblingA()
	{
		if (null != Selection.activeTransform)
		{
			BCTools.SortChildSibling(Selection.activeTransform, true);
		}
	}

	[MenuItem("BCTool/Sort Sibling/big to small")]
	public static void SortSiblingB()
	{
		if (null != Selection.activeTransform)
		{
			BCTools.SortChildSibling(Selection.activeTransform, false);
		}
	}
}
