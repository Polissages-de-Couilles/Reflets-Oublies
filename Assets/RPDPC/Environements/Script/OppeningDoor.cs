using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OppeningDoor : MonoBehaviour
{
    [SerializeField] private Animator DoorAnimator;

    [Header("Collision Mode")]
    [SerializeField] private bool Collison_Mode;

    [Header("Lever Mode")]
    [SerializeField] private bool Lever_Mode;
    [SerializeField] private List<Lever> levers = new List<Lever>();

    private void Start()
    {
        if(Lever_Mode)
        {
            foreach (Lever lever in levers)
            {
                lever.LeverActivation += LeverCheck;
            }
        }
    }

    public void LeverCheck()
    {
        foreach(Lever lever in levers)
        {
            if (lever.On == false) return;
        }
        OpenDoor();
    }

    private void OpenDoor()
    {
        DoorAnimator.SetTrigger("Ouvre");
    }

    private void CloseDoor()
    {
        DoorAnimator.SetTrigger("Ferme");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Collison_Mode && other.gameObject == GameManager.Instance.Player)
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Collison_Mode && other.gameObject == GameManager.Instance.Player)
        {
            CloseDoor();
        }
    }

}
