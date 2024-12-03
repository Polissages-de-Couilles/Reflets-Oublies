using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/Stun")]
public class SOStun : StateBase
{
    public override StateEntityBase PrepareEntityInstance()
    {
        return new StunEntity();
    }
}
