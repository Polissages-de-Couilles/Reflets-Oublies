using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDeathManager : MonoBehaviour
{
    public Action OnBotDied;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<IDamageable>().OnDamageTaken += CallCheckBotHealth;
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

            OnBotDied?.Invoke();

            Animator animator = GetComponent<Animator>();
            GetComponent<MoneyDrop>().DropMonney();
            GetComponent<StateMachineManager>().enabled = false;
            animator.Play("Die");
            float dieLength = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(dieLength);

            Destroy(gameObject);
        }
    }
}
