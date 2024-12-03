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
        rmir.Init(false, null, null, false, searchCenter, searchRange, shouldOnlyMoveOnce, WaitForMoveToFinishBeforeEndOrSwitchingState, rangeWaitBetweenMoves, null, 0, 0, 0, Vector2.zero);
        return rmir;
    }
}
