using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCUITweener), true)]
public class BCUITweenerEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		BCEditorTools.SetLabelWidth(110f);
		base.OnInspectorGUI();
		DrawCommonProperties();
	}

	BCUITweener tw;
	SerializedProperty onFinished;
	SerializedProperty onWePointed1;
    SerializedProperty onWePointed2;
    SerializedProperty onWePointed3;
    string[] chooseNumStr = new string[] { "0", "1", "2", "3" };
    int[] chooseNum = new int[] { 0, 1, 2, 3 };
	void init()
	{
		tw = target as BCUITweener;
		onFinished = serializedObject.FindProperty("onFinished");
		onWePointed1 = serializedObject.FindProperty("onWePointed1");
        onWePointed2 = serializedObject.FindProperty("onWePointed2");
        onWePointed3 = serializedObject.FindProperty("onWePointed3");
    }

	protected void DrawCommonProperties ()
	{
		if (null == tw || null == onFinished || null == onWePointed1)
		{
			init();
		}

		if (Application.isPlaying)
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("PlayForward"))
				tw.PlayForward();
			if (GUILayout.Button("PlayReverse"))
				tw.PlayReverse();
			if (GUILayout.Button("PlayForwardForce"))
			{
				tw.PlayForwardForce();
			}
			if (GUILayout.Button("PlayReverseForce"))
			{
				tw.PlayReverseForce();
			}
			if (GUILayout.Button("Stop"))
			{
				tw.Stop();
			}
			EditorGUILayout.EndHorizontal();
		}
		else
		{
			EditorGUILayout.HelpBox("Some buttons will appear when running.", MessageType.Info);
		}

		if (BCEditorTools.DrawHeader("Tweener"))
		{
			BCEditorTools.BeginContents();
			BCEditorTools.SetLabelWidth(110f);

			GUI.changed = false;

            BCUITweener.Style style = (BCUITweener.Style)EditorGUILayout.EnumPopup("Play Style", tw.style);

            if (style== BCUITweener.Style.Once)
            {
                GUILayout.BeginHorizontal();
                GUI.color = Color.white;
                GUILayout.EndHorizontal();
            }

			AnimationCurve curve = EditorGUILayout.CurveField("Animation Curve", tw.animationCurve, GUILayout.Width(170f), GUILayout.Height(62f));
			//UITweener.Method method = (UITweener.Method)EditorGUILayout.EnumPopup("Play Method", tw.method);

			GUILayout.BeginHorizontal();
			float dur = EditorGUILayout.FloatField("Duration", tw.duration, GUILayout.Width(170f));
			GUILayout.Label("seconds");
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			float del = EditorGUILayout.FloatField("Start Delay", tw.delay, GUILayout.Width(170f));
			GUILayout.Label("seconds");
			GUILayout.EndHorizontal();

			//int tg = EditorGUILayout.IntField("Tween Group", tw.tweenGroup, GUILayout.Width(170f));
			bool ts = EditorGUILayout.Toggle("Ignore TimeScale", tw.ignoreTimeScale);

			if (GUI.changed)
			{
				BCEditorTools.RegisterUndo("Tween Change", tw);
				tw.animationCurve = curve;
				//tw.method = method;
				tw.style = style;
				tw.ignoreTimeScale = ts;
                //tw.tweenGroup = tg;
                tw.duration = dur;
				tw.delay = del;
				BCEditorTools.SetDirty(tw);
				GUI.changed = false;
			}
			BCEditorTools.EndContents();
		}

		BCEditorTools.SetLabelWidth(80f);
		serializedObject.Update();
		EditorGUILayout.PropertyField(onFinished);


		BCEditorTools.SetLabelWidth(120f);
		tw.EventPointsCount = EditorGUILayout.IntPopup("Events We Point", tw.EventPointsCount, chooseNumStr,chooseNum);
		if(tw.EventPointsCount > 0)
		{
			tw.tWePoint[0] = EditorGUILayout.Slider("Time We Point 1",tw.tWePoint[0],0f,1f);
			BCEditorTools.SetLabelWidth(80f);
			EditorGUILayout.PropertyField(onWePointed1);
		}
        BCEditorTools.SetLabelWidth(120f);
        if (tw.EventPointsCount > 1)
        {
            tw.tWePoint[1] = EditorGUILayout.Slider("Time We Point 2", tw.tWePoint[1], 0f, 1f);
            BCEditorTools.SetLabelWidth(80f);
            EditorGUILayout.PropertyField(onWePointed2);
        }
        BCEditorTools.SetLabelWidth(120f);
        if (tw.EventPointsCount > 2)
        {
            tw.tWePoint[2] = EditorGUILayout.Slider("Time We Point 3", tw.tWePoint[2], 0f, 1f);
            BCEditorTools.SetLabelWidth(80f);
            EditorGUILayout.PropertyField(onWePointed3);
        }
        serializedObject.ApplyModifiedProperties();
	}
}
