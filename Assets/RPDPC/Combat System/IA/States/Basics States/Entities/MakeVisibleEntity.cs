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
        if (parent.GetComponent<MeshRenderer>() != null) parent.GetComponent<MeshRenderer>().enabled = true;
        if (parent.GetComponent<Rigidbody>() != null) parent.GetComponent<Rigidbody>().useGravity = true;
        if (parent.GetComponent<Lockable>() != null) parent.GetComponent<Lockable>().CanBeLock = true;
        Renderer[] rs = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
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
