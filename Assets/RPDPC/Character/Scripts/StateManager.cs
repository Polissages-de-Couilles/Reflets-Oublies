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
        GameManager.Instance.DialogueManager.OnNode += OnNode;
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

    void OnNode(MeetAndTalk.BaseNodeData node)
    {
        if(node is MeetAndTalk.DialogueNodeData)
        {
            if((node as MeetAndTalk.DialogueNodeData).CanMove)
            {
                SetPlayerState(States.idle);
                return;
            }
        }

        SetPlayerState(States.talk);
    }
}
