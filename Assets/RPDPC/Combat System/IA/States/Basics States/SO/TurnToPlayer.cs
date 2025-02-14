using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/Turn to Player")]
public class TurnToPlayer : StateBase
{
    [SerializeField] protected float turnDuration;
    public override StateEntityBase PrepareEntityInstance()
    {
        TurnToPlayerEntity ttpe = new TurnToPlayerEntity();
        ttpe.Init(turnDuration);
        return ttpe;
    }
}
