using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCTweenGroup))]
public class BCTweenGroupEditor : Editor
{
	BCTweenGroup _target;
	void OnEnable()
	{
		_target = target as BCTweenGroup;
	}
	public override void OnInspectorGUI ()
	{
		if(Application.isPlaying)
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Play all Forward"))
			{
				_target.PlayForwardForce();
			}
			if (GUILayout.Button("Play all Reverse"))
			{
				_target.PlayReverseForce();
			}
			if (GUILayout.Button("Stop All"))
			{
				_target.StopAll();
			}
			EditorGUILayout.EndHorizontal();
		}	
		else
		{
			EditorGUILayout.HelpBox("Some buttons will appear when running.", MessageType.Info);
		}
		DrawDefaultInspector();
	}
}
