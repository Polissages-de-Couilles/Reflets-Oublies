using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManyStatesEntity : StateEntityBase
{
    List<StateListItem> States;
    StateEntityBase currentState;
    bool actionInvoked = false;

    public override void Init(List<StateListItem> States)
    {
        this.States = States;
    }

    public override void ExitState()
    {
        Debug.Log("EXIT STATE MII PLAZA");
        manager.FORCEDONOTSEARCH = false;
        manager.shouldSearchStates = true;
        onActionFinished?.Invoke();
    }

    public override void OnEndState()
    {
        Debug.Log("END STATE MII PLAZA");
        manager.StopCoroutine(PlayStates());
    }

    public override void OnEnterState()
    {
        Debug.Log("ON ENTER PLAY MANY STATES MII PLAZA");
        actionInvoked = false;
        manager.shouldSearchStates = false;
        manager.FORCEDONOTSEARCH = true;
        manager.StartCoroutine(PlayStates());
    }

    public override void OnUpdate()
    {
        manager.shouldSearchStates = false;
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    IEnumerator PlayStates()
    {
        foreach (StateListItem sli in States)
        {
            yield return new WaitForSeconds(sli.delayBeforePlay);
            currentState = sli.state.PrepareEntityInstance();
            Debug.Log($"Play Many States Played : {currentState}");
            currentState.InitGlobalVariables(manager, parent, player, sli.state.conditions, -1, sli.state.isHostileState, animator, sli.state.animationNames, sli.state.isAttack);
            currentState.AddHostileToPlayerState();
            currentState.onActionFinished += OnActionInvoked;
            currentState.OnEnterState();
            yield return new WaitUntil(() => actionInvoked);
            actionInvoked = false;
            currentState.onActionFinished -= OnActionInvoked;
            currentState.OnEndState();
        }

        ExitState();
    }
    private void OnActionInvoked()
    {
        actionInvoked = true;
    }
}
