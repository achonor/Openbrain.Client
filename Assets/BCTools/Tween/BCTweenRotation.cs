using UnityEngine;

/// <summary>
/// Tween the object's rotation.
/// </summary>

public class BCTweenRotation : BCUITweener
{
	public Vector3 from;
	public Vector3 to;
	public bool quaternionLerp = false;
	[HideInInspector]
	public bool fromIsCurrent = false;
	[HideInInspector]
	public bool RelativeRotate = false;
	[HideInInspector]
	public bool Line_2D_to_3D = false;
	public Transform leftLine;
	public Transform rightLine;


	public Transform mTrans;

	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public Quaternion value { get { return cachedTransform.localRotation; } set { cachedTransform.localRotation = value; } }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = quaternionLerp ? Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), factor) :
			Quaternion.Euler(new Vector3(
			/*Mathf.Lerp(from.x, to.x, factor),
			Mathf.Lerp(from.y, to.y, factor),
			Mathf.Lerp(from.z, to.z, factor)
			 */
			from.x*(1-factor) + to.x*factor,
			from.y*(1-factor) + to.y*factor,
			from.z*(1-factor) + to.z*factor
			));
		
		if(Line_2D_to_3D)
		{
			if (leftLine == null || rightLine == null) return;
			bool isfront = leftLine.position.z < rightLine.position.z;
			leftLine.gameObject.SetActive(isfront);
			rightLine.gameObject.SetActive(!isfront);
		}
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public BCTweenRotation Begin (GameObject go, float duration, Quaternion rot)
	{
		BCTweenRotation comp = BCUITweener.Begin<BCTweenRotation>(go, duration);
		comp.from = comp.value.eulerAngles;
		comp.to = rot.eulerAngles;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value.eulerAngles; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value.eulerAngles; }

	[ContextMenu("Assume value of 'From'")]
	public override void SetCurrentValueToStart() { value = Quaternion.Euler(from); }

	[ContextMenu("Assume value of 'To'")]
	public override void SetCurrentValueToEnd() { value = Quaternion.Euler(to); }

	private Vector3 old_from, old_to;
	void Awake() { old_from = from; old_to = to; }
	protected override void Start()
	{
		if (fromIsCurrent)
		{
			from = value.eulerAngles;
			if(RelativeRotate)
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
				from = value.eulerAngles;
				if(RelativeRotate)
				{
					to = from + (old_to-old_from);
				}
				else
					to = old_to;
			}
			else
			{
				to = value.eulerAngles;
				if(RelativeRotate)
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
