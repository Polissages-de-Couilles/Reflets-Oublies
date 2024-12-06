using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FromSpawnerManager : MonoBehaviour
{
    [HideInInspector] public GameObject spawner;

    private void Start()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            GetComponent<NavMeshAgent>().Warp(hit.position);
        }
    }

    public bool isFromSpawner()
    {
        return spawner != null;
    }
}
