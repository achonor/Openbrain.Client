using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Effect2HtmlEditor : EditorWindow
{
	private string skillEffectPath = "";
	private string materialPath = "";
	private string htmlFilePath = "";
	private string htmlbuff;
    [MenuItem("BCTool/Effect -> Html")]
    public static void ShowWindow()
    {
        Effect2HtmlEditor pEditor = (Effect2HtmlEditor)EditorWindow.GetWindow(typeof(Effect2HtmlEditor));
    }

	void OnEnable()
	{
		skillEffectPath = Application.dataPath + "/Resources/Effect";
		htmlFilePath = Application.dataPath + "/Docs/特效使用材质球情况.html";
		materialPath = Application.dataPath + "/Artwork/Effect/Spell/FX/_Materials";
	}

	void resetHtml()
	{
		htmlbuff = "<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=UTF-8\">\r\n<a name=\"top\"/>\r\n";
	}

	void OnGUI()
	{
		FileOperation.showIOPath("特效prefab目录：",ref skillEffectPath, true);
		FileOperation.showIOPath("材质球目录：", ref materialPath, true);
		if (FileOperation.showIOPath("保存html路径：", ref htmlFilePath, false, true, "特效使用材质球情况", "html"))
			DoExport();
	}

	void DoExport()
	{
		resetHtml();
		htmlbuff += "<DL>\r\n";
		htmlbuff += "<DT><H4>特效prefab目录：</H4><DD>" + skillEffectPath + "\r\n";
		htmlbuff += "<DT><H4>材质球目录：</H4><DD>" + materialPath + "\r\n";
		htmlbuff += "<DT><H4>保存html路径：</H4><DD>" + htmlFilePath + "\r\n";
		htmlbuff += "<DT><a href=\"#one\">已使用</a>\r\n";
		htmlbuff += "<DT><a href=\"#two\">未使用</a>\r\n";
		htmlbuff += "</DL>\r\n";
		htmlbuff += "<a name=\"one\"><H1>已使用</H1></a>\r\n";
		htmlbuff += "<a href=\"#top\">回页首</a>\r\n";
		htmlbuff += "<HR>\r\n";
		htmlbuff += "<DL>\r\n";
		List<FileInfo> prefabFileList = FileOperation.GetFilesIncludeChildFold(skillEffectPath, "*.prefab");
		Dictionary<string, bool> usedMaterialDic = new Dictionary<string,bool>();
		string rootPath = Application.dataPath.Substring(0,Application.dataPath.LastIndexOf("/")+1);
		for (int i = 0; i < prefabFileList.Count; i++)
		{
			bool prefabHasShown = false;
			string fileshortpath = FileOperation.SetPathMode(prefabFileList[i].FullName, false);
			fileshortpath = fileshortpath.Replace(rootPath, "");
			GameObject go = AssetDatabase.LoadAssetAtPath(fileshortpath,typeof(GameObject)) as GameObject;
			if (go == null)
			{
				Debug.LogWarning(fileshortpath + " can not be a gameobject. You may check this prefab file.");
				continue;
			}
			Renderer[] renderlist = go.GetComponentsInChildren<Renderer>(true);
			                                               
			for (int j = 0; j < renderlist.Length; j++)
			{
				if (renderlist[j] == null)
				{
					continue;
				}
				for (int k = 0; k < renderlist[j].sharedMaterials.Length; k++)
				{
					if (renderlist[j].sharedMaterials[k] == null)
					{
						Debug.LogWarning(fileshortpath + " 路径下有" + renderlist[j].name + "物体，它的Renderer组件的材质球列表有留空的！");
						continue;
					}
					if (!prefabHasShown)
					{
						htmlbuff += "<DT><H4>" + go.name + "</H4>\r\n";
						prefabHasShown = true;
					}
					string materialName = renderlist[j].sharedMaterials[k].name.Replace(" (Instance)", "")+".mat";
					if (!usedMaterialDic.ContainsKey(materialName))
						usedMaterialDic.Add(materialName, false);
					if (renderlist[j].sharedMaterials[k].mainTexture == null)
					{
						//有些材质球不需要图片,直接用shader解决的
						//Debug.LogWarning(fileshortpath + " 路径下有" + renderlist[j].name + "物体，它的"+materialName+"材质球没设置图片！");
						htmlbuff += "<DD>" + materialName + "\r\n";
					}
					else
						htmlbuff += "<DD>" + materialName + "---->" + renderlist[j].sharedMaterials[k].mainTexture.name + "\r\n";
				}
			}
		}
		htmlbuff += "</DL>\r\n";
		htmlbuff += "<a name=\"two\"><H1>疑似未使用的材质球</H1></a>\r\n";
		htmlbuff += "<a href=\"#top\">回页首</a>\r\n";
		htmlbuff += "<HR>\r\n";
		htmlbuff += "<DL>\r\n";
		List<FileInfo> matFileList = FileOperation.GetFilesIncludeChildFold(materialPath, "*.mat");
		for (int i = 0; i < matFileList.Count; i++)
		{
			if (!usedMaterialDic.ContainsKey(matFileList[i].Name))
			{
				htmlbuff += "<DD>" + matFileList[i].Name + "</DD>\r\n";
			}
		}
		htmlbuff += "</DL>\r\n";

		FileOperation.Save(htmlFilePath, htmlbuff);
		
		
		htmlbuff = "";

		EditorUtility.DisplayDialog("Export Html Resurt", "Done!", "OK");
	}
}