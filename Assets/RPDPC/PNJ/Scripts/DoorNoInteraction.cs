using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNoInteraction : MonoBehaviour
{
    [SerializeField] private Animator DoorAnimator;

    public void OpenDoor()
    {
        DoorAnimator.SetTrigger("Ouvre");
    }

    public void CloseDoor()
    {
        DoorAnimator.SetTrigger("Ferme");
    }
}
