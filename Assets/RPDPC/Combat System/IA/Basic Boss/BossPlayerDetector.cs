using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayerDetector : MonoBehaviour
{
    public Action PlayerDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player || other.transform.IsChildOf(GameManager.Instance.Player.transform))
        {
            PlayerDetected?.Invoke();
        }
    }
}
