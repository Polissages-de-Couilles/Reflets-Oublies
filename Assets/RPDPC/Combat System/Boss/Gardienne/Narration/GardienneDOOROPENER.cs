using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardienneDOOROPENER : MonoBehaviour
{
    [SerializeField] DoorNoInteraction door;

    private void Start()
    {
        GetComponent<BossDeathManager>().OnBotDied += OnBossDied;
    }

    private void OnBossDied()
    {
        door.OpenDoor();
    }
}
