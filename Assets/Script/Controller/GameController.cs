using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    void Start()
    {
        //游戏开始，打开登陆界面
        UIManager.OpenUI("Prefabs/LoginUI", UIManager.Instance.GameUIRoot, (GameObject obj) =>
        {
            Debug.Log("GameController.Start Open LoginUI Success!");
        });
    }
}