using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromSpawnerManager : MonoBehaviour
{
    [HideInInspector] public GameObject spawner;

    public bool isFromSpawner()
    {
        return spawner != null;
    }
}
