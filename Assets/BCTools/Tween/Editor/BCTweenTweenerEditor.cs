using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCTweenTweener))]
public class BCTweenTweenerEditor : BCUITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		DrawCommonProperties();
	}
}
