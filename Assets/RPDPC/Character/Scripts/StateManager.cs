using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum States
    {
        idle,
        attack,
        dash,
        stun,
        talk
    }
    public States playerState;

    public void Start()
    {
        GameManager.Instance.DialogueManager.StartDialogueEvent.AddListener(() => SetPlayerState(States.talk));
        GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(() => SetPlayerState(States.idle));
    }

    public void SetPlayerState(States state)
    {
        playerState = state;
    }

    public void SetPlayerState(States state, float duration)
    {
        StartCoroutine(doSetPlayerState(state, duration));
    }

    IEnumerator doSetPlayerState(States state, float duration)
    {
        Debug.Log("Set Player State to " + state);
        playerState = state;
        yield return new WaitForSeconds(duration);
        if (playerState == state) //Don't reset State if a new state as been set within the delay
        {
            clearState();
        }
    }

    void clearState()
    {
        playerState = States.idle;
    }
}
