using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    private StateManager stateManager;
    private AnimationManager animationManager;
    private CharacterController characterController;
    [SerializeField] GameObject vfx;

    void Start()
    {
        stateManager = GetComponent<StateManager>();
        characterController = GetComponent<CharacterController>();
        animationManager = GetComponent<AnimationManager>();
        GetComponent<PlayerDamageable>().OnDamageTaken += CheckPlayerHealth;
    }

    void CheckPlayerHealth(float damageTaken, float playerHealth)
    {
        Debug.Log(stateManager.playerState);
        if (playerHealth <= 0 && stateManager.playerState != StateManager.States.death)
        {
            characterController.enabled = false;
            stateManager.SetPlayerState(StateManager.States.death);
            animationManager.Death();
            Debug.Log("The player is dead.");
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return null;

        float animationDuration = animationManager.Animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name.Contains("Death")).length;
        Instantiate(vfx, this.gameObject.transform);

        yield return new WaitForSeconds(animationDuration);

        GameManager.Instance.DeathUi.SetActive(true);
    }
}
