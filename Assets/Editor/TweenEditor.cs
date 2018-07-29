using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenBase), true)]
public class TweenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TweenBase tweenClass = (TweenBase)target;
        //  按钮
        if (GUILayout.Button("Play"))  
        {
            tweenClass.Play();
            return;
        }
        //  按钮
        if (GUILayout.Button("Reset"))
        {
            tweenClass.Init();
            return;
        }
    }
}
