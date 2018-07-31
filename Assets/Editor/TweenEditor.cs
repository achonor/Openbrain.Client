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
        if (GUILayout.Button("Play"))  
        {
            tweenClass.Play();
            return;
        }
        if (GUILayout.Button("ReversePlay"))
        {
            tweenClass.ReversePlay();
            return;
        }
        if (GUILayout.Button("Reset"))
        {
            tweenClass.Init();
            return;
        }
    }
}
