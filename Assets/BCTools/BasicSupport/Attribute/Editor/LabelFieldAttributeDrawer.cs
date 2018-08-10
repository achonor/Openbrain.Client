using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomPropertyDrawer(typeof(LabelFieldAttribute))]

public class LabelFieldAttributeDrawer : DecoratorDrawer
{
	LabelFieldAttribute myattribute = null;
	public override void OnGUI(Rect position)
	{
		if(myattribute == null)
		{
			myattribute = attribute as LabelFieldAttribute;
		}
		EditorGUI.LabelField(position,myattribute.str);
	}
}
