using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectOfTypeEntity : StateEntityBase
{
    List<string> ListOfTypesToDESTROY;

    public override void Init(List<string> ListOfTypesToDESTROY)
    {
        this.ListOfTypesToDESTROY = ListOfTypesToDESTROY;
    }

    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
        foreach (string type in ListOfTypesToDESTROY)
        {
            foreach (Component comp in MonoBehaviour.FindObjectsOfType(Type.GetType(type)))
            {
                MonoBehaviour.Destroy(comp.gameObject);
            }
        }

        ExitState();
    }

    public override void OnUpdate()
    {
    }
}
