using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GroundBase : MonoBehaviour
{
    [SerializeField]
    protected Image img_H;
    [SerializeField]
    protected Image img_V;
    [SerializeField]
    protected GroundType mtype;
    public enum GroundType
    {
        RL,
        TB,
        RB,
        RT,
        LB,
        LT,
        LTR,
        TRB,
        RBL,
        BLT,
        LTRB
    }
    public GroundType groundType
    {
        get { return mtype; }
        set
        {
            if(value == GroundType.RB || value == GroundType.RT || value == GroundType.TRB)
            {
                img_H.transform.localPosition = new Vector3(10.95f, 0, 0);
                img_H.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 42.9f);
            }
            else if(value == GroundType.LB || value == GroundType.LT || value == GroundType.BLT)
            {
                img_H.transform.localPosition = new Vector3(-10.95f, 0, 0);
                img_H.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 42.9f);
            }
            else  if(value == GroundType.RL || value == GroundType.LTRB || value == GroundType.RBL || value == GroundType.LTR)
            {
                img_H.transform.localPosition = Vector3.zero;
                img_H.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 64.8f);
            }
            else
            {
                img_H.transform.localPosition = Vector3.zero;
                img_H.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,0f);
            }

            if(value == GroundType.RB || value == GroundType.LB || value == GroundType.RBL)
            {
                img_V.transform.localPosition = new Vector3(0, -10.5f, 0);
                img_V.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 42f);
            }
            else if(value == GroundType.LT || value == GroundType.RT || value == GroundType.LTR)
            {
                img_V.transform.localPosition = new Vector3(0, 10.5f, 0);
                img_V.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 42f);
            }
            else if(value == GroundType.TB || value == GroundType.LTRB || value == GroundType.TRB || value == GroundType.BLT)
            {
                img_V.transform.localPosition = new Vector3(0, 0, 0);
                img_V.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 63f);
            }
            else
            {
                img_V.transform.localPosition = new Vector3(0, 0, 0);
                img_V.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0f);
            }

            mtype = value;
        }
    }
#if UNITY_EDITOR
    [ContextMenu("RefreshType")]
    void RefreshType()
    {
        groundType = mtype;
    }
#endif
}
