using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Game/IA/States/Base/RandomMoveInRange")]
public class SORandomMoveInRange : StateBase
{
    [SerializeField] RandomMode randomMode;
    [SerializeField] Vector3 searchCenter;
    [SerializeField] float searchRange;
    [SerializeField] bool shouldOnlyMoveOnce = false;
    [SerializeField] bool WaitForMoveToFinishBeforeEndOrSwitchingState = false;
    [SerializeField] Vector2 rangeWaitBetweenMoves;

    public override StateEntityBase PrepareEntityInstance()
    {
        RandomMoveInRangeEntity rmir = new RandomMoveInRangeEntity();
        rmir.Init(randomMode,searchCenter, searchRange, shouldOnlyMoveOnce, WaitForMoveToFinishBeforeEndOrSwitchingState, rangeWaitBetweenMoves);
        return rmir;
    }
}

public enum RandomMode
{
    CLASSIC,
    CROSS,
}
