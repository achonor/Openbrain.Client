using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using Object = UnityEngine.Object;


public class BCFindRefEditor : EditorWindow
{
	[MenuItem("BCTool/BCFindRef")]
	static void ShowWindow()
	{
		GetWindow<BCFindRefEditor>();
	}

	private Vector2 m_vScrollMove;
    private Vector2 m_vScrollVec;
    private Color mColor = Color.green;
    private string mColorStr = "#00FF00FF";
    private bool mShowColor = true;
    private string savePath = "";
	
	Object selectFile;
	List<Object> selectFolders = new List<Object>();
	List<Object> results = new List<Object>();
	bool searchPrefab = true;
	bool searchScene = true;
    bool searchMaterial = false;


    string guid = "";

	
	void OnGUI()
	{
        Rect rightClickRect = EditorGUILayout.BeginHorizontal();
        EditorGUILayout.HelpBox("Make Sure Asset Serialization Mode is \"Force Text\" before use me.", MessageType.Info);
        float widthColorBar = EditorGUILayout.BeginVertical().width;
        if (mShowColor)
        {
            mColor = EditorGUILayout.ColorField(mColor, GUILayout.Width(80));
            if (GUILayout.Button("switch",GUILayout.Width(80)))
            {
                mShowColor = false;
                this.ColorToStr();
            }
        }
        else
        {
            mColorStr = EditorGUILayout.TextField(mColorStr, GUILayout.Width(80));
            if (GUILayout.Button("switch", GUILayout.Width(80)))
            {
                mShowColor = true;
                this.StrToColor();
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        rightClickRect.xMax -= widthColorBar;
        if (Event.current.isMouse && Event.current.button == 1 && rightClickRect.Contains(Event.current.mousePosition))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Save"), false, _Save);
            menu.AddItem(new GUIContent("Load"), false, _Load);
            menu.AddItem(new GUIContent("Export Package"), false, _Export);
            menu.ShowAsContext();
            Event.current.Use();
        }
        EditorGUIUtility.labelWidth = 35;
        Object obj1 = EditorGUILayout.ObjectField("File:", selectFile, typeof(Object));
		if (GUI.changed)
		{
			selectFile = obj1;
		}
		EditorGUILayout.LabelField("Select Folder:");
        Rect dragRect = EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Add"))
		{
			selectFolders.Add(new Object());
		}
        EditorGUILayout.EndHorizontal();
        Event e = Event.current;
        if (dragRect.Contains(e.mousePosition))
        {
            if (e.type == EventType.DragUpdated)
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            else if (e.type == EventType.DragPerform)
            {
                Object[] objs = DragAndDrop.objectReferences;
                e.Use();
                for (int i = 0; objs != null && i < objs.Length; i++)
                {
                    if (objs[i] != null)
                    {
                        selectFolders.Add(objs[i]);
                    }
                }
            }
        }
        GUILayout.Box("", GUILayout.Height(4), GUILayout.ExpandWidth(true));
        m_vScrollVec = EditorGUILayout.BeginScrollView(m_vScrollVec, false, false);
		for (int i = 0; i < selectFolders.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();
			Object obj2 = EditorGUILayout.ObjectField("" + (i + 1), selectFolders[i], typeof(Object));
			if (GUI.changed)
			{
				selectFolders[i] = obj2;
			}
			if (GUILayout.Button("Delete",GUILayout.Width(80)))
			{
				selectFolders.RemoveAt(i);
			}
			EditorGUILayout.EndHorizontal();
		}
        EditorGUILayout.EndScrollView();
		EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 95;
        this.searchPrefab = EditorGUILayout.Toggle("Search Prefab:", this.searchPrefab);
		this.searchScene = EditorGUILayout.Toggle("Search Scene:", this.searchScene);
        this.searchMaterial = EditorGUILayout.Toggle("Search Material:", this.searchMaterial);
        EditorGUILayout.EndHorizontal();
		if (GUILayout.Button("Search Refrence"))
		{
			DoSearch();
		}
        EditorGUIUtility.labelWidth = 35;
        GUILayout.Box("", GUILayout.Height(4), GUILayout.ExpandWidth(true));
		m_vScrollMove = EditorGUILayout.BeginScrollView(m_vScrollMove, false, false);
		for (int i = 0; i < results.Count; i++)
		{
			/*results[i] = */EditorGUILayout.ObjectField("" + (i + 1), results[i], typeof(Object));
		}
		EditorGUILayout.EndScrollView();

	}

	void DoSearch()
	{
        if (selectFile != null)
        {
            guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetOrScenePath(selectFile));
        }
        else
        {
            EditorUtility.DisplayDialog("Warning","Choose a file in project first!", "OK");
            return;
        }
		results.Clear();
		//Debug.Log(AssetDatabase.GetAssetOrScenePath(selectFolders[0]));
		for (int i = 0; i < selectFolders.Count; i++)
		{
            if (selectFolders[i] == null)
            {
                continue;
            }
			string path = AssetDatabase.GetAssetOrScenePath(selectFolders[i]);
			string fullpath = Application.dataPath.Replace("Assets","") + path;
			fullpath = fullpath.Replace("/", "\\");
			DirectoryInfo dinfo = new DirectoryInfo(fullpath);
			AddFileResurt(dinfo);
		}
	}

