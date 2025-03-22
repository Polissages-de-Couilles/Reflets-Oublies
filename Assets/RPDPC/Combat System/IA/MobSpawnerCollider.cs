using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MobSpawnerCollider : MonoBehaviour
{
    public Action onPlayerEnterTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.Instance.Player)
        {
            onPlayerEnterTrigger?.Invoke();
        }
    }
}
