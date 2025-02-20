using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class TurnToPlayerEntity : StateEntityBase
{
    protected float turnDuration;

    public override void ExitState()
    {
        onActionFinished?.Invoke();
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

    protected IEnumerator DoLookAt()
    {
        manager.shouldSearchStates = false;
        
        while (Quaternion.Angle(Quaternion.Euler(0, parent.transform.eulerAngles.y, 0), Quaternion.LookRotation(player.transform.position - parent.transform.position, Vector3.up)) > 5)
        {
            Debug.Log(Quaternion.Angle(Quaternion.Euler(0, parent.transform.eulerAngles.y, 0), Quaternion.LookRotation(player.transform.position - parent.transform.position, Vector3.up)));
            Vector3 targetVector = new Vector3(player.transform.position.x, parent.transform.position.y, player.transform.position.z);
            parent.transform.DOLookAt(targetVector, turnDuration * (Quaternion.Angle(Quaternion.Euler(0, parent.transform.eulerAngles.y, 0), Quaternion.LookRotation(player.transform.position - parent.transform.position, Vector3.up)) / 360f)).SetEase(Ease.Linear);
            yield return new WaitForFixedUpdate();
        }
        manager.shouldSearchStates = true;
        ExitState();
        
    }
}
