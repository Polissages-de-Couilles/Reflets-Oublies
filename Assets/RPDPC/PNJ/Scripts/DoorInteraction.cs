using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : Interactible
{
    [SerializeField] private Animator DoorAnimator;

    public override void OnInteraction()
    {
        DoorAnimator.SetTrigger("Ouvre");

        base.OnInteraction();
    }
}
