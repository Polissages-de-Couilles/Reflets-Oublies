using MeetAndTalk.GlobalValue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StateBase : ScriptableObject
{
    public float priority;
    public Action onActionFinished;
    [SerializeField] ConditionExpression conditions;

    public void BaseInit(StateMachineManager manager, GameObject parent, GameObject player)
    {
        conditions.baseCondition.Init(parent, player);
        foreach (ConditionCalculs cc in conditions.otherParts)
        {
            cc.secondCondition.Init(parent, player);
        }

        Init(manager, parent, player);
    }
    public abstract void Init(StateMachineManager manager, GameObject parent, GameObject player);
    public bool isStateValid() 
    {
        bool currentResult = conditions.baseCondition.isConditionFulfilled();
        foreach (ConditionCalculs cc in conditions.otherParts)
        {
            switch (cc.logicalGate)
            {
                case logicalGates.AND:
                    currentResult = currentResult & cc.secondCondition.isConditionFulfilled();

                    break;
                case logicalGates.NAND:
                    currentResult = !(currentResult & cc.secondCondition.isConditionFulfilled());

                    break;
                case logicalGates.OR:
                    currentResult = currentResult | cc.secondCondition.isConditionFulfilled();

                    break;
                case logicalGates.NOR:
                    currentResult = !(currentResult | cc.secondCondition.isConditionFulfilled());

                    break;
                case logicalGates.XOR:
                    currentResult = currentResult ^ cc.secondCondition.isConditionFulfilled();

                    break;
                case logicalGates.XNOR:
                    currentResult = !(currentResult ^ cc.secondCondition.isConditionFulfilled());

                    break;
            }
        }
        return currentResult;
    }

    public abstract void OnEnterState();
    public abstract void OnUpdate();
    public abstract void OnEndState(); //Is made to be called in the state's script, with the call of onActionFinished
    public abstract void ExitState(); //Is made to be called by StateMachineManager, or other scipts
}
public enum logicalGates
{
    AND,
    NAND,
    OR,
    NOR,
    XOR,
    XNOR
}

[Serializable]
public struct ConditionCalculs
{
    public logicalGates logicalGate;
    public ConditionBase secondCondition;
}

[Serializable]
public struct ConditionExpression
{
    public ConditionBase baseCondition;
    public List<ConditionCalculs> otherParts;
}