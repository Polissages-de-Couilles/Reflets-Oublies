using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(StateMachineManager))]
public class BotDeathManager : MonoBehaviour
{
    const string DeathAnimName = "Die";
    StateMachineManager stateMachine;
    public Action OnBotDied;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<IDamageable>().OnDamageTaken += CallCheckBotHealth;
        stateMachine = GetComponent<StateMachineManager>();
    }

    void CallCheckBotHealth(float damageTaken, float playerHealth)
    {
        StartCoroutine(CheckBotHealth(damageTaken, playerHealth));
    }

    // Update is called once per frame
    IEnumerator CheckBotHealth(float damageTaken, float playerHealth)
    {
        if (playerHealth <= 0)
        {
            Debug.Log("The bot (" + gameObject + ") is dead.");
            FromSpawnerManager FSM = GetComponent<FromSpawnerManager>();
            if (FSM.isFromSpawner())
            {
                MobSpawnerEntity MSE = FSM.spawner.GetComponent<StateMachineManager>().GetSpawnState() as MobSpawnerEntity;
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