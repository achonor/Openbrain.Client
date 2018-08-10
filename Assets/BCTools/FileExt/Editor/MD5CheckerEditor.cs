using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEditor;
using UnityEngine;
public class MD5CheckerEditor : EditorWindow
{
	[MenuItem("BCTool/File MD5 Checker")]
	static void ShowWindow()
	{
		GetWindow<MD5CheckerEditor> ();
	}

	string filepath = "";
	string md5str = "";
	float playme = 0.5f;
	void OnGUI()
	{
		FileOperation.showIOPath("文件路径：", ref filepath, false);
		EditorGUILayout.TextField("MD5 编码：", md5str);
		playme = EditorGUILayout.Slider("蛋疼了就动我：", playme, 0f, 1f);
		if(GUILayout.Button("Get Md5",GUILayout.Height(100f)))
		{
			md5str = FileOperation.GetMD5(filepath);
			playme = 0.618f;
		}
	}
}