using MeetAndTalk.GlobalValue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StateBase : ScriptableObject
{
    public float priority;
    public List<ConditionExpression> conditions;
    public bool isHostileState;

    public abstract StateEntityBase PrepareEntityInstance();
}
