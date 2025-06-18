using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearWall : MonoBehaviour
{
    [SerializeField] BossStartManager bsm;
    [SerializeField] BotDamageable id;

    private void Start()
    {
        bsm.OnBossIsActivated += OnBossActivated;
        id.OnDamageTaken += OnDamageTaken;
    }

    private void OnDamageTaken(float arg1, float arg2)
    {
        if (arg2 == 0)
        {
            id.OnDamageTaken -= OnDamageTaken;
            Destroy(gameObject);
        }
    }

    void OnBossActivated()
    {
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = true;
        }
    }
}
