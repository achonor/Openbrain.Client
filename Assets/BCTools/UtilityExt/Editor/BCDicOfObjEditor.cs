using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Object = UnityEngine.Object;

[CustomEditor(typeof(BCDicOfObj))]
public class BCDicOfObjEditor : Editor
{
	string searchstr = "";
	List<BCDicOfObj.ObjData> searchList = new List<BCDicOfObj.ObjData>();
	BCDicOfObj bo;
	void OnEnable()
	{
		bo = target as BCDicOfObj;
	}
	public override void OnInspectorGUI ()
	{
        if(!string.IsNullOrEmpty(bo.tip))
        {
            EditorGUILayout.LabelField(bo.tip);
        }
		GUILayout.Space(3f);
		EditorGUILayout.BeginHorizontal();
		searchstr = EditorGUILayout.TextField(searchstr);
		if (GUILayout.Button("按名搜"))
		{
			searchList.Clear();
			searchList = bo.FindObjsByName(searchstr);
		}
		if (GUILayout.Button("按描述搜"))
		{
			searchList.Clear();
			searchList = bo.FindObjsByTag(searchstr);
		}
		EditorGUILayout.EndHorizontal();
		BCEditorTools.SetLabelWidth(130f);
		for (int i = 0; i < searchList.Count; i++)
		{
			GUI.color = Color.cyan;
			BCEditorTools.BeginContents();
			GUI.color = Color.white;
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("-", GUILayout.Width(30)))
			{

				bo.objList.Remove(searchList[i]);
				searchList.RemoveAt(i);
				BCEditorTools.EndContents();
				EditorGUILayout.EndHorizontal();
				return;
			}
			Object obj = EditorGUILayout.ObjectField(searchList[i].obj, typeof(Object));
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(2f);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("描述：", GUILayout.Width(40f));
			string str = EditorGUILayout.TextField(searchList[i].Desc);
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(1f);
			BCEditorTools.EndContents();
			if (GUI.changed)
			{
				BCEditorTools.RegisterUndo("Tween Change", bo);
				searchList[i].obj = obj;
				searchList[i].Desc = str;
				BCEditorTools.SetDirty(bo);
			}
		}
		
		for(int i=0;i<bo.objList.Count;i++)
		{
			BCEditorTools.BeginContents();
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button ("-",GUILayout.Width(30)))
			{
				bo.objList.RemoveAt(i);
				BCEditorTools.EndContents();
				EditorGUILayout.EndHorizontal();
				return;
			}
			Object obj = EditorGUILayout.ObjectField(bo.objList[i].obj,typeof(Object));
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(2f);
			Rect titleRect = EditorGUILayout.BeginHorizontal();
			titleRect.xMax = titleRect.xMin + 40f;
			if (Event.current.isMouse && Event.current.button == 1 && titleRect.Contains(Event.current.mousePosition))
			{
				GenericMenu menu = new GenericMenu();
				menu.AddItem(new GUIContent("Duplicate one"), false, _duplicate, bo.objList[i]);
				if (bo.objList[i].obj != null && bo.objList[i].obj is AudioClip)
				{
					menu.AddItem(new GUIContent("Play Audio"), false, _PlayAudioClip, bo.objList[i]);
				}
				menu.ShowAsContext();
				Event.current.Use();
			}
			EditorGUILayout.LabelField("描述：", GUILayout.Width(40f));
			string str = EditorGUILayout.TextField(bo.objList[i].Desc);
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(1f);
			BCEditorTools.EndContents();

			if (GUI.changed)
			{
				BCEditorTools.RegisterUndo("Tween Change", bo);
				bo.objList[i].obj = obj;
				bo.objList[i].Desc = str;
				BCEditorTools.SetDirty(bo);
			}
		}
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("  ",GUILayout.ExpandWidth(true));
		if(GUILayout.Button("+",GUILayout.Width(50)))
		{
			BCDicOfObj.ObjData addMe = new BCDicOfObj.ObjData();
			bo.objList.Add(addMe);
		}
		EditorGUILayout.EndHorizontal();
	}

	void _duplicate(object node)
	{
		BCDicOfObj.ObjData n = node as BCDicOfObj.ObjData;
		int index = bo.objList.IndexOf(n);
		BCDicOfObj.ObjData newone = new BCDicOfObj.ObjData() { 
			Desc = n.Desc,
			obj = n.obj
		};
		bo.objList.Insert(index, newone);
	}

	void _PlayAudioClip(object node)
	{
		BCDicOfObj.ObjData n = node as BCDicOfObj.ObjData;
		BCEditorTools.PlayClip(n.obj as AudioClip);
	}
}
