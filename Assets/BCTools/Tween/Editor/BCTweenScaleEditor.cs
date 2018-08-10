using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCTweenScale))]
public class BCTweenScaleEditor : BCUITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		BCEditorTools.SetLabelWidth(120f);

		BCTweenScale tw = target as BCTweenScale;
		GUI.changed = false;

		Vector3 from = EditorGUILayout.Vector3Field("From", tw.from);
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
		Vector3 to = EditorGUILayout.Vector3Field("To", tw.to);
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

		bool fromIsCurrent = EditorGUILayout.Toggle("From Is Current", tw.fromIsCurrent);
		bool RelativeScale = false;
		if(fromIsCurrent)
		{
			RelativeScale = EditorGUILayout.Toggle ("->Relative Scale",tw.RelativeScale);
		}

		tw.mTrans = EditorGUILayout.ObjectField("Transform", tw.mTrans, typeof(Transform)) as Transform;

		if (GUI.changed)
		{
			BCEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			tw.fromIsCurrent = fromIsCurrent;
			tw.RelativeScale = RelativeScale;
			BCEditorTools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
