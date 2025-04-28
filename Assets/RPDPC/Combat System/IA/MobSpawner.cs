using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> monstersPrefabs = new List<GameObject>();
    [SerializeField] int nbMaxMonters;
    [SerializeField] float secondsAfterLeavingAndReentiringToSpawn = 30f;

    bool doOnce = false;
    [HideInInspector] public List<GameObject> spawnedMobs = new List<GameObject>();
    List<Collider> colliders = new List<Collider>();
    List<MobSpawnerCollider> mscolliders = new List<MobSpawnerCollider>();

    float timeSinceLeaved;
    bool doIncreaseTimer = true;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLeaved = secondsAfterLeavingAndReentiringToSpawn;
        colliders = GetComponentsInChildren<Collider>().ToList();
        mscolliders = GetComponentsInChildren<MobSpawnerCollider>().ToList();
        foreach (MobSpawnerCollider msc in mscolliders)
        {
            msc.onPlayerEnterTrigger += OnPlayerEnterTrigger;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doIncreaseTimer)
        {
            timeSinceLeaved += Time.deltaTime;
            if (timeSinceLeaved > secondsAfterLeavingAndReentiringToSpawn)
            {
                doIncreaseTimer = false;
            }
        }
    }

    private void OnPlayerEnterTrigger()
    {
        if (timeSinceLeaved > secondsAfterLeavingAndReentiringToSpawn)
        {
            SpawnMobEvenly();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player)
        {
            timeSinceLeaved = 0f;
            doIncreaseTimer = true;
        }
    }

    void SpawnMobEvenly() 
    {
        int nbmobs = spawnedMobs.Count;
        for (int i = 0; i < nbMaxMonters - nbmobs; i++)
        {
            SpawnMob(colliders[i % colliders.Count]);
        }
    }

    void SpawnMob(Collider col)
    {
        Vector3 RandomPoint = new Vector3(Random.Range(col.bounds.min.x, col.bounds.max.x), 0, Random.Range(col.bounds.min.z, col.bounds.max.z));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(RandomPoint, out hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            GameObject mob = Instantiate(monstersPrefabs[Random.Range(0, monstersPrefabs.Count)], hit.position, Quaternion.identity);
            //mob.GetComponent<NavMeshAgent>().Warp(hit.position);
            mob.GetComponent<FromSpawnerManager>().spawner = gameObject;
            mob.GetComponent<FromSpawnerManager>().spawnerCollider = col;
            spawnedMobs.Add(mob);
            mob.name = mob.name + " " + spawnedMobs.IndexOf(mob);
        }
    }
}
