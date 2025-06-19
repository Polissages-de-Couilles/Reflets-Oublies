using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/DestroyObjectOfType")]
public class DestroyObjectOfType : StateBase
{
    [SerializeField] List<string> ListOfTypesToDESTROY;

    public override StateEntityBase PrepareEntityInstance()
    {
        DestroyObjectOfTypeEntity destroyObjectOfTypeEntity = new DestroyObjectOfTypeEntity();
        destroyObjectOfTypeEntity.Init(ListOfTypesToDESTROY);
        return destroyObjectOfTypeEntity;
    }
}
