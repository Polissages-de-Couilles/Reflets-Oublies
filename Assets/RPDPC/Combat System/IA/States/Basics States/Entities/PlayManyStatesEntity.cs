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
        onActionFinished?.Invoke();
        manager.shouldSearchStates = true;
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
        actionInvoked = false;
        manager.shouldSearchStates = false;
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
            currentState.InitGlobalVariables(manager, parent, player, sli.state.conditions, -1, sli.state.isHostileState, animator, sli.state.animationNames);
            currentState.AddHostileToPlayerState();
            currentState.OnEnterState();
            currentState.onActionFinished += OnActionInvoked;
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
