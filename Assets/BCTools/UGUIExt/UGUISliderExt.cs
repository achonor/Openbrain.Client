using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UGUISliderExt : MonoBehaviour
{
	[HideInInspector]
	public int textMode = 0;
	//m/n模式下，最大值与实际值
	[SerializeField,HideInInspector]
	private int mRealValue, mMaxValue;
	public int RealValue
	{
		get { return mRealValue; }
		set
		{
			if (value < 0)
				mRealValue = 0;
			else
				mRealValue = value;
			RefreshTextMode0();
		}
	}
	public int MaxValue
	{
		get { return mMaxValue; }
		set
		{
			if (value <= 0)
			{
				mMaxValue = 1;
			}
			else
				mMaxValue = value;
			RefreshTextMode0();
		}
	}

	//百分比小数点后保留位数
	[HideInInspector]
	public int percentDotNum = 0;
	[SerializeField, HideInInspector]
	private float mPercentValue;
	public float PercentValue
	{
		get { return mPercentValue; }
		set
		{
			if (value < 0)
				mPercentValue = 0f;
			else
				mPercentValue = value;
			RefreshTextMode1();
		}
	}
	[HideInInspector]
	public bool BeyondRich = false;
	[HideInInspector]
	public Color BeyondColor;
	[HideInInspector]
	public bool NotBeyondRich = false;
	[HideInInspector]
	public Color NotBeyondColor;

	private bool DragMode = false;

	//-----------以下是要显示的--------------

	public Text textPart;
	public Slider sliderPart;

	void RefreshTextMode0()
	{
		float newvalue = mRealValue*1.0f/mMaxValue;
		if (sliderPart != null)
		{
			DragMode = false;
			sliderPart.value = newvalue;
		}
		if (textPart != null)
		{
			if (mRealValue > mMaxValue && BeyondRich)
			{
				textPart.text = GetColorString(this.BeyondColor) + mRealValue + "</color>/" + mMaxValue;
			}
			else if (mRealValue < mMaxValue && NotBeyondRich)
			{
				textPart.text = GetColorString(this.NotBeyondColor) + mRealValue + "</color>/" + mMaxValue;
			}
			else
			{
				textPart.text = string.Format("{0}/{1}", mRealValue, mMaxValue);
			}
		}
	}

	void RefreshTextMode1()
	{
		if (sliderPart != null)
		{
			DragMode = false;
			sliderPart.value = mPercentValue;
		}
		if (textPart != null)
		{
			if (mPercentValue > 1 && BeyondRich)
			{
				textPart.text = GetColorString(this.BeyondColor) + GetPercentValue(mPercentValue,this.percentDotNum) + "</color>%";
			}
			else if (mPercentValue < 1 && NotBeyondRich)
			{
				textPart.text = GetColorString(this.NotBeyondColor) + GetPercentValue(mPercentValue, this.percentDotNum) + "</color>%";
			}
			else
			{
				textPart.text = GetPercentValue(mPercentValue, this.percentDotNum) + "%";
			}
		}
	}

	public void OnSliderDrag()
	{
		if(this.sliderPart == null)return;
		if (!DragMode)
		{
			DragMode = true;
			return;
		}
		if (textMode == 0)
			RealValue = (int)(sliderPart.value * this.MaxValue + 0.5f);
		else if (textMode == 1)
			PercentValue = sliderPart.value;
	}

	string GetColorString(Color c)
	{
		return string.Format("<color=#{0:X2}{1:X2}{2:X2}>", (int)(255*c.r), (int)(255*c.g), (int)(255*c.b));
	}

	string GetPercentValue(float value,int DotNum)
	{
		float tmp = value * 100;
		if (DotNum == 1)
		{
			return string.Format("{0:0.0}", tmp);
		}
		else if (DotNum == 2)
		{
			return string.Format("{0:0.00}", tmp);
		}
		else if (DotNum == 3)
		{
			return string.Format("{0:0.000}", tmp);
		}
		else
		{
			return string.Format("{0:0}", (int)(tmp+0.5f));
		}
	}

	[ContextMenu("Test")]
	void Test()
	{
		Debug.Log(GetPercentValue(0.5f, 1));
		Debug.Log(GetPercentValue(0.5f, 2));
		Debug.Log(GetPercentValue(0.5f, 3));
		Debug.Log(GetPercentValue(0.5f, 0));
	}

}
