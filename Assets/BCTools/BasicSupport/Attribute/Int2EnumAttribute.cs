using UnityEngine;
using System.Collections;

public class Int2EnumAttribute : PropertyAttribute
{
	public string[] enumNames;
	public Int2EnumAttribute(string str)
	{
		if(enumNames == null || enumNames.Length == 0)
		{
			enumNames = str.Split(',');
		}
	}
}
