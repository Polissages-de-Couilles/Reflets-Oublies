using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/Guard")]
public class SOGuard : StateBase
{
    [SerializeField] string guardAnim;
    [SerializeField] string guardHitAnim;

    public override StateEntityBase PrepareEntityInstance()
    {
        GuardEntity ge = new GuardEntity();
        ge.Init(guardAnim, guardHitAnim);
        return ge;
    }
}
