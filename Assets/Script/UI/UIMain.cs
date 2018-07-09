using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : UIBase {


    Polygon polygon;
    Transform[] pointList = new Transform[6];
    Transform[] maxPointList = new Transform[6];
    private void Awake()
    {
        //六边形控件
        polygon = transform.Find("Hexagon/Polygon").GetComponent<Polygon>();
        //六边形顶点
        Transform pointParent = transform.Find("Hexagon/Polygon");
        for (int idx = 0; idx < pointParent.childCount; idx++)
        {
            pointList[idx] = pointParent.Find((idx + 1).ToString());
        }
        Transform maxPointParent = transform.Find("Hexagon/MaxPoint");
        for (int idx = 0; idx < maxPointParent.childCount; idx ++)
        {
            maxPointList[idx] = maxPointParent.Find((idx + 1).ToString());
        }
    }

    private void Start()
    {
        float[] values = new float[6] { 0.8f, 0.6f, 0.9f, 0.1f, 0.5f, 1.0f };
        for(int idx = 0; idx < values.Length; idx ++)
        {
            var point = pointList[idx];
            var maxPoint = maxPointList[idx];
            point.localPosition = (maxPoint.localPosition - point.localPosition) * values[idx];
        }

    }

}
