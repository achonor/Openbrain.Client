using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Tween the object's color.
/// </summary>

public class BCTweenColor : BCUITweener
{
	[HideInInspector]
	public Color from = Color.white;
	[HideInInspector]
	public Color to = Color.white;
	[HideInInspector]
	public bool UGUIMode = false;
	public Image[] uguiImages;
	public Text[] uguiTexts;
	public CanvasRenderer[] uguiCanvasRens;
	public Renderer[] uguiModeRenders;

	bool mCached = false;
	Material mMat;
	Light mLight;
	SpriteRenderer mSr;

	void Cache ()
	{
		mCached = true;
		if (UGUIMode)
		{
			return;
		}


		mSr = GetComponent<SpriteRenderer>();
		if (mSr != null) return;

#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
		Renderer ren = renderer;
#else
		Renderer ren = GetComponent<Renderer>();
#endif
		if (ren != null)
		{
			mMat = ren.material;
			return;
		}

#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
		mLight = light;
#else
		mLight = GetComponent<Light>();
#endif
	}

	[System.Obsolete("Use 'value' instead")]
	public Color color { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public Color value
	{
		get
		{
			if (!mCached) Cache();
			if (UGUIMode)
			{
				if (uguiImages != null && uguiImages.Length > 0 && uguiImages[0]!= null)
					return uguiImages[0].color;
				else if (uguiTexts != null && uguiTexts.Length > 0 && uguiTexts[0] != null)
					return uguiTexts[0].color;
				else if (uguiCanvasRens != null && uguiCanvasRens.Length > 0 && uguiCanvasRens[0] != null)
					return uguiCanvasRens[0].GetColor();
				else if (uguiModeRenders != null && uguiModeRenders.Length > 0 && uguiModeRenders[0] != null)
				{
					if (uguiModeRenders[0] is SpriteRenderer)
					{
						return (uguiModeRenders[0] as SpriteRenderer).color;
					}
					else if (uguiModeRenders[0].material != null)
					{
						return uguiModeRenders[0].material.color;
					}
				}
			}
			else if (mMat != null) return mMat.color;
			else if (mSr != null) return mSr.color;
			else if (mLight != null) return mLight.color;
			return Color.black;
		}
		set
		{
			if (!mCached) Cache();
			if (UGUIMode)
			{
				if (uguiImages != null)
				{
					for (int i = 0; i < uguiImages.Length; i++)
					{
						if (uguiImages[i] == null) continue;
						uguiImages[i].color = value;
					}
				}
				if (uguiTexts != null)
				{
					for (int i = 0; i < uguiTexts.Length; i++)
					{
						if (uguiTexts[i] == null) continue;
						uguiTexts[i].color = value;
					}
				}
				if (uguiCanvasRens != null)
				{
					for (int i = 0; i < uguiCanvasRens.Length; i++)
					{
						if (uguiCanvasRens[i] == null) continue;
						uguiCanvasRens[i].SetColor(value);
					}
				}
				if (uguiModeRenders != null)
				{
					for (int i = 0; i < uguiModeRenders.Length; i++)
					{
						if (uguiModeRenders[i] == null) continue;
						if (uguiModeRenders[i] is SpriteRenderer)
							(uguiModeRenders[i] as SpriteRenderer).color = value;
						else if (uguiModeRenders[i].material != null)
							uguiModeRenders[i].material.color = value;
					}
				}
			}
			else if (mMat != null) mMat.color = value;
			else if (mSr != null) mSr.color = value;
			else if (mLight != null)
			{
				mLight.color = value;
				mLight.enabled = (value.r + value.g + value.b) > 0.01f;
			}
		}
	}

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) { value = Color.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public BCTweenColor Begin (GameObject go, float duration, Color color)
	{
#if UNITY_EDITOR
		if (!Application.isPlaying) return null;
#endif
		BCTweenColor comp = BCUITweener.Begin<BCTweenColor>(go, duration);
		comp.from = comp.value;
		comp.to = color;

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
}
