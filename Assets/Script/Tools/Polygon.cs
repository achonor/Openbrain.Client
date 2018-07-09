using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class Polygon : Graphic
{
    void Update()
    {
        //SetAllDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (0 == transform.childCount)
        {
            return;
        }
        //清除
        vh.Clear();
        //绘制的颜色
        Color32 color32 = color;
        //添加中心点
        vh.AddVert(transform.localPosition, color32, new Vector2(0f, 0f));
        foreach (Transform child in transform)
        {
            vh.AddVert(child.localPosition, color32, new Vector2(0f, 0f));
        }
        //几何图形中的三角形
        for(int i = 1; i < vh.currentVertCount - 1; i++)
        {
            vh.AddTriangle(0, i, i + 1);
        }
        //闭合
        vh.AddTriangle(0, vh.currentVertCount - 1, 1);
    }
}