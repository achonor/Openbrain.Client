using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Diagnostics;

public class MenuTools
{
    [MenuItem("Tools/Quick Handle")]
    public static void QuickHandle()
    {
        //打包AssetBundle到StreamingAssetsPath
        AssetBundleBuider.BuildAll();
        //拷贝lua到StreamingAssetsPath
        ToLuaMenu.CopyLuaFilesToStreaming();
        //对lua代码加密
        LuaToBytecode(GameConst.streamingPath);

        //生成md5列表文件
        DirectoryInfo md5Folder = new DirectoryInfo(GameConst.streamingPath);
        //创建filelist.txt
        var utf8WithoutBom = new System.Text.UTF8Encoding(false);
        FileStream fileStream = new FileStream(GameConst.streamingPath + "/" + GameConst.filelistName, FileMode.Create);
        StreamWriter fileWriter = new StreamWriter(fileStream, utf8WithoutBom);

        CreateFileList(md5Folder, fileWriter, "");
        //保存文件
        fileWriter.Flush();
        fileWriter.Close();
        fileStream.Close();

        //拷贝一份到Persistent
        DirectoryInfo bundleFolder = new DirectoryInfo(GameConst.streamingPath);
        Function.OverDirectory(bundleFolder, GameConst.persistentPath);
    }

    [MenuItem("Lua/ReconnectionLuaDebug")]
    public static void ReconnectionLuaDebug()
    {
        if(null == LuaScriptManager.Instance)
        {
            UnityEngine.Debug.LogError("Need to start the game first!");
            return;
        }
        LuaScriptManager.Instance.ReconnectionLuaDebug();
    }
    public static void LuaToBytecode(string path)
    {
        DirectoryInfo luaPath = new DirectoryInfo(path);
        LuaToBytecode(luaPath);
    }
    /// <summary>
    /// 加密目录下所有lua文件
    /// </summary>
    /// <param name="luaPath"></param>
    private static void LuaToBytecode(DirectoryInfo luaPath)
    {
        foreach (var luaFile in luaPath.GetFiles("*.lua"))
        {
            //string outString = RunCmd(string.Format("D:/LuaJIT-2.1.0/bin/luajit.exe -b {0} {1}", luaFile.FullName, luaFile.FullName));
            string outString = RunCmd("D:/LuaJIT-2.1.0/bin/luajit.exe", string.Format("-b {0} {1}", luaFile.FullName, luaFile.FullName));
            UnityEngine.Debug.Log(outString);
        }
        foreach (DirectoryInfo nextFolder in luaPath.GetDirectories())
        {
            LuaToBytecode(nextFolder);
        }
    }

    /// <summary>
    /// 写入filelist.txt
    /// </summary>
    /// <param name="folder">目录信息</param>
    /// <param name="fileWriter">文件流</param>
    /// <param name="prefix">文件名前缀</param>
    private static void CreateFileList(DirectoryInfo folder, StreamWriter fileWriter, string prefix)
    {
        foreach (var nextFile in folder.GetFiles())
        {
            if (GameConst.filelistName == nextFile.Name || ".meta" == nextFile.Extension)
            {
                continue;
            }
            string tmpLine = string.Format("{0}/{1}|{2}|{3}", prefix, nextFile.Name, Function.GetMD5HashFromFile(nextFile.FullName), nextFile.Length);
            UnityEngine.Debug.Log(tmpLine);
            fileWriter.WriteLine(tmpLine);
        }
        foreach (DirectoryInfo nextFolder in folder.GetDirectories())
        {
            string newPrefix = string.Format("{0}/{1}", prefix, nextFolder.FullName.Substring(folder.FullName.Length + 1));
            CreateFileList(nextFolder, fileWriter, newPrefix);
        }
    }

    /// <summary>
    /// 拷贝bytes文件
    /// </summary>
    /// <param name="theFolder"></param>
    /// <param name="path"></param>
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

    public static string RunCmd(string fileName, string arguments)
    {
        ProcessStartInfo processInfo = new ProcessStartInfo();
        processInfo.FileName = fileName;
        processInfo.Arguments = arguments;
        processInfo.UseShellExecute = false;        //是否使用操作系统shell启动
        processInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
        processInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
        processInfo.RedirectStandardError = true;   //重定向标准错误输出
        processInfo.CreateNoWindow = true;          //不显示程序窗口
        //启动
        Process process = Process.Start(processInfo);

        //获取cmd窗口的输出信息
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();//等待程序执行完退出进程
        process.Close();
        return output;
    }
}
