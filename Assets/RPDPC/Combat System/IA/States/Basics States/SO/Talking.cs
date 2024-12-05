using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/Talking")]
public class Talking : StateBase
{
    public override StateEntityBase PrepareEntityInstance()
    {
        return new TalkingEntity();
    }
}
