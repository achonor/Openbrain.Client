using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
public class BCEnableTool : MonoBehaviour
{
	public bool needEnable = true;
	public bool needDisable = false;
	public bool needStart = false;
	public bool needUpdate = false;
	public bool needPerSecond = false;
	public float perSecondN = 1f;
	[SerializeField]
	public UnityEvent m_OnEnable=new UnityEvent();
	[SerializeField]
	public UnityEvent m_OnDisable=new UnityEvent();
	[SerializeField]
	public UnityEvent m_OnStart=new UnityEvent();
	[SerializeField]
	public UnityEvent m_OnUpdate=new UnityEvent();
	[SerializeField]
	public UnityEvent m_OnUpdatePerSec=new UnityEvent();
	
	public void OnEnable()
	{
		if(!needEnable)return;
		m_OnEnable.Invoke();
	}
	public void OnDisable()
	{
		if(!needDisable)return;
		m_OnDisable.Invoke();
	}
	public void Start()
	{
		if(!needStart)return;
		m_OnStart.Invoke();
	}
	float secondCounter = 999f;
	void Update()
	{
		if (needUpdate)
		{
			m_OnUpdate.Invoke();
		}
		if (needPerSecond)
		{
			secondCounter += Time.deltaTime;
			if (secondCounter >= this.perSecondN)
			{
				m_OnUpdatePerSec.Invoke();
				secondCounter = 0f;
			}
		}
	}
}
