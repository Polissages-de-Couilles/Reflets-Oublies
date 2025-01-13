using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayerDetector : MonoBehaviour
{
    public Action PlayerDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player)
        {
            PlayerDetected?.Invoke();
        }
    }
}
