using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {
    public void Open()
    {
        UIManager.OpenUI("Prefabs/LoginUI", UIManager.Instance.GameUIRoot, (GameObject obj) =>
        {
            Debug.Log("GameController.Start Open LoginUI Success!");
        });
    }
}
