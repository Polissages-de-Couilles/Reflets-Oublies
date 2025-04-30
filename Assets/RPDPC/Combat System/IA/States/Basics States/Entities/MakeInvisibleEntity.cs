using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeInvisibleEntity : StateEntityBase
{
    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
        parent.GetComponent<MeshRenderer>().enabled = false;
        MeshRenderer[] rs = parent.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in rs)
        {
            r.enabled = false;
        }
        parent.GetComponent<Collider>().enabled = false;
        ExitState();
    }

    public override void OnUpdate()
    {
    }
}
