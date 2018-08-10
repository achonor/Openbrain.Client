using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[RequireComponent(typeof(ScrollRect))]
public class UGUIScrollRectEveSys : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
	[HelpBox(2, "You must add this.OnScrollValueChange() to ScrollRect.OnValueChanged() ")]
	[Range(0.05f, 0.6f)]
	public float BeyondValue = 0.28f;
	ScrollRect m_Scroll = null;
	ScrollRect myScroll{
		get{
			if(null == m_Scroll)
			{
				m_Scroll = this.GetComponent<ScrollRect>();
			}
			return m_Scroll;
		}
		set{m_Scroll = value;}
	}

	//Dont write this Rate,keep it 0.5
	public float Rate = 0.5f;
	public void OnScrollValueChange()
	{
		if (myScroll.horizontal)
			Rate = myScroll.horizontalNormalizedPosition;
		else if (myScroll.vertical)
			Rate = 1f-myScroll.verticalNormalizedPosition;
		else
		{
			return;
		}

		//judge endDrag
		if (canEventWork)
		{
			if (Rate < (0f-BeyondValue))
			{
				onBeyondBegin.Invoke();
				canEventWork = false;
				//Debug.Log("onBeyondBegin");
			}
			else if (Rate > (1+BeyondValue))
			{
				onBeyondEnd.Invoke();
				canEventWork = false;
				//Debug.Log("onBeyondEnd");
			}
		}
	}

	[SerializeField]
	UnityEvent onBeyondBegin = new UnityEvent();
	[SerializeField]
	UnityEvent onBeyondEnd = new UnityEvent();
	[SerializeField]
	UnityEvent onEndDrag = new UnityEvent();

	bool canEventWork = false;
	public void OnEndDrag(PointerEventData eventData)
	{
		canEventWork = false;
		onEndDrag.Invoke();
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		canEventWork = true;
	}

	void OnEnable()
	{
		//init
		canEventWork = false;
		Rate = 0.5f;
	}	
}
