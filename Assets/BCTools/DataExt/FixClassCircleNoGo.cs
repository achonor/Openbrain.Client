using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FixClassCircleNoGo
{
	public List<FixClassCircle.ParamMore> paramlist = new List<FixClassCircle.ParamMore>();
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
	public class Node:FixClassCircle.Node
	{
		public Node(){}
		public Node(FixClassCircleNoGo parent){this.home = parent;}
		new public FixClassCircleNoGo home = null;
		new public Node father = null;
		new public List<Node> children = new List<Node>();

		public override void ResizeParams()
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
		protected override bool checkhome()
		{
			bool rt = home == null;
			if(rt)
			{
				Debug.LogError("The Node should have a FixClassCircleNoGo Home");
			}
			return rt;
		}
		public override string getParamS(string paramName)
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
		public override void setParam(string paramName,float value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1)
			{
				if(home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.FloatNormal)
				{
					this.otherParamValues[i] = ""+value;
				}
				else if(home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.FloatSlider)
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
		public override void setParam(string paramName,int value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1)
			{
				if(home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.IntNormal || home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.IntEnum || home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.IntEnumMap)
				{
					this.otherParamValues[i] = ""+value;
				}
				else if(home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.IntSlider)
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
		public override void setParamEnumMap(string paramName,int index,bool value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.IntEnumMap)
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
		public override void setParam(string paramName,bool value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && (home.paramlist[this.paramIndex].Detail[i].type >= FixClassCircle.ParamType.Boolean && home.paramlist[this.paramIndex].Detail[i].type <= FixClassCircle.ParamType.BoolShowBelow))
			{
				this.otherParamValues[i] = value? "True":"False";
			}
		}
		public override void setParam(string paramName,Vector4 value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1)
			{
				if(home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.Vec2)
				{
					this.otherParamValues[i] = "" + value.x + "," + value.y;
				}
				else if(home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.Vec3)
				{
					this.otherParamValues[i] = "" + value.x + "," + value.y + "," + value.z;
				}
				else if(home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.Vec4)
				{
					this.otherParamValues[i] = "" + value.x + "," + value.y + "," + value.z + "," + value.w;
				}
			}
		}
		public override void setParam(string paramName,string value)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && (home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.StringArea || home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.StringLine) )
			{
				this.otherParamValues[i] = value;
			}
		}
		public override void setParam(string paramName,Color c)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.color)
			{
				string result = "" + c.r + "," + c.g + "," + c.b + "," + c.a;
				this.otherParamValues[i] = result;
			}
		}
		public override void setParamArray(string paramName,int[] array)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.ArrayInt)
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
		public override void setParamArray(string paramName,float[] array)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.ArrayFloat)
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
		public override void setParamArray(string paramName,string[] array)
		{
			if(checkhome())return;
			int i = home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && home.paramlist[this.paramIndex].Detail[i].type == FixClassCircle.ParamType.ArrayString)
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

	public Node root = new Node();

	//use FixClassCircle.SerializableNode

	public List<FixClassCircle.SerializableNode> serializedNodes = new List<FixClassCircle.SerializableNode>();

	[NonSerialized]
	public bool CanNot_OnBeforeSerialize = false;
	bool CanNot_OnBeforeSerialize_2 = false;//for check Auto_OnBeforeSerialize
	[Tooltip("Show the count of each Node's children")]
	public bool ShowSize = false;
	
	#region serialize
	public void OnBeforeSerialize()
	{
		if(CanNot_OnBeforeSerialize)return;
		if(CanNot_OnBeforeSerialize_2)return;
		CanNot_OnBeforeSerialize_2 = true;
		//unity is about to read the serializedNodes field's contents. lets make sure
		//we write out the correct data into that field "just in time".
		serializedNodes.Clear();
		FixClassCircle.SerializableNode seriRoot = formSeriNode(root);
		serializedNodes.Add(seriRoot);
		letChildBeSerilize(root, seriRoot);
	}
	[ContextMenu("Record to Serialize")]
	public void Record2SerializeOnce()
	{
		this.CanNot_OnBeforeSerialize_2 = false;
	}
	private void letChildBeSerilize(Node fatherNode, FixClassCircle.SerializableNode fatherSerialNode)
	{
		List<FixClassCircle.SerializableNode> seriListTmp = new List<FixClassCircle.SerializableNode>();
		for (int i = 0; i < fatherNode.children.Count; i++)
		{
			if (i == 0 && fatherSerialNode != null)
				fatherSerialNode.indexOfFirstChild = serializedNodes.Count;
			FixClassCircle.SerializableNode seriTmp = formSeriNode(fatherNode.children[i]);
			serializedNodes.Add(seriTmp);
			seriListTmp.Add(seriTmp);
		}
		for (int i = 0; fatherNode.children != null && i < fatherNode.children.Count && i < seriListTmp.Count; i++)
		{
			letChildBeSerilize(fatherNode.children[i], seriListTmp[i]);
		}
	}
	private FixClassCircle.SerializableNode formSeriNode(Node n)
	{
		//In case of array out of range
		for (int i = paramlist.Count - 1; i < n.paramIndex; i++)
		{
			paramlist.Add(new FixClassCircle.ParamMore());
		}

		var serializedNode = new FixClassCircle.SerializableNode()
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

	#region file
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
	public void Export(string filePath)
	{
		string result = ExportString();
		FixClassCircle.Save (filePath,result);
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

	public void Import(string filePath)
	{
		CanNot_OnBeforeSerialize = true;
		fileResult = FixClassCircle.Load(filePath);
		if(fileResult != null)
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

			FixClassCircle.ParamMore paramData = new FixClassCircle.ParamMore();
			paramData.desc = GetString(row, col++);
			int leng = GetInt(row, col++);
			for (int j = 0; j < leng; j++)
			{
				FixClassCircle.UseParam useParam = new FixClassCircle.UseParam();
				useParam.NameCh = GetString(row, col++);
				useParam.NameEng = GetString(row, col++);
				useParam.type = (FixClassCircle.ParamType)GetInt(row, col++);
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
			FixClassCircle.SerializableNode seriNode = new FixClassCircle.SerializableNode();
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

	//--------Export C# Class File
	public void exportClass()
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
			List<FixClassCircle.UseParam> detail = this.paramlist[i].Detail;
			for(int j=0;j<detail.Count;j++)
			{
				buffer += "\r\n\tpublic const string " + detail[j].NameEng.Replace(" ","_").ToUpper() + "_" +i + " = \"" + detail[j].NameEng +"\";";
			}
		}
		buffer += "\r\n}";
		FixClassCircle.Save (fileName,buffer);
	}
	
	//-------Export Java Class File
	public void exportClassJava()
	{
		string fileName = "";
		string classname = "ConV"+root.tag.Replace(" ","");
		#if UNITY_EDITOR
		fileName = EditorUtility.SaveFilePanel("choose the path you save", Application.dataPath, classname, "java");
		#endif
		if (string.IsNullOrEmpty(fileName))
			return;
		classname = fileName.Substring(fileName.LastIndexOf("/")+1);
		classname = classname.Replace(".java","");
		string buffer = "public class "+ classname +"\r\n{";
		for(int i=0;i<this.paramlist.Count;i++)
		{
			List<FixClassCircle.UseParam> detail = this.paramlist[i].Detail;
			for(int j=0;j<detail.Count;j++)
			{
				buffer += "\r\n\tpublic final String " + detail[j].NameEng.Replace(" ","_").ToUpper() + "_" +i + " = \"" + detail[j].NameEng +"\";";
			}
		}
		buffer += "\r\n}";
		FixClassCircle.Save (fileName,buffer);
	}
	#endregion Import/Export

	#region tool
	[ContextMenu("Check repeat in paramList")]
	public void checkRepeat()
	{
		for(int i=0;i<this.paramlist.Count;i++)
		{
			List<FixClassCircle.UseParam> detail = this.paramlist[i].Detail;
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
}
