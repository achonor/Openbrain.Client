using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCTweenAnything))]
public class BCTweenAnythingEditor : BCUITweenerEditor
{
	BCTweenAnything tw;
	SerializedProperty onUpdateEvents;
	void init()
	{
		tw = target as BCTweenAnything;
		//var serializedObject = new UnityEditor.SerializedObject(tw);
		onUpdateEvents = serializedObject.FindProperty("onUpdateEvents");
	}

	public override void OnInspectorGUI ()
	{
		if (null == tw || null == onUpdateEvents)
		{
			init();
		}

		GUILayout.Space(6f);
		BCEditorTools.SetLabelWidth(120f);

		GUI.changed = false;

		Vector4 from = EditorGUILayout.Vector4Field("From", tw.from);
		Vector4 to = EditorGUILayout.Vector4Field("To", tw.to);

		EditorGUILayout.Vector4Field("Value",tw.value);
		bool useUpdate = EditorGUILayout.Toggle("Use Update", tw.useUpdate);

		if (GUI.changed)
		{
			BCEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			tw.useUpdate = useUpdate;
			BCEditorTools.SetDirty(tw);
			GUI.changed = false;
		}

		DrawCommonProperties();
		BCEditorTools.SetLabelWidth(80f);
		if (useUpdate)
		{
			EditorGUILayout.PropertyField(onUpdateEvents);
			if (GUI.changed)
			{
				serializedObject.ApplyModifiedProperties();
				Debug.Log("GUIChanged");
			}
		}
	}
}
