using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    private StateManager stateManager;
    private AnimationManager animationManager;

    void Start()
    {
        stateManager = GetComponent<StateManager>();
        animationManager = GetComponent<AnimationManager>();
        GetComponent<PlayerDamageable>().OnDamageTaken += CheckPlayerHealth;
    }

    void CheckPlayerHealth(float damageTaken, float playerHealth)
    {
        Debug.Log(stateManager.playerState);
        if (playerHealth <= 0 && stateManager.playerState != StateManager.States.death)
        {
            stateManager.SetPlayerState(StateManager.States.death);
            animationManager.Death();
            Debug.Log("The player is dead.");
        }
    }

    IEnumerator Respawn()
    {
        //Afficher Ecran de fin

        yield return null;
    }
}
