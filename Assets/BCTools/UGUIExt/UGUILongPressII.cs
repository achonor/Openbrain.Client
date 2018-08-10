using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class UGUILongPressII :MonoBehaviour
{
	public float interval=0.1f;

	[SerializeField]
	UnityEvent m_OnLongpress=new UnityEvent();


	private bool isPointDown=false;
	private float lastInvokeTime;
	
	// Update is called once per frame
	void Update ()
	{
		if(isPointDown)
		{
			if(Time.time-lastInvokeTime>interval)
			{
				//触发点击;
				m_OnLongpress.Invoke();
				isPointDown = false;
			}
		}

	}


	//OnPointerDown,OnPointerUp,OnPointerExit,OnDrag  should be called by EventTrigger Component outside
	public void OnPointerDown ()
	{
		isPointDown = true;
		lastInvokeTime = Time.time;
	}

	public void OnPointerUp ()
	{
		isPointDown = false;
	}

	public void OnPointerExit ()
	{
		isPointDown = false;
	}

	public void OnDrag()
	{
		isPointDown = false;
	}
}
