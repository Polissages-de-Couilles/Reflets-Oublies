using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/FollowListOfPositions")]
public class FollowListOfPositions : StateBase
{
    [SerializeField] List<Vector3> positions;
    [SerializeField] bool loop;
    public override StateEntityBase PrepareEntityInstance()
    {
        FollowListOfPositionsEntity flop = new FollowListOfPositionsEntity();
        flop.Init(positions, loop);
        return flop;
    }
}
