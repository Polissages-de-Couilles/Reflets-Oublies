using MeetAndTalk.GlobalValue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StateBase : ScriptableObject
{
    public float priority;
    public bool isGood;
    public Action onActionFinished;

    public abstract void Init(GameObject parent, GameObject player);
    public abstract bool isStateValid();
    public abstract void OnEnterState();
    public abstract void OnUpdate();
    public abstract void OnEndState(); //Is made to be called in the state's script, with the call of onActionFinished
    public abstract void ExitState(); //Is made to be called by StateMachineManager, or other scipts
}
