using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCTweenColor))]
public class BCTweenColorEditor : BCUITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		BCEditorTools.SetLabelWidth(120f);

		BCTweenColor tw = target as BCTweenColor;
		GUI.changed = false;

		Color from = EditorGUILayout.ColorField("From", tw.from);
		Color to = EditorGUILayout.ColorField("To", tw.to);
		tw.UGUIMode = EditorGUILayout.Toggle("UGUI Mode", tw.UGUIMode);
		if (tw.UGUIMode)
			DrawDefaultInspector();

		if (GUI.changed)
		{
			BCEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			BCEditorTools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
