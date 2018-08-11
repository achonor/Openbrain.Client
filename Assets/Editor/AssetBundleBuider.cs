using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class AssetBundleBuider {
    //打包
    [MenuItem("AssetBundle/Build All")]
    public static void BuildAll()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        //如果目录不存在，就创建一个目录
        if (!Directory.Exists(GameConst.abStreamingPath))
        {
            Directory.CreateDirectory(GameConst.abStreamingPath);
        }

        //打包
        BuildPipeline.BuildAssetBundles(GameConst.abStreamingPath, BuildAssetBundleOptions.None, target);
    }
}