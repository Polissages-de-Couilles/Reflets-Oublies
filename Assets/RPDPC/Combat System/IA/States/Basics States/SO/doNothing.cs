using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/doNothing")]
public class SOdoNothing : StateBase
{
    public override StateEntityBase PrepareEntityInstance()
    {
        return new doNothingEntity();
    }
}
