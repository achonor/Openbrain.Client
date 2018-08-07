using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class AssetBundleBuider
{
    static string assetBundlePath = Application.dataPath + "/AssetBundles";

    static Dictionary<BuildTarget, string> platformConfig = new Dictionary<BuildTarget, string>
    {
        {BuildTarget.StandaloneWindows,  "/editor"},
        {BuildTarget.iOS,  "/ios"},
        {BuildTarget.Android,  "/android"},
    };

    //创建编辑器菜单目录
    [MenuItem("AssetBundle/Build")]
    private static void Build()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

        string fullPath = assetBundlePath + platformConfig[target];
        //如果目录不存在，就创建一个目录
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
        BuildPipeline.BuildAssetBundles(fullPath, BuildAssetBundleOptions.None, target);
    }
}