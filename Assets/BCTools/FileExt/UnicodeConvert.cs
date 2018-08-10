using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

public class UnicodeConvert
{
	/// <summary>
	/// 汉字转换为Unicode编码
	/// </summary>
	/// <param name="str">要编码的汉字字符串</param>
	/// <returns>Unicode编码的的字符串</returns>
	public static string ToUnicode(string str)
	{
		byte[] bts = Encoding.Unicode.GetBytes(str); //utf-16
		string r = "";
		for (int i = 0; i < bts.Length; i += 2)
			r += "\\u" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
		return r;
	}
	/// <summary>
	/// 将Unicode编码转换为汉字字符串
	/// </summary>
	/// <param name="str">Unicode编码字符串</param>
	/// <returns>汉字字符串</returns>
	public static string ToGB2312(string str)
	{
		string r = "";
		MatchCollection mc = Regex.Matches(str, @"\\u([\w]{2})([\w]{2})");//, RegexOptions.Compiled | RegexOptions.IgnoreCase);
		byte[] bts = new byte[2];
		foreach(Match m in mc )
		{
			bts[0] = (byte)int.Parse(m.Groups[2].Value, NumberStyles.HexNumber);
			bts[1] = (byte)int.Parse(m.Groups[1].Value, NumberStyles.HexNumber);
			r += Encoding.Unicode.GetString(bts);
		}
		return r;
	}

	public static string FuckEmoji(string str)//通过我可以干掉各种emoji符号 [\uD800-\uDBFF][\uDC00-\uDFFF]
	{
		if(string.IsNullOrEmpty(str))return str;
		str = ToUnicode(str);
		string[] tempstr = str.Split(new string[] { @"\u" },StringSplitOptions.RemoveEmptyEntries);
		string rt = "";
		for (int i = 0; i < tempstr.Length; i++)
		{
			if (tempstr[i].Length > 4 || tempstr[i].StartsWith("e") || tempstr[i].StartsWith("f"))
			{
				continue;
			}
			rt += "\\u" + tempstr[i]; 
		}
		rt = ToGB2312(rt);
		return rt;
	}
}