	void AddFileResurt(DirectoryInfo dinfo)
	{
		if (dinfo.Exists)
		{
			FileInfo[] finfo = null;
			if (this.searchScene)
			{
				finfo = MergerArray(finfo,dinfo.GetFiles("*.unity"));
			}
			if (this.searchPrefab)
			{
				finfo = MergerArray(finfo, dinfo.GetFiles("*.prefab"));
			}
            if (this.searchMaterial)
            {
                finfo = MergerArray(finfo, dinfo.GetFiles("*.mat"));
            }
            for (int i = 0; finfo != null && i < finfo.Length; i++)
			{
                StreamReader reader = new StreamReader(finfo[i].FullName, System.Text.Encoding.UTF8);
                if (reader.ReadToEnd().Contains(guid))
				{
                    string tempstr = finfo[i].FullName.Replace("\\", "/");
                    tempstr = tempstr.Replace(Application.dataPath, "");
                    tempstr = "Assets" + tempstr;
                    Object obj = AssetDatabase.LoadAssetAtPath(tempstr, typeof(Object));
                    results.Add(obj);
				}
                reader.Close();
			}
			DirectoryInfo[] dinfoarray = dinfo.GetDirectories();
			for (int j = 0; dinfoarray != null && j < dinfoarray.Length; j++)
			{
				AddFileResurt(dinfoarray[j]);
			}
		}
	}

	FileInfo[] MergerArray(FileInfo[] First, FileInfo[] Second)
	{
		if (First == null) return Second;
		if (Second == null) return First;
		FileInfo[] result = new FileInfo[First.Length + Second.Length];
		First.CopyTo(result, 0);
		Second.CopyTo(result, First.Length);
		return result;
	}

    void _Save()
    {
        savePath = EditorUtility.SaveFilePanel("choose the path you save", savePath, "", "txt");
        if (!string.IsNullOrEmpty(savePath))
        {
            StreamWriter sw = new StreamWriter(savePath, false,System.Text.Encoding.UTF8);
            this.ColorToStr();
            sw.WriteLine(this.mColorStr);
            sw.WriteLine(AssetDatabase.GetAssetPath(selectFile));
            sw.WriteLine("" + selectFolders.Count);
            for (int i = 0; i < selectFolders.Count; i++)
            {
                sw.WriteLine(AssetDatabase.GetAssetPath(selectFolders[i]));
            }
            sw.WriteLine("" + results.Count);
            for (int i = 0; i < results.Count; i++)
            {
                sw.WriteLine(AssetDatabase.GetAssetPath(results[i]));
            }
            sw.Close();
        }
    }

    void _Load()
    {
        savePath = EditorUtility.OpenFilePanel("choose the file to load", savePath, "txt");
        if (!string.IsNullOrEmpty(savePath))
        {
            int num = 0;
            StreamReader sr = new StreamReader(savePath);
            this.mColorStr = sr.ReadLine();
            this.StrToColor();
            selectFile = AssetDatabase.LoadAssetAtPath(sr.ReadLine(),typeof(Object));
            int.TryParse(sr.ReadLine(), out num);
            this.selectFolders.Clear();
            for (int i = 0; i < num; i++)
            {
                selectFolders.Add(AssetDatabase.LoadAssetAtPath(sr.ReadLine(), typeof(Object)));
            }
            int.TryParse(sr.ReadLine(), out num);
            this.results.Clear();
            for (int i = 0; i < num; i++)
            {
                results.Add(AssetDatabase.LoadAssetAtPath(sr.ReadLine(), typeof(Object)));
            }
            sr.Close();
        }
    }

    void _Export()
    {
        string tempstr = EditorUtility.SaveFilePanel("Export to", "", "", "unitypackage");
        if (!string.IsNullOrEmpty(tempstr))
        {
            string[] paths = new string[selectFolders.Count];
            for(int i=0;i<selectFolders.Count;i++)
            {
                paths[i] = AssetDatabase.GetAssetPath(selectFolders[i]);
            }
            AssetDatabase.ExportPackage(paths, tempstr, ExportPackageOptions.Recurse);
        }
    }

    int hexCharToInt(char c)
    {
        int rt = 0;
        if (c >= 'A')
        {
            rt = (int)c - (int)'A' + 10;
        }
        else
        {
            rt = (int)c - (int)'0';
        }
        return rt;
    }

    void ColorToStr()
    {
        mColorStr = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", (int)(255 * mColor.r), (int)(255 * mColor.g), (int)(255 * mColor.b), (int)(255 * mColor.a));
    }

    void StrToColor()
    {
        char[] tmpArr = mColorStr.ToUpper().ToCharArray();
        if (tmpArr.Length == 7 || tmpArr.Length == 9)
        {
            mColor.r = (hexCharToInt(tmpArr[1]) * 16 + hexCharToInt(tmpArr[2])) / 255f;
            mColor.g = (hexCharToInt(tmpArr[3]) * 16 + hexCharToInt(tmpArr[4])) / 255f;
            mColor.b = (hexCharToInt(tmpArr[5]) * 16 + hexCharToInt(tmpArr[6])) / 255f;
            if (tmpArr.Length == 9)
            {
                mColor.a = (hexCharToInt(tmpArr[7]) * 16 + hexCharToInt(tmpArr[8])) / 255f;
            }
            else
            {
                mColor.a = 1f;
            }
        }
    }

}
