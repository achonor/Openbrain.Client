using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCTweenRectTranSize))]
public class BCTweenRectSizeEditor : BCUITweenerEditor
{
	string[] modeStr= {"Height","Width"};
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		BCEditorTools.SetLabelWidth(120f);
		BCTweenRectTranSize tw = target as BCTweenRectTranSize;
		GUI.changed = false;
		int sizeMode = EditorGUILayout.Popup(tw.sizeMode, modeStr);
		float from = EditorGUILayout.FloatField("From", tw.from);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Set local From"))
		{
			from = tw.value;
			GUI.changed = true;
		}
		if (GUILayout.Button("Reset By local From"))
		{
			tw.value = from;
		}
		EditorGUILayout.EndHorizontal();
		float to = EditorGUILayout.FloatField("To", tw.to);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Set local To"))
		{
			to = tw.value;
			GUI.changed = true;
		}
		if (GUILayout.Button("Reset By local To"))
		{
			tw.value = to;
		}
		EditorGUILayout.EndHorizontal();

		tw.mTran = EditorGUILayout.ObjectField("Transform", tw.mTran, typeof(RectTransform)) as RectTransform;

		if (GUI.changed)
		{
			BCEditorTools.RegisterUndo("Tween Change", tw);
			tw.sizeMode = sizeMode;
			tw.from = from;
			tw.to = to;
			BCEditorTools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
