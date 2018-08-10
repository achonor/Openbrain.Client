using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomPropertyDrawer(typeof(Int2EnumAttribute))]
public class Int2EnumAttributeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		Int2EnumAttribute _target = attribute as Int2EnumAttribute;

		if(property.propertyType == SerializedPropertyType.Enum)
		{
			property.intValue = EditorGUI.Popup(position,label.text,property.intValue,property.enumNames);
		}
		else if(property.propertyType == SerializedPropertyType.Integer)
		{
			property.intValue = EditorGUI.Popup(position,label.text,property.intValue,_target.enumNames);
		}
	}
}
