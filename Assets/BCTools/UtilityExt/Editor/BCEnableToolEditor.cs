using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCEnableTool))]
public class BCTEnableToolEditor : Editor
{
	BCEnableTool be;
	SerializedProperty m_OnEnable;
	SerializedProperty m_OnDisable;
	SerializedProperty m_OnStart;
	SerializedProperty m_OnUpdate;
	SerializedProperty m_OnUpdatePerSec;
	void init()
	{
		be = target as BCEnableTool;
		//var serializedObject = new UnityEditor.SerializedObject(be);
		m_OnEnable = serializedObject.FindProperty("m_OnEnable");
		m_OnDisable = serializedObject.FindProperty("m_OnDisable");
		m_OnStart = serializedObject.FindProperty("m_OnStart");
		m_OnUpdate = serializedObject.FindProperty("m_OnUpdate");
		m_OnUpdatePerSec = serializedObject.FindProperty("m_OnUpdatePerSec");
	}
	public override void OnInspectorGUI ()
	{
		if(null == be || null == m_OnEnable || null == m_OnDisable || null == m_OnStart || null == m_OnUpdate || null == m_OnUpdatePerSec)
		{
			init();
		}
		GUILayout.Space(5f);
		EditorGUILayout.BeginHorizontal();
		if (be.needEnable)
		{
			if (GUILayout.Button("Enable",EditorStyles.toolbarButton))
			{
				be.needEnable = false;
			}
		}
		else
		{
			if (GUILayout.Button("Enable"))
			{
				be.needEnable = true;
			}
		}
		if (be.needDisable)
		{
			if (GUILayout.Button("Disable", EditorStyles.toolbarButton))
			{
				be.needDisable = false;
			}
		}
		else
		{
			if (GUILayout.Button("Disable"))
			{
				be.needDisable = true;
			}
		}
		if (be.needStart)
		{
			if (GUILayout.Button("Start", EditorStyles.toolbarButton))
			{
				be.needStart = false;
			}
		}
		else
		{
			if (GUILayout.Button("Start"))
			{
				be.needStart = true;
			}
		}
		if (be.needUpdate)
		{
			if (GUILayout.Button("Update", EditorStyles.toolbarButton))
			{
				be.needUpdate = false;
			}
		}
		else
		{
			if (GUILayout.Button("Update"))
			{
				be.needUpdate = true;
			}
		}
		if (be.needPerSecond)
		{
			if (GUILayout.Button("PerSecond", EditorStyles.toolbarButton))
			{
				be.needPerSecond = false;
			}
		}
		else
		{
			if (GUILayout.Button("PerSecond"))
			{
				be.needPerSecond = true;
			}
		}
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(5f);
		serializedObject.Update();
		if(be.needEnable)
		{
			EditorGUILayout.PropertyField(m_OnEnable);
		}
		if(be.needDisable)
		{
			EditorGUILayout.PropertyField(m_OnDisable);
		}
		if(be.needStart)
		{
			EditorGUILayout.PropertyField(m_OnStart);
		}
		if(be.needUpdate)
		{
			EditorGUILayout.PropertyField(m_OnUpdate);
		}
		if(be.needPerSecond)
		{
			be.perSecondN = EditorGUILayout.FloatField("Per Second N", be.perSecondN);
			EditorGUILayout.PropertyField(m_OnUpdatePerSec);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
