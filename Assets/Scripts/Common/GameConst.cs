using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//常量
public class GameConst {

    public static Color Gray = new Color(125, 125, 125, 255) / 255f;
    public static Color Bule = new Color(66, 178, 231, 255) / 255f;
    public static Color LightRed = new Color(255, 153, 153, 255) / 255f;
    public static Color LightBlue = new Color(138, 214, 230, 255) / 255f;
    public static Color Yellow = new Color(255, 207, 49, 255) / 255f;
    public static Color RedBlock = new Color(247, 73, 74, 255) / 255f;
    public static Color BlueBlock = new Color(8, 69, 99, 255) / 255f;

    //是否使用Persistent目录的资源
#if UNITY_EDITOR
    public static bool UsePersistent = true;
#else
    public static bool UsePersistent = true;
#endif

#if UNITY_STANDALONE
    public static string osDir = "Win";
#elif UNITY_ANDROID
    public static string osDir = "Android";
#elif UNITY_IPHONE
    public static string osDir = "iOS";        
#else
    public static string osDir = "";        
#endif
    //资源路径
    public static string streamingPath = Application.streamingAssetsPath + "/" + GameConst.osDir;
    public static string persistentPath = Application.persistentDataPath + "/" + GameConst.osDir;
    //Streaming中的AssetBundles的路径
    public static string abStreamingPath = Application.streamingAssetsPath + "/" + GameConst.osDir + "/AssetBundles";
    //AssetBundles的路径
    public static string assetbundleRootPath = Application.persistentDataPath + "/" + GameConst.osDir + "/AssetBundles/";

    //fileList.txt
    public static string filelistName = "filelist.txt";

    //streaming目录的url
#if UNITY_EDITOR || (!UNITY_ANDROID)
    public static string streamingUrl = "file://" + Application.streamingAssetsPath + "/" + GameConst.osDir;
#else
    public static string streamingUrl = Application.streamingAssetsPath + "/" + GameConst.osDir;
#endif
    //热更新服务器地址
    public static string hotUpdateUrl = "http://106.12.93.89:8080/openbrain/" + GameConst.osDir;
}
