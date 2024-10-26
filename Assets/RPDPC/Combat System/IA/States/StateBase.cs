using MeetAndTalk.GlobalValue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StateBase : ScriptableObject
{
    public float priority;
    public Action onActionFinished;
    [SerializeField] List<ConditionExpression> conditions;

    public void BaseInit(StateMachineManager manager, GameObject parent, GameObject player)
    {
        foreach (ConditionExpression c in conditions)
        {
            c.baseCondition.Init(parent, player);
            foreach (ConditionCalculs cc in c.otherParts)
            {
                cc.secondCondition.Init(parent, player);
            }
        }
        
        Init(manager, parent, player);
    }
    public abstract void Init(StateMachineManager manager, GameObject parent, GameObject player);
    public bool isStateValid() 
    {
        bool currentResult = false;
        foreach (ConditionExpression c in conditions)
        {
            bool currentExpressionResult = c.baseCondition.isConditionFulfilled();
            foreach (ConditionCalculs cc in c.otherParts)
            {
                switch (cc.logicalGate)
                {
                    case logicalGates.AND:
                        currentExpressionResult = currentExpressionResult & cc.secondCondition.isConditionFulfilled();

                        break;
                    case logicalGates.NAND:
                        currentExpressionResult = !(currentExpressionResult & cc.secondCondition.isConditionFulfilled());

                        break;
                    case logicalGates.OR:
                        currentExpressionResult = currentExpressionResult | cc.secondCondition.isConditionFulfilled();

                        break;
                    case logicalGates.NOR:
                        currentExpressionResult = !(currentExpressionResult | cc.secondCondition.isConditionFulfilled());

                        break;
                    case logicalGates.XOR:
                        currentExpressionResult = currentExpressionResult ^ cc.secondCondition.isConditionFulfilled();

                        break;
                    case logicalGates.XNOR:
                        currentExpressionResult = !(currentExpressionResult ^ cc.secondCondition.isConditionFulfilled());

                        break;
                }
            }
            if (conditions.IndexOf(c) == 0)
            {
                currentResult = currentExpressionResult;
            }
            else 
            {
                switch (c.logicalGate)
                {
                    case logicalGates.AND:
                        currentResult = currentResult & currentExpressionResult;

                        break;
                    case logicalGates.NAND:
                        currentResult = !(currentResult & currentExpressionResult);

                        break;
                    case logicalGates.OR:
                        currentResult = currentResult | currentExpressionResult;

                        break;
                    case logicalGates.NOR:
                        currentResult = !(currentResult | currentExpressionResult);

                        break;
                    case logicalGates.XOR:
                        currentResult = currentResult ^ currentExpressionResult;

                        break;
                    case logicalGates.XNOR:
                        currentResult = !(currentResult ^ currentExpressionResult);

                        break;
                }
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
    public logicalGates logicalGate;
    public ConditionBase baseCondition;
    public List<ConditionCalculs> otherParts;
}