using UnityEngine;
using System;
[RequireComponent(typeof(BCUITweener))]
public class BCTweenRandTrigger : MonoBehaviour
{
	public BCRandomMaker randMaker;
	public Vector3 from;
    public Vector3 to;
    public float duration = 1f;
    public float delay;

    public TweenFromRule[] from_rule = new TweenFromRule[3];
    public TweenToRule[] to_rule = new TweenToRule[3];
    public TweenTimeRule durationRule;
    public TweenTimeRule delayRule;
	
    public string[] ruleTagFrom = new string[3];//x,y,z
    public string[] ruleTagTo = new string[3];//x,y,z
    public string ruleTagDuration;
    public string ruleTagDelay;

    public enum TweenFromRule
    {
        ConstValue,
        Current,
        RandValue,
    }
    public enum TweenToRule
    {
        ConstValue,
        RandValue,
        FromPlusRand,
        FromPlusFormula,
    }
    public enum TweenTimeRule
    {
        ConstValue,
        RandValue,
    }

	BCUITweener m_tweener = null;
	BCUITweener tweener
	{
		get {
			if (m_tweener == null)
			{
				m_tweener = GetComponent<BCUITweener>();
			}
			return m_tweener;
		}
	}

    //call me before play
    public void JudgeTween()
    {
        if (randMaker == null || tweener == null) return;
        Vector3 fromfinal = from;
        Vector3 tofinal = to;
        float durationFinal = duration;
        float delayfinal = delay;
        //-------------First handle the situation of Current mode-------------
        if (tweener is BCTweenRotation)
        {
            BCTweenRotation tween = tweener as BCTweenRotation;
			Vector3 realvalue = tween.value.eulerAngles;
            if (from_rule[0] == TweenFromRule.Current)
            {
				fromfinal.x = realvalue.x;
            }
            if (from_rule[1] == TweenFromRule.Current)
            {
				fromfinal.y = realvalue.y;
            }
            if (from_rule[2] == TweenFromRule.Current)
            {
				fromfinal.z = realvalue.z;
            }
        }
        else if (tweener is BCTweenScale)
        {
            BCTweenScale tween = tweener as BCTweenScale;
            if (from_rule[0] == TweenFromRule.Current)
            {
                fromfinal.x = tween.value.x;
            }
            if (from_rule[1] == TweenFromRule.Current)
            {
                fromfinal.y = tween.value.y;
            }
            if (from_rule[2] == TweenFromRule.Current)
            {
                fromfinal.z = tween.value.z;
            }
        }
        else if (tweener is BCTweenPosition)
        {
            BCTweenPosition tween = tweener as BCTweenPosition;
            if (from_rule[0] == TweenFromRule.Current)
            {
                fromfinal.x = tween.value.x;
            }
            if (from_rule[1] == TweenFromRule.Current)
            {
                fromfinal.y = tween.value.y;
            }
            if (from_rule[2] == TweenFromRule.Current)
            {
                fromfinal.z = tween.value.z;
            }
        }
        //----------then handle other----------------------
        if (from_rule[0] == TweenFromRule.RandValue)
        {
            fromfinal.x = randMaker.GetRand(ruleTagFrom[0]);
        }
        if (from_rule[1] == TweenFromRule.RandValue)
        {
            fromfinal.y = randMaker.GetRand(ruleTagFrom[1]);
        }
        if (from_rule[2] == TweenFromRule.RandValue)
        {
            fromfinal.z = randMaker.GetRand(ruleTagFrom[2]);
        }
        if (to_rule[0] == TweenToRule.RandValue)
        {
            tofinal.x = randMaker.GetRand(ruleTagTo[0]);
        }
        if (to_rule[1] == TweenToRule.RandValue)
        {
            tofinal.y = randMaker.GetRand(ruleTagTo[1]);
        }
        if (to_rule[2] == TweenToRule.RandValue)
        {
            tofinal.z = randMaker.GetRand(ruleTagTo[2]);
        }
        if (to_rule[0] == TweenToRule.FromPlusRand)
        {
            tofinal.x = fromfinal.x + randMaker.GetRand(ruleTagTo[0]);
        }
        if (to_rule[1] == TweenToRule.FromPlusRand)
        {
            tofinal.y = fromfinal.y + randMaker.GetRand(ruleTagTo[1]);
        }
        if (to_rule[2] == TweenToRule.FromPlusRand)
        {
            tofinal.z = fromfinal.z + randMaker.GetRand(ruleTagTo[2]);
        }
        if (to_rule[0] == TweenToRule.FromPlusFormula)
        {
            tofinal.x = randMaker.CalculateForm(ruleTagTo[0], fromfinal.x);
        }
        if (to_rule[1] == TweenToRule.FromPlusFormula)
        {
            tofinal.y = randMaker.CalculateForm(ruleTagTo[1], fromfinal.y);
        }
        if (to_rule[2] == TweenToRule.FromPlusFormula)
        {
            tofinal.z = randMaker.CalculateForm(ruleTagTo[2], fromfinal.z);
        }

        //--------------------------------------------------
        if (tweener is BCTweenRotation)
        {
            BCTweenRotation tween = tweener as BCTweenRotation;
            tween.from = fromfinal;
            tween.to = tofinal;
        }
        else if (tweener is BCTweenScale)
        {
            BCTweenScale tween = tweener as BCTweenScale;
            tween.from = fromfinal;
            tween.to = tofinal;
        }
        else if (tweener is BCTweenPosition)
        {
            BCTweenPosition tween = tweener as BCTweenPosition;
            tween.from = fromfinal;
            tween.to = tofinal;
        }

        if (durationRule == TweenTimeRule.RandValue)
        {
            durationFinal = randMaker.GetRand(ruleTagDuration);
        }
        if (delayRule == TweenTimeRule.RandValue)
        {
            delayfinal = randMaker.GetRand(ruleTagDelay);
        }
        tweener.delay = delayfinal;
        tweener.duration = durationFinal;
    }

	public void JudgeThenJumpToEnd()
	{
		JudgeTween();
		tweener.SetCurrentValueToEnd();
	}
}
