using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/MakeInvisible")]
public class MakeInvisible : StateBase
{
    public override StateEntityBase PrepareEntityInstance()
    {
        MakeInvisibleEntity makeInvisibleEntity = new MakeInvisibleEntity();
        return makeInvisibleEntity;
    }
}
