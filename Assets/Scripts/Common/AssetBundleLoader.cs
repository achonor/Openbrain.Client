using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class AssetBundleLoader {
    //static string AssetbundleRootPath = Application.persistentDataPath + "/";
#if UNITY_IOS
    static string AssetbundleRootPath = Application.streamingAssetsPath + "/AssetBundles/ios/";
#elif UNITY_ANDROID
    static string AssetbundleRootPath = Application.streamingAssetsPath + "/AssetBundles/android/";
#else
    static string AssetbundleRootPath = Application.streamingAssetsPath + "/AssetBundles/editor/";
#endif

    static Dictionary<string, AssetBundle> assetBundleDict = new Dictionary<string, AssetBundle>();

    public static AssetBundle LoadAssetBundle(string abName)
    {

        string abPath = AssetbundleRootPath + abName;
        AssetBundle assetBundle = AssetBundle.LoadFromFile(abPath);
        if (null != assetBundle)
        {
            assetBundleDict.Add(abName, assetBundle);
            return assetBundle;
        }
        else
        {
            Debug.LogError("Not path = " + abPath);
        }
        return null;
    }

    public static T LoadFileFromAssetBundle<T>(string abName, string fileName) where T : Object
    {
        AssetBundle assetBundle = null;
        if (!assetBundleDict.ContainsKey(abName))
        {
            LoadAssetBundle(abName);
        }
        if (assetBundleDict.TryGetValue(abName, out assetBundle))
        {
            T asset = assetBundle.LoadAsset(fileName) as T;
            return asset;
        }
        else {
            Debug.LogError("Not found AssetBundle name = " + abName);
            return null;
        }
    }
}
