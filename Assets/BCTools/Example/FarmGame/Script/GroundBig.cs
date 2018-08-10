using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GroundBig : GroundBase
{
    [SerializeField]
    FarmMain mainControl;
    [SerializeField]
    Button btn;
    [SerializeField]
    GameObject goContent;
    [SerializeField]
    BCTweenRotation tweenRotate;
    
    //index of the list
    int index = 0;
    [HideInInspector]
    public bool gotWater = false;
    [HideInInspector]
    public bool hasChecked = false;

    public void InitGround(int index)
    {
        this.btn.enabled = true;
        this.index = index;
        this.gotWater = false;
        this.goContent.SetActive(false);
        this.goContent.GetComponent<BCTweenColor>().SetCurrentValueToStart();
        this.img_H.GetComponent<BCTweenColor>().SetCurrentValueToStart();
    }
    public int GetIndex()
    {
        return this.index;
    }
    public int FindUpIndex()
    {
        return this.index - 5;   //return value < 0 when can not find
    }
    public int FindDownIndex()
    {
        if (this.index < 20)
            return this.index + 5;
        return -1;      //return value < 0 when can not find
    }
    public int FindLeftIndex()
    {
        if (this.index % 5 > 0)
            return this.index - 1;
        return -1;   //return value < 0 when can not find
    }
    public int FindRightIndex()
    {
        if (this.index % 5 < 4)
            return this.index + 1;
        return -1;  //return value < 0 when can not find
    }
    public bool IsShow()
    {
        return this.goContent.activeSelf;
    }
    public bool CanGoUp()
    {
        return this.goContent.activeSelf && this.groundType.ToString().Contains("T");
    }
    public bool CanGoDown()
    {
        return this.goContent.activeSelf &&  this.groundType.ToString().Contains("B");
    }
    public bool CanGoLeft()
    {
        return this.goContent.activeSelf && this.groundType.ToString().Contains("L");
    }
    public bool CanGoRight()
    {
        return this.goContent.activeSelf && this.groundType.ToString().Contains("R");
    }

    public void ChangeColor()
    {
        if(!gotWater)
        {
            this.goContent.GetComponent<BCTweenColor>().PlayForwardForce();
            this.img_H.GetComponent<BCTweenColor>().PlayForwardForce();
            gotWater = true;
        }
    }

    public void CancelWater()
    {
        if(gotWater)
        {
            this.goContent.GetComponent<BCTweenColor>().PlayReverseForce();
            this.img_H.GetComponent<BCTweenColor>().PlayReverseForce();
            gotWater = false;
        }
    }

    public void OnClickMe()
    {
        this.groundType = mainControl.HandleClickGround();
        this.goContent.SetActive(true);
        this.goContent.GetComponent<Button>().enabled = !FarmMain.IsPassMode;
        this.btn.enabled = false;
    }

    public void Rotate(bool isLeft)
    {
        GroundType newType = GroundType.LTRB;
        if (isLeft)
        {
            tweenRotate.from = new Vector3(0, 0, -90f);
            switch (this.groundType)
            {
                case GroundType.BLT:
                    newType = GroundType.RBL;
                    break;
                case GroundType.LB:
                    newType = GroundType.RB;
                    break;
                case GroundType.LT:
                    newType = GroundType.LB;
                    break;
                case GroundType.LTR:
                    newType = GroundType.BLT;
                    break;
                case GroundType.RB:
                    newType = GroundType.RT;
                    break;
                case GroundType.RBL:
                    newType = GroundType.TRB;
                    break;
                case GroundType.RL:
                    newType = GroundType.TB;
                    break;
                case GroundType.RT:
                    newType = GroundType.LT;
                    break;
                case GroundType.TB:
                    newType = GroundType.RL;
                    break;
                case GroundType.TRB:
                    newType = GroundType.LTR;
                    break;
            }
        }
        else
        {
            tweenRotate.from = new Vector3(0, 0, 90f);
            switch (this.groundType)
            {
                case GroundType.BLT:
                    newType = GroundType.LTR;
                    break;
                case GroundType.LB:
                    newType = GroundType.LT;
                    break;
                case GroundType.LT:
                    newType = GroundType.RT;
                    break;
                case GroundType.LTR:
                    newType = GroundType.TRB;
                    break;
                case GroundType.RB:
                    newType = GroundType.LB;
                    break;
                case GroundType.RBL:
                    newType = GroundType.BLT;
                    break;
                case GroundType.RL:
                    newType = GroundType.TB;
                    break;
                case GroundType.RT:
                    newType = GroundType.RB;
                    break;
                case GroundType.TB:
                    newType = GroundType.RL;
                    break;
                case GroundType.TRB:
                    newType = GroundType.RBL;
                    break;
            }
        }
        this.groundType = newType;
        tweenRotate.PlayForwardForce();
    }
}
