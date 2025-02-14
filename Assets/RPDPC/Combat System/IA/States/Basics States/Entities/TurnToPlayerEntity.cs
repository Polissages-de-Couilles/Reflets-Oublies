using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnToPlayerEntity : StateEntityBase
{
    protected float turnDuration;

    public override void ExitState()
    {
    }

    public override void Init(float turnDuration)
    {
        this.turnDuration = turnDuration;
    }

    public override void OnEndState()
    {
        manager.StopCoroutine(DoLookAt());
    }

    public override void OnEnterState()
    {
        manager.StartCoroutine(DoLookAt());
    }

    public override void OnUpdate()
    {
    }

    IEnumerator DoLookAt()
    {
        yield return parent.transform.DOLookAt(new Vector3(player.transform.position.x,parent.transform.position.y, player.transform.position.z), turnDuration).WaitForCompletion();
    }
}
