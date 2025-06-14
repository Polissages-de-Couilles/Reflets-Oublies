using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/PushPlayer")]
public class PushPlayer : StateBase
{
    [SerializeField] Vector3 direction;
    [SerializeField] float force;
    [SerializeField] float duration;

    public override StateEntityBase PrepareEntityInstance()
    {
        PushPlayerEntity pushPlayerEntity = new PushPlayerEntity();
        pushPlayerEntity.Init(direction, force, duration);
        return pushPlayerEntity;
    }
}
