using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// see:  http://blogs.unity3d.com/2014/06/24/serialization-in-unity/
/// </summary>

public class FixClassCircle : MonoBehaviour, ISerializationCallbackReceiver
{
	public enum ParamType
	{
		StringLine,
		StringArea,
		IntNormal,
		IntSlider,		//str,min#max
		IntEnum,		//str
		IntEnumMap,		//str
		FloatNormal,
		FloatSlider,	//str,min#max
		Boolean,
		BoolHideBelow,	//str			//hide params below until end or next BoolHideBelow
		BoolShowBelow,  //str           //not hide params below
		Vec2,			//str,str
		Vec3,			//str,str,str
		Vec4,			//str,str,str,str
		ArrayInt,		//str,str,str,str...
		ArrayFloat,		//str,str,str,str...
		ArrayString,	//str,str,str,str...
		color,			//same to ArrayFloat
	}
	[Serializable]
	public class UseParam
	{
		[Tooltip("Use this in Inspector")]
		public string NameCh;
		[Tooltip("Use this in code")]
		public string NameEng;
		[Tooltip("Choose the variable type you want")]
		public ParamType type;
		[Tooltip("value type should be \"A,B,C...\"")]
		public string EnumDesc;
		//for editor
		[NonSerialized]
		public bool foldout = false;
	}
	[Serializable]
	public class ParamMore
	{
		public List<UseParam> Detail = new List<UseParam>();
		//for editor
		[NonSerialized]
		public bool foldout = false;
		public string desc = "";
		[NonSerialized]
		public bool EditMode = false;
	}
	//[Tooltip("The defination of each kind of Node")]
	[HideInInspector]
	public List<ParamMore> paramlist = new List<ParamMore>();
	public int getParamInfo(string paramName,int paramIndex)
	{
		for(int i=0;i<paramlist[paramIndex].Detail.Count;i++)
		{
			if(paramlist[paramIndex].Detail[i].NameEng.Equals(paramName))
			{
				return i;
			}
		}
		return -1;
	}
	//---------------------------------------------------------------------
	//node class that is used at runtime
	public class Node
	{
		public string tag = "Element";
		public string tagCh = "";
		public int paramIndex = 0;
		public List<string> otherParamValues = new List<string>();
		public List<Node> children = new List<Node>();
		public bool foldout = false;
		public bool editMode = false;
        public bool forSearch = false;
        public string searchStr = "";
		//public string delTag = "";	//Delete child by tag,not useful temply
		public FixClassCircle home = null; //base FixclassCircle
		public Node father = null; //father
		public Node(){}
		public Node(FixClassCircle parent){this.home = parent;}


