using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/TurnToPlayerWhileGuarding")]
public class TurnToPlayerWhileGuarding : TurnToPlayer
{
    [SerializeField] string guardAnim;
    [SerializeField] string guardHitAnim;

    public override StateEntityBase PrepareEntityInstance()
    {
        TurnToPlayerWhileGardingEntity ttpe = new TurnToPlayerWhileGardingEntity();
        ttpe.Init(turnDuration, guardAnim, guardHitAnim);
        return ttpe;
    }
}
