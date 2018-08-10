using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomPropertyDrawer(typeof(String2EnumAttribute))]
public class String2EnumAttributeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		String2EnumAttribute _target = attribute as String2EnumAttribute;

		if(property.propertyType == SerializedPropertyType.String)
		{
			int index = 0;
			for (int i = 0; i < _target.enumNames.Length; i++)
			{
				if (property.stringValue.Equals(_target.enumNames[i]))
				{
					index = i;
					break;
				}
			}
			index = EditorGUI.Popup(position, label.text, index, _target.enumNames);
			property.stringValue = _target.enumNames[index];
		}
	}
}
