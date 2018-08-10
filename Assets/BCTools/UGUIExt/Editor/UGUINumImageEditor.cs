using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UGUINumImage))]
public class UGUINumImageEditor : Editor
{
	UGUINumImage _target;
	void OnEnable()
	{
		_target = target as UGUINumImage;
	}
	public override void OnInspectorGUI ()
	{
		int tempi = EditorGUILayout.IntField("Num",_target.Num);
		if(GUI.changed)
		{
			_target.Num = tempi;
		}
		DrawDefaultInspector();
	}
}
