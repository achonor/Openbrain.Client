using UnityEngine;
using System.Collections;
//像花瓣一样的圆形环绕，每一瓣都是一个父物体套一个子物体，父物体转z轴形成环绕，子物体pos仅y有值作为距离
//这个脚本所在物体，必须是所有花瓣的直接父物体
public class CircleLayout : MonoBehaviour
{
	public Transform[] ctrTrans;//花瓣中的父物体
	public float distance;
	public bool clockwise = true;
    public bool ignoreHideTran = true;//ctrTrans中，若有自身隐藏者将不参与环状排列
	public void setCircle()
	{
        if (ctrTrans == null || ctrTrans.Length < 1) return;
		float direction = clockwise ? 1f : -1f;
	    int realLength = 0;
	    if (ignoreHideTran)
	    {
	        for (int i = 0; i < ctrTrans.Length; i++)
	        {
	            if (ctrTrans[i].gameObject.activeSelf)
	                realLength ++;
	        }
	    }
	    else
	    {
	        realLength = ctrTrans.Length;
	    }

	    for (int i = 0,j=0; i < ctrTrans.Length; i++)
		{
		    if (ignoreHideTran && !ctrTrans[i].gameObject.activeSelf)
		    {
		        continue;
		    }
			ctrTrans[i].localPosition = Vector3.zero;
			ctrTrans[i].localScale = Vector3.one;
			ctrTrans[i].localRotation = Quaternion.Euler(new Vector3(0, 0, 360f * j / realLength * direction));
			Transform child = ctrTrans[i].GetChild(0);
			if (child != null)
			{
				child.localPosition = new Vector3(0, distance, 0);
			}
		    j++;
		}
	}

	public void setChildScale(Vector3 value)
	{
		for (int i = 0; i < ctrTrans.Length; i++)
		{
			Transform child = ctrTrans[i].GetChild(0);
			if (child != null)
			{
				child.localScale = value;
			}
		}
	}

	public void setChildRotation(Vector3 value)
	{
		for (int i = 0; i < ctrTrans.Length; i++)
		{
			Transform child = ctrTrans[i].GetChild(0);
			if (child != null)
			{
				child.localRotation = Quaternion.Euler(value);
			}
		}
	}

	public void setChildWorldRotation(Vector3 value)
	{
		for (int i = 0; i < ctrTrans.Length; i++)
		{
			Transform child = ctrTrans[i].GetChild(0);
			if (child != null)
			{
				child.rotation = Quaternion.Euler(value);
			}
		}
	}

	public void fillTransArrBySibling()
	{
		ctrTrans = new Transform[this.transform.childCount];
		for (int i = 0; i < ctrTrans.Length; i++)
		{
			Transform child = transform.GetChild(i);
			int index = child.GetSiblingIndex();
			ctrTrans[index] = child;
		}
	}
}