		//when the paramIndex change,you should do this
		public virtual void ResizeParams()
		{
			if(checkhome())return;
			for(int i=this.otherParamValues.Count;i<home.paramlist[this.paramIndex].Detail.Count;i++)
			{
				this.otherParamValues.Add("");
			}
			if(this.otherParamValues.Count > home.paramlist[this.paramIndex].Detail.Count && this.otherParamValues.Count>0)
			{
				this.otherParamValues.RemoveRange(home.paramlist[this.paramIndex].Detail.Count,this.otherParamValues.Count-this.home.paramlist[this.paramIndex].Detail.Count);
			}
		}
		public Node GetChild(string childName)
		{
			return this.children.Find(m => m.tag.Equals(childName));
		}
		public Node GetChildByValue(string paramName,string value)
		{
			Node n = null;
			for(int i=0;i<children.Count;i++)
			{
				if(children[i].getParamS(paramName).Equals(value))
				{
					n = children[i];
					break;
				}
			}
			return n;
		}
		public Node GetChildByValue(string paramName,int value)
		{
			Node n = null;
			for(int i=0;i<children.Count;i++)
			{
				if(children[i].getParamI(paramName) == value)
				{
					n = children[i];
					break;
				}
			}
			return n;
		}
		//================================================
		protected virtual bool checkhome()
		{
			bool rt = home == null;
			if(rt)
			{
				Debug.LogError("The Node should have a FixClassCircle home");
			}
			return rt;
		}
		public float getParamF(string paramName)
		{
			float rt = 0f;
			string[] ss = getParamS(paramName).Split(',');
			if(ss.Length>0)
			{
				float.TryParse(ss[0],out rt);
			}
			return rt;
		}
		public int getParamI(string paramName)
		{
			int rt = 0;
			string[] ss = getParamS(paramName).Split(',');
			if(ss.Length>0)
			{
				int.TryParse(ss[0],out rt);
			}
			return rt;
		}
		public bool getParamEnumMap(string paramName,int index)
		{
			bool rt = false;
			int total = getParamI(paramName);
			int temp = 1<<index;
			rt = (temp & total) != 0;
			return rt;
		}
		public bool getParamB(string paramName)
		{
			bool rt = getParamS(paramName).Equals("True")? true : false;
			return rt;
		}
		public Vector4 getParamV(string paramName)
		{
			Vector4 rt;
			string[] ss = getParamS(paramName).Split(',');
			float[] f4 = new float[4];
			for(int i=0;i<ss.Length;i++)
			{
				float.TryParse(ss[i],out f4[i]);
			}
			rt = new Vector4(f4[0],f4[1],f4[2],f4[3]);
			return rt;
		}
		public int[] getParamArrayInt(string paramName)
		{
			int[] rt;
			string[] ss = getParamS(paramName).Split(',');
			rt = new int[ss.Length];
			for(int j=0;j<ss.Length;j++)
			{
				int.TryParse(ss[j],out rt[j]);
			}
			return rt;
		}
		public float[] getParamArrayFloat(string paramName)
		{
			float[] rt;
			string[] ss = getParamS(paramName).Split(',');
			rt = new float[ss.Length];
			for(int j=0;j<ss.Length;j++)
			{
				float.TryParse(ss[j],out rt[j]);
			}
			return rt;
		}
		public string[] getParamArrayString(string paramName)
		{
			string[] ss = getParamS(paramName).Split(',');
			return ss;
		}
		public Color getParamColor(string paramName)
		{
			Color rt = new Color(0,0,0,1);
			float[] value = getParamArrayFloat(paramName);
			if(value!= null && value.Length == 4)
			{
				rt.r = value[0];
				rt.g = value[1];
				rt.b = value[2];
				rt.a = value[3];
			}
			return rt;
		}
		public virtual string getParamS(string paramName)
		{
			string rt = "";
			if(checkhome())return rt;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i > -1)
			{
				rt = this.otherParamValues[i];
			}
			return rt;
		}
		
