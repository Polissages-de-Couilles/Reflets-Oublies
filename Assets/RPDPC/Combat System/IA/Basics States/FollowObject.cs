using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IA/base/FollowObject")]
public class FollowObject : StateBase
{
    public GameObject ObjToFollow;
    GameObject parent;

    [Tooltip("Minimal distance between the two object to launch the follow")]
    public float minDistance;

    public override void Init(GameObject parent, GameObject player)
    {
        this.parent = parent;
        ObjToFollow = player;
    }

    public override bool isStateValid()
    {
        return ObjToFollow != null && Vector3.Distance(ObjToFollow.transform.position, parent.transform.position) <= minDistance;
    }

    public override void ExitState()
    {
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
    }

    public override void OnUpdate()
    {
    }
}
