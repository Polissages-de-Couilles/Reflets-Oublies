using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/MakeVisible")]
public class MakeVisible : StateBase
{
    public override StateEntityBase PrepareEntityInstance()
    {
        MakeVisibleEntity makeVisibleEntity = new MakeVisibleEntity();
        return makeVisibleEntity;
    }
}
