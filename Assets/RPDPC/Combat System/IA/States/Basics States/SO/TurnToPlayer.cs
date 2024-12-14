using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/Turn to Player")]
public class TurnToPlayer : StateBase
{
    [SerializeField] float turnDuration;
    public override StateEntityBase PrepareEntityInstance()
    {
        TurnToPlayerEntity ttpe = new TurnToPlayerEntity();
        ttpe.Init(false, null, null, false, Vector3.zero, 0, false, false, Vector2.zero, null, 0, 0, 0, Vector2.zero, turnDuration);
        return ttpe;
    }
}
