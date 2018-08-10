using UnityEngine;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// These curves maybe useful when you create tweener dynamically
/// </summary>

public class BCDicOfObj : MonoBehaviour
{
    [HideInInspector] public string tip = "";
	[Serializable]
	public class ObjData
	{
		public string Desc;
		public Object obj;
	}
	public List<ObjData> objList = new List<ObjData>();
	public ObjData GetObjByTag(string tag)
	{
		for (int i = 0; i < objList.Count; i++)
		{
			if (objList[i].Desc.StartsWith(tag))
			{
				return objList[i];
			}
		}
		return null;
	}
	public List<ObjData> FindObjsByName(string name)
	{
		List<ObjData> rt = new List<ObjData>();
		if (string.IsNullOrEmpty(name))
		{
			for (int i = 0; i < objList.Count; i++)
			{
				if (objList[i].obj == null)
				{
					rt.Add(objList[i]);
				}
			}
			return rt;
		}
		for (int i = 0; i < objList.Count; i++)
		{
			if (objList[i].obj != null && objList[i].obj.name.Contains(name))
			{
				rt.Add(objList[i]);
			}
		}
		return rt;
	}
	public List<ObjData> FindObjsByTag(string tag)
	{
		List<ObjData> rt = new List<ObjData>();
		if (string.IsNullOrEmpty(tag)) return rt;
		for (int i = 0; i < objList.Count; i++)
		{
			if (objList[i] != null && !string.IsNullOrEmpty(objList[i].Desc) && objList[i].Desc.Contains(tag))
			{
				rt.Add(objList[i]);
			}
		}
		return rt;
	}

    public GameObject GetGo(string tag)
    {
        GameObject go = null;
        Object obj = GetObj(tag);
        if (obj != null)
        {
            go = obj as GameObject;
        }
        return go;
    }

    public Transform GetTran(string tag)
    {
        GameObject go = GetGo(tag);
        if (go != null)
            return go.transform;
        else
            return null;
    }

    public Object GetObj(string tag)
    {
        ObjData obj = GetObjByTag(tag);
        if (obj != null)
            return obj.obj;
        else
            return null;
    }

    public T GetComponent<T>(string tag)
    {
        GameObject go = GetGo(tag);
        if (go != null)
        {
            return go.GetComponent<T>();
        }
        return default(T);
    }


#if UNITY_EDITOR
	[ContextMenu("Fill Dic By Select")]
	void fillDicBySelection()
	{
		Object[] objs = UnityEditor.Selection.objects;
		for (int i = 0; objs != null && i < objs.Length; i++)
		{
			if (objs[i] != null)
			{
				objList.Add(new ObjData(){Desc = "",obj = objs[i]});
			}
		}
	}
	[ContextMenu("Sort By Name Forward")]
	void sortByNameForward()
	{
		objList.Sort(_sortNameForward);
	}
	[ContextMenu("Sort By Name Reverse")]
	void sortByNameReverse()
	{
		objList.Sort(_sortNameReverse);
	}
	int _sortNameForward(ObjData a,ObjData b)
	{
		string aname = a.obj == null ? "" : a.obj.name;
		string bname = b.obj == null ? "" : b.obj.name;
		return string.Compare(aname, bname);
	}
	int _sortNameReverse(ObjData a, ObjData b)
	{
		string aname = a.obj == null ? "" : a.obj.name;
		string bname = b.obj == null ? "" : b.obj.name;
		return string.Compare(bname,aname);
	}
	[ContextMenu("Sort By Desc Forward")]
	void sortByDescForward()
	{
		objList.Sort(_sortDescForward);
	}
	[ContextMenu("Sort By Desc Reverse")]
	void sortByDescReverse()
	{
		objList.Sort(_sortDescReverse);
	}
	int _sortDescForward(ObjData a, ObjData b)
	{
		return string.Compare(a.Desc, b.Desc);
	}
	int _sortDescReverse(ObjData a, ObjData b)
	{
		return string.Compare(b.Desc, a.Desc);
	}
	[ContextMenu("Sort Flip All")]
	void flipall()
	{
		objList.Reverse();
	}
	[ContextMenu("Select them all")]
	void selectAll()
	{
		Object[] objs = new Object[objList.Count];
		for(int i=0;i<objList.Count;i++)
		{
			objs[i] = objList[i].obj;
		}
		UnityEditor.Selection.objects = objs;
	}
	[ContextMenu("Export them")]
	void ExportThem()
	{
		string[] paths = new string[objList.Count];
		for (int i = 0; i < objList.Count; i++)
		{
			paths[i] = AssetDatabase.GetAssetPath(objList[i].obj);
		}
		string filename = EditorUtility.SaveFilePanel("Export to", "","", "unitypackage");
		if (!string.IsNullOrEmpty(filename))
		{
			AssetDatabase.ExportPackage(paths, filename, ExportPackageOptions.Recurse);
		}
	}

#endif
}
