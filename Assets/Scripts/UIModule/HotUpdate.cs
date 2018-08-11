using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;


//热更新
public class HotUpdate : MonoBehaviour {

    public Text desText;
    public Text countText;
    public Slider slider;

    private void Awake()
    {
    }

    private void Start()
    {
        //检查persistent中是否有filelist.txt
        string filelistPath = GameConst.persistentPath + "/" + GameConst.filelistName;
        if (!File.Exists(filelistPath))
        {
            //第一次运行
            //拷贝Streaming目录下所有文件到Persistent目录
            CopyFileToPersistent((isOK) =>
            {
                if (false == isOK)
                {
                    Debug.LogError("CopyFileToPersistent Faild!");
                    return;
                }
                StartHotUpdate();
            });
        }
        else
        {
            //检查热更新文件
            StartHotUpdate();
        }
    }
    /// <summary>
    /// 拷贝Streaming目录下所有文件到Persistent目录
    /// </summary>
    /// <param name="callback"></param>
    private void CopyFileToPersistent(System.Action<bool> callback)
    {
        Debug.Log("HotUpdate.CopyFileToPersistent Start");
        //描述文字
        desText.text = Language.GetTextByKey(10601);
        //先加载filelist.txt
        WWWLoad(GameConst.streamingUrl + "/" + GameConst.filelistName, (www) =>
        {
            string[] lines = www.text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            StartCoroutine(_DownloadFileToPersistent(GameConst.streamingUrl, lines, (isOK)=> {
                if (isOK)
                {
                    Function.WriteFile(www.bytes, GameConst.persistentPath + "/" + GameConst.filelistName);
                }
                callback(isOK);
            }));
        });
    }

    //热更新
    private void StartHotUpdate()
    {
        Debug.Log("HotUpdate.StartHotUpdate Start");
        //描述文字
        desText.text = Language.GetTextByKey(10601);
        //读取本地filelist.txt
        FileInfo localFilelist = new FileInfo(GameConst.persistentPath + "/" + GameConst.filelistName);
        StreamReader streamReader = localFilelist.OpenText();
        string localText = streamReader.ReadToEnd();
        //关闭文件
        streamReader.Close();
        string[] localLines = localText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        //本地文件名和md5码的map
        Dictionary<string, string> localFileMap = new Dictionary<string, string>();
        for (int idx = 0; idx < localLines.Length; idx++)
        {
            string[] keyValue = localLines[idx].Split('|');
            localFileMap.Add(keyValue[0], keyValue[1]);
        }


        //加载服务器filelist.txt
        WWWLoad(GameConst.hotUpdateUrl + "/" + GameConst.filelistName, (www) =>
        {
            string[] newLines = www.text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            float updateSize = 0;
            //获取差异文件列表
            List<string> diffList = new List<string>();
            for (int idx = 0; idx < newLines.Length; idx++)
            {
                string[] keyValue = newLines[idx].Split('|');
                if (!localFileMap.ContainsKey(keyValue[0]) || localFileMap[keyValue[0]] != keyValue[1])
                {
                    diffList.Add(newLines[idx]);
                    updateSize += Convert.ToInt32(keyValue[2]);
                }
            }
            if (0 == diffList.Count)
            {
                //不需要更新
                EnterMainScene();
                return;
            }
            //描述文字
            desText.text = string.Format(Language.GetTextByKey(10602), CommonMethod.DataSize2String(updateSize));
            //开始热更新
            StartCoroutine(_DownloadFileToPersistent(GameConst.hotUpdateUrl, diffList.ToArray(), (isOK) =>
            {
                if (isOK)
                {
                    Function.WriteFile(www.bytes, GameConst.persistentPath + "/" + GameConst.filelistName);
                    //切换场景
                    EnterMainScene();
                }
                else
                {
                    Debug.LogError("HotUpdate.StartHotUpdate Faild!");
                }

            }));
        });
    }
    /// <summary>
    /// 从srcUrl下载文件覆盖到Persistent目录
    /// </summary>
    /// <param name="srcUrl"></param>
    /// <param name="filelist">需要下载的文件信息</param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator _DownloadFileToPersistent(string srcUrl, string[] filelist, System.Action<bool> callback)
    {
        //初始化进度条
        SetSlider(0, filelist.Length);
        //显示进度条
        slider.gameObject.SetActive(true);


        for (int idx = 0; idx < filelist.Length; idx++)
        {
            string[] keyValue = filelist[idx].Split('|');
            string filePath = srcUrl + keyValue[0];
            WWW www = new WWW(filePath);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                callback(true);
                yield break;
            }
            //写入到Persistent
            string newFilePath = GameConst.persistentPath + keyValue[0];
            Function.WriteFile(www.bytes, newFilePath);
            //刷新进度条
            SetSlider(idx + 1, filelist.Length);
        }
        callback(true);
    }

    //设置进度条
    private void SetSlider(int count, int maxCount)
    {
        countText.text = string.Format("{0}/{1}", count, maxCount);
        slider.value = ((float)count) / maxCount;
    }

    //进入游戏主场景
    private void EnterMainScene()
    {
        //切换场景
        SceneManager.LoadScene("Main");
    }
    //异步加载文件
    private IEnumerator _WWWLoad(string filePath, System.Action<WWW> callback)
    {
        WWW www = new WWW(filePath);
        yield return www;
        callback(www);
    }
    public void WWWLoad(string filePath, System.Action<WWW> callback)
    {
        StartCoroutine(_WWWLoad(filePath, callback));
    }
}
