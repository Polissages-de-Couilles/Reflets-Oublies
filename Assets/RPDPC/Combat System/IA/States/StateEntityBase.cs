using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateEntityBase
{
    public Action onActionFinished;
    protected StateMachineManager manager;
    protected GameObject parent;
    protected GameObject player;
    protected Animator animator;
    protected List<string> animationNames;
    public float priority;
    protected List<ConditionExpression> conditions;
    public bool isHostileState;

    public void InitGlobalVariables(StateMachineManager manager, GameObject parent, GameObject player, List<ConditionExpression> conditions, float priority, bool isHostileState, Animator animator, List<string> animationNames)
    {
        this.manager = manager;
        this.parent = parent;
        this.player = player;
        this.conditions = conditions;
        this.priority = priority;
        this.isHostileState = isHostileState;
        this.animator = animator;
        this.animationNames = new (animationNames);
        foreach (ConditionExpression c in conditions)
        {
            c.baseCondition.Init(parent, player);
            foreach (ConditionCalculs cc in c.otherParts)
            {
                cc.secondCondition.Init(parent, player);
            }
        }
    }

    public virtual void Init(
    )
    { }

    public virtual void Init(
        bool isIntelligent //FollowPlayer ou RunAwayFromPlayer
    )
    {}

    public virtual void Init(
        bool isIntelligent, //FollowPlayerWhileGuarding
        string guardAnim,
        string guardHitAnim
    )
    { }

    public virtual void Init(
        List<SOAttack.AttackDetails> attacks, bool doAllAttacks //Attack
    )
    { }

    public virtual void Init(
        List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks //ProjectileAttack
    )
    {}

    public virtual void Init(
        Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves //RandomMoveInRange
    )
    { }

    public virtual void Init(
        GameObject monsterPrefab, int nbToSpawnAtEnterState, int mobMaxNb, float spawnRange, Vector2 rangeTimeBetweenSpawns //Monster Spawner
    )
    { }

    public virtual void Init(
        float turnDuration //TurnToPlayer
    )
    { }

    public virtual void Init(
        List<Vector3> positions, bool loop //FollowListOfPositions
    )
    { }

    public bool isStateValid()
    {
        bool currentResult = false;
        foreach (ConditionExpression c in conditions)
        {
            c.baseCondition.Init(parent, player);
            bool currentExpressionResult = c.baseCondition.isConditionFulfilled();

            foreach (ConditionCalculs cc in c.otherParts)
            {
                cc.secondCondition.Init(parent, player);
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

    public void AddHostileToPlayerState()
    {
        if (isHostileState)
        {
            player.GetComponent<StateManager>().addHostileEnemy(parent);
        }
    }
    public void RemoveHostileFromPlayerState()
    {
        if (isHostileState)
        {
            player.GetComponent<StateManager>().removeHostileEnemy(parent);
        }
    }

    public abstract void OnEnterState();
    public abstract void OnUpdate();
    public abstract void OnEndState(); //Is made to be called by StateMachineManager, or other scipts
    public abstract void ExitState(); //Is made to be called in the state's script, with the call of onActionFinished
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