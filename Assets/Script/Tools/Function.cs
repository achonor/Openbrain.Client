using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class Function{
    private static DateTime _time1970;
    public static DateTime time1970 {
        get
        {
            if (null == _time1970)
            {
                _time1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            }
            return _time1970;
        }
    }

    public static long GetServerTime()
    {
        TimeSpan ts = DateTime.UtcNow - time1970;
        return Convert.ToInt64(ts.TotalSeconds);
    }
}
