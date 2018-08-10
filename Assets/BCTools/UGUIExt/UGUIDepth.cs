using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UGUIDepth : MonoBehaviour
{
	public int order;
	public bool isUI = true;
	public bool useParticle = true;
	[ContextMenu("Set Again")]
	void Start () 
	{
		if(isUI){
			Canvas canvas = GetComponent<Canvas>();
			if( canvas == null){
				canvas = gameObject.AddComponent<Canvas>();
			}
			
			GraphicRaycaster ray = GetComponent<GraphicRaycaster>();
			if (ray == null){
				ray = gameObject.AddComponent<GraphicRaycaster>();
			}

			canvas.overrideSorting = true;
			canvas.sortingOrder = order;
		}
		if (useParticle)
		{
			ParticleSystem[] paticles = this.GetComponentsInChildren<ParticleSystem>(true);
			if (paticles != null)
			{
				for (int i = 0; i < paticles.Length; i++)
				{
					Renderer render = paticles[i].GetComponent<Renderer>();
					if (render != null)
					{
						render.sortingOrder = order;
					}
				}
			}
		}
	}
	public void SetAgain()//when do this ,better not enabled
	{
		Start();
	}
	public void Clear()//when do this ,better not enabled
	{
		GraphicRaycaster gr = this.GetComponent<GraphicRaycaster>();
		if (gr != null)
		{
			MonoBehaviour.Destroy(gr);
		}
		Canvas cv = this.GetComponent<Canvas>();
		if (cv != null)
		{
			MonoBehaviour.Destroy(cv);
		}
	}
}
