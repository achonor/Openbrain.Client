using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCTweenPosition))]
public class BCTweenPositionEditor : BCUITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		BCEditorTools.SetLabelWidth(120f);

		BCTweenPosition tw = target as BCTweenPosition;
		GUI.changed = false;
		bool useTransform = EditorGUILayout.Toggle("Use Transform", tw.UseTransform);

		Vector3 from = tw.from;
		Transform fromTran = null, toTran = null;
		if (useTransform)
		{
			fromTran = EditorGUILayout.ObjectField("From", tw.fromTran, typeof(Transform)) as Transform;
		}
		else
		{
			from = EditorGUILayout.Vector3Field("From", tw.from);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Set From"))
			{
				from = tw.value;
				GUI.changed = true;
			}
			if (GUILayout.Button("Reset By From"))
			{
				tw.value = from;
			}
			EditorGUILayout.EndHorizontal();
		}
		Vector3 to = tw.to;
		if (useTransform)
		{
			toTran = EditorGUILayout.ObjectField("To", tw.toTran, typeof(Transform)) as Transform;
		}
		else
		{
			to = EditorGUILayout.Vector3Field("To", tw.to);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Set To"))
			{
				to = tw.value;
				GUI.changed = true;
			}
			if (GUILayout.Button("Reset By To"))
			{
				tw.value = to;
			}
			EditorGUILayout.EndHorizontal();
		}


		bool fromIsCurrent = false, RelativeMove = false;
		bool LockX = false, LockY = false, LockZ = false;
		if (!useTransform)
		{
			fromIsCurrent = EditorGUILayout.Toggle("From Is Current", tw.fromIsCurrent);
			if (fromIsCurrent)
			{
				RelativeMove = EditorGUILayout.Toggle("->Relative Move", tw.RelativeMove);
			}
			LockX = EditorGUILayout.Toggle("Lock X", tw.LockX);
			LockY = EditorGUILayout.Toggle("Lock Y", tw.LockY);
			LockZ = EditorGUILayout.Toggle("Lock Z", tw.LockZ);
		}

		tw.mTrans = EditorGUILayout.ObjectField("Transform", tw.mTrans, typeof(Transform)) as Transform;

		bool UsePathCurve = EditorGUILayout.Toggle("Use Path Curve", tw.UsePathCurve);
		AnimationCurve curve = new AnimationCurve();
		if(UsePathCurve)
			curve = EditorGUILayout.CurveField("Path Curve", tw.pathCurve, GUILayout.Width(300f), GUILayout.Height(50f));

		if (GUI.changed)
		{
			BCEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			tw.fromIsCurrent = fromIsCurrent;
			tw.RelativeMove = RelativeMove;
			tw.LockX = LockX;
			tw.LockY = LockY;
			tw.LockZ = LockZ;
			tw.UsePathCurve = UsePathCurve;
			if(UsePathCurve)
				tw.pathCurve = curve;
			tw.UseTransform = useTransform;
			if (useTransform)
			{
				tw.fromTran = fromTran;
				tw.toTran = toTran;
			}
			BCEditorTools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