		//-------------------------------------------------------
		public virtual void setParam(string paramName,float value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1)
			{
				if(home.paramlist[this.paramIndex].Detail[i].type == ParamType.FloatNormal)
				{
					this.otherParamValues[i] = ""+value;
				}
				else if(home.paramlist[this.paramIndex].Detail[i].type == ParamType.FloatSlider)
				{
					string[] ss = this.otherParamValues[i].Split(',');
					if(ss.Length != 2)return;	//should not happen
					
					string[] ssB = ss[1].Split('#');
					if(ssB.Length != 2)return;	//should not happen
					float min,max;
					float.TryParse(ssB[0],out min);
					float.TryParse(ssB[1],out max);
					if(value<min)value = min;
					if(value>max)value = max;
					ss[0] = ""+value;
					this.otherParamValues[i] = ss[0] + "," + ss[1];
				}
			}
		}
		public void setParamPlus(string paramName,float plusvalue)
		{
			float value = getParamF(paramName);
			value += plusvalue;
			setParam(paramName,value);
		}
		public virtual void setParam(string paramName,int value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1)
			{
				if(home.paramlist[this.paramIndex].Detail[i].type == ParamType.IntNormal || home.paramlist[this.paramIndex].Detail[i].type == ParamType.IntEnum || home.paramlist[this.paramIndex].Detail[i].type == ParamType.IntEnumMap)
				{
					this.otherParamValues[i] = ""+value;
				}
				else if(home.paramlist[this.paramIndex].Detail[i].type == ParamType.IntSlider)
				{
					string[] ss = this.otherParamValues[i].Split(',');
					if(ss.Length != 2)return;	//should not happen
					
					string[] ssB = ss[1].Split('#');
					if(ssB.Length != 2)return;	//should not happen
					int min,max;
					int.TryParse(ssB[0],out min);
					int.TryParse(ssB[1],out max);
					if(value<min)value = min;
					if(value>max)value = max;
					ss[0] = ""+value;
					this.otherParamValues[i] = ss[0] + "," + ss[1];
				}
			}
		}
		public void setParamPlus(string paramName,int plusvalue)
		{
			int value = getParamI(paramName);
			value += plusvalue;
			setParam(paramName,value);
		}
		public virtual void setParamEnumMap(string paramName,int index,bool value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && home.paramlist[this.paramIndex].Detail[i].type == ParamType.IntEnumMap)
			{
				int total = getParamI(paramName);
				int temp = 1<<index;
				if(!value)
				{
					temp = -1 - temp;
					total &= temp;
				}
				else
				{
					total |= temp;
				}
				this.otherParamValues[i] = ""+total;
			}
		}
		public virtual void setParam(string paramName,bool value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && (home.paramlist[this.paramIndex].Detail[i].type >= ParamType.Boolean && home.paramlist[this.paramIndex].Detail[i].type <= ParamType.BoolShowBelow))
			{
				this.otherParamValues[i] = value? "True":"False";
			}
		}
		public virtual void setParam(string paramName,Vector4 value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1)
			{
				if(home.paramlist[this.paramIndex].Detail[i].type == ParamType.Vec2)
				{
					this.otherParamValues[i] = "" + value.x + "," + value.y;
				}
				else if(home.paramlist[this.paramIndex].Detail[i].type == ParamType.Vec3)
				{
					this.otherParamValues[i] = "" + value.x + "," + value.y + "," + value.z;
				}
				else if(home.paramlist[this.paramIndex].Detail[i].type == ParamType.Vec4)
				{
					this.otherParamValues[i] = "" + value.x + "," + value.y + "," + value.z + "," + value.w;
				}
			}
		}
		public void setParamPlus(string paramName,Vector4 plusvalue)
		{
			Vector4 value = getParamV (paramName);
			value += plusvalue;
			setParam(paramName,value);
		}
		public virtual void setParam(string paramName,string value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && (home.paramlist[this.paramIndex].Detail[i].type == ParamType.StringArea || home.paramlist[this.paramIndex].Detail[i].type == ParamType.StringLine) )
			{
				this.otherParamValues[i] = value;
			}
		}
		public void setParamPlus(string paramName,string plusvalue)
		{
			string value = getParamS(paramName);
			value += plusvalue;
			setParam(paramName,value);
		}
		public virtual void setParam(string paramName,Color c)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && home.paramlist[this.paramIndex].Detail[i].type == ParamType.color)
			{
				string result = "" + c.r + "," + c.g + "," + c.b + "," + c.a;
				this.otherParamValues[i] = result;
			}
		}
		public virtual void setParamArray(string paramName,int[] array)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && home.paramlist[this.paramIndex].Detail[i].type == ParamType.ArrayInt)
			{
				string result = "";
				for(int j=0;j<array.Length;j++)
				{
					result += array[j];
					if(j != array.Length-1)
					{
						result += ",";
					}
				}
				this.otherParamValues[i] = result;
			}
		}
		public virtual void setParamArray(string paramName,float[] array)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && home.paramlist[this.paramIndex].Detail[i].type == ParamType.ArrayFloat)
			{
				string result = "";
				for(int j=0;j<array.Length;j++)
				{
					result += array[j];
					if(j != array.Length-1)
					{
						result += ",";
					}
				}
				this.otherParamValues[i] = result;
			}
		}
		public virtual void setParamArray(string paramName,string[] array)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && home.paramlist[this.paramIndex].Detail[i].type == ParamType.ArrayString)
			{
				string result = "";
				for(int j=0;j<array.Length;j++)
				{
					result += array[j];
					if(j != array.Length-1)
					{
						result += ",";
					}
				}
				this.otherParamValues[i] = result;
			}
		}
	}
	//the root of what we use at runtime. not serialized.
	public Node root = new Node();
	
	
	
	//---------------------------------------------------------------------
	//node class that we will use for serialization
	[Serializable]
	public class SerializableNode
	{
		public string tag;
		public string tagCh;
		public int paramIndex = 0;
		public List<string> otherParamValues = new List<string>();
		public int childCount;
		public int indexOfFirstChild;
	}
	//the field we give unity to serialize.This is not for user to edit directly!!
	[HideInInspector]
	public List<SerializableNode> serializedNodes = new List<SerializableNode>();
	
	[NonSerialized]
	public bool CanNot_OnBeforeSerialize = false;
	bool CanNot_OnBeforeSerialize_2 = false;//for check Auto_OnBeforeSerialize
	[Tooltip("Show the count of each Node's children")]
	public bool ShowSize = true;
	[Tooltip("switch if auto serialize Node list or not")]
	public bool AutoOnBeforeSerialize = false;
	
	#region serialize
	public void OnBeforeSerialize()
	{
		if(CanNot_OnBeforeSerialize)return;
		if(CanNot_OnBeforeSerialize_2 && !AutoOnBeforeSerialize)return;
		if(!AutoOnBeforeSerialize)
			CanNot_OnBeforeSerialize_2 = true;
		//unity is about to read the serializedNodes field's contents. lets make sure
		//we write out the correct data into that field "just in time".
		serializedNodes.Clear();
		SerializableNode seriRoot = formSeriNode(root);
		serializedNodes.Add(seriRoot);
		letChildBeSerilize(root, seriRoot);
	}
	[ContextMenu("Record to Serialize")]
	public void Record2SerializeOnce()
	{
		if(!this.AutoOnBeforeSerialize)
			this.CanNot_OnBeforeSerialize_2 = false;
	}
	private void letChildBeSerilize(Node fatherNode,SerializableNode fatherSerialNode)
	{
		List<SerializableNode> seriListTmp = new List<SerializableNode>();
		for (int i = 0; i < fatherNode.children.Count; i++)
		{
			if (i == 0 && fatherSerialNode != null)
				fatherSerialNode.indexOfFirstChild = serializedNodes.Count;
			SerializableNode seriTmp = formSeriNode(fatherNode.children[i]);
			serializedNodes.Add(seriTmp);
			seriListTmp.Add(seriTmp);
		}
		for (int i = 0; fatherNode.children != null && i < fatherNode.children.Count && i < seriListTmp.Count; i++)
		{
			letChildBeSerilize(fatherNode.children[i], seriListTmp[i]);
		}
	}
	private SerializableNode formSeriNode(Node n)
	{
		//In case of array out of range
		for (int i = paramlist.Count - 1; i < n.paramIndex; i++)
		{
			paramlist.Add(new ParamMore());
		}

		var serializedNode = new SerializableNode()
		{
			tag = n.tag,
			tagCh = n.tagCh,
			childCount = n.children.Count,
			paramIndex = n.paramIndex
		};
		n.ResizeParams();
		foreach (string str in n.otherParamValues)
		{
			serializedNode.otherParamValues.Add(str);
		}
		return serializedNode;
	}
	
	public void OnAfterDeserialize()
	{
		//Unity has just written new data into the serializedNodes field.
		//let's populate our actual runtime data with those new values.
		if (serializedNodes.Count > 0)
			root = ReadNodeFromSerializedNodes (0);
		else
			root = new Node (this);
	}
	
	private Node ReadNodeFromSerializedNodes(int index)
	{
		var serializedNode = serializedNodes [index];
		var children = new List<Node> ();
		Node rt = new Node(this) {
			tag = serializedNode.tag,
			tagCh = serializedNode.tagCh,
			paramIndex = serializedNode.paramIndex
		};
		for(int i=0; i!= serializedNode.childCount; i++)
		{
			Node temp = ReadNodeFromSerializedNodes(serializedNode.indexOfFirstChild + i);
			temp.father = rt;
			children.Add(temp);
		}
		rt.children = children;
		
		for (int i=0; i<serializedNode.otherParamValues.Count; i++)
		{
			rt.otherParamValues.Add(serializedNode.otherParamValues[i]);
		}
		return rt;
	}
	
	
	#endregion serialize
	#if UNITY_EDITOR
	[ContextMenu("doSomeTest")]
	void doSomeTest()
	{
		EditorUtility.DisplayDialog("Title",root.getParamS("param1"),"OK");
	}
	#endif
	
	#region file
	public static void Save(string fileName,string saveString)
	{
		try
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Create);
			StreamWriter writer = new StreamWriter(fileStream);
			writer.Write(saveString);
			writer.Close();
			fileStream.Close();
		}
		catch (Exception e)
		{
			#if UNITY_EDITOR
			EditorUtility.DisplayDialog("Error!",e.Message,"OK");
			#endif
		}
	}
	public static List<List<string>> Load(string fileName)
	{
		List<List<string>> rt = null;
		if (string.IsNullOrEmpty(fileName))
			return rt;
		if (!File.Exists(fileName))
			return rt;
		else
		{
			StreamReader reader = new StreamReader(fileName);
			string content = reader.ReadToEnd();
			reader.Close();
			rt = LoadContent(content);
			reader.Close();
		}
		return rt;
	}

	public static List<List<string>> LoadContent(string content)
	{
		List<List<string>> rt = new List<List<string>>();
		StringReader sr = new StringReader(content);
		string line = null;
		while ((line = sr.ReadLine()) != null)
		{
			line = line.Trim('\t');
			if (line == "")
				continue;
			List<string> items = new List<string>();
			items.AddRange(line.Split('\x1A'));
			for (int i = 0; i < items.Count; i++)
				items[i].Trim('\t');
			rt.Add(items);
		}
		sr.Close();
		return rt;
	}
	private List<List<string>> fileResult;
	private int GetInt(int row, int col)
	{
		int rt = 0;
		int.TryParse( GetString(row,col),out rt);
		return rt;
	}
	private string GetString(int row, int col)
	{
		string rt = "";
		try{
			rt = fileResult[row - 1][col - 1];
		}catch(Exception e)
		{
			Debug.LogError("Array out of range when do GetString(row,col)");
		}
        if (rt.Equals("[None]"))
            rt = "";
        else
        {
            rt = rt.Replace("\\n", "\n");
        }
        return rt;
	}
	private void AddColumn(ref string result,string str)
	{
        if (string.IsNullOrEmpty(str))
            str = "[None]";
        else
        {
            str = str.Replace("\r", "");
            str = str.Replace("\n", "\\n");
        }
		if(result.LastIndexOf("\x1A") == result.Length-1 || result.LastIndexOf("\n") == result.Length-1)
			result += str;
		else
			result += "\x1A"+str;
	}
	private void AddColumn(ref string result,int num)
	{
		AddColumn(ref result,""+num);
	}
	private void AddRow(ref string result)
	{
		result += '\n';
	}
	#endregion file
	
	#region Import/Export
	[ContextMenu("Export")]
	public void Export()
	{
		string fileName = "";
		#if UNITY_EDITOR
		fileName = EditorUtility.SaveFilePanel("choose the path you save", string.Empty, string.Empty, "txt");
		#endif
		if (string.IsNullOrEmpty(fileName))
			return;
		string result = ExportString();
		Save (fileName,result);
	}

	public string ExportString()
	{
		string result = "";
		AddColumn(ref result, paramlist.Count);
		AddRow(ref result);
		for (int i = 0; i < paramlist.Count; i++)
		{
			AddColumn(ref result, paramlist[i].desc);
			AddColumn(ref result, paramlist[i].Detail.Count);
			for (int j = 0; j < paramlist[i].Detail.Count; j++)
			{
				AddColumn(ref result, paramlist[i].Detail[j].NameCh);
				AddColumn(ref result, paramlist[i].Detail[j].NameEng);
				AddColumn(ref result, (int)paramlist[i].Detail[j].type);
				AddColumn(ref result, paramlist[i].Detail[j].EnumDesc);
			}
			AddRow(ref result);
		}
		AddColumn(ref result, serializedNodes.Count);
		AddRow(ref result);
		for (int i = 0; i < serializedNodes.Count; i++)
		{
			AddColumn(ref result, serializedNodes[i].tagCh);
			AddColumn(ref result, serializedNodes[i].tag);
			AddColumn(ref result, serializedNodes[i].paramIndex);
			AddColumn(ref result, serializedNodes[i].otherParamValues.Count);
			for (int j = 0; j < serializedNodes[i].otherParamValues.Count; j++)
			{
				AddColumn(ref result, serializedNodes[i].otherParamValues[j]);
			}
			AddColumn(ref result, serializedNodes[i].childCount);
			AddColumn(ref result, serializedNodes[i].indexOfFirstChild);
			AddRow(ref result);
		}
		return result;
	}

	[ContextMenu("Import")]
	public void Import()
	{
		string fileName = "";
		#if UNITY_EDITOR
		fileName = EditorUtility.OpenFilePanel("choose the path you load", string.Empty, "txt");
		#endif
		if (string.IsNullOrEmpty(fileName))
			return;
		CanNot_OnBeforeSerialize = true;
		fileResult = Load(fileName);
		whenImport();
		CanNot_OnBeforeSerialize = false;
	}

	private void whenImport()
	{
		int row = 1, col = 1;
		int length = GetInt(row++, col);
		paramlist.Clear();
		for (int i = 0; i < length; i++)
		{

			ParamMore paramData = new ParamMore();
			paramData.desc = GetString(row, col++);
			int leng = GetInt(row, col++);
			for (int j = 0; j < leng; j++)
			{
				UseParam useParam = new UseParam();
				useParam.NameCh = GetString(row, col++);
				useParam.NameEng = GetString(row, col++);
				useParam.type = (ParamType)GetInt(row, col++);
				useParam.EnumDesc = GetString(row, col++);
				paramData.Detail.Add(useParam);
			}
			paramlist.Add(paramData);
			row++;
			col = 1;
		}
		length = GetInt(row++, col);
		serializedNodes.Clear();
		for (int i = 0; i < length; i++)
		{
			SerializableNode seriNode = new SerializableNode();
			seriNode.tagCh = GetString(row, col++);
			seriNode.tag = GetString(row, col++);
			seriNode.paramIndex = GetInt(row, col++);
			int leng = GetInt(row, col++);
			for (int j = 0; j < leng; j++)
			{
				seriNode.otherParamValues.Add(GetString(row, col++));
			}
			seriNode.childCount = GetInt(row, col++);
			seriNode.indexOfFirstChild = GetInt(row, col++);
			serializedNodes.Add(seriNode);
			row++;
			col = 1;
		}
		OnAfterDeserialize();
	}

	public void ImportByString(string content)
	{
		CanNot_OnBeforeSerialize = true;
		fileResult = FixClassCircle.LoadContent(content);
		whenImport();
		CanNot_OnBeforeSerialize = false;
	}

	[ContextMenu("Export C# Class File")]
	void exportClass()
	{
		string fileName = "";
		string classname = "ConV"+root.tag.Replace(" ","");
		#if UNITY_EDITOR
		fileName = EditorUtility.SaveFilePanel("choose the path you save", Application.dataPath, classname, "cs");
		#endif
		if (string.IsNullOrEmpty(fileName))
			return;
		classname = fileName.Substring(fileName.LastIndexOf("/")+1);
		classname = classname.Replace(".cs","");
		string buffer = "public class "+ classname +"\r\n{";
		for(int i=0;i<this.paramlist.Count;i++)
		{
			List<UseParam> detail = this.paramlist[i].Detail;
			for(int j=0;j<detail.Count;j++)
			{
				buffer += "\r\n\tpublic const string " + detail[j].NameEng.Replace(" ","_").ToUpper() + "_" +i + " = \"" + detail[j].NameEng +"\";";
			}
		}
		buffer += "\r\n}";
		Save (fileName,buffer);
	}

	[ContextMenu("Export Java Class File")]
	void exportClassJava()
	{
		string fileName = "";
		string classname = "ConV"+root.tag.Replace(" ","");
		#if UNITY_EDITOR
		fileName = EditorUtility.SaveFilePanel("choose the path you save", Application.dataPath, classname, "java");
		#endif
		if (string.IsNullOrEmpty(fileName))
			return;
		classname = fileName.Substring(fileName.LastIndexOf("/")+1);
		classname = classname.Replace("java","");
		string buffer = "public class "+ classname +"\r\n{";
		for(int i=0;i<this.paramlist.Count;i++)
		{
			List<UseParam> detail = this.paramlist[i].Detail;
			for(int j=0;j<detail.Count;j++)
			{
				buffer += "\r\n\tpublic final String " + detail[j].NameEng.Replace(" ","_").ToUpper() + "_" +i + " = \"" + detail[j].NameEng +"\";";
			}
		}
		buffer += "\r\n}";
		Save (fileName,buffer);
	}
	#endregion Import/Export

	#region tool
	[ContextMenu("Check repeat in paramList")]
	void checkRepeat()
	{
		for(int i=0;i<this.paramlist.Count;i++)
		{
			List<UseParam> detail = this.paramlist[i].Detail;
			this.paramlist[i].foldout = false;
			Dictionary<string,bool> nameDic = new Dictionary<string, bool>();//bool value is useless
			for(int j=0,k=1;j<detail.Count;j++)
			{
				if(nameDic.ContainsKey(detail[j].NameEng))
				{
					this.paramlist[i].foldout = true;
					detail[j].foldout = true;
					detail[j].NameEng += "_" + k++;
				}
				else
				{
					detail[j].foldout = false;
				}
				nameDic.Add(detail[j].NameEng,true);
			}
		}
	}
	#endregion tool

	#region public_tool
	public static Node DuplicateNode(Node node)	//information of node.father won't be duplicated
	{
		Node rtn = new Node(){
			tag = node.tag,
			tagCh = node.tagCh,
			paramIndex = node.paramIndex,
			home = node.home,
			foldout = node.foldout
		};
		for(int i=0;i<node.otherParamValues.Count;i++)
		{
			rtn.otherParamValues.Add(node.otherParamValues[i]);
		}
		for(int j=0;j<node.children.Count;j++)
		{
			Node tempNode = DuplicateNode(node.children[j]);
			tempNode.father = rtn;
			rtn.children.Add (tempNode);
		}
		return rtn;
	}
	#endregion public_tool
	
	#region old method
	public static Node GetChild(Node node,string childname)
	{
		return node.children.Find(m => m.tag.Equals(childname));
	}
	
	public Node GetChild(string childname)
	{
		return root.children.Find(m => m.tag.Equals(childname));
	}
	
	public Node GetChildByValue(Node father,string paramName,string value)
	{
		Node n = null;
		for(int i=0;i<father.children.Count;i++)
		{
			if(father.children[i].getParamS(paramName).Equals(value))
			{
				n = father.children[i];
				break;
			}
		}
		return n;
	}
	
	public Node GetChildByValue(string paramName, string value)
	{
		Node n = null;
		for (int i = 0; i < root.children.Count; i++)
		{
			if (root.children[i].getParamS(paramName).Equals(value))
			{
				n = root.children[i];
				break;
			}
		}
		return n;
	}
	
	
	public Node GetChildByValue(Node father,string paramName,int value)
	{
		Node n = null;
		for(int i=0;i<father.children.Count;i++)
		{
			if(father.children[i].getParamI(paramName) == value)
			{
				n = father.children[i];
				break;
			}
		}
		return n;
	}
	
	public Node GetChildByValue(string paramName, int value)
	{
		Node n = null;
		for (int i = 0; i < root.children.Count; i++)
		{
			if (root.children[i].getParamI(paramName) == value)
			{
				n = root.children[i];
				break;
			}
		}
		return n;
	}
	
	#endregion
}
