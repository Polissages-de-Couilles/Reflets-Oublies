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
        stun
    }
    public States playerState;

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
