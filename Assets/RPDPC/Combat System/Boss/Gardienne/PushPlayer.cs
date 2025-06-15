using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/PushPlayer")]
public class PushPlayer : StateBase
{
    [SerializeField] Vector3 direction;
    [SerializeField] float force;
    [SerializeField] float duration;
    [SerializeField] float durationOfLiberation;

    public override StateEntityBase PrepareEntityInstance()
    {
        PushPlayerEntity pushPlayerEntity = new PushPlayerEntity();
        pushPlayerEntity.Init(direction, force, duration, durationOfLiberation);
        return pushPlayerEntity;
    }
}
