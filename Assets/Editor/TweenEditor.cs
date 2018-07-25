using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenBase))]
public class TweenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TweenBase tweenClass = (TweenBase)target;
        //帮组提示框
        EditorGUILayout.HelpBox("Play", MessageType.Error);
        //  按钮
        if (GUILayout.Button("Play"))  
        {
            tweenClass.Play();
            return;
        }
    }
}
