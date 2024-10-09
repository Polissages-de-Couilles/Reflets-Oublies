using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerDamageable>().OnDamageTaken += CheckPlayerHealth;
    }

    void CheckPlayerHealth(float damageTaken, float playerHealth)
    {
        if (playerHealth <= 0)
        {
            Debug.Log("The player is dead.");
        }
    }
}
