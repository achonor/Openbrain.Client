using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UGUITypingEffect : MonoBehaviour
{
	public Text m_text = null;
	public int mSecondPerWord = 240;
	public AudioSource BeginSound;
	public float BeginSoundStartTime;
	public AudioSource playingSound;
	public float PlayingSoundStartTime;
	
	private string valueStr = "";
	private string tempStr = "";
	private bool isPlaying = false;
	public bool IsPlaying
	{
		get{
			return this.isPlaying;
		}
	}
	
	[SerializeField]
	UnityEvent m_OnFinish=new UnityEvent();
	
	[ContextMenu("Play")]
	public void PlayCurrentText()
	{
		Play(m_text.text);
	}
	public void Play(string str)
	{
		if(str.Length < 1)return; //空字符串
		str = str.Replace("\\n", "\n");
		valueStr = str;
		mfactor = 1;
		tempStr = "";
		endstring = "";
		SetTempStr();
		timer = 0f-mSecondPerWord*1.0f/1000;
		isPlaying = true;
		if(BeginSound == null || BeginSound.clip == null)return;
		BeginSound.Play();
		BeginSound.time = BeginSoundStartTime;
	}
	public void JumpToEnd()
	{
		mfactor = valueStr.Length; //超大数直接读完
		if (mfactor < 1) mfactor = 1;
	}

	string endstring = "";
	void SetTempStr()//0 ~ length-1,index of valueStr
	{
		if(checkStart("color")){}
		else if(checkStart("size")){}
		else if(checkStart("b")){}
		else if(checkStart("i")){}
		else
		{
			tempStr = valueStr.Substring(0,mfactor);
			mfactor ++;
		}
		m_text.text = tempStr+endstring;

		if(mfactor > valueStr.Length)
		{
			isPlaying = false;
			m_OnFinish.Invoke();
		}
	}
	bool checkStart(string tag)//tag 比如 color
	{
		//string hasShow = valueStr.Substring(0,mfactor);
		string willShow = valueStr.Substring(mfactor);
		string endTag = "</"+tag+">";
		if(willShow.StartsWith("<"+tag))
		{
			int tagLeng = willShow.IndexOf(">")+1;
			//string willShowRemoveTag = willShow.Substring(tagLeng);
			mfactor += tagLeng;
			endstring = endstring.Insert(0,endTag);
			if(checkStart("color")){}
			else if(checkStart("size")){}
			else if(checkStart("b")){}
			else if(checkStart("i")){}
			else
			{
				return false;
			}
			return true;
		}
		else if(willShow.StartsWith(endTag))
		{
			int endleng = endTag.Length;//"</color>"的长度
			mfactor += endleng;
			endstring = endstring.Replace(endTag,"");
			tempStr += endTag;
			if(checkStart("color")){}
			else if(checkStart("size")){}
			else if(checkStart("b")){}
			else if(checkStart("i")){}
			else
			{
				return false;
			}
			return true;
		}
		return false;
	}

	int mfactor = 0;
	float timer = 0f; 
	void Update()
	{
		if(!isPlaying)return;
		if(timer*1000 < mSecondPerWord)
		{
			timer += Time.deltaTime;
		}
		else
		{
			SetTempStr();
			timer = 0f;
			if(playingSound == null || playingSound.clip == null)return;
			if(!playingSound.isPlaying)
				playingSound.Play();
			playingSound.time = PlayingSoundStartTime;
		}
	}
}
