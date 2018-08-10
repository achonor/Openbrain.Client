using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]

public class MinMaxRangeAttributeDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return 36f;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		MinMaxRangeAttribute range = attribute as MinMaxRangeAttribute;
		if(property.propertyType == SerializedPropertyType.Vector2)
		{
			Vector2 vec;
			if(property.vector2Value.x <range.minLimit || property.vector2Value.y > range.maxLimit)
			{
				float middle = (range.minLimit+range.maxLimit)/2f;
				vec.x = middle;
				vec.y = middle;
				property.vector2Value = vec;
				return;
			}
			if(range.useMiddle)
			{
				vec = property.vector2Value;
				bool changed = false;
				if(vec.x > range.middle)
				{
					vec.x = range.middle;
					changed = true;
				}
				if(vec.y < range.middle)
				{
					vec.y = range.middle;
					changed = true;
				}
				if(changed)
					property.vector2Value = vec;
			}
			float x=property.vector2Value.x,y=property.vector2Value.y;
			position.height = 16f;
			EditorGUI.MinMaxSlider(label,position,ref x,ref y,range.minLimit,range.maxLimit);
			vec.x = x;
			vec.y = y;
			property.vector2Value = vec;
			position.y += 20f;
			vec = EditorGUI.Vector2Field(position,"--->",property.vector2Value);

			if(vec.x < range.minLimit)
			{
				vec.x = range.minLimit;
			}
			if(vec.y > range.maxLimit)
			{
				vec.y = range.maxLimit;
			}
			if(range.useMiddle)
			{
				if(vec.x > range.middle)
				{
					vec.x = range.middle;
				}
				if(vec.y < range.middle)
				{
					vec.y = range.middle;
				}
			}
			else if(vec.y < vec.x)
			{
				vec.y = vec.x;
			}

			property.vector2Value = vec;
		}
	}
}
