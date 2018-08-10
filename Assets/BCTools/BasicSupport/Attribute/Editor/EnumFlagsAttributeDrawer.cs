using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]

public class EnumFlagsAttributeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect _position,SerializedProperty _property,GUIContent _label)
	{
		///
		///枚举值的数值最后为一个数字，若要去的其代表的或包含的数值，必须通过按位运算来提取
		///

		_property.intValue = EditorGUI.MaskField(_position,_label,_property.intValue,_property.enumNames);
	}
}
