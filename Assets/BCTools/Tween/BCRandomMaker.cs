using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BCRandomMaker : MonoBehaviour
{
	[SerializeField]
	List<RandomData> randomList = new List<RandomData>();
	[System.Serializable]
	public class RandomData
	{
		public string tag = "";
		public float min = 0, max = 0;
		public bool isInt = true;
		public List<FormulaData> formulaList = new List<FormulaData>();
		public float getRand()
		{
			float rt = 0f;
			if (isInt)
			{
				rt = (float)Random.Range((int)min, (int)max);
			}
			else
			{
				rt = Random.Range(min, max);
			}
            rt = calculateForm(rt);
			return rt;
		}
        public float calculateForm(float fvalue)
        {
            float rt = fvalue;
            for (int i = 0; formulaList != null && i < formulaList.Count; i++)
            {
                if (formulaList[i].mode.Equals("Add"))
                {
                    rt += formulaList[i].value;
                }
                else if (formulaList[i].mode.Equals("Minus"))
                {
                    rt -= formulaList[i].value;
                }
                else if (formulaList[i].mode.Equals("Multiply"))
                {
                    rt *= formulaList[i].value;
                }
                else if (formulaList[i].mode.Equals("Divide"))
                {
                    rt /= formulaList[i].value;
                }
                else if (formulaList[i].mode.Equals("Pow"))
                {
                    rt = Mathf.Pow(rt, formulaList[i].value);
                }
                else if (formulaList[i].mode.Equals("Mod"))
                {
                    rt = (float)((int)rt%(int)formulaList[i].value);
                }
            }
            return rt;
        }
	}
	[System.Serializable]
	public class FormulaData
	{
		[String2Enum("Add,Minus,Multiply,Divide,Pow,Mod")]
		public string mode;
		public float value = 1f;
	}
	
	void Start()
	{
        Random.InitState(System.DateTime.Now.Millisecond);
        //Random.seed = System.DateTime.Now.Millisecond;
	}
	public float GetRand(string tag)
	{
		float rt = 0f;
		for (int i = 0; i < randomList.Count; i++)
		{
			if (randomList[i].tag.Equals(tag))
			{
				rt = randomList[i].getRand();
				break;
			}
		}
		return rt;
	}

	public float GetRand(int index)
	{
		float rt = 0f;
		if (randomList != null && randomList.Count > index)
		{
			rt = randomList[index].getRand();
		}
		return rt;
	}

	public float GetRand()
	{
		return GetRand(0);
	}

    //仅利用公式部分，而忽略随机数的生成
    public float CalculateForm(int index, float value)
    {
        float rt = 0f;
        if (randomList != null && randomList.Count > index)
        {
            rt = randomList[index].calculateForm(value);
        }
        return rt;
    }

    public float CalculateForm(string tag, float value)
    {
        float rt = 0f;
        for (int i = 0; i < randomList.Count; i++)
        {
            if (randomList[i].tag.Equals(tag))
            {
                rt = randomList[i].calculateForm(value);
                break;
            }
        }
        return rt;
    }
	#if UNITY_EDITOR
	[ContextMenu("test")]
	void test()
	{
		EditorUtility.DisplayDialog("Rand Num Is:", ""+GetRand(), "OK");
	}
	#endif



	public static int GetRandByWeight(int[] weight)//return [0,weight.Length)
	{
		int randMax = 0;
		for (int i = 0; i < weight.Length; i++)
		{
			randMax += weight[i];
		}
		int randValue = Random.Range(0, randMax-1);//rand is [0, randMax)
		int tempi = 0;
		for (int i = 0; i < weight.Length; i++)
		{
			tempi += weight[i];
			if (randValue < tempi) return i;
		}
		return 0;
	}
}
