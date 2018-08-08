using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class AssetBundleBuider
{
    static string assetBundlePath = Application.dataPath + "/AssetBundles";
    static string streamingBundlePath = Application.streamingAssetsPath + "/AssetBundles";

    static string luaSrcPath = Application.dataPath + "/Lua/LuaScripts";
    static string luaTempPath = Application.dataPath + "/Lua/LuaScriptsTemp";



    static Dictionary<BuildTarget, string> platformConfig = new Dictionary<BuildTarget, string>
    {
        {BuildTarget.StandaloneWindows,  "/editor"},
        {BuildTarget.iOS,  "/ios"},
        {BuildTarget.Android,  "/android"},
    };

    //打包l
    [MenuItem("AssetBundle/Build")]
    private static void BuildLuaFile()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        string fullPath = assetBundlePath + platformConfig[target];
        //如果目录不存在，就创建一个目录
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
        if (!Directory.Exists(luaTempPath))
        {
            Directory.CreateDirectory(luaTempPath);
        }
        //拷贝lua文件
        DirectoryInfo luaFolder = new DirectoryInfo(luaSrcPath);
        ChangeLua2Bytes(luaFolder, luaTempPath);

        //打包
        BuildPipeline.BuildAssetBundles(fullPath, BuildAssetBundleOptions.None, target);

        //生成md5列表文件
        DirectoryInfo md5Folder = new DirectoryInfo(fullPath);
        //创建filelist.txt
        var utf8WithoutBom = new System.Text.UTF8Encoding(false);
        FileStream fileStream = new FileStream(fullPath + "/" + "filelist.txt", FileMode.Create);
        StreamWriter fileWriter = new StreamWriter(fileStream, utf8WithoutBom);
        foreach (var nextFile in md5Folder.GetFiles("*.assetbundle"))
        {
            string tmpLine = string.Format("{0}|{1}|{2}", nextFile.Name, GetMD5HashFromFile(nextFile.FullName), nextFile.Length);
            Debug.Log(tmpLine);
            fileWriter.WriteLine(tmpLine);
        }
        //保存文件
        fileWriter.Flush();
        fileWriter.Close();
        fileStream.Close();

        //拷贝一份到StreamingAssets
        DirectoryInfo bundleFolder = new DirectoryInfo(assetBundlePath);
        OverDirectory(bundleFolder, streamingBundlePath);
    }

    private static void ChangeLua2Bytes(DirectoryInfo theFolder, string path)
    {
        foreach (var nextFile in theFolder.GetFiles("*.lua"))
        {
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            //打开源文件
            StreamReader srcFile = new StreamReader(nextFile.FullName, utf8WithoutBom);
            string srcText = srcFile.ReadToEnd();
            srcFile.Close();
            //写入到旧文件
            FileStream fileStream = new FileStream(path + "/" + Path.ChangeExtension(nextFile.Name + nextFile.Extension, "bytes"), FileMode.Create);
            StreamWriter desFile = new StreamWriter(fileStream, utf8WithoutBom);
            desFile.Write(srcText);
            desFile.Flush();
            desFile.Close();
            fileStream.Close();
        }
        foreach (DirectoryInfo nextFolder in theFolder.GetDirectories())
        {
            ChangeLua2Bytes(nextFolder, path);
        }
    }
    /// <summary>
    /// 目录覆盖
    /// </summary>
    /// <param name="sourceDir"></param>
    /// <param name="destDir"></param>
    private static void OverDirectory(DirectoryInfo sourceDir, string destDir)
    {
        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }
        foreach (var nextFile in sourceDir.GetFiles())
        {
            File.Copy(nextFile.FullName, destDir + "/" + nextFile.Name, true);
        }
        foreach (DirectoryInfo nextFolder in sourceDir.GetDirectories())
        {
            OverDirectory(nextFolder, destDir + "/" + nextFolder.Name);
        }
    }
    /// <summary>
    /// 获取文件MD5
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private static string GetMD5HashFromFile(string fileName)
    {
        try
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }
}