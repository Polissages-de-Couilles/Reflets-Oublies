using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDeathManager : MonoBehaviour
{
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
                MobSpawnerEntity MSE = FSM.spawner.GetComponent<StateMachineManager>().getCurrentState() as MobSpawnerEntity;
                if (MSE != null)
                {
                    MSE.spawnedMobs.Remove(gameObject);
                }
            }

            GetComponent<MoneyDrop>().DropMonney();

            yield return new WaitForSeconds(0.5f);

            Destroy(gameObject);
        }
    }
}
