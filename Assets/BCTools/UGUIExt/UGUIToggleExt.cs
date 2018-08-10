using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UGUIToggleExt : MonoBehaviour
{
	Toggle m_toggle = null;
	public Toggle _Toggle
	{
		get
		{
			if (m_toggle == null)
				m_toggle = this.GetComponent<Toggle>();
			return m_toggle;
		}
	}
	[HelpBox(2,"when Toggle is single ,you can use these three")]
	public GameObject showWhenOn = null;
	[Tooltip("If this is on ,Do not need UGUI Toggle any more.You should Add EventTrigger component instead.")]
	public bool singleToggle = false;
	public bool IsSingleToggleOn = false;
	[SerializeField]
	UnityEvent m_OnToggleOn=new UnityEvent();
	[SerializeField]
	UnityEvent m_OnToggleOff=new UnityEvent();
	void Start()
	{
		if (singleToggle)
		{
			showWhenOn.SetActive(IsSingleToggleOn);
		}
		else
		{
			OnValueChange();
		}
	}
	void OnEnable()
	{
		if (singleToggle && showWhenOn != null)
		{
			showWhenOn.SetActive(IsSingleToggleOn);
		}
	}
	public void SetIsSingleTogOn(bool isOn)
	{
		this.IsSingleToggleOn = isOn;
		if (singleToggle && showWhenOn != null)
		{
			showWhenOn.SetActive(IsSingleToggleOn);
		}
	}
	//toggle will call this
	public void OnValueChange()
	{
		if (_Toggle == null) return;
		if (showWhenOn != null)
			showWhenOn.SetActive(_Toggle.isOn);
		if (_Toggle.isOn)
		{
			if (m_OnToggleOn != null)
				m_OnToggleOn.Invoke();
		}
		else
		{
			if(m_OnToggleOff != null)
				m_OnToggleOff.Invoke();
		}
	}


	public void OnSingleToggleClick()
	{
		if (IsSingleToggleOn)
		{
			IsSingleToggleOn = false;
			if (m_OnToggleOff != null)
				m_OnToggleOff.Invoke();
		}
		else
		{
			IsSingleToggleOn = true;
			if (m_OnToggleOn != null)
				m_OnToggleOn.Invoke();
		}
		if (showWhenOn != null)
			showWhenOn.SetActive(IsSingleToggleOn);
	}

	[ContextMenu("Test SwitchUGUIToggleOnForce()")]
	public void TurnOffUGUIToggleForce()
	{
		SwitchToggleOnForce(false);
	}

	public void TurnOnUGUIToggleForce()
	{
		SwitchToggleOnForce(true);
	}

	public void SwitchToggleOnForce(bool isON)
	{
		if (_Toggle == null) return;
		bool allowSwitchOff = false;
		if (_Toggle.group != null)
		{
			allowSwitchOff = _Toggle.group.allowSwitchOff;
			_Toggle.group.allowSwitchOff = true;
		}
		_Toggle.isOn = isON;
		if (_Toggle.group != null)
			_Toggle.group.allowSwitchOff = allowSwitchOff;
	}
}
