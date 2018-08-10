using UnityEngine;
using System.Collections;

public class String2EnumAttribute : PropertyAttribute
{
	public string[] enumNames;
	public String2EnumAttribute(string str)
	{
		if(enumNames == null || enumNames.Length == 0)
		{
			enumNames = str.Split(',');
		}
	}
}
