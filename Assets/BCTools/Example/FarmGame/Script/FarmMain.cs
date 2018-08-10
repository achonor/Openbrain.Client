using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FarmMain : MonoBehaviour
{
    public static bool IsPassMode = true;
    [SerializeField]
    List<GroundLittle> groundQueue = new List<GroundLittle>();
    [SerializeField]
    List<GroundBig> groundBigQueue = new List<GroundBig>();
    [SerializeField]
    GridLayoutGroup groundQueueGrid;
    [SerializeField]
    BCRandomMaker randomMaker;
    [SerializeField]
    GameObject AssitGo;
    [SerializeField]
    GameObject AssitPassGo;
    [SerializeField]
    GameObject AssitRotateGo;
    [SerializeField]
    Text passTxt;
    [SerializeField]
    GameObject TipPanel;
    [SerializeField]
    Text TipText;
    [SerializeField]
    Button TipBlankButton;
    [SerializeField]
    Button TipExitButton;

    #region ChooseMode
    [SerializeField]
    Button PassModeButton;
    [SerializeField]
    Button RotateModeButton;
    [SerializeField]
    GameObject ChooseRotatePanel;
    [SerializeField]
    BCTweenRectTranSize ChooseModeTween;
    [SerializeField]
    GameObject ChooseModeBlock;
    #endregion

    int mPassCount = 4;
    int passCount
    {
        get { return mPassCount; }
        set
        {
            passTxt.text = "" + value;
            mPassCount = value;
        }
    }
    int[] CurrentHardLevel;
    #region Setting
    int[] normalLevel = new int[]
    {
        100,//RL
        100,//TB
        100,//RB
        100,//RT
        100,//LB
        100,//LT
        100,//LTR
        100,//TRB
        100,//RBL
        100,//BLT
        100,//LTRB
    };
    #endregion Setting
    void Awake()
    {
        CurrentHardLevel = normalLevel;
        IsPassMode = PlayerPrefs.GetInt("IsPassMode") == 0 ? true : false;
        ReFreshMode();
    }
    void ReFreshMode()
    {
        if(IsPassMode)
        {
            AssitPassGo.SetActive(true);
            AssitRotateGo.SetActive(false);
            PassModeButton.transform.SetAsFirstSibling();
        }
        else
        {
            AssitPassGo.SetActive(false);
            AssitRotateGo.SetActive(true);
            RotateModeButton.transform.SetAsFirstSibling();
        }
    }
    void Start()
    {
        for(int i=0; i<4; i++)
        {
            GroundLittle gb = Instantiate<GroundLittle>(groundQueue[0]);
            gb.transform.SetParent(groundQueueGrid.transform,false);
            groundQueue.Add(gb);
        }
        for(int i=0; i<groundQueue.Count; i++)
        {
            groundQueue[i].groundType = (GroundBase.GroundType)BCRandomMaker.GetRandByWeight(CurrentHardLevel);
        }

        for(int i=0;i<24;i++)
        {
            GroundBig gb = Instantiate<GroundBig>(groundBigQueue[0]);
            gb.transform.SetParent(groundBigQueue[0].transform.parent, false);
            gb.InitGround(i+1);
            groundBigQueue.Add(gb);
        }
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            TipText.text = "Sure To Exit ?";
            TipExitButton.gameObject.SetActive(true);
            TipPanel.SetActive(true);
        }
    }

    void MoveQueue()
    {
        groundQueue[groundQueue.Count - 1].groundType = (GroundBase.GroundType)BCRandomMaker.GetRandByWeight(CurrentHardLevel);
        groundQueueGrid.enabled = false;
        GroundLittle temp = groundQueue[0];
        temp.transform.SetAsLastSibling();
        groundQueue.Remove(temp);
        groundQueue.Add(temp);
        for (int i = 0; i < groundQueue.Count; i++)
        {
            groundQueue[i].tweenPos.PlayForwardForce();
        }
    }

    public void OnClickPass()
    {
        MoveQueue();
        passCount--;
        if(passCount <1)
        {
            passCount = 4;
            AssitGo.SetActive(false);
        }
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickPassMode()
    {
        if(IsPassMode)
        {
            ChooseModeBlock.SetActive(true);
            ChooseModeTween.PlayForwardForce();
        }
        else
        {
            ChooseModeTween.SetCurrentValueToStart();
            IsPassMode = true;
            PlayerPrefs.SetInt("IsPassMode",0);
            ReFreshMode();
            ChooseModeBlock.SetActive(false);
            ReStart();
        }
    }
    public void OnClickRotateMode()
    {
        if (!IsPassMode)
        {
            ChooseModeBlock.SetActive(true);
            ChooseModeTween.PlayForwardForce();
        }
        else
        {
            ChooseModeTween.SetCurrentValueToStart();
            IsPassMode = false;
            PlayerPrefs.SetInt("IsPassMode", -1);
            ReFreshMode();
            ChooseModeBlock.SetActive(false);
            ReStart();
        }
    }
    GroundBig currentGroud;
    public void OnClickRotateGround(GroundBig ground)
    {
        currentGroud = ground;
        if(AssitRotateGo.activeInHierarchy)
        {
            this.ChooseRotatePanel.SetActive(true);
        }
    }
    public void OnClickRotate(bool isLeft)
    {
        this.ChooseRotatePanel.SetActive(false);
        currentGroud.Rotate(isLeft);
        passCount--;
        if (passCount < 1)
        {
            passCount = 4;
            AssitGo.SetActive(false);
        }
        CheckWater();
    }

    public GroundBase.GroundType HandleClickGround()
    {
        GroundBase.GroundType rt = groundQueue[0].groundType;
        MoveQueue();
        return rt;
    }

    public void ReStart()
    {
        if(!IsPassMode)
        {
            ChooseModeTween.SetCurrentValueToStart();
            ChooseModeBlock.SetActive(false);
            ChooseRotatePanel.SetActive(false);
        }
        for (int i = 0; i < groundQueue.Count; i++)
        {
            groundQueue[i].groundType = (GroundBase.GroundType)BCRandomMaker.GetRandByWeight(CurrentHardLevel);
        }
        for (int i = 0; i < groundBigQueue.Count; i++)
        {
            groundBigQueue[i].InitGround(i);
        }
        TipBlankButton.onClick.RemoveListener(ReStart);
    }

    List<int> gotWaterIndexRecord = new List<int>();
    public void CheckWater()
    {
        gotWaterIndexRecord.Clear();
        for (int i=0;i<groundBigQueue.Count;i++)
        {
            groundBigQueue[i].hasChecked = false;
            if(!IsPassMode)
            {
                gotWaterIndexRecord.Add(i);
            }
        }
        if(groundBigQueue[2].CanGoUp())
        {
            CheckWater(groundBigQueue[2]);
        }
        if(!IsPassMode)
        {
            for(int i=0;i<gotWaterIndexRecord.Count;i++)
            {
                groundBigQueue[gotWaterIndexRecord[i]].CancelWater();
            }
        }
        CheckWin();
    }
    public void CheckWater(GroundBig gb)
    {
        if (!IsPassMode)
        {
            gotWaterIndexRecord.Remove(gb.GetIndex());
        }
        gb.ChangeColor();
        gb.hasChecked = true;
        if (gb != groundBigQueue[2] && gb.CanGoUp())
        {
            int upIndex = gb.FindUpIndex();
            if (upIndex >= 0 && !groundBigQueue[upIndex].hasChecked && groundBigQueue[upIndex].CanGoDown())
            {
                CheckWater(groundBigQueue[upIndex]);
            }
        }
        if (gb.CanGoDown())
        {
            int downIndex = gb.FindDownIndex();
            if (downIndex >= 0 && !groundBigQueue[downIndex].hasChecked && groundBigQueue[downIndex].CanGoUp())
            {
                CheckWater(groundBigQueue[downIndex]);
            }
        }
        if (gb.CanGoLeft())
        {
            int leftIndex = gb.FindLeftIndex();
            if (leftIndex >= 0 && !groundBigQueue[leftIndex].hasChecked && groundBigQueue[leftIndex].CanGoRight())
            {
                CheckWater(groundBigQueue[leftIndex]);
            }
        }
        if (gb.CanGoRight())
        {
            int rightIndex = gb.FindRightIndex();
            if (rightIndex >= 0 && !groundBigQueue[rightIndex].hasChecked && groundBigQueue[rightIndex].CanGoLeft())
            {
                CheckWater(groundBigQueue[rightIndex]);
            }
        }
    }

    void CheckWin()
    {
        bool win = true;
        for (int i = 0; i < groundBigQueue.Count; i++)
        {
            if(!groundBigQueue[i].IsShow())
            {
                return;
            }
            if (!groundBigQueue[i].gotWater)
                win = false;
        }
        if(win)
        {
            TipText.text = "You Win";
        }
        else if(!IsPassMode && AssitRotateGo.activeInHierarchy)
        {
            return;
        }
        else
        {
            TipText.text = "You Lose";
        }
        TipBlankButton.onClick.AddListener(ReStart);
        TipExitButton.gameObject.SetActive(false);
        TipPanel.SetActive(true);
    }
}
