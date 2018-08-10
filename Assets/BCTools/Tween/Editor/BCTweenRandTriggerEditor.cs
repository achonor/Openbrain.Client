using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BCTweenRandTrigger))]
public class BCTweenRandTriggerEditor : Editor
{
    BCTweenRandTrigger _target;
    void OnEnable()
    {
        _target = target as BCTweenRandTrigger;
        if (_target.randMaker == null)
        {
            _target.randMaker = _target.GetComponent<BCRandomMaker>();
        }
    }
	public override void OnInspectorGUI ()
	{
        BCEditorTools.SetLabelWidth(95f);
        _target.randMaker = (BCRandomMaker)EditorGUILayout.ObjectField("Random Maker",_target.randMaker, typeof(BCRandomMaker));
        GUILayout.Space(5f);
        _target.durationRule = (BCTweenRandTrigger.TweenTimeRule)EditorGUILayout.EnumPopup("Duration", _target.durationRule);
        if (_target.durationRule == BCTweenRandTrigger.TweenTimeRule.ConstValue)
        {
            _target.duration = EditorGUILayout.FloatField("-> value", _target.duration);
        }
        else
        {
            _target.ruleTagDuration = EditorGUILayout.TextField("-> Tag", _target.ruleTagDuration);
        }
        _target.delayRule = (BCTweenRandTrigger.TweenTimeRule)EditorGUILayout.EnumPopup("Start Delay", _target.delayRule);
        if (_target.delayRule == BCTweenRandTrigger.TweenTimeRule.ConstValue)
        {
            _target.delay = EditorGUILayout.FloatField("-> value", _target.delay);
        }
        else
        {
            _target.ruleTagDelay = EditorGUILayout.TextField("-> Tag", _target.ruleTagDelay);
        }
        GUILayout.Space(5f);

        if (BCEditorTools.DrawHeader("Tween Transfrom"))
        {
            BCEditorTools.BeginContents();
            BCEditorTools.SetLabelWidth(110f);


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("From", GUILayout.Width(40));
            BCEditorTools.BeginContents();
            EditorGUILayout.BeginVertical();
            _target.from_rule[0] = (BCTweenRandTrigger.TweenFromRule)EditorGUILayout.EnumPopup(_target.from_rule[0]);
            if (_target.from_rule[0] == BCTweenRandTrigger.TweenFromRule.ConstValue)
            {
                _target.from.x = EditorGUILayout.FloatField(_target.from.x);
            }
            else if (_target.from_rule[0] == BCTweenRandTrigger.TweenFromRule.RandValue)
            {
                BCEditorTools.SetLabelWidth(30f);
                _target.ruleTagFrom[0] = EditorGUILayout.TextField("Tag",_target.ruleTagFrom[0]);
                BCEditorTools.SetLabelWidth(110f);
            }
            EditorGUILayout.EndVertical();
            BCEditorTools.EndContents();
            BCEditorTools.BeginContents();
            EditorGUILayout.BeginVertical();
            _target.from_rule[1] = (BCTweenRandTrigger.TweenFromRule)EditorGUILayout.EnumPopup(_target.from_rule[1]);
            if (_target.from_rule[1] == BCTweenRandTrigger.TweenFromRule.ConstValue)
            {
                _target.from.y = EditorGUILayout.FloatField(_target.from.y);
            }
            else if (_target.from_rule[1] == BCTweenRandTrigger.TweenFromRule.RandValue)
            {
                BCEditorTools.SetLabelWidth(30f);
                _target.ruleTagFrom[1] = EditorGUILayout.TextField("Tag", _target.ruleTagFrom[1]);
                BCEditorTools.SetLabelWidth(110f);
            }
            EditorGUILayout.EndVertical();
            BCEditorTools.EndContents();
            BCEditorTools.BeginContents();
            EditorGUILayout.BeginVertical();
            _target.from_rule[2] = (BCTweenRandTrigger.TweenFromRule)EditorGUILayout.EnumPopup(_target.from_rule[2]);
            if (_target.from_rule[2] == BCTweenRandTrigger.TweenFromRule.ConstValue)
            {
                _target.from.z = EditorGUILayout.FloatField(_target.from.z);
            }
            else if (_target.from_rule[2] == BCTweenRandTrigger.TweenFromRule.RandValue)
            {
                BCEditorTools.SetLabelWidth(30f);
                _target.ruleTagFrom[2] = EditorGUILayout.TextField("Tag", _target.ruleTagFrom[2]);
                BCEditorTools.SetLabelWidth(110f);
            }
            EditorGUILayout.EndVertical();
            BCEditorTools.EndContents();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            //----------------------------------------------------------
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("To", GUILayout.Width(40));
            BCEditorTools.BeginContents();
            EditorGUILayout.BeginVertical();
            _target.to_rule[0] = (BCTweenRandTrigger.TweenToRule)EditorGUILayout.EnumPopup(_target.to_rule[0]);
            if (_target.to_rule[0] == BCTweenRandTrigger.TweenToRule.ConstValue)
            {
                _target.to.x = EditorGUILayout.FloatField(_target.to.x);
            }
            else
            {
                BCEditorTools.SetLabelWidth(30f);
                _target.ruleTagTo[0] = EditorGUILayout.TextField("Tag", _target.ruleTagTo[0]);
                BCEditorTools.SetLabelWidth(110f);
            }
            EditorGUILayout.EndVertical();
            BCEditorTools.EndContents();
            BCEditorTools.BeginContents();
            EditorGUILayout.BeginVertical();
            _target.to_rule[1] = (BCTweenRandTrigger.TweenToRule)EditorGUILayout.EnumPopup(_target.to_rule[1]);
            if (_target.to_rule[1] == BCTweenRandTrigger.TweenToRule.ConstValue)
            {
                _target.to.y = EditorGUILayout.FloatField(_target.to.y);
            }
            else
            {
                BCEditorTools.SetLabelWidth(30f);
                _target.ruleTagTo[1] = EditorGUILayout.TextField("Tag", _target.ruleTagTo[1]);
                BCEditorTools.SetLabelWidth(110f);
            }
            EditorGUILayout.EndVertical();
            BCEditorTools.EndContents();
            BCEditorTools.BeginContents();
            EditorGUILayout.BeginVertical();
            _target.to_rule[2] = (BCTweenRandTrigger.TweenToRule)EditorGUILayout.EnumPopup(_target.to_rule[2]);
            if (_target.to_rule[2] == BCTweenRandTrigger.TweenToRule.ConstValue)
            {
                _target.to.z = EditorGUILayout.FloatField(_target.to.z);
            }
            else
            {
                BCEditorTools.SetLabelWidth(30f);
                _target.ruleTagTo[2] = EditorGUILayout.TextField("Tag", _target.ruleTagTo[2]);
                BCEditorTools.SetLabelWidth(110f);
            }
            EditorGUILayout.EndVertical();
            BCEditorTools.EndContents();
            EditorGUILayout.EndHorizontal();

            BCEditorTools.EndContents();
        }
	}
}
