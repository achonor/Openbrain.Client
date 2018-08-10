using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HelpBoxAttribute : PropertyAttribute
{
	public int line;
	public string note;
#if UNITY_EDITOR
	public MessageType messageType;
	public HelpBoxAttribute(int line,string note,MessageType messageType)
	{
		this.line = line;
		this.note = note;
		this.messageType = messageType;
		checkwrong();
	}
	public HelpBoxAttribute(int line,string note)
	{
		this.line = line;
		this.note = note;
		this.messageType = MessageType.Info;
		checkwrong();
	}
	public HelpBoxAttribute(int line,string note,int messageType)
	{
		this.line = line;
		this.note = note;
		this.messageType = (MessageType)messageType;
		checkwrong();
	}
#else
	public HelpBoxAttribute(int line,string note){}
	public HelpBoxAttribute(int line,string note,int messageType){}
#endif
	void checkwrong()
	{
		if(this.line <2)
		{
			this.line = 2;
		}
	}
}
