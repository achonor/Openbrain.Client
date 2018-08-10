using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Tween the object's alpha. Works with both UI widgets as well as renderers.
/// </summary>

public class BCTweenAlpha : BCUITweener
{
	[HideInInspector]
	[Range(0f, 1f)] public float from = 1f;
	[HideInInspector]
	[Range(0f, 1f)] public float to = 1f;
	[HideInInspector]
	public bool UGUIMode = false;
	public Image[] uguiImages;
	public Text[] uguiTexts;
	public CanvasRenderer[] uguiCanvasRens;
	public CanvasGroup[] uguiCanvasGroups;
	public Renderer[] uguiModeRenders;

	bool mCached = false;
	Material mMat;
	SpriteRenderer mSr;

	[System.Obsolete("Use 'value' instead")]
	public float alpha { get { return this.value; } set { this.value = value; } }

	void Cache ()
	{
		mCached = true;
		if(UGUIMode)
		{
			return;
		}
		mSr = GetComponent<SpriteRenderer>();

		if (mSr == null)
		{
			Renderer ren = GetComponent<Renderer>();
			if (ren != null) mMat = ren.material;
		}
	}

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public float value
	{
		get
		{
			if (!mCached) Cache();
			if(UGUIMode)
			{
				if (uguiImages != null && uguiImages.Length > 0 && uguiImages[0] != null)
					return uguiImages[0].color.a;
				else if (uguiTexts != null && uguiTexts.Length > 0 && uguiTexts[0] != null)
					return uguiTexts[0].color.a;
				else if (uguiCanvasRens != null && uguiCanvasRens.Length > 0 && uguiCanvasRens[0] != null)
				{
					return uguiCanvasRens[0].GetAlpha();
				}
				else if (uguiCanvasGroups != null && uguiCanvasGroups.Length > 0 && uguiCanvasGroups[0] != null)
				{
					return uguiCanvasGroups[0].alpha;
				}
				else if (uguiModeRenders != null && uguiModeRenders.Length > 0 && uguiModeRenders[0] != null)
				{
					if (uguiModeRenders[0] is SpriteRenderer)
					{
						return (uguiModeRenders[0] as SpriteRenderer).color.a;
					}
					else if (uguiModeRenders[0].material != null)
					{
						return uguiModeRenders[0].material.color.a;
					}
				}
			}

			else if (mSr != null) return mSr.color.a;
			else if (mMat != null) return mMat.color.a;
			
			return 1f;
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
						Color tempcolor = uguiImages[i].color;
						tempcolor.a = value;
						uguiImages[i].color = tempcolor;
					}
				}
				if (uguiTexts != null)
				{
					for (int i = 0; i < uguiTexts.Length; i++)
					{
						if (uguiTexts[i] == null) continue;
						Color tempcolor = uguiTexts[i].color;
						tempcolor.a = value;
						uguiTexts[i].color = tempcolor;
					}
				}
				if (uguiCanvasRens != null)
				{
					for (int i = 0; i < uguiCanvasRens.Length; i++)
					{
						if (uguiCanvasRens[i] == null) continue;
						uguiCanvasRens[i].SetAlpha(value);
					}
				}
				if (uguiCanvasGroups != null)
				{
					for (int i = 0; i < uguiCanvasGroups.Length; i++)
					{
						if (uguiCanvasGroups[i] == null) continue;
						uguiCanvasGroups[i].alpha = value;
					}
				}
				if (uguiModeRenders != null)
				{
					for (int i = 0; i < uguiModeRenders.Length; i++)
					{
						if (uguiModeRenders[i] == null) continue;
						if (uguiModeRenders[i] is SpriteRenderer)
						{
							SpriteRenderer srd = uguiModeRenders[i] as SpriteRenderer;
							Color tempcolor = srd.color;
							tempcolor.a = value;
							srd.color = tempcolor;
						}
						else if (uguiModeRenders[i].material != null)
						{
							Color tempcolor = uguiModeRenders[i].material.color;
							tempcolor.a = value;
							uguiModeRenders[i].material.color = tempcolor;
						}
					}
				}
			}
			else if (mSr != null)
			{
				Color c = mSr.color;
				c.a = value;
				mSr.color = c;
			}
			else if (mMat != null)
			{
				Color c = mMat.color;
				c.a = value;
				mMat.color = c;
			}
		}
	}

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public BCTweenAlpha Begin (GameObject go, float duration, float alpha)
	{
		BCTweenAlpha comp = BCUITweener.Begin<BCTweenAlpha>(go, duration);
		comp.from = comp.value;
		comp.to = alpha;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
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
