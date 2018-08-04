using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRoot : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
