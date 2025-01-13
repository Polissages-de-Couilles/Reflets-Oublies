using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Game/IA/States/Base/RandomMoveInRange")]
public class SORandomMoveInRange : StateBase
{
    [SerializeField] Vector3 searchCenter;
    [SerializeField] float searchRange;
    [SerializeField] bool shouldOnlyMoveOnce = false;
    [SerializeField] bool WaitForMoveToFinishBeforeEndOrSwitchingState = false;
    [SerializeField] Vector2 rangeWaitBetweenMoves;

    public override StateEntityBase PrepareEntityInstance()
    {
        RandomMoveInRangeEntity rmir = new RandomMoveInRangeEntity();
        rmir.Init(searchCenter, searchRange, shouldOnlyMoveOnce, WaitForMoveToFinishBeforeEndOrSwitchingState, rangeWaitBetweenMoves);
        return rmir;
    }
}
