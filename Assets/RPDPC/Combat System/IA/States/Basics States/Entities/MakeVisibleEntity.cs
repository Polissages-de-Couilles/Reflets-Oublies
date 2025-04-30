using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeVisibleEntity : StateEntityBase
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
        parent.GetComponent<MeshRenderer>().enabled = true;
        MeshRenderer[] rs = parent.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in rs)
        {
            r.enabled = true;
        }
        parent.GetComponent<Collider>().enabled = true;
        ExitState();
    }

    public override void OnUpdate()
    {
    }
}
