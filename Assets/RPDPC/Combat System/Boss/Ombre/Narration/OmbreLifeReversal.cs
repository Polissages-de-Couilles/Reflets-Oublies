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
        float maxhealth = pd.maxHealth;
        foreach (MemorySO mem in GameManager.Instance.MemoryManager.AllMemory)
        {
            if (mem._isTaken)
            {
                maxhealth += 30;
            }
            else
            {
                maxhealth -= 30;
            }
        }
        pd.SetMaxHealth(maxhealth);
    }
}
