using System;
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
        talk,
        death
    }
    public States playerState;

    List<GameObject> hostileEnemies = new List<GameObject>();
    Action<bool> OnFightStateChanged;
    private CharacterController characterController;

    public void Start()
    {
        characterController = GetComponent<CharacterController>();
        GameManager.Instance.DialogueManager.OnNode += OnNode;
        GameManager.Instance.DialogueManager.StartDialogueEvent.AddListener(() => SetPlayerState(States.talk));
        GameManager.Instance.DialogueManager.EndDialogueEvent.AddListener(() => SetPlayerState(States.idle));
    }

    public void SetPlayerState(States state)
    {
        if (playerState == States.death) return;
        playerState = state;
    }

    public void SetPlayerState(States state, float duration)
    {
        if (playerState == States.death) return;
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
        if (playerState == States.death) return;
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

    public void addHostileEnemy(GameObject enemy)
    {
        //Debug.Log("Add hostile State");
        hostileEnemies.Add(enemy);
        OnFightStateChanged?.Invoke(true);
    }
    public void removeHostileEnemy(GameObject enemy)
    {
        //Debug.Log("Remove hostile State");
        hostileEnemies.Remove(enemy);
        if (hostileEnemies.Count == 0)
        {
            //Debug.Log("No hostile enemies");
            OnFightStateChanged?.Invoke(false);
        }
    }
}
