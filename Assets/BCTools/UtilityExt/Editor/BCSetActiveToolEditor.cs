using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCSetActiveTool))]
public class BCSetActiveToolEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		BCEditorTools.SetLabelWidth(130f);
		BCSetActiveTool bt = target as BCSetActiveTool;
		bt.Reverse = EditorGUILayout.Toggle("Set Active Reverse",bt.Reverse);
		for(int i=0;i<bt.list.Count;i++)
		{
			BCEditorTools.BeginContents();
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button ("-",GUILayout.Width(30)))
			{
				bt.list.RemoveAt(i);
				BCEditorTools.EndContents();
				EditorGUILayout.EndHorizontal();
				return;
			}
			GameObject go = EditorGUILayout.ObjectField(bt.list[i].go,typeof(GameObject)) as GameObject;
			EditorGUILayout.EndHorizontal();
			bool setActive = EditorGUILayout.Toggle("Set Active",bt.list[i].setActive);
			BCEditorTools.EndContents();

			if (GUI.changed)
			{
				BCEditorTools.RegisterUndo("Tween Change", bt);
				bt.list[i].go = go;
				bt.list[i].setActive = setActive;
				BCEditorTools.SetDirty(bt);
			}
		}
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("  ",GUILayout.ExpandWidth(true));
		if(GUILayout.Button("+",GUILayout.Width(50)))
		{
			BCSetActiveTool.SetActiveData addMe = new BCSetActiveTool.SetActiveData();
			if(bt.list.Count>0)
				addMe.setActive = bt.list[bt.list.Count-1].setActive;
			bt.list.Add(addMe);
		}
		EditorGUILayout.EndHorizontal();
	}
}
