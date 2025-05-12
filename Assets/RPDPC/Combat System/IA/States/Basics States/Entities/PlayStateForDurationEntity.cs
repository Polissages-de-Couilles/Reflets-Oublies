using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStateForDurationEntity : StateEntityBase
{
    StateBase State;
    float duration;

    public override void Init(StateBase State, float duration)
    {
        this.State = State;
        this.duration = duration;
    }

    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void OnEndState()
    {
        manager.shouldSearchStates = true;
    }

    public override void OnEnterState()
    {
        manager.StartCoroutine(playState());
        manager.shouldSearchStates = false;
    }

    public override void OnUpdate()
    {
        manager.shouldSearchStates = false;
    }

    IEnumerator playState() 
    {
        StateEntityBase seb = State.PrepareEntityInstance();

        seb.InitGlobalVariables(manager, parent, player, State.conditions, -1, State.isHostileState, animator, State.animationNames);
        seb.AddHostileToPlayerState();
        seb.OnEnterState();
        yield return new WaitForSeconds(duration);
        seb.OnEndState();
        ExitState();
    }
}
