using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(StateMachineManager))]
public class BotDeathManager : MonoBehaviour
{
    protected const string DeathAnimName = "Die";
    protected StateMachineManager stateMachine;
    public Action OnBotDied;

    // Start is called before the first frame update
    protected void Start()
    {
        GetComponent<IDamageable>().OnDamageTaken += CallCheckBotHealth;
        stateMachine = GetComponent<StateMachineManager>();
    }

    protected void CallCheckBotHealth(float damageTaken, float playerHealth)
    {
        StartCoroutine(CheckBotHealth(damageTaken, playerHealth));
    }

    // Update is called once per frame
    virtual protected IEnumerator CheckBotHealth(float damageTaken, float playerHealth)
    {
        if (playerHealth <= 0)
        {
            Debug.Log("The bot (" + gameObject + ") is dead.");

            GameManager.Instance.Player.GetComponent<StateManager>().removeHostileEnemy(gameObject);
            
            if(TryGetComponent(out Lockable lockable))
            {
                lockable.CanBeLock = false;
            }

            FromSpawnerManager FSM = GetComponent<FromSpawnerManager>();
            if (FSM.isFromSpawner())
            {
                MobSpawner MSE = FSM.spawner.GetComponent<MobSpawner>();
                if (MSE != null)
                {
                    MSE.spawnedMobs.Remove(gameObject);
                }
            }

            stateMachine.Animator.Play(DeathAnimName);
            float animationDuration = stateMachine.Animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == DeathAnimName).length;
            stateMachine.enabled = false;

            if (TryGetComponent<MoneyDrop>(out MoneyDrop money))
            {
                money.DropMonney();
            }

            OnBotDied?.Invoke();
            yield return new WaitForSeconds(animationDuration);

            Destroy(gameObject);
        }
    }
}