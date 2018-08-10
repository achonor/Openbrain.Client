using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(BCTweenNumText))]
public class BCTweenNumTextEditor : BCUITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		BCEditorTools.SetLabelWidth(120f);

		BCTweenNumText tw = target as BCTweenNumText;

		DrawDefaultInspector();
		if (tw.soundStartTime < 0)
			tw.soundStartTime = 0;
		if (tw.soundSkipCount < 0)
			tw.soundStartTime = 0;
		DrawCommonProperties();
	}
}
