using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEditor;
using UnityEngine;
public class ToUnicodeEditor : EditorWindow
{
	[MenuItem("BCTool/AnyString <-> Unicode")]
	static void ShowWindow()
	{
		GetWindow<ToUnicodeEditor> ();
	}

	string anyString = "";
	string unistr = "";
	void OnGUI()
	{
		anyString = EditorGUILayout.TextField("Any String:",anyString);
		unistr = EditorGUILayout.TextField("Unicode:",unistr);
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("AnyString-->Unicode"))
		{
			unistr = UnicodeConvert.ToUnicode(anyString);
		}
		if(GUILayout.Button("Unicode-->AnyString"))
		{
			anyString = UnicodeConvert.ToGB2312(unistr);
		}
		EditorGUILayout.EndHorizontal();
	}
}