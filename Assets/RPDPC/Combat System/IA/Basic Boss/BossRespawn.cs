using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossRespawn : MonoBehaviour
{
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] List<string> ListOfTypesToDESTROY;

    public void Respawn()
    {
        if (GetComponentInChildren<BotDamageable>() != null && GetComponentInChildren<BotDamageable>().getCurrentHealth() != 0)
        {
            foreach (string type in ListOfTypesToDESTROY)
            {
                foreach (Component comp in MonoBehaviour.FindObjectsOfType(Type.GetType(type)))
                {
                    if (comp.gameObject != null)
                    {
                        MonoBehaviour.Destroy(comp.gameObject);
                    }
                }
            }

            GetComponentInChildren<BossStartManager>().OnBotDied();

            Instantiate(prefabToSpawn);

            Destroy(gameObject);
        }
    }
}
