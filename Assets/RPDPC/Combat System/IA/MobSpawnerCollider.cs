using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MobSpawnerCollider : MonoBehaviour
{
    public Action<MobSpawnerCollider> onPlayerEnterTrigger;
    public Action<MobSpawnerCollider> onPlayerExitTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.Instance.Player)
        {
            onPlayerEnterTrigger?.Invoke(this);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject == GameManager.Instance.Player)
        {
            onPlayerExitTrigger?.Invoke(this);
        }
    }
}
