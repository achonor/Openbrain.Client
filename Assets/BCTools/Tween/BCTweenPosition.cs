using UnityEngine;

/// <summary>
/// Tween the object's position.
/// </summary>

public class BCTweenPosition : BCUITweener
{
	public Vector3 from;
	public Vector3 to;
	[HideInInspector]
	public bool fromIsCurrent = false;
    [HideInInspector]
    public bool RelativeMove = false;

	[HideInInspector]
	public bool LockX = false;
	[HideInInspector]
	public bool LockY = false;
	[HideInInspector]
	public bool LockZ = false;

	//---------使用实际物体控制起始坐标---------
	[SerializeField]
	private bool useTransform = false;
	public bool UseTransform
	{
		get { return useTransform; }
		set
		{
			useTransform = value;
			if (useTransform)
			{
				fromIsCurrent = false;
				RelativeMove = false;
				LockX = false;
				LockY = false;
				LockZ = false;
			}
		}
	}
	[HideInInspector]
	public Transform fromTran;
	[HideInInspector]
	public Transform toTran;

	//---------------for PathCurve---------------
	[HideInInspector]
	public bool UsePathCurve = false;
	[HideInInspector]
	public AnimationCurve pathCurve = new AnimationCurve(new Keyframe(0f, 1f, 0f, 0f), new Keyframe(1f, 1f, 0f, 0f));
	private float mdistance = 0;
	private Vector3 mLeftNormalVector = Vector3.zero;
	//---------------for PathCurve---------------

	public Transform mTrans;

	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public Vector3 value
	{
		get
		{
			return cachedTransform.localPosition;
		}
		set
		{
			cachedTransform.localPosition = value;
		}
	}

	void Awake() { old_from = from; old_to = to; }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) 
	{
		if(!UsePathCurve)
			value = from * (1f - factor) + to * factor;
		else
		{
			Vector3 temp = from * (1f - factor) + to * factor;
			value = mdistance * ((pathCurve.Evaluate(this.tweenFactor)-1)*6) * mLeftNormalVector + temp;
		}
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public BCTweenPosition Begin (GameObject go, float duration, Vector3 pos)
	{
		BCTweenPosition comp = BCUITweener.Begin<BCTweenPosition>(go, duration);
		comp.from = comp.value;
		comp.to = pos;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
	public override void DoOnUpdate(float factor, bool isFinished)
	{
		if (!alreadySetFrom)
		{
			Start();
			alreadySetFrom = true;
		}
		base.DoOnUpdate(factor, isFinished);
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value; }

	[ContextMenu("Assume value of 'From'")]
	public override void SetCurrentValueToStart() { value = from; }

	[ContextMenu("Assume value of 'To'")]
	public override void SetCurrentValueToEnd() { value = to; }

	private Vector3 old_from,old_to;
    public Vector3 Old_from
    {
        set { old_from = value; }
        get { return old_from; }
    }
    public Vector3 Old_to
    {
        set { old_to = value; }
        get { return old_to; }
    }
	protected override void Start()
	{
		if (!alreadySetFrom)
		{
			if (UseTransform && mTrans != null)
			{
				Vector3 pos = mTrans.localPosition;
				if (fromTran != null)
				{
					mTrans.position = fromTran.position;
					from = mTrans.localPosition;
				}
				if (toTran != null)
				{
					mTrans.position = toTran.position;
					to = mTrans.localPosition;
				}
				mTrans.localPosition = pos;
			}
			else if (fromIsCurrent)
			{
				from = value;
				if (RelativeMove)
				{
					to = from + (old_to - old_from);
				}
				else
					to = old_to;
			}
		}
		checkLockAxis(true);

		if (UsePathCurve)
		{
			mdistance = Vector2.Distance(from, to);
			Vector3 temp = to - from;
			mLeftNormalVector.x = -1f * temp.y;
			mLeftNormalVector.y = temp.x;
			mLeftNormalVector.Normalize();
		}

		base.Start();
	}

	bool alreadySetFrom = false; //Play执行过之后，Start就不必重复执行了
	public override void Play(bool forward)
	{
		alreadySetFrom = true;
		if (UseTransform && mTrans != null)
		{
			Vector3 pos = mTrans.localPosition;
			if (fromTran != null)
			{
				mTrans.position = fromTran.position;
				from = mTrans.localPosition;
			}
			if (toTran != null)
			{
				mTrans.position = toTran.position;
				to = mTrans.localPosition;
			}
			mTrans.localPosition = pos;
		}
		else if (fromIsCurrent)
		{
			if (forward)
			{
				from = value;
                if(RelativeMove)
                {
                    to = from + (old_to-old_from);
                }
                else
				    to = old_to;
			}
			else
			{
				to = value;
                if(RelativeMove)
                {
                    from = to + (old_from-old_to);
                }
				else
                    from = old_from;
			}
		}
		checkLockAxis(forward);

		if (UsePathCurve)
		{
			mdistance = Vector2.Distance(from, to);
			Vector3 temp = to - from;
			mLeftNormalVector.x = -1f * temp.y;
			mLeftNormalVector.y = temp.x;
			mLeftNormalVector.Normalize();
		}

		base.Play(forward);
	}
	void checkLockAxis(bool forward)
	{
		if(forward)
		{
			if (LockX)
			{
				to.x = from.x;
			}
			if (LockY)
			{
				to.y = from.y;
			}
			if (LockZ)
			{
				to.z = from.z;
			}
		}
		else
		{
			if (LockX)
			{
				from.x = to.x;
			}
			if (LockY)
			{
				from.y = to.y;
			}
			if (LockZ)
			{
				from.z = to.z;
			}
		}
	}
}
