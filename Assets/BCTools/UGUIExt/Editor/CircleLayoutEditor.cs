using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CircleLayout))]
public class CircleLayoutEditor : Editor
{
	Vector3 childScale = Vector3.one;
	Vector3 childRotation = Vector3.zero;
	Vector3 childWorldRotation = Vector3.zero;
	public override void OnInspectorGUI()
	{
		CircleLayout _target = target as CircleLayout;
		if (GUILayout.Button("Fill Trans Arr By Sibling"))
		{
			_target.fillTransArrBySibling();
		}
		DrawDefaultInspector();
		if (GUILayout.Button("Set Circle"))
		{
			_target.setCircle();
		}
		EditorGUILayout.BeginHorizontal();
		childScale = EditorGUILayout.Vector3Field("Child Scale", childScale);
		if (GUILayout.Button("Set"))
		{
			_target.setChildScale(childScale);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		childRotation = EditorGUILayout.Vector3Field("Child Rotation", childRotation);
		if (GUILayout.Button("Set"))
		{
			_target.setChildRotation(childRotation);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		childWorldRotation = EditorGUILayout.Vector3Field("Child World Rotation", childWorldRotation);
		if (GUILayout.Button("Set"))
		{
			_target.setChildWorldRotation(childWorldRotation);
		}
		EditorGUILayout.EndHorizontal();
	}
}
