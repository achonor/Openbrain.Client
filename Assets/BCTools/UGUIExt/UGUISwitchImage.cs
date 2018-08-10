using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UGUISwitchImage : MonoBehaviour
{
	public int defaultIndex = -1;
	public Sprite[] spriteArr;
	public Image[] imgsCtr;

	public bool SetAllImg2Sprite(int index)//成功返回true
	{
		if (spriteArr == null || index < 0 || index >= spriteArr.Length || spriteArr[index] == null || imgsCtr == null) return false;
		for (int i = 0; i < imgsCtr.Length; i++)
		{
			if(imgsCtr[i].sprite != spriteArr[index])
				imgsCtr[i].sprite = spriteArr[index];
		}
		return true;
	}
	void OnEnable()
	{
		if (defaultIndex > -1)
		{
			SetAllImg2Sprite(defaultIndex);
		}
	}

	public bool SetImg2Sprite(int index, Image img)//成功返回true
	{
		if (spriteArr == null || index < 0 || index >= spriteArr.Length || spriteArr[index] == null) return false;
		img.sprite = spriteArr[index];
		return true;
	}
	public bool SetImg2Sprite(int index, int imgIndex)//成功返回true
	{
		if (spriteArr == null || index < 0 || index >= spriteArr.Length || spriteArr[index] == null || imgsCtr == null || imgsCtr[imgIndex] == null) return false;
		imgsCtr[imgIndex].sprite = spriteArr[index];
		return true;
	}
}
