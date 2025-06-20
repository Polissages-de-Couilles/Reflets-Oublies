using UnityEngine;
using UnityEngine.AI;

public class FromSpawnerManager : MonoBehaviour
{
    [HideInInspector] public GameObject spawner;
    public Collider spawnerCollider;

    private void Start()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            GetComponent<NavMeshAgent>().Warp(hit.position);
        }
        foreach (FromSpawnerManager fsm in GetComponentsInChildren<FromSpawnerManager>())
        {
            fsm.spawner = spawner;
            fsm.spawnerCollider = spawnerCollider;
        }
    }

    public bool isFromSpawner()
    {
        return spawner != null;
    }
}
