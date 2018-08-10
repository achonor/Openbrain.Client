using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomPropertyDrawer(typeof(ToDividLineAttribute))]


public class ToDividLineAttributeDrawer : PropertyDrawer 
{
	public override void OnGUI(Rect position,SerializedProperty property,GUIContent label)
	{
		Rect arect = position;
		arect.y +=10f;
		arect.height = 2f;
		EditorGUI.ProgressBar(arect,0,"");
	}
}
