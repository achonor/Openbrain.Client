﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class UGUILongPressEventTrigger :Selectable,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler,IDragHandler
{
	public float interval=0.1f;

	[SerializeField] private UnityEvent m_OnLongpress = new UnityEvent();
    [SerializeField] private UnityEvent m_OnElseClick = new UnityEvent();


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

	public new void OnPointerDown (PointerEventData eventData)
	{
        base.OnPointerDown(eventData);
		isPointDown = true;
		lastInvokeTime = Time.time;
	}

	public new void OnPointerUp (PointerEventData eventData)
	{
        base.OnPointerUp(eventData);
        if(isPointDown)
        {
            if(Time.time - lastInvokeTime < interval)
            {
                m_OnElseClick.Invoke();
            }
        }
		isPointDown = false;
	}

	public new void OnPointerExit (PointerEventData eventData)
	{
        base.OnPointerExit(eventData);
		isPointDown = false;
	}

	public void OnDrag (PointerEventData eventData)
	{
		isPointDown = false;
	}
}
