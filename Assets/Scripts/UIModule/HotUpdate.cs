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

    private IEnumerator _CopyFileToPersistent(string[] filelist, System.Action<bool> callback)
    {
        //初始化进度条
        SetSlider(0, filelist.Length);
        //显示进度条
        slider.gameObject.SetActive(true);
        //描述文字
        desText.text = Language.GetTextByKey(10601);

        for (int idx = 0; idx < filelist.Length; idx++)
        {
            string[] keyValue = filelist[idx].Split('|');
            string filePath = GameConst.streamingUrl + keyValue[0];
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

    private void CopyFileToPersistent(System.Action<bool> callback)
    {
        Debug.Log("HotUpdate.CopyFileToPersistent Start");
        //先加载filelist.txt
        WWWLoad(GameConst.streamingUrl + "/" + GameConst.filelistName, (www) =>
        {
            string[] lines = www.text.Split(new string[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            StartCoroutine(_CopyFileToPersistent(lines, (isOK)=>{
                if (isOK)
                {
                    Function.WriteFile(www.bytes, GameConst.persistentPath + "/" + GameConst.filelistName);
                }
                callback(isOK);
            }));
        });
    }

    private void StartHotUpdate()
    {
        Debug.Log("HotUpdate.StartHotUpdate Start");


        //切换场景
        SceneManager.LoadScene("Main");
    }

    //设置进度条
    private void SetSlider(int count, int maxCount)
    {
        countText.text = string.Format("{0}/{1}", count, maxCount);
        slider.value = ((float)count) / maxCount;
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
