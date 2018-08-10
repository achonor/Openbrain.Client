using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCTweenRotation))]
public class BCTweenRotationEditor : BCUITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		BCEditorTools.SetLabelWidth(120f);

		BCTweenRotation tw = target as BCTweenRotation;
		GUI.changed = false;

		Vector3 from = EditorGUILayout.Vector3Field("From", tw.from);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Set local From"))
		{
			from = tw.mTrans.localEulerAngles;
			GUI.changed = true;
		}
		if (GUILayout.Button("Reset By local From"))
		{
			tw.mTrans.localEulerAngles = from;
		}
		EditorGUILayout.EndHorizontal();
		Vector3 to = EditorGUILayout.Vector3Field("To", tw.to);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Set local To"))
		{
			to = tw.mTrans.localEulerAngles;
			GUI.changed = true;
		}
		if (GUILayout.Button("Reset By local To"))
		{
			tw.mTrans.localEulerAngles = to;
		}
		EditorGUILayout.EndHorizontal();

		bool fromIsCurrent = EditorGUILayout.Toggle("From Is Current", tw.fromIsCurrent);
		bool RelativeRotate = false;
		if(fromIsCurrent)
		{
			RelativeRotate = EditorGUILayout.Toggle ("->Relative Rotate",tw.RelativeRotate);
		}

		tw.mTrans = EditorGUILayout.ObjectField("Transform",tw.mTrans,typeof(Transform)) as Transform;
		bool Line_2D_to_3D = EditorGUILayout.Toggle("Line 2D to 3D", tw.Line_2D_to_3D);
		if (Line_2D_to_3D)
		{
			tw.leftLine = EditorGUILayout.ObjectField("Left Line", tw.leftLine, typeof(Transform)) as Transform;
			tw.rightLine = EditorGUILayout.ObjectField("Left Line", tw.rightLine, typeof(Transform)) as Transform;
		}


		if (GUI.changed)
		{
			BCEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			tw.fromIsCurrent = fromIsCurrent;
			tw.RelativeRotate = RelativeRotate;
			tw.Line_2D_to_3D = Line_2D_to_3D;
			BCEditorTools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
