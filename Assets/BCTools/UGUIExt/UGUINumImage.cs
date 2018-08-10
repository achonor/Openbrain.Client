﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UGUINumImage : MonoBehaviour
{
	[SerializeField,HideInInspector]
	private int num;
	public int Num
	{
		get{
			return this.num;
		}
		set{
			this.num = value;
			if(this.num <0)
				this.num = 0;
			SetImages();
		}
	}
	public List<Sprite> numSpriteList = new List<Sprite>();
	public List<Image> imageList = new List<Image>();
	void Start()
	{
		SetImages();
	}
	void SetImages()
	{
		string numstr = ""+num;
		for(int i=0;i<numstr.Length;i++)
		{
			int n = (int)(numstr[i] - '0');
			//防越界
			if(i>=imageList.Count || n>=numSpriteList.Count)
				break;
			if(imageList[i].sprite != numSpriteList[n])
				imageList[i].sprite = numSpriteList[n];
			if(!imageList[i].gameObject.activeSelf)
				imageList[i].gameObject.SetActive(true);
		}
		for(int j=numstr.Length;j<imageList.Count;j++)
		{
			if(imageList[j].gameObject.activeSelf)
				imageList[j].gameObject.SetActive(false);
		}
	}
}
