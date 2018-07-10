using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UITable : UIBase {

    private void Awake()
    {
    }

    private void Start()
    {
        
    }

    public override void OnOpen()
    {
        //默认打开主UI
        UIManager.OpenUI("Prefabs/MainUI", UIManager.Instance.GameUIRoot, (GameObject obj) =>
        {
            Debug.Log("UITable.OnOpen Open MainUI Success!");
        });
    }
}
