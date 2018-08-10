using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCTweenAlpha))]
public class BCTweenAlphaEditor : BCUITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		BCEditorTools.SetLabelWidth(120f);

		BCTweenAlpha tw = target as BCTweenAlpha;
		GUI.changed = false;

		float from = EditorGUILayout.Slider("From", tw.from, 0f, 1f);
		float to = EditorGUILayout.Slider("To", tw.to, 0f, 1f);
		tw.UGUIMode = EditorGUILayout.Toggle("UGUI Mode", tw.UGUIMode);
		if (GUI.changed)
		{
			BCEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			BCEditorTools.SetDirty(tw);
		}
		if(tw.UGUIMode)
			DrawDefaultInspector();
		DrawCommonProperties();
	}
}
