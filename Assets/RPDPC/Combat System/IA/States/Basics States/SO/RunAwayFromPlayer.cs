using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/RunAwayFromPlayer")]
public class RunAwayFromPlayer : StateBase
{
    public override StateEntityBase PrepareEntityInstance()
    {
        RunAwayFromPlayerEntity rafp = new RunAwayFromPlayerEntity();
        rafp.Init(false, null, null, false, new Vector3(), 0, false, false, new Vector2(), null, 0, 0, 0, Vector2.zero, 0, null, false);
        return rafp;
    }
}
