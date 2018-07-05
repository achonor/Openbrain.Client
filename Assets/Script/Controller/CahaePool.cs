using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CahaePool : MonoBehaviour {
    public static CahaePool Instance;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
}
