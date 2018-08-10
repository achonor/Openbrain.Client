using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomPropertyDrawer(typeof(DividingLineAttribute))]

public class DividingLineAttributeDrawer : DecoratorDrawer
{
	public override void OnGUI(Rect position)
	{
		Rect arect = position;
		arect.y +=10f;
		arect.height = 2f;
		EditorGUI.ProgressBar(arect,0,"");
	}
}
