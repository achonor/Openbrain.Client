using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language {
    private static Dictionary<int, string> textDict = new Dictionary<int, string>() {
        //tips
        {00001, @"敬请期待..."},
        {00002, @"体力不足..."},
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
        {10109, @"大脑" },
        //匹配界面
        {10201, @"{0}名"},
        {10202, @"正在匹配"},
        {10203, @"正在匹配."},
        {10204, @"正在匹配.."},
        {10205, @"正在匹配..."},
        //准备界面
        {10301, @"第一回合"},
        {10302, @"第二回合"},
        {10303, @"第三回合"},
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
