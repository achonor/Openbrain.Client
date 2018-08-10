using UnityEngine;

/// <summary>
/// Tween the object's local scale.
/// </summary>

public class BCTweenScale : BCUITweener
{
	public Vector3 from = Vector3.one;
	public Vector3 to = Vector3.one;
	[HideInInspector]
	public bool fromIsCurrent = false;
	[HideInInspector]
	public bool RelativeScale = false;

	public Transform mTrans;

	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

	public Vector3 value { get { return cachedTransform.localScale; } set { cachedTransform.localScale = value; } }

	[System.Obsolete("Use 'value' instead")]
	public Vector3 scale { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = from * (1f - factor) + to * factor;
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public BCTweenScale Begin (GameObject go, float duration, Vector3 scale)
	{
		BCTweenScale comp = BCUITweener.Begin<BCTweenScale>(go, duration);
		comp.from = comp.value;
		comp.to = scale;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value; }

	[ContextMenu("Assume value of 'From'")]
	public override void SetCurrentValueToStart() { value = from; }

	[ContextMenu("Assume value of 'To'")]
	public override void SetCurrentValueToEnd() { value = to; }

	private Vector3 old_from, old_to;
	void Awake() { old_from = from; old_to = to; }
	protected override void Start()
	{
		if (fromIsCurrent)
		{
			from = value;
			if(RelativeScale)
			{
				to = from + (old_to-old_from);
			}
			else
				to = old_to;
		}
		base.Start();
	}
	public override void Play(bool forward)
	{
		if (fromIsCurrent)
		{
			if (forward)
			{
				from = value;
				if(RelativeScale)
				{
					to = from + (old_to-old_from);
				}
				else
					to = old_to;
			}
			else
			{
				to = value;
				if(RelativeScale)
				{
					from = to + (old_from-old_to);
				}
				else
					from = old_from;
			}
		}
		base.Play(forward);
	}
}
