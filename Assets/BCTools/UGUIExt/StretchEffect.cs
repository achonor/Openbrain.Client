using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class StretchEffect : BaseMeshEffect
{
    public Color color = Color.white;
    public Transform LeftUp;
    public Transform RightUp;
    public Transform LeftBottom;
    public Transform RightBottom;
    public bool backSlash = false;
    public bool enableChange = true;
    private RectTransform mrectTran;
    private RectTransform rectTran
    {
        get {
            if (mrectTran == null)
                mrectTran = this.GetComponent<RectTransform>();
            return mrectTran;
        }
    }
    public override void ModifyMesh(VertexHelper vh)
    {
        if (this.enabled && LeftUp && RightUp && LeftBottom && RightBottom)
        {
            vh.Clear();
            vh.AddVert(LeftBottom.localPosition, color, new Vector2(0f, 0f));
            vh.AddVert(LeftUp.localPosition, color, new Vector2(0f, 1f));
            vh.AddVert(RightUp.localPosition, color, new Vector2(1f, 1f));
            vh.AddVert(RightBottom.localPosition, color, new Vector2(1f, 0f));
            if (backSlash)
            {
                vh.AddTriangle(1, 0, 3);
                vh.AddTriangle(1, 2, 3);
            }
            else
            {
                vh.AddTriangle(0, 1, 2);
                vh.AddTriangle(2, 3, 0);
            }
        }
    }

    void Update()
    {
        if (enableChange)
        {
            this.OnEnable();
        }
    }

}
