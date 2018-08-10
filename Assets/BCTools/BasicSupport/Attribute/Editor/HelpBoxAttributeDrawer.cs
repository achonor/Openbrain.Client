using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomPropertyDrawer(typeof(HelpBoxAttribute))]

public class HelpBoxAttributeDrawer : DecoratorDrawer
{
	HelpBoxAttribute myattribute = null;
	public override float GetHeight ()
	{
		float rt = 14f;
		if(myattribute == null)
		{
			myattribute = attribute as HelpBoxAttribute;
		}
		rt = myattribute.line*14f;
		rt +=2f;
		return rt;
	}
	public override void OnGUI(Rect position)
	{
		if(myattribute == null)
		{
			myattribute = attribute as HelpBoxAttribute;
		}
		Rect arect = position;
		arect.height = myattribute.line*14f;
		EditorGUI.HelpBox(arect,myattribute.note,(MessageType)myattribute.messageType);
	}
}
