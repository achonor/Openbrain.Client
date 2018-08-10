package extensions.room.game.athletics.common;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.util.*;

public class FixClassCircle
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
		BoolShowBelow,	//str			//not hide params below
		Vec2,			//str,str
		Vec3,			//str,str,str
		Vec4,			//str,str,str,str
		ArrayInt,		//str,str,str,str...
		ArrayFloat,		//str,str,str,str...
		ArrayString,	//str,str,str,str...
		color,			//same to ArrayFloat
	}
	public class UseParam
	{
		public String NameCh;	//Use this in Inspector
		public String NameEng;	//Use this in code
		public ParamType type;
		public String EnumDesc;	//value type should be "A,B,C..."
	}
	public class ParamMore
	{
		public List<UseParam> Detail = new ArrayList<UseParam>();
		public String desc;
	}
	public List<ParamMore> paramlist = new ArrayList<ParamMore>();
	public int getParamInfo(String paramName,int paramIndex)
	{
		for(int i=0;i<paramlist.get(paramIndex).Detail.size();i++)
		{
			if(paramlist.get(paramIndex).Detail.get(i).NameEng.compareTo(paramName) == 0)
			{
				return i;
			}
		}
		return -1;
	}
	//---------------------------------------------------------------------
	//node class that is used at runtime
	public static class Node
	{
		public String tag = "Element";
		public String tagCh = "";
		public int paramIndex = 0;
		public List<String> otherParamValues = new ArrayList<String>();
		public List<Node> children = new ArrayList<Node>();
		public FixClassCircle home = null;
		public Node father = null;
		public Node(){}
		public Node(FixClassCircle home){this.home = home;}
		//when the paramIndex change,you should do this
		public void ResizeParams()
		{
			if(checkhome())return;
			for(int i=this.otherParamValues.size();i<this.home.paramlist.get(this.paramIndex).Detail.size();i++)
			{
				this.otherParamValues.add("");
			}
			if(this.otherParamValues.size() > this.home.paramlist.get(this.paramIndex).Detail.size() && this.otherParamValues.size()>0)
			{
				this.otherParamValues = this.otherParamValues.subList(0, this.home.paramlist.get(this.paramIndex).Detail.size()-1);
			}
		}
		public Node GetChild(String childName)
		{
			Node n = null;
			for(int i=0;i<children.size();i++)
			{
				if(children.get(i).tag.compareTo(childName) == 0)
				{
					n = children.get(i);
					break;
				}
			}
			return n;
		}
		public Node GetChildByValue(String paramName,String value)
		{
			Node n = null;
			for(int i=0;i<children.size();i++)
			{
				if(children.get(i).getParamS(paramName).compareTo(value) == 0)
				{
					n = children.get(i);
					break;
				}
			}
			return n;
		}
		public Node GetChildByValue(String paramName,int value)
		{
			Node n = null;
			for(int i=0;i<children.size();i++)
			{
				if(children.get(i).getParamI(paramName) == value)
				{
					n = children.get(i);
					break;
				}
			}
			return n;
		}
		//================================================
		boolean checkhome()
		{
			boolean rt = home == null;
			if(rt)
			{
				System.out.println("The Node should have a FixClassCircle home");
			}
			return rt;
		}
		public float getParamF(String paramName)
		{
			float rt = 0f;
			String[] ss = getParamS(paramName).split(",");
			if(ss.length>0)
			{
				rt = Float.parseFloat(ss[0]);
			}
			return rt;
		}
		public int getParamI(String paramName)
		{
			int rt = 0;
			String[] ss = getParamS(paramName).split(",");
			if(ss.length>0)
			{
				rt = Integer.parseInt(ss[0]);
			}
			return rt;
		}
		public boolean getParamEnumMap(String paramName,int index)
		{
			boolean rt = false;
			int total = getParamI(paramName);
			int temp = 1<<index;
			rt = (temp & total) != 0;
			return rt;
		}
		public boolean getParamB(String paramName)
		{
			boolean rt = getParamS(paramName).compareTo("True") == 0? true : false;
			return rt;
		}
		public float[] getParamV(String paramName)
		{
			String[] ss = getParamS(paramName).split(",");
			float[] f4 = new float[4];
			for(int i=0;i<ss.length;i++)
			{
				f4[i] = Float.parseFloat(ss[i]);
			}
			return f4;
		}
		public int[] getParamArrayInt(String paramName)
		{
			int[] rt;
			String[] ss = getParamS(paramName).split(",");
			rt = new int[ss.length];
			for(int j=0;j<ss.length;j++)
			{
				rt[j] = Integer.parseInt(ss[j]);
			}
			return rt;
		}
		public float[] getParamArrayFloat(String paramName)
		{
			float[] rt;
			String[] ss = getParamS(paramName).split(",");
			rt = new float[ss.length];
			for(int j=0;j<ss.length;j++)
			{
				rt[j] = Float.parseFloat(ss[j]);
			}
			return rt;
		}
		public String[] getParamArrayString(String paramName)
		{
			String[] ss = getParamS(paramName).split(",");
			return ss;
		}
		public String getParamS(String paramName)
		{
			String rt = "";
			if(checkhome())return rt;
			int i = this.home.getParamInfo(paramName,this.paramIndex);
			if(i > -1)
			{
				rt = this.otherParamValues.get(i);
			}
			return rt;
		}

		//-------------------------------------------------------
		public void setParam(String paramName,float value)
		{
			if(checkhome())return;
			int i = this.home.getParamInfo(paramName,this.paramIndex);
			if(i>-1)
			{
				if(this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.FloatNormal)
				{
					this.otherParamValues.set(i,""+value);
				}
				else if(this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.FloatSlider)
				{
					String[] ss = this.otherParamValues.get(i).split(",");
					if(ss.length != 2)return;	//should not happen
					
					String[] ssB = ss[1].split("#");
					if(ssB.length != 2)return;	//should not happen
					float min,max;
					min = Float.parseFloat(ssB[0]);
					max = Float.parseFloat(ssB[1]);
					if(value<min)value = min;
					if(value>max)value = max;
					ss[0] = ""+value;
					this.otherParamValues.set(i,ss[0] + "," + ss[1]);
				}
			}
		}
		public void setParamPlus(String paramName,float plusvalue)
		{
			float value = getParamF(paramName);
			value += plusvalue;
			setParam(paramName,value);
		}
		public void setParam(String paramName,int value)
		{
			if(checkhome())return;
			int i = this.home.getParamInfo(paramName,this.paramIndex);
			if(i>-1)
			{
				if(this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.IntNormal || this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.IntEnum || this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.IntEnumMap)
				{
					this.otherParamValues.set(i,""+value);
				}
				else if(this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.IntSlider)
				{
					String[] ss = this.otherParamValues.get(i).split(",");
					if(ss.length != 2)return;	//should not happen
					
					String[] ssB = ss[1].split("#");
					if(ssB.length != 2)return;	//should not happen
					int min,max;
					min = Integer.parseInt(ssB[0]);
					max = Integer.parseInt(ssB[1]);
					if(value<min)value = min;
					if(value>max)value = max;
					ss[0] = ""+value;
					this.otherParamValues.set(i, ss[0] + "," + ss[1]);
				}
			}
		}
		public void setParamPlus(String paramName,int plusvalue)
		{
			int value = getParamI(paramName);
			value += plusvalue;
			setParam(paramName,value);
		}
		public void setParamEnumMap(String paramName,int index,boolean value)
		{
			if(checkhome())return;
			int i = this.home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.IntEnumMap)
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
				this.otherParamValues.set(i, ""+total);
			}
		}
		public void setParam(String paramName,boolean value)
		{
			if(checkhome())return;
			int i = this.home.getParamInfo(paramName,this.paramIndex);
			ParamType paramtype = this.home.paramlist.get(this.paramIndex).Detail.get(i).type;
			if(i>-1 && (paramtype == ParamType.Boolean || paramtype == ParamType.BoolShowBelow || paramtype == ParamType.BoolHideBelow))
			{
				this.otherParamValues.set(i,value? "True":"False");
			}
		}
		public void setParamVec(String paramName,float[] value)
		{
			if(checkhome())return;
			int i = this.home.getParamInfo(paramName,this.paramIndex);
			if(i>-1)
			{
				if(this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.Vec2)
				{
					this.otherParamValues.set(i,"" + value[0] + "," + value[1]);
				}
				else if(this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.Vec3)
				{
					this.otherParamValues.set(i, "" + value[0] + "," + value[1] + "," + value[2]);
				}
				else if(this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.Vec4)
				{
					this.otherParamValues.set(i,"" + value[0] + "," + value[1] + "," + value[2] + "," + value[3]);
				}
			}
		}
		public void setParam(String paramName,String value)
		{
			if(checkhome())return;
			int i = this.home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && (this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.StringArea || this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.StringLine) )
			{
				this.otherParamValues.set(i,value);
			}
		}
		public void setParamArray(String paramName,int[] array)
		{
			if(checkhome())return;
			int i = this.home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.ArrayInt)
			{
				String result = "";
				for(int j=0;j<array.length;j++)
				{
					result += array[j];
					if(j != array.length-1)
					{
						result += ",";
					}
				}
				this.otherParamValues.set(i,result);
			}
		}
		public void setParamArray(String paramName,float[] array)
		{
			if(checkhome())return;
			int i = this.home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.ArrayFloat)
			{
				String result = "";
				for(int j=0;j<array.length;j++)
				{
					result += array[j];
					if(j != array.length-1)
					{
						result += ",";
					}
				}
				this.otherParamValues.set(i,result);
			}
		}
		public void setParamArray(String paramName,String[] array)
		{
			if(checkhome())return;
			int i = this.home.getParamInfo(paramName,this.paramIndex);
			if(i>-1 && this.home.paramlist.get(this.paramIndex).Detail.get(i).type == ParamType.ArrayString)
			{
				String result = "";
				for(int j=0;j<array.length;j++)
				{
					result += array[j];
					if(j != array.length-1)
					{
						result += ",";
					}
				}
				this.otherParamValues.set(i,result);
			}
		}

	}
	//the root of what we use at runtime. not serialized.
	public Node root = new Node();
	
	
	
	//---------------------------------------------------------------------
	//node class that we will use for serialization
	public class SerializableNode
	{
		public String tag;
		public String tagCh;
		public int paramIndex = 0;
		public List<String> otherParamValues = new ArrayList<String>();
		public int childCount;
		public int indexOfFirstChild;
	}
	//the field we give unity to serialize.This is not for user to edit directly!!
	public List<SerializableNode> serializedNodelist = new ArrayList<SerializableNode>();
	

	public void OnBeforeSerialize()
	{
		//unity is about to read the serializedNodes field's contents. lets make sure
		//we write out the correct data into that field "just in time".
		serializedNodelist.clear();
		SerializableNode seriRoot = formSeriNode(root);
		serializedNodelist.add(seriRoot);
		letChildBeSerilize(root, seriRoot);
	}

	private void letChildBeSerilize(Node fatherNode,SerializableNode fatherSerialNode)
	{
		List<SerializableNode> seriListTmp = new ArrayList<SerializableNode>();
		for (int i = 0; i < fatherNode.children.size(); i++)
		{
			if (i == 0 && fatherSerialNode != null)
				fatherSerialNode.indexOfFirstChild = serializedNodelist.size();
			SerializableNode seriTmp = formSeriNode(fatherNode.children.get(i));
			serializedNodelist.add(seriTmp);
			seriListTmp.add(seriTmp);
		}
		for (int i = 0; fatherNode.children != null && i < fatherNode.children.size() && i < seriListTmp.size(); i++)
		{
			letChildBeSerilize(fatherNode.children.get(i), seriListTmp.get(i));
		}
	}
	private SerializableNode formSeriNode(Node n)
	{
		//In case of array out of range
		for(int i=paramlist.size()-1; i < n.paramIndex; i++)
		{
			paramlist.add(new ParamMore());
		}

		SerializableNode serializedNode = new SerializableNode ();
		serializedNode.tag = n.tag;
		serializedNode.tagCh = n.tagCh;
		serializedNode.childCount = n.children.size();
		serializedNode.paramIndex = n.paramIndex;
		
		n.ResizeParams();
		for(int i=0;i<n.otherParamValues.size();i++)
		{
			serializedNode.otherParamValues.add(n.otherParamValues.get(i));
		}
		return serializedNode;
	}
	
	public void OnAfterDeserialize()
	{
		//Unity has just written new data into the serializedNodes field.
		//let's populate our actual runtime data with those new values.
		if (serializedNodelist.size() > 0)
			root = ReadNodeFromSerializedNodes (0);
		else
			root = new Node (this);
	}
	
	private Node ReadNodeFromSerializedNodes(int index)
	{
		SerializableNode serializedNode = serializedNodelist.get(index);
		List<Node> children = new ArrayList<Node>();
		Node rt = new Node(this);
		rt.tag = serializedNode.tag;
		rt.tagCh = serializedNode.tagCh;
		rt.paramIndex = serializedNode.paramIndex;
		for(int i=0; i!= serializedNode.childCount; i++)
		{
			Node temp = ReadNodeFromSerializedNodes(serializedNode.indexOfFirstChild + i);
			temp.father = rt;
			children.add(temp);
		}
		
		rt.children = children;
		
		for (int i=0; i<serializedNode.otherParamValues.size(); i++)
		{
			rt.otherParamValues.add(serializedNode.otherParamValues.get(i));
		}
		return rt;
	}
	


	private void Save(String fileName, String saveString)
	{
		try
		{
			BufferedWriter bWriter = new BufferedWriter(new OutputStreamWriter(new FileOutputStream(fileName), "UTF-8"));
			bWriter.write(saveString);
			bWriter.flush();
			bWriter.close();
		}
		catch (IOException e)
		{
			e.printStackTrace();
		}
	}
	private void Load(String fileName) {
		if ((new java.io.File(fileName)).isFile()) {

			final String lineSplitter = "\n";
			fileResult.clear();
			StringBuffer sbBuffer = new StringBuffer();
			BufferedReader bReader = null;

			try {
				bReader = new BufferedReader(new FileReader(fileName));
			} catch (FileNotFoundException e1) {
				e1.printStackTrace();
			}
			try {
				while (bReader.ready()) {
					sbBuffer.append(bReader.readLine());
					sbBuffer.append(lineSplitter);
				}
			} catch (IOException e) {
				e.printStackTrace();
			} finally {
				try {
					bReader.close();
				} catch (IOException e) {
					e.printStackTrace();
				}
			}

			LoadContent(sbBuffer.toString());
		} else {
			System.out.println("No file named " + fileName);
		}
	}
	private void LoadContent(String content)
	{
		final String lineSplitter = "\n";
		final String columnSplitter = "\032";
		String[] lines = content.split(lineSplitter, -1);

		for (int i = 0; i < lines.length - 1; i++) {
			java.util.ArrayList<String> line = new java.util.ArrayList<String>();
			fileResult.add(line);
			String[] columns = lines[i].split(columnSplitter, -1);
			for (int j = 0; j < columns.length; j++) {
				line.add(columns[j].trim());
			}
		}
	}
	private List<List<String>> fileResult;
	private int GetInt(int row, int col)
	{
		int rt = 0;
		rt = Integer.parseInt(GetString(row,col));
		return rt;
	}
	private String GetString(int row, int col)
	{
		String rt = "";
		try{
			rt = fileResult.get(row - 1).get(col - 1);
		}catch(Exception e)
		{
			e.printStackTrace();
		}
		if(rt.compareTo("[None]") == 0)
			rt = "";
		else
        {
            rt = rt.replace("\\n", "\n");
        }
		return rt;
	}
	private String AddColumn(String result,String str)
	{
		if(str == null || str.isEmpty())
			str = "[None]";
		else
        {
            str = str.replace("\r", "");
            str = str.replace("\n", "\\n");
        }
		if(result.lastIndexOf("\032") == result.length()-1 || result.lastIndexOf("\n") == result.length()-1)
			result += str;
		else
			result += "\032"+str;
		return result;
	}
	private String AddColumn(String result,int num)
	{
		result = AddColumn(result,""+num);
		return result;
	}
	private String AddRow(String result)
	{
		result += '\n';
		return result;
	}


	public void Export(String loadfileName)
	{
		String fileName = "data/dataAthletics"+loadfileName;
		if (fileName == null || fileName.isEmpty())
			return;
		String result = ExportString();
		Save (fileName,result);
	}
	public String ExportString()
	{
		String result = "";
		result = AddColumn (result,paramlist.size());
		result = AddRow(result);
		for(int i=0;i<paramlist.size();i++)
		{
			result = AddColumn (result,paramlist.get(i).desc);
			result = AddColumn (result,paramlist.get(i).Detail.size());
			for(int j=0;j<paramlist.get(i).Detail.size();j++)
			{
				result = AddColumn (result,paramlist.get(i).Detail.get(j).NameCh);
				result = AddColumn (result,paramlist.get(i).Detail.get(j).NameEng);
				result = AddColumn (result,paramlist.get(i).Detail.get(j).type.ordinal());
				result = AddColumn (result,paramlist.get(i).Detail.get(j).EnumDesc);
			}
			result = AddRow(result);
		}
		result = AddColumn (result,serializedNodelist.size());
		result = AddRow(result);
		for(int i=0;i<serializedNodelist.size();i++)
		{
			result = AddColumn(result,serializedNodelist.get(i).tagCh);
			result = AddColumn(result,serializedNodelist.get(i).tag);
			result = AddColumn(result,serializedNodelist.get(i).paramIndex);
			result = AddColumn(result,serializedNodelist.get(i).otherParamValues.size());
			for(int j=0;j<serializedNodelist.get(i).otherParamValues.size();j++)
			{
				result = AddColumn(result,serializedNodelist.get(i).otherParamValues.get(j));
			}
			result = AddColumn(result,serializedNodelist.get(i).childCount);
			result = AddColumn(result,serializedNodelist.get(i).indexOfFirstChild);
			result = AddRow (result);
		}
		return result;
	}
	public void Import(String loadfileName)
	{
		String fileName = "data/dataAthletics"+loadfileName;
		if (fileName == null || fileName.isEmpty())
			return;
		Load(fileName);
		whenImport();
	}
	private void whenImport()
	{
		int row = 1,col = 1;
		int length = GetInt(row++,col);
		paramlist.clear();
		for(int i=0;i<length;i++)
		{
			ParamMore paramData = new ParamMore();
			paramData.desc = GetString(row,col++);
			int leng = GetInt(row,col++);
			for(int j=0;j<leng;j++)
			{
				UseParam useParam = new UseParam();
				useParam.NameCh = GetString(row,col++);
				useParam.NameEng = GetString (row,col++);
				useParam.type = ParamType.values()[GetInt (row,col++)];
				useParam.EnumDesc = GetString (row,col++);
				paramData.Detail.add(useParam);
			}
			paramlist.add(paramData);
			row ++;
			col = 1;
		}
		length = GetInt(row++,col);
		serializedNodelist.clear();
		for(int i=0;i<length;i++)
		{
			SerializableNode seriNode = new SerializableNode();
			seriNode.tagCh 				= GetString(row,col++);
			seriNode.tag 				= GetString(row,col++);
			seriNode.paramIndex 		= GetInt(row,col++);
			int leng = GetInt(row,col++);
			for(int j=0;j<leng;j++)
			{
				seriNode.otherParamValues.add(GetString (row,col++));
			}
			seriNode.childCount 		= GetInt(row,col++);
			seriNode.indexOfFirstChild 	= GetInt(row,col++);
			serializedNodelist.add(seriNode);
			row ++;
			col = 1;
		}
		OnAfterDeserialize();
	}
	public void ImportByString(String content)
	{
		LoadContent(content);
		whenImport();
	}
	public static Node DuplicateNode(Node node)	//information of node.father won't be duplicated
	{
		Node rtn = new Node();
		rtn.tag = node.tag;
		rtn.tagCh = node.tagCh;
		rtn.paramIndex = node.paramIndex;
		rtn.home = node.home;
		for(int i=0;i<node.otherParamValues.size();i++)
		{
			rtn.otherParamValues.add(node.otherParamValues.get(i));
		}
		for(int j=0;j<node.children.size();j++)
		{
			Node tempNode = DuplicateNode(node.children.get(j));
			tempNode.father = rtn;
			rtn.children.add (tempNode);
		}
		return rtn;
	}
}
