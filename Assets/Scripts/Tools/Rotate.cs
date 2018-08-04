using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotate : MonoBehaviour {
    //每秒钟旋转多少度
    public float speed;
    //旋转的欧拉参数
    private Vector3 rotateValue;

    private void Start()
    {
        rotateValue = new Vector3(0, 0, -speed);
    }

    private void Update()
    {
        transform.Rotate(rotateValue * Time.deltaTime);
    }
}
