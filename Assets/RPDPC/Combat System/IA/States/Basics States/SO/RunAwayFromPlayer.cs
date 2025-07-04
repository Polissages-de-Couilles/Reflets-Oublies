using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/RunAwayFromPlayer")]
public class RunAwayFromPlayer : StateBase
{
    [SerializeField] bool keepWatchingPlayer;

    public override StateEntityBase PrepareEntityInstance()
    {
        RunAwayFromPlayerEntity rafp = new RunAwayFromPlayerEntity();
        rafp.Init(keepWatchingPlayer);
        return rafp;
    }
}
