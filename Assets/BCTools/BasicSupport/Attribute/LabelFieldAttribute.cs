using UnityEngine;
using System.Collections;

public class LabelFieldAttribute : PropertyAttribute
{
	public string str;
	public LabelFieldAttribute(string str)
	{
		this.str = str;
	}
}
