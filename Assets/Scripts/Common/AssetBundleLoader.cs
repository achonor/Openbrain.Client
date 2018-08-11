using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class AssetBundleLoader {

    static Dictionary<string, AssetBundle> assetBundleDict = new Dictionary<string, AssetBundle>();

    public static AssetBundle LoadAssetBundle(string abName)
    {

        string abPath = GameConst.assetbundleRootPath + abName + ".assetbundle";
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

    public static AssetBundleRequest LoadFileFromAssetBundleAsync(string abName, string fileName)
    {
        AssetBundle assetBundle = null;
        if (!assetBundleDict.ContainsKey(abName))
        {
            LoadAssetBundle(abName);
        }
        if (assetBundleDict.TryGetValue(abName, out assetBundle))
        {
            AssetBundleRequest assetBundleRequest = assetBundle.LoadAssetAsync(fileName);
            return assetBundleRequest;
        }
        else
        {
            Debug.LogError("Not found AssetBundle name = " + abName);
            return null;
        }
    }
}
