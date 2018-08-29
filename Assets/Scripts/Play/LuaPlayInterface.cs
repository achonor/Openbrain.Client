using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaPlayInterface : PlayBase {

    private LuaBehaviour luaBehaviour;

    public void Awake()
    {
        luaBehaviour = transform.GetComponent<LuaBehaviour>();
    }

    protected override void CreateProblem()
    {
        luaBehaviour.CallLuaFunction("CreateProblem");
    }

    public override int GetPlayID()
    {
        return luaBehaviour.CallLuaFunction<int>("GetPlayID");
    }
}