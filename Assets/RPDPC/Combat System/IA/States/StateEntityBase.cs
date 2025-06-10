using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using MeetAndTalk;

public abstract class StateEntityBase
{
    public Action onActionFinished;
    protected StateMachineManager manager;
    protected GameObject parent;
    protected GameObject player;
    protected Animator animator;
    protected List<string> animationNames = new List<string>();
    public bool isAttack;
    public float priority;
    protected List<ConditionExpression> conditions;
    public bool isHostileState;

    public void InitGlobalVariables(StateMachineManager manager, GameObject parent, GameObject player, List<ConditionExpression> conditions, float priority, bool isHostileState, Animator animator, List<string> animationNames, bool isAttack)
    {
        this.manager = manager;
        this.parent = parent;
        this.player = player;
        this.conditions = conditions;
        this.priority = priority;
        this.isHostileState = isHostileState;
        this.animator = animator;
        this.animationNames = new (animationNames);
        this.isAttack = isAttack;
        foreach (ConditionExpression c in conditions)
        {
            c.baseCondition.Init(parent, player, this);
            foreach (ConditionCalculs cc in c.otherParts)
            {
                cc.secondCondition.Init(parent, player, this);
            }
        }
    }

    public StateMachineManager GetStateManager()
    {
        return manager;
    }

    public virtual void Init(
    )
    { }

    public virtual void Init(
        bool isIntelligent //RunAwayFromPlayer
    )
    { }

    public virtual void Init(
        bool isIntelligent, //FollowPlayer
        bool shouldStopWhenNear,
        float stopNearDistance,
        bool shouldStopWhenFar,
        float stopFarDistance
    )
    {}

    public virtual void Init(
        bool isIntelligent, //FollowPlayerWhileGuarding
        string guardAnim,
        string guardHitAnim
    )
    { }

    public virtual void Init(
        List<SOAttack.AttackDetails> attacks, bool doAllAttacks, Vector2 timeWithoutAttackAfter //Attack
    )
    { }

    public virtual void Init(
        List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks, Vector2 timeWithoutAttackAfter //ProjectileAttack
    )
    { }

    public virtual void Init(
        RandomMode randomMode,Vector3 searchCenter, float searchRange, bool shouldOnlyMoveOnce, bool WaitForMoveToFinishBeforeEndOrSwitchingState, Vector2 rangeWaitBetweenMoves //RandomMoveInRange
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
        float turnDuration, //TurnToPlayer
        string guardAnim,
        string guardHitAnim
    )
    { }

    public virtual void Init(
        List<Vector3> positions, bool loop //FollowListOfPositions
    )
    { }

    public virtual void Init(
        string guardAnim, //Guard
        string guardHitAnim
    )
    { }

    public virtual void Init(
        TeleportMode teleportMode,  //Teleport
        Vector3 SetPoint,
        List<Vector3> RandomPointInZone,
        float RandomPointInCircularZone,
        Vector3 SymetricPoint,
        float distanceWithPlayer,
        bool HateToSeePlayer,
        bool IgnoreY,
        bool SnapToNavMesh
    )
    { }

    public virtual void Init(
        List<StateListItem> States   //PlayManyStates
    )
    { }

    public virtual void Init(
        StateBase State,   //PlayStateForDuration
        float duration
    )
    { }

    public virtual void Init(
        List<SOAttack.AttackDetails> attacks,     //Attack and Turning 
        bool doAllAttacks, 
        Vector2 timeWithoutAttackAfter, 
        float turnDuration
        )
    { }

    public virtual void Init(
        DialogueContainerSO dialogueGOOD, //Talk
        DialogueContainerSO dialogueBAD,
        DialogueContainerSO dialogueNEUTRAL,
        List<string> ListOfTypesToDESTROY
    )
    { }

    public bool isStateValid()
    {
        bool currentResult = false;
        foreach (ConditionExpression c in conditions)
        {
            c.baseCondition.Init(parent, player, this);
            bool currentExpressionResult = c.baseCondition.isConditionFulfilled() ^ c.not ;

            foreach (ConditionCalculs cc in c.otherParts)
            {
                cc.secondCondition.Init(parent, player, this);
                switch (cc.logicalGate)
                {
                    case logicalGates.AND:
                        currentExpressionResult = currentExpressionResult & (cc.secondCondition.isConditionFulfilled() ^ cc.not);

                        break;
                    case logicalGates.NAND:
                        currentExpressionResult = !(currentExpressionResult & (cc.secondCondition.isConditionFulfilled() ^ cc.not));

                        break;
                    case logicalGates.OR:
                        currentExpressionResult = currentExpressionResult | (cc.secondCondition.isConditionFulfilled() ^ cc.not);

                        break;
                    case logicalGates.NOR:
                        currentExpressionResult = !(currentExpressionResult | (cc.secondCondition.isConditionFulfilled() ^ cc.not));

                        break;
                    case logicalGates.XOR:
                        currentExpressionResult = currentExpressionResult ^ (cc.secondCondition.isConditionFulfilled() ^ cc.not);

                        break;
                    case logicalGates.XNOR:
                        currentExpressionResult = !(currentExpressionResult ^ (cc.secondCondition.isConditionFulfilled() ^ cc.not));

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
    public bool not;
    public ConditionBase secondCondition;
}

[Serializable]
public struct ConditionExpression
{
    public logicalGates logicalGate;
    public ConditionBase baseCondition;
    public bool not;
    public List<ConditionCalculs> otherParts;
}