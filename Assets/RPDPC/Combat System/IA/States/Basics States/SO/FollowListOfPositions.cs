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
        flop.Init(false, null, null, false, new Vector3(), 0, false, false, new Vector2(), null, 0, 0, 0, Vector2.zero, 0, positions, loop);
        return flop;
    }
}
