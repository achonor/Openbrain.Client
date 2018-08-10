using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UGUISliderExt))]
public class UGUISliderExtEditor : Editor
{
	UGUISliderExt _target;
	string[] TEXTMODE = new string[] { "m|n", "n%" };
	string[] DOTNUM = new string[] { "0", "1","2","3"};
	void OnEnable()
	{
		_target = target as UGUISliderExt;
	}
	public override void OnInspectorGUI ()
	{
		int textMode = 0, RealValue = 0, MaxValue = 1, percentDotNum = 0;
		float PercentValue = 0;
		bool BeyondRich = false, NotBeyondRich = false;
		Color BeyondColor = Color.black, NotBeyondColor = Color.black;


		textMode = EditorGUILayout.Popup("Text Mode", _target.textMode, TEXTMODE);
		if (textMode == 0)
		{
			RealValue = EditorGUILayout.IntField("Real Value", _target.RealValue);
			MaxValue = EditorGUILayout.IntField("Max Value", _target.MaxValue);
		}
		else if (textMode == 1)
		{
			percentDotNum = EditorGUILayout.Popup("Percent Dot Num", _target.percentDotNum, DOTNUM);
			PercentValue = EditorGUILayout.FloatField("Percent Value", _target.PercentValue);
		}
		BeyondRich = EditorGUILayout.Toggle("Beyond Rich", _target.BeyondRich);
		if (BeyondRich)
		{
			BeyondColor = EditorGUILayout.ColorField("Beyond Color", _target.BeyondColor);
		}
		NotBeyondRich = EditorGUILayout.Toggle("Not Beyond Rich", _target.NotBeyondRich);
		if (NotBeyondRich)
		{
			NotBeyondColor = EditorGUILayout.ColorField("Not Beyond Color", _target.NotBeyondColor);
		}


		if (GUI.changed)
		{
			BCEditorTools.RegisterUndo("UGUI Change", _target);
			_target.textMode = textMode;
			if (textMode == 0)
			{
				_target.RealValue = RealValue;
				_target.MaxValue = MaxValue;
			}
			else if (textMode == 1)
			{
				_target.percentDotNum = percentDotNum;
				_target.PercentValue = PercentValue;
			}
			_target.BeyondRich = BeyondRich;
			_target.NotBeyondRich = NotBeyondRich;
			if(BeyondRich)
				_target.BeyondColor = BeyondColor;
			if(NotBeyondRich)
				_target.NotBeyondColor = NotBeyondColor;
			BCEditorTools.SetDirty(_target);
		}
		DrawDefaultInspector();
	}
}
