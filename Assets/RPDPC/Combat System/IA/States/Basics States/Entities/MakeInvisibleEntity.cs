using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (parent.GetComponent<Renderer>() != null) parent.GetComponent<Renderer>().enabled = false;
        if (parent.GetComponent<Rigidbody>() != null) parent.GetComponent<Rigidbody>().useGravity = false;
        Renderer[] rs = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
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
