using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language {
    private static Dictionary<int, string> textDict = new Dictionary<int, string>() {
        //tips
        {00001, @"敬请期待..."},
        //等级
        {10001, @"青铜" },
        {10002, @"白银" },
        {10003, @"黄金" },
        {10004, @"铂金" },
        {10005, @"钻石" },
        //熟练度
        {10101, @"脚趾" },
        {10102, @"小腿" },
        {10103, @"膝盖" },
        {10104, @"大腿" },
        {10105, @"屁股" },
        {10106, @"肚子" },
        {10107, @"胸膛" },
        {10108, @"脖子" },
        {10119, @"大脑" }
    };


    public static string GetTextByKey(int key)
    {
        string ret = "";
        if (textDict.TryGetValue(key, out ret))
        {
            return ret;
        }
        return ret;
    }
}
