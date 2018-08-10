using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LineEndingEditor : EditorWindow
{
	private string textFolderPath = "";
	private FileOperation.ELineEnd lineEndType = FileOperation.ELineEnd.Windows;
	private int searchPatternType = 0;
	private string[] SEARCH = new string[]{"*.cs","*.txt","*.meta","*.xml","*.csv","*.*"};
    [MenuItem("BCTool/Line Ending")]
    public static void ShowWindow()
    {
        LineEndingEditor pEditor = (LineEndingEditor)EditorWindow.GetWindow(typeof(LineEndingEditor));
		
    }

	void OnEnable()
	{
		textFolderPath = Application.dataPath;
	}

	void OnGUI()
	{
		FileOperation.showIOPath("文本文件所在目录：",ref textFolderPath, true);
		lineEndType = (FileOperation.ELineEnd)EditorGUILayout.EnumPopup("行尾希望转换成：",lineEndType);
		searchPatternType = EditorGUILayout.Popup("希望处理的文件类型：",searchPatternType,SEARCH);
		if(GUILayout.Button("执行"))
		{
			if(EditorUtility.DisplayDialog("请确认", "确定执行该目录吗：\n" + textFolderPath, "OK", "Cancle"))
				DoConvert();
		}
	}

	void DoConvert()
	{
		List<FileInfo> fileInfolist = FileOperation.GetFilesIncludeChildFold(textFolderPath,SEARCH[searchPatternType]);
		for(int i=0;i<fileInfolist.Count;i++)
		{
			FileOperation.SetFileLineEnding(fileInfolist[i].FullName,lineEndType);
		}
		EditorUtility.DisplayDialog("Line Ending", "Done!", "OK");
	}
}
