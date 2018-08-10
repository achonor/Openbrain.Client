using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class FixClassCircleNoGoEditor : EditorWindow
{
	[MenuItem("BCTool/FixClassCircleNoGo")]
	static void ShowWindow()
	{
		GetWindow<FixClassCircleNoGoEditor>();
	}
	FixClassCircleNoGo _target = null;
	const int indentation = 16;
	private Vector2 m_vScrollMove;
	private int counter = 0;
	const int counterLimit = 10;
	void OnEnable()
	{
		_target = new FixClassCircleNoGo();
		saveEnable = false;
		if (_target != null && _target.root.home == null)
		{
			_target.root.home = _target;
		}
	}

	TextAsset savefileinProject;
	string filePath = "";
	bool saveEnable = false;
	GUIContent title_1 = new GUIContent("Load File", "Choose a text file to load, or even drag a file on me");
	GUIContent title_2 = new GUIContent("Save", "Save text file, the path was shown under this");
	GUIContent title_3 = new GUIContent("Save As", "Choose save path by yourself,and save it!");
	GUIContent title_4 = new GUIContent("->C#", "Export C# class file ,record variable name in paramlist");
	GUIContent title_5 = new GUIContent("->Java", "Export Java class file ,record variable name in paramlist");
	GUIContent title_6 = new GUIContent("Check Param Repeat", "show variables which should not repeat in paramlist by change foldout state");

	void OnGUI()
	{
		counter++;
		if (counter >= counterLimit)
		{
			_target.OnBeforeSerialize();
			counter = 0;
		}
		Rect loadButtonRect = EditorGUILayout.BeginHorizontal();
		loadButtonRect.xMax = loadButtonRect.xMin + 75f;
		Event e = Event.current;
		if(loadButtonRect.Contains(e.mousePosition))
		{
			if(e.type == EventType.DragUpdated)
				DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
			else if (e.type == EventType.DragPerform)
			{
				UnityEngine.Object[] objs = DragAndDrop.objectReferences;
				e.Use();
				if (objs.Length > 0 && objs[0] is TextAsset)
				{
					this.savefileinProject = (TextAsset)objs[0];
					_target.ImportByString(this.savefileinProject.text);
					filePath = Application.dataPath.Replace("/Assets","/") + AssetDatabase.GetAssetOrScenePath(this.savefileinProject);
					saveEnable = true;
				}
			}
		}
		if (GUILayout.Button(title_1, GUILayout.Width(70)))
		{
			string strtmp = EditorUtility.OpenFilePanel("choose the path you load", filePath, "txt");
			if(!string.IsNullOrEmpty(strtmp))
			{
				filePath = strtmp;
				_target.Import(filePath);
				saveEnable = true;
			}
		}

		if (saveEnable && GUILayout.Button(title_2, GUILayout.Width(50)))
		{
            _target.Record2SerializeOnce();
            _target.OnBeforeSerialize();
			if (!string.IsNullOrEmpty(filePath))
				_target.Export(filePath);
		}
		if (GUILayout.Button(title_3, GUILayout.Width(65)))
		{
            _target.Record2SerializeOnce();
            _target.OnBeforeSerialize();
			string strtmp = EditorUtility.SaveFilePanel("choose the path you save", string.Empty, string.Empty, "txt");
			if(!string.IsNullOrEmpty(strtmp))
			{
				filePath = strtmp;
				_target.Export(filePath);
				saveEnable = true;
			}
		}
		if (GUILayout.Button(title_4, GUILayout.Width(50)))
		{
			_target.exportClass();
		}
		if (GUILayout.Button(title_5, GUILayout.Width(60)))
		{
			_target.exportClassJava();
		}
		if (GUILayout.Button(title_6, GUILayout.Width(135)))
		{
			ParamListFoldOut = true;
			_target.checkRepeat();
		}
		EditorGUILayout.EndHorizontal();

		if(saveEnable)
		{
			EditorGUILayout.LabelField("Path:",filePath);
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUIUtility.labelWidth = 100;
		_target.ShowSize = EditorGUILayout.Toggle("Show size", _target.ShowSize,GUILayout.MaxWidth(150));
		EditorGUIUtility.labelWidth = 150;
		EditorGUILayout.EndHorizontal();
		GUILayout.Box("", GUILayout.Height(1),GUILayout.ExpandWidth(true));
		m_vScrollMove = EditorGUILayout.BeginScrollView(m_vScrollMove, false, false);
		EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth * 0.18f;
		DrawParamList();
		EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth * 0.45f;
        if (GUI.changed)
        {
            _target.Record2SerializeOnce();
        }
		GUILayout.Space(5);
		GUILayout.Box("", GUILayout.Height(4), GUILayout.ExpandWidth(true));
		GUILayout.Space(5);
		EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth * 0.32f;
		Display(_target.root);
		EditorGUILayout.EndScrollView();

	}
	FixClassCircleNoGo.Node clipboardNode = null;
	void copyNode(object obj)
	{
		FixClassCircleNoGo.Node node = obj as FixClassCircleNoGo.Node;
		if (node != null)
		{
			clipboardNode = FixClassCircleNoGo.DuplicateNode(node);
		}
	}
	private bool forceQuitDraw = false;
	void cutNode(object obj)
	{
		FixClassCircleNoGo.Node node = obj as FixClassCircleNoGo.Node;
		if (node != null)
		{
			clipboardNode = node;
			node.father.children.Remove(node);
			forceQuitDraw = true;
		}
	}
	void pasteNode(object obj)
	{
		if (clipboardNode != null)
		{
			FixClassCircleNoGo.Node node = obj as FixClassCircleNoGo.Node;
			clipboardNode.father = node;
			node.children.Add(clipboardNode);
		}
	}
	void emptyClipboard(object obj)
	{
		if (clipboardNode != null)
		{
			clipboardNode = null;
		}
	}
	//GUIContent forbutton0 = new GUIContent("Serialize", "\'public Node root\' will be recorded as serialize list");
	GUIContent forbutton1 = new GUIContent("+", "Add Child");
	GUIContent forbutton2 = new GUIContent("x", "Remove all children");
	GUIContent forbutton3 = new GUIContent("X", "Delete this node");
	GUIContent forbutton4 = new GUIContent("", "Set Node Tag by first enum param");
	GUIContent forbutton5 = new GUIContent("D", "Duplicate element in front of this node");
	GUIContent forbutton6 = new GUIContent("E", "Edit Mode is Close");
    GUIContent forbutton6_2 = new GUIContent("E", "Edit Mode is Open");
	GUIContent forbutton7 = new GUIContent("S", "Search in childs");
	Color color_normal = Color.white;
	Color color_Edit = Color.cyan;
	bool hidebelowparam = false;
	//draw defination part
	bool ParamListFoldOut = false;
	bool ParamListEditMode = false;
	int ParamListSize = 1;
	void DrawParamList()
	{
		if (_target == null) return;
		EditorGUILayout.BeginHorizontal();
		ParamListFoldOut = EditorGUILayout.Foldout(ParamListFoldOut, "Struct List" + (_target.ShowSize ? (" / size:" + _target.paramlist.Count) : ""));
        if (ParamListEditMode)
        {
            if (GUILayout.Button(forbutton6_2, EditorStyles.toolbarButton, GUILayout.Width(20)))
            {
                ParamListEditMode = false;
            }
        }
        else
        {
            if (GUILayout.Button(forbutton6, GUILayout.Width(20)))
            {
                ParamListEditMode = true;
            }
        }
		if (GUILayout.Button(forbutton1, GUILayout.Width(20)))
		{
            _target.paramlist.Add(new FixClassCircle.ParamMore());
            ParamListFoldOut = true;
		}
		EditorGUILayout.EndHorizontal();
		if (ParamListEditMode)
		{
			EditorGUILayout.BeginHorizontal();
			GUI.color = color_Edit;
			GUILayout.Space(indentation);
			ParamListSize = EditorGUILayout.IntField("size:", ParamListSize);
			if (GUILayout.Button("SetSize"))
			{
				if (ParamListSize < 1) ParamListSize = 1;
				if (ParamListSize < _target.paramlist.Count)
				{
					_target.paramlist.RemoveRange(ParamListSize, _target.paramlist.Count - ParamListSize);
				}
				else if (ParamListSize > _target.paramlist.Count)
				{
					for (int tempi = _target.paramlist.Count; tempi < ParamListSize; tempi++)
					{
						_target.paramlist.Add(new FixClassCircle.ParamMore());
					}
				}
			}
			GUI.color = color_normal;
			EditorGUILayout.EndHorizontal();
		}
		if (!ParamListFoldOut) return;
		for (int i = 0; i < _target.paramlist.Count; i++)
		{
			FixClassCircle.ParamMore paramnode = _target.paramlist[i];
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(indentation);
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();
			fixFoldoutPos(paramnode.foldout);
            
			paramnode.foldout = EditorGUILayout.Foldout(paramnode.foldout, i + "-" + paramnode.desc + (_target.ShowSize ? (" / size:" + paramnode.Detail.Count) : ""));
            if (paramnode.EditMode)
            {
                if (GUILayout.Button(forbutton6_2, EditorStyles.toolbarButton, GUILayout.Width(20)))
                {
                    paramnode.EditMode = false;
                }
            }
            else
            {
                if (GUILayout.Button(forbutton6, GUILayout.Width(20)))
                {
                    paramnode.EditMode = true;
                }
            }

			if (GUILayout.Button(forbutton1, GUILayout.Width(20)))
			{
                string defaultName = "Member_" + (paramnode.Detail.Count + 1);
                paramnode.Detail.Add(new FixClassCircle.UseParam() { NameCh = defaultName,NameEng = defaultName });
                paramnode.foldout = true;
			}
			if (GUILayout.Button(forbutton3, GUILayout.Width(20)))
			{
				_target.paramlist.Remove(paramnode);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				return;
			}
			EditorGUILayout.EndHorizontal();
			if (paramnode.EditMode)
			{
				EditorGUILayout.BeginHorizontal();
				GUI.color = color_Edit;
				GUILayout.Space(indentation);
				paramnode.desc = EditorGUILayout.TextField("Desc", paramnode.desc);
				GUI.color = color_normal;
				EditorGUILayout.EndHorizontal();
			}
			if (paramnode.foldout)
			{
				for (int j = 0; j < paramnode.Detail.Count; j++)
				{
					FixClassCircle.UseParam useParam = paramnode.Detail[j];
					EditorGUILayout.BeginHorizontal();
					GUILayout.Space(indentation);
					EditorGUILayout.BeginVertical();
					EditorGUILayout.BeginHorizontal();
					fixFoldoutPos(useParam.foldout);
					useParam.foldout = EditorGUILayout.Foldout(useParam.foldout, useParam.NameCh);
					if (GUILayout.Button(forbutton5, GUILayout.Width(22)))
					{
                        FixClassCircle.UseParam newUse = new FixClassCircle.UseParam();
                        newUse.NameCh = useParam.NameCh;
                        newUse.NameEng = useParam.NameEng;
                        newUse.type = useParam.type;
                        newUse.EnumDesc = useParam.EnumDesc;
                        paramnode.Detail.Insert(j, newUse);
                        resync(i, j, 0);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        break;
					}
					if (GUILayout.Button(forbutton3, GUILayout.Width(20)))
					{
                        paramnode.Detail.Remove(useParam);
                        resync(i, j, 1);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        break;
					}
					EditorGUILayout.EndHorizontal();
					if (useParam.foldout)
					{
						EditorGUILayout.BeginHorizontal();
						GUILayout.Space(indentation+13);
						EditorGUILayout.BeginVertical();

						useParam.NameCh = EditorGUILayout.TextField("Name Ch", useParam.NameCh);
						useParam.NameEng = EditorGUILayout.TextField("Name Eng", useParam.NameEng);
						useParam.type = (FixClassCircle.ParamType)EditorGUILayout.EnumPopup("Type", useParam.type);
						if (useParam.type == FixClassCircle.ParamType.IntEnum || useParam.type == FixClassCircle.ParamType.IntEnumMap)
						{
							useParam.EnumDesc = EditorGUILayout.TextField("Enum Desc", useParam.EnumDesc);
						}
						else if (!string.IsNullOrEmpty(useParam.EnumDesc))
						{
							useParam.EnumDesc = "";
						}

						EditorGUILayout.EndVertical();
						EditorGUILayout.EndHorizontal();
					}
					EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}
	}

	void fixFoldoutPos(bool foldout)
	{
		if (!foldout) GUILayout.Space(4);
	}

	void resync(int i, int j, int mode)
	{
		_target.CanNot_OnBeforeSerialize = true;
		for (int n = 0; n < _target.serializedNodes.Count; n++)
		{
			if (_target.serializedNodes[n].paramIndex == i)
			{
				if (mode == 0) //duplicate
				{
					_target.serializedNodes[n].otherParamValues.Insert(j, _target.serializedNodes[n].otherParamValues[j]);
				}
				else if (mode == 1) //remove
				{
					_target.serializedNodes[n].otherParamValues.RemoveAt(j);
				}
			}
		}
		_target.OnAfterDeserialize();
		_target.CanNot_OnBeforeSerialize = false;
	}
	//draw data part
	bool Display(FixClassCircleNoGo.Node node)	//return if Display should do next time
	{
		Rect titleRect = EditorGUILayout.BeginHorizontal();
		string titlename = node.tagCh;
		if (!string.IsNullOrEmpty(node.tagCh))
			titlename += " / ";
		titlename += node.tag;
		if (_target.ShowSize)
			titlename += " / size:" + node.children.Count;

		if (node.forSearch)
		{
			node.foldout = EditorGUILayout.Foldout(node.foldout, titlename, EditorStyles.foldoutPreDrop);
		}
		else
		{
			node.foldout = EditorGUILayout.Foldout(node.foldout, titlename);
		}

        if (node.editMode)
        {
            if (GUILayout.Button(forbutton6_2, EditorStyles.toolbarButton, GUILayout.Width(20)))
            {
                node.editMode = false;
                //重置搜索
                node.searchStr = "";
                foreach (var child in node.children)
                {
                    child.forSearch = false;
                }
                _target.Record2SerializeOnce();
            }
        }
        else
        {
            if (GUILayout.Button(forbutton6, GUILayout.Width(20)))
            {
                node.editMode = true;
            }
        }
		if (Event.current.isMouse && Event.current.button == 1 && titleRect.Contains(Event.current.mousePosition))
		{
			GenericMenu menu = new GenericMenu();
			menu.AddItem(new GUIContent("Copy Node"), false, copyNode, node);
			if (node.father != null)
			{
				menu.AddItem(new GUIContent("Cut Node"), false, cutNode, node);
			}
			menu.AddItem(new GUIContent("Paste Node as a child"), false, pasteNode, node);
			menu.AddItem(new GUIContent("Empty clipboard"), false, emptyClipboard, null);
			menu.ShowAsContext();
			Event.current.Use();
		}

		if (GUILayout.Button(forbutton1, GUILayout.Width(20)))
		{
			node.children.Add(new FixClassCircleNoGo.Node(_target) { father = node});
			_target.Record2SerializeOnce();
            node.foldout = true;
		}
        /*
		if (GUILayout.Button(forbutton2, GUILayout.Width(20)))
		{
			node.children.Clear();
			_target.Record2SerializeOnce();
		}*/
		if (node.father != null)
		{
			if (GUILayout.Button(forbutton5, GUILayout.Width(22)))
			{
				FixClassCircleNoGo.Node nodetmp = FixClassCircleNoGo.DuplicateNode(node);
				nodetmp.father = node.father;
				int index = node.father.children.IndexOf(node);
				node.father.children.Insert(index, nodetmp);
				_target.Record2SerializeOnce();
				return true;
			}
            if (GUILayout.Button(forbutton3, GUILayout.Width(20)))
            {
                node.father.children.Remove(node);
                _target.Record2SerializeOnce();
                return true;
            }
		}
		EditorGUILayout.EndHorizontal();
		if (!node.foldout)
		{
			EditorGUIUtility.labelWidth -= indentation;
		}
		if (node.editMode)
		{
			GUILayout.BeginHorizontal();
			GUI.color = color_Edit;
			GUILayout.Space(indentation);

			node.tagCh = EditorGUILayout.TextField(node.tagCh, GUILayout.Width(90));
			node.tag = EditorGUILayout.TextField(node.tag, GUILayout.Width(95));
			if (GUILayout.Button(forbutton4,EditorStyles.radioButton, GUILayout.Width(20)))
			{
				FixClassCircle.ParamMore temp = _target.paramlist[node.paramIndex];
				if (temp.Detail.Count > 0 && temp.Detail[0].type == FixClassCircle.ParamType.IntEnum)
				{
					string[] ss = temp.Detail[0].EnumDesc.Split(',');
					int index = 0;
					int.TryParse(node.otherParamValues[0], out index);
					node.tag = ss[index];
					_target.Record2SerializeOnce();
				}
			}
			//EditorGUILayout.LabelField("Struct", GUILayout.Width(40));
            node.paramIndex = EditorGUILayout.Popup(node.paramIndex, getParamlistEnumStrArr(), GUILayout.Width(70));
			if (node.paramIndex < 0) node.paramIndex = 0;
			node.searchStr = EditorGUILayout.TextField(node.searchStr, GUILayout.Width(70));
			if (GUILayout.Button(forbutton7, GUILayout.Width(20)))
			{
				foreach (var child in node.children)
				{
					if (node.searchStr.Length > 0 && (child.tag.Contains(node.searchStr) || child.tagCh.Contains(node.searchStr)))
					{
						child.forSearch = true;
					}
					else
					{
						child.forSearch = false;
						child.foldout = false;
					}
				}
			}
			GUI.color = color_normal;
			GUILayout.EndHorizontal();
		}
        if (!node.foldout)
        {
            return false;
        }
		if (_target.paramlist.Count - 1 < node.paramIndex)
		{
			EditorGUIUtility.labelWidth -= indentation;
			return false;//wait until list count is enough
		}
		for (int i = 0; i < _target.paramlist[node.paramIndex].Detail.Count && i < node.otherParamValues.Count; i++)
		{
			FixClassCircle.ParamType tempT = _target.paramlist[node.paramIndex].Detail[i].type;
			if (hidebelowparam && tempT != FixClassCircle.ParamType.BoolHideBelow && tempT != FixClassCircle.ParamType.BoolShowBelow)
			{
				continue;
			}
			GUILayout.BeginHorizontal();
			GUILayout.Space(indentation+13);
			drawParams(node, i);
			GUILayout.EndHorizontal();
		}
		hidebelowparam = false;

		//---------------------------------------------------
		GUILayout.BeginHorizontal();
		GUILayout.Space(indentation);
		EditorGUIUtility.labelWidth -= indentation;
		GUILayout.BeginVertical();

		foreach (var child in node.children)
		{
			if (Display(child) || forceQuitDraw)
			{
				forceQuitDraw = false;
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				return true;
			}
			EditorGUIUtility.labelWidth += indentation; // not plus not reduce
		}

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		return false;
	}


	void drawParams(FixClassCircleNoGo.Node node, int i)
	{
		string titleName = _target.paramlist[node.paramIndex].Detail[i].NameCh;
		int tempInt = 0, tempI1 = 0, tempI2 = 0;
		float tempF = 0f;
		bool tempB = false;
		Vector4 tempv4 = Vector4.zero;
		Color colorTmp = new Color();
		switch (_target.paramlist[node.paramIndex].Detail[i].type)
		{
			case FixClassCircle.ParamType.StringLine:
				{
					node.otherParamValues[i] = EditorGUILayout.TextField(titleName, node.otherParamValues[i]);
					break;
				}
			case FixClassCircle.ParamType.StringArea:
				{
					GUILayout.Label(titleName);
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					GUILayout.Space(indentation);
					node.otherParamValues[i] = EditorGUILayout.TextArea(node.otherParamValues[i], GUILayout.MinHeight(32f));
					break;
				}
			case FixClassCircle.ParamType.IntNormal:
				{
					int.TryParse(node.otherParamValues[i], out tempInt);
					tempInt = EditorGUILayout.IntField(titleName, tempInt);
					node.otherParamValues[i] = "" + tempInt;
					break;
				}
			case FixClassCircle.ParamType.IntSlider:
				{
					string[] sAs = node.otherParamValues[i].Split(',');
					if (sAs.Length != 2)
					{
						node.otherParamValues[i] = "0,-10#10";
						sAs = node.otherParamValues[i].Split(',');
					}
					string[] sBs = sAs[1].Split('#');
					if (sBs.Length != 2)
					{
						node.otherParamValues[i] = "0,-10#10";
						sAs = node.otherParamValues[i].Split(',');
						sBs = sAs[1].Split('#');
					}
					//int.TryParse(node.otherParamValues[i],out tempInt);
					int.TryParse(sAs[0], out tempInt);
					int.TryParse(sBs[0], out tempI1);
					int.TryParse(sBs[1], out tempI2);
					tempInt = EditorGUILayout.IntSlider(titleName, tempInt, tempI1, tempI2);
					string result = "" + tempInt;
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					GUILayout.Space(indentation+13);
					tempI1 = EditorGUILayout.IntField("-->Min", tempI1);
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					GUILayout.Space(indentation+13);
					tempI2 = EditorGUILayout.IntField("-->Max", tempI2);
					result += "," + tempI1 + "#" + tempI2;
					node.otherParamValues[i] = result;
					break;
				}
			case FixClassCircle.ParamType.IntEnum:
				{
					string[] ss = _target.paramlist[node.paramIndex].Detail[i].EnumDesc.Split(',');
					if (ss.Length < 2)
					{
						_target.paramlist[node.paramIndex].Detail[i].EnumDesc = "A,B";
						ss = _target.paramlist[node.paramIndex].Detail[i].EnumDesc.Split(',');
					}
					int.TryParse(node.otherParamValues[i], out tempInt);
					tempInt = EditorGUILayout.Popup(titleName, tempInt, ss);
					node.otherParamValues[i] = "" + tempInt;
					break;
				}
			case FixClassCircle.ParamType.IntEnumMap:
				{
					string[] ss = _target.paramlist[node.paramIndex].Detail[i].EnumDesc.Split(',');
					if (ss.Length < 2)
					{
						_target.paramlist[node.paramIndex].Detail[i].EnumDesc = "A,B";
						ss = _target.paramlist[node.paramIndex].Detail[i].EnumDesc.Split(',');
					}
					int.TryParse(node.otherParamValues[i], out tempInt);
					tempInt = EditorGUILayout.MaskField(titleName, tempInt, ss);
					node.otherParamValues[i] = "" + tempInt;
					break;
				}
			case FixClassCircle.ParamType.FloatNormal:
				{
					float.TryParse(node.otherParamValues[i], out tempF);
					tempF = EditorGUILayout.FloatField(titleName, tempF);
					node.otherParamValues[i] = "" + tempF;
					break;
				}
			case FixClassCircle.ParamType.FloatSlider:
				{
					string[] sAs = node.otherParamValues[i].Split(',');
					if (sAs.Length != 2)
					{
						node.otherParamValues[i] = "0,-10#10";
						sAs = node.otherParamValues[i].Split(',');
					}
					string[] sBs = sAs[1].Split('#');
					if (sBs.Length != 2)
					{
						node.otherParamValues[i] = "0,-10#10";
						sAs = node.otherParamValues[i].Split(',');
						sBs = sAs[1].Split('#');
					}
					float.TryParse(sAs[0], out tempF);
					float.TryParse(sBs[0], out tempv4.x);
					float.TryParse(sBs[1], out tempv4.y);
					tempF = EditorGUILayout.Slider(titleName, tempF, tempv4.x, tempv4.y);
					string result = "" + tempF;
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					GUILayout.Space(indentation+13);
					EditorGUILayout.LabelField("-->Range");
					tempv4 = EditorGUILayout.Vector2Field("", tempv4, GUILayout.MaxWidth(200), GUILayout.MaxHeight(16));
					result += "," + tempv4.x + "#" + tempv4.y;
					node.otherParamValues[i] = result;
					break;
				}
			case FixClassCircle.ParamType.Boolean:
				{
					tempB = node.otherParamValues[i].Equals("True");
					tempB = EditorGUILayout.Toggle(titleName, tempB);
					node.otherParamValues[i] = tempB ? "True" : "False";
					break;
				}
			case FixClassCircle.ParamType.BoolHideBelow:
				{
					tempB = node.otherParamValues[i].Equals("True");
					tempB = EditorGUILayout.Toggle(titleName, tempB);
					hidebelowparam = tempB;
					node.otherParamValues[i] = tempB ? "True" : "False";
					break;
				}
			case FixClassCircle.ParamType.BoolShowBelow:
				{
					tempB = node.otherParamValues[i].Equals("True");
					tempB = EditorGUILayout.Toggle(titleName, tempB);
					hidebelowparam = !tempB;
					node.otherParamValues[i] = tempB ? "True" : "False";
					break;
				}
			case FixClassCircle.ParamType.Vec2:
				{
					string[] ss = node.otherParamValues[i].Split(',');
					if (ss.Length != 2)
					{
						node.otherParamValues[i] = "0,0";
						ss = node.otherParamValues[i].Split(',');
					}
					float.TryParse(ss[0], out tempv4.x);
					float.TryParse(ss[1], out tempv4.y);
					EditorGUILayout.LabelField(titleName, GUILayout.MaxWidth(EditorGUIUtility.labelWidth));
					tempv4 = EditorGUILayout.Vector2Field("", tempv4, GUILayout.MaxWidth(250),GUILayout.MaxHeight(16));
					node.otherParamValues[i] = "" + tempv4.x + "," + tempv4.y;
					break;
				}
			case FixClassCircle.ParamType.Vec3:
				{
					string[] ss = node.otherParamValues[i].Split(',');
					if (ss.Length != 3)
					{
						node.otherParamValues[i] = "0,0,0";
						ss = node.otherParamValues[i].Split(',');
					}
					float.TryParse(ss[0], out tempv4.x);
					float.TryParse(ss[1], out tempv4.y);
					float.TryParse(ss[2], out tempv4.z);
					EditorGUILayout.LabelField(titleName, GUILayout.MaxWidth(EditorGUIUtility.labelWidth));
					tempv4 = EditorGUILayout.Vector3Field("", tempv4, GUILayout.MaxWidth(270),GUILayout.MaxHeight(16));
					node.otherParamValues[i] = "" + tempv4.x + "," + tempv4.y + "," + tempv4.z;
					break;
				}
			case FixClassCircle.ParamType.Vec4:
				{
					string[] ss = node.otherParamValues[i].Split(',');
					if (ss.Length != 4)
					{
						node.otherParamValues[i] = "0,0,0,0";
						ss = node.otherParamValues[i].Split(',');
					}
					float.TryParse(ss[0], out tempv4.x);
					float.TryParse(ss[1], out tempv4.y);
					float.TryParse(ss[2], out tempv4.z);
					float.TryParse(ss[3], out tempv4.w);
					EditorGUILayout.LabelField(titleName, GUILayout.MaxWidth(EditorGUIUtility.labelWidth));
					tempv4 = EditorGUILayout.Vector4Field("", tempv4,GUILayout.MaxWidth(400));
					node.otherParamValues[i] = "" + tempv4.x + "," + tempv4.y + "," + tempv4.z + "," + tempv4.w;
					break;
				}
			case FixClassCircle.ParamType.ArrayInt:
				{
					string[] ss = node.otherParamValues[i].Split(',');

					int size = ss.Length;
					size = EditorGUILayout.IntField(titleName + "(size)", size);
					if (size < 0) size = 0;
					for (int j = ss.Length; j < size; j++)
					{
						node.otherParamValues[i] += ",0";
					}
					ss = node.otherParamValues[i].Split(',');
					string result = "";
					for (int j = 0; j < size; j++)
					{
						int.TryParse(ss[j], out tempInt);
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						GUILayout.Space(indentation+13);
						tempInt = EditorGUILayout.IntField("-->" + j, tempInt);
						result += tempInt;
						if (j != size - 1)
						{
							result += ",";
						}
					}
					node.otherParamValues[i] = result;
					break;
				}
			case FixClassCircle.ParamType.ArrayFloat:
				{
					string[] ss = node.otherParamValues[i].Split(',');

					int size = ss.Length;
					size = EditorGUILayout.IntField(titleName + "(size)", size);
					if (size < 0) size = 0;
					for (int j = ss.Length; j < size; j++)
					{
						node.otherParamValues[i] += ",0";
					}
					ss = node.otherParamValues[i].Split(',');
					string result = "";
					for (int j = 0; j < size; j++)
					{
						float.TryParse(ss[j], out tempF);
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						GUILayout.Space(indentation+13);
						tempF = EditorGUILayout.FloatField("-->" + j, tempF);
						result += tempF;
						if (j != size - 1)
						{
							result += ",";
						}
					}
					node.otherParamValues[i] = result;
					break;
				}
			case FixClassCircle.ParamType.ArrayString:
				{
					string[] ss = node.otherParamValues[i].Split(',');

					int size = ss.Length;
					size = EditorGUILayout.IntField(titleName + "(size)", size);
					if (size < 0) size = 0;
					for (int j = ss.Length; j < size; j++)
					{
						node.otherParamValues[i] += ",0";
					}
					ss = node.otherParamValues[i].Split(',');
					string result = "";
					for (int j = 0; j < size; j++)
					{
						int.TryParse(ss[j], out tempInt);
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						GUILayout.Space(indentation+13);
						ss[j] = EditorGUILayout.TextField("-->" + j, ss[j]);
						result += ss[j];
						if (j != size - 1)
						{
							result += ",";
						}
					}
					node.otherParamValues[i] = result;
					break;
				}
			case FixClassCircle.ParamType.color:
				{
					string[] ss = node.otherParamValues[i].Split(',');
					if (ss.Length != 4)
					{
						node.otherParamValues[i] = "0,0,0,1";
						ss = node.otherParamValues[i].Split(',');
					}
					float.TryParse(ss[0], out colorTmp.r);
					float.TryParse(ss[1], out colorTmp.g);
					float.TryParse(ss[2], out colorTmp.b);
					float.TryParse(ss[3], out colorTmp.a);
					colorTmp = EditorGUILayout.ColorField(titleName, colorTmp);
					node.otherParamValues[i] = "" + colorTmp.r + "," + colorTmp.g + "," + colorTmp.b + "," + colorTmp.a;
					break;
				}

		}

        
	}
    string[] getParamlistEnumStrArr()
    {
        string[] rt = new string[_target.paramlist.Count];
        for(int i=0;i<_target.paramlist.Count;i++)
        {
            rt[i] = i+" "+_target.paramlist[i].desc;
        }
        return rt;
    }
}
