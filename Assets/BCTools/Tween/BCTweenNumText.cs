using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Tween the Text's num.
/// </summary>

public class BCTweenNumText : BCUITweener
{
	public int from;
	public int to;
	public Text uguiText;
	public AudioSource sound;
	public float soundStartTime = 0.1f;
	public int soundSkipCount = 0;
	public FenWei nFenWei = FenWei.None;
	public InsertChar insertChar = InsertChar.space;

	public enum FenWei
	{
		None,
		thousand,
		ten_thousand,
	}
	public enum InsertChar
	{
		space,
		Comma,
	}



	bool mCached = false;

	void Cache ()
	{
		mCached = true;
		if(uguiText == null)
		{
			uguiText = GetComponent<Text>();
		}
		if(sound == null)
			sound = GetComponent<AudioSource>();
	}

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public int value
	{
		get
		{
			if (!mCached) Cache();
			if(uguiText != null)
			{
				int rt = 0;
				string tempstr = uguiText.text;
				if(nFenWei != FenWei.None)
				{
					if(insertChar == InsertChar.space)
						tempstr = BCTools.ConvertToNormalNumStr(tempstr, " ");
					else if(insertChar == InsertChar.Comma)
						tempstr = BCTools.ConvertToNormalNumStr(tempstr, ",");
				}
				int.TryParse(tempstr, out rt);
				return rt;
			}
			return 0;
		}
		set
		{
			if (!mCached) Cache();
			setTextValue(value);
		}
	}
	void setTextValue(int value)
	{
		if (uguiText == null) return;
		string tempstr = "" + value;
		if (nFenWei != FenWei.None)
		{
			BCTools.ConvertToMoneyMode(ref tempstr, ((nFenWei == FenWei.thousand) ? 3 : 4), ((insertChar == InsertChar.space) ? " " : ","));
		}
		uguiText.text = tempstr;
	}
    new void OnDisable()
    {
        base.OnDisable();
        if (this.tweenFactor > 0.95f)
        {
            setTextValue(to);
        }
        else if (this.tweenFactor < 0.05f)
        {
            setTextValue(from);
        }
    }

    int soundSkip = 0;
	/// <summary>
	/// Tween the value.
	/// </summary>
	protected override void OnUpdate (float factor, bool isFinished) 
	{
		int oldValue = value;
		value = (int)Mathf.Lerp((float)from, (float)to, factor);
		if(sound != null && value != oldValue)
		{
			if(soundSkip == 0)
			{
				if(sound.isPlaying)
				{
					sound.time = soundStartTime;
				}
				else
					sound.Play();
			}
			soundSkip ++;
			if(soundSkip > this.soundSkipCount)
				soundSkip = 0;
		}
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public BCTweenNumText Begin (GameObject go, float duration, int Num)
	{
		BCTweenNumText comp = BCUITweener.Begin<BCTweenNumText>(go, duration);
		comp.from = comp.value;
		comp.to = Num;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	public override void Play (bool forward)
	{
		soundSkip = 0;
		base.Play (forward);
	}

	protected override void Start ()
	{
		soundSkip = 0;
		base.Start ();
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue() { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue() { to = value; }

	[ContextMenu("Assume value of 'From'")]
	public override void SetCurrentValueToStart() { value = from; }

	[ContextMenu("Assume value of 'To'")]
	public override void SetCurrentValueToEnd() { value = to; }
}
