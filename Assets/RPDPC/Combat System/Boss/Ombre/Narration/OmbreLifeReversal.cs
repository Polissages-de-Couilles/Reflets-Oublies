using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmbreLifeReversal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BossStartManager>().OnBossIsActivated += OnBossActivated;
    }

    private void OnBossActivated()
    {
        PlayerDamageable pd = GameManager.Instance.Player.GetComponent<PlayerDamageable>();

        float desiredHealth = 100;
        foreach(MemorySO mem in GameManager.Instance.MemoryManager.EncounteredMemory)
        {
            if(mem._isTaken)
            {
                desiredHealth += 20;
            }
            else
            {
                desiredHealth -= 5;
            }
        }
        if(pd.maxHealth != desiredHealth) pd.SetMaxHealth(desiredHealth);
    }
}
