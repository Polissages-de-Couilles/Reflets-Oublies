using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    [HideInInspector] public List<MemorySO> AllMemory => _allMemory;
    [SerializeField]private List<MemorySO> _allMemory = new List<MemorySO>();

    private void Awake()
    {
        foreach (MemorySO mem in _allMemory) {
            mem._isTaken = false;
        }
    }
}
