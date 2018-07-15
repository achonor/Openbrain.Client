using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMethod {

    public static void EnterGame()
    {
        //加载TableUI
        UIManager.OpenUI("Prefabs/TableUI", UIManager.Instance.BaseUIRoot, (GameObject obj) =>
        {
            UserEventManager.TriggerEvent("LoginGame");
            Debug.Log("CommonMethod.EnterGame Open TableUI Success!");
        });
    }
}
