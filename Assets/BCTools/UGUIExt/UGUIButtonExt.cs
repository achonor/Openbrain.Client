using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class UGUIButtonExt : MonoBehaviour
{
	[Tooltip("you can type anything here\njust let you remember\nthe function of this button")]
	public string describe;
	[SerializeField]
	UnityEvent m_MyEvent = new UnityEvent();
	Button mButton = null;
	public Button _Button
	{
		get
		{
			if (mButton == null)
				mButton = this.GetComponent<Button>();
			return mButton;
		}
	}
    [ContextMenu("OnClick")]
	public void OnClick()
	{
		if(_Button == null)return;
		_Button.onClick.Invoke();
	}
    [ContextMenu("DoMyEvent")]
    public void DoMyEvent()
	{
		m_MyEvent.Invoke();
	}
    [ContextMenu("Quit")]
    public void Quit()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
		Application.Quit();
	}
}
