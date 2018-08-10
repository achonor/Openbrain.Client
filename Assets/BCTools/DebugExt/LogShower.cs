using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogShower : MonoBehaviour
{
    private bool showNormal = true;
    private bool showWarning = true;
    private bool showError = true;
    public int maxLogCount = 2000;
    [SerializeField] private ScrollRect logScoll;
    [SerializeField] private RectTransform logUnit;
    [SerializeField] private Text detailTxt;
    [SerializeField] private Toggle switchShow;
    [SerializeField] private Toggle switchRecord;
    [SerializeField] private Toggle switchLogNormal;
    [SerializeField] private Toggle switchLogWarning;
    [SerializeField] private Toggle switchLogError;
    [SerializeField] private Toggle switchTopNew;
    [SerializeField] private Button btnClear;
    private RectTransform tranContent;
    private bool Recording = true;
    private RectTransform rectTran;
    private float panelHeight = 400;
    private float singleHeight = 30;
    private LogData currentDetailInfo = new LogData("","",LogType.Log);
    private int uiCount = 1;
    private bool newLog = false;
    private const int ScrollCounterMax = 2;//拖拽时刷新界面的跳帧个数
    private int ScrollCounter = ScrollCounterMax;
    private bool newLogOnTop = false;//true的话新的log出现在上方

    private List<LogUIData> logUIList = new List<LogUIData>();
    private List<LogData> logList = new List<LogData>();

    class LogUIData
    {
        public RectTransform ui;
        public Text logTxt;
        public LogData data;

        public LogUIData(RectTransform go)
        {
            data = new LogData("", "", LogType.Log);
            ui = go;
            logTxt = go.GetChild(0).GetComponent<Text>();
        }

        public void SetUI(float localPosY)
        {
            switch (data.type)
            {
                case LogType.Log:
                    logTxt.text = string.Format("({0})<color=#94E5FB>{1}</color>", data.count, data.logString);
                    break;
                case LogType.Warning:
                    logTxt.text = string.Format("({0})<color=#FFFF00>{1}</color>", data.count, data.logString);
                    break;
                default:
                    logTxt.text = string.Format("({0})<color=#FF4242>{1}</color>", data.count, data.logString);
                    break;
            }
            ui.localPosition = new Vector3(0,localPosY,0);
            ui.gameObject.SetActive(true);
        }
    }
    class LogData
    {
        public int count;
        public string logString;
        public string stacTrace;
        public LogType type;
        public LogData(string a, string b, LogType c)
        {
            logString = a;
            stacTrace = b;
            type = c;
            count = 1;
        }

        public void Copy(LogData d)
        {
            logString = d.logString;
            stacTrace = d.stacTrace;
            type = d.type;
            count = d.count;
        }

        public void Set(string a, string b, LogType c)
        {
            logString = a;
            stacTrace = b;
            type = c;
            count = 1;
        }
    }

    void Awake()
    {
        rectTran = GetComponent<RectTransform>();
        tranContent = logUnit.parent.GetComponent<RectTransform>();
        panelHeight = rectTran.rect.height;
        singleHeight = logUnit.rect.height + 1;
        uiCount = (int)(panelHeight / singleHeight + 0.99f);
        logUnit.gameObject.SetActive(false);
        logUIList.Add(new LogUIData(logUnit));
        for (int i = 1; i < uiCount; i++)
        {
            RectTransform rtran = RectTransform.Instantiate<RectTransform>(logUnit);
            rtran.SetParent(logUnit.parent, false);
            logUIList.Add(new LogUIData(rtran));
            rtran.GetComponent<Button>().onClick.AddListener(() => { OnClickUnit(rtran); });
        }
        logUnit.GetComponent<Button>().onClick.AddListener(() => { OnClickUnit(logUnit); });
        logScoll.onValueChanged.AddListener(OnScrollValueChanged);
        switchShow.onValueChanged.AddListener(OnActiveToggleChange);
        switchRecord.onValueChanged.AddListener(OnRecordToggleChange);
        switchLogNormal.onValueChanged.AddListener(OnLogNormalToggleChange);
        switchLogWarning.onValueChanged.AddListener(OnLogWarningToggleChange);
        switchLogError.onValueChanged.AddListener(OnLogErrorToggleChange);
        switchTopNew.onValueChanged.AddListener(OnTopNewChange);
        btnClear.onClick.AddListener(OnClickClear);
        switchRecord.gameObject.SetActive(false);
        switchLogNormal.gameObject.SetActive(false);
        switchLogWarning.gameObject.SetActive(false);
        switchLogError.gameObject.SetActive(false);
        switchTopNew.gameObject.SetActive(false);
        btnClear.gameObject.SetActive(false);
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
    }

 
    void OnEnable()
    {
#if UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
        Application.RegisterLogCallback(HandleLog);
#endif
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8
        Application.logMessageReceived += HandleLog;
#endif
    }
    void OnDisable()
    {
#if UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
        Application.RegisterLogCallback(null);
#endif
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8
        Application.logMessageReceived -= HandleLog;
#endif
    }
    void Update()
    {
        if (newLog)
        {
            newLog = false;
            RefreshUIList();
        }
    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (!Recording) return;
        //logString主体,stackTrace详情
        bool needAdd = true;
        if (logList.Count > 0)
        {
            LogData data = logList[logList.Count - 1];
            if (data.logString.Equals(logString) && data.stacTrace.Equals(stackTrace))
            {
                data.count++;
                needAdd = false;
            }
        }
        if (needAdd)
        {
            LogData data = GetUsableLogData();
            if (data == null)
            {
                data = new LogData(logString, stackTrace, type);
            }
            else
            {
                data.Set(logString, stackTrace, type);
            }
            logList.Add(data);
            if (logList.Count > this.maxLogCount)
            {
                //此处采用全部进缓冲池，最大程度减少GC，如果也想省一些内存，可以对dataPool长度设限
                dataPool.Add(logList[0]);
                logList.RemoveAt(0);
            }
        }
        this.newLog = true;
    }

    private int GetIndexFirstShow()
    {
        int rt = 0;
        if (tranContent.localPosition.y >= 0)
        {
            rt = (int)(tranContent.localPosition.y / singleHeight);
        }
        return rt;
    }
    private void OnScrollValueChanged(Vector2 vec)
    {
        if (ScrollCounter >= ScrollCounterMax || vec.y <0.02f || vec.y > 0.98f)
        {
            newLog = true;
            ScrollCounter = 0;
        }
        else
        {
            ScrollCounter++;
        }
    }
    private void RefreshUIList()
    {
        int indexFirstShow = GetIndexFirstShow();
        int uiIndex = 0;
        int ShowCount = 0;
        bool logOnTop = this.newLogOnTop;
        for (int index = 0; index < logList.Count; index++)
        {
            int i = index;
            if (logOnTop)
            {
                i = logList.Count - 1 - index;
            }
            bool needSetUI = false;
            switch (logList[i].type)
            {
                case LogType.Log:
                    if (showNormal)
                    {
                        ShowCount++;
                        needSetUI = true;
                    }
                    break;
                case LogType.Warning:
                    if (showWarning)
                    {
                        ShowCount++;
                        needSetUI = true;
                    }
                    break;
                default:
                    if (showError)
                    {
                        ShowCount++;
                        needSetUI = true;
                    }
                    break;
            }
            if (ShowCount > indexFirstShow && needSetUI)
            {
                if (uiIndex < uiCount)
                {
                    logUIList[uiIndex].data.Copy(logList[i]);
                    logUIList[uiIndex].SetUI((ShowCount-1)*singleHeight*-1);
                }
                uiIndex++;
            }
        }
        for (; uiIndex < uiCount; uiIndex++)
        {
            logUIList[uiIndex].ui.gameObject.SetActive(false);
        }
        tranContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ShowCount * singleHeight);
    }
    private void OnClickUnit(RectTransform rtran)
    {
        for (int i = 0; i < logUIList.Count; i++)
        {
            if (logUIList[i].ui == rtran)
            {
                currentDetailInfo.Copy(logUIList[i].data);
                RefreshDetailText();
            }
        }
    }
    private void RefreshDetailText()
    {
        switch (currentDetailInfo.type)
        {
            case LogType.Log:
                detailTxt.text = string.Format("<color=#94E5FB>{0}</color>\n{1}", currentDetailInfo.logString, currentDetailInfo.stacTrace);
                break;
            case LogType.Warning:
                detailTxt.text = string.Format("<color=#FFFF00>{0}</color>\n{1}", currentDetailInfo.logString, currentDetailInfo.stacTrace);
                break;
            default:
                detailTxt.text = string.Format("<color=#FF4242>{0}</color>\n{1}", currentDetailInfo.logString, currentDetailInfo.stacTrace);
                break;
        }
    }

    private void OnActiveToggleChange(bool isOn)
    {
        if (isOn)
        {
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);
        }
        else
        {
            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        }
        switchRecord.gameObject.SetActive(isOn);
        switchLogNormal.gameObject.SetActive(isOn);
        switchLogWarning.gameObject.SetActive(isOn);
        switchLogError.gameObject.SetActive(isOn);
        switchTopNew.gameObject.SetActive(isOn);
        btnClear.gameObject.SetActive(isOn);
    }

    private void OnRecordToggleChange(bool isOn)
    {
        this.Recording = isOn;
    }
    private void OnLogNormalToggleChange(bool isOn)
    {
        this.showNormal = isOn;
        newLog = true;
    }
    private void OnLogWarningToggleChange(bool isOn)
    {
        this.showWarning = isOn;
        newLog = true;
    }
    private void OnLogErrorToggleChange(bool isOn)
    {
        this.showError = isOn;
        newLog = true;
    }

    private void OnTopNewChange(bool isOn)
    {
        this.newLogOnTop = isOn;
        newLog = true;
    }
    private void OnClickClear()
    {
        for (int i = 0; i < logList.Count; i++)
        {
            dataPool.Add(logList[i]);
        }
        logList.Clear();
        newLog = true;
    }

    #region LogData缓冲池
    private List<LogData> dataPool = new List<LogData>();
    private LogData GetUsableLogData()
    {
        LogData rt = null;
        if (dataPool.Count > 0)
        {
            rt = dataPool[0];
            dataPool.RemoveAt(0);
        }
        return rt;
    }
    #endregion

#if UNITY_EDITOR
    [ContextMenu("Test Log")]
    private void TestLog()
    {
        for (int i = 0; i < 2; i++)
        {
            Debug.Log("LogNormal");
        }
        Debug.LogWarning("LogWarning");
        Debug.LogError("LogError");
    }
#endif
}