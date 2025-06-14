using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactible
{
    public Action LeverActivation;
    public bool On = false;

    [SerializeField] private Animator animationLever;

    public override void OnInteraction()
    {
        if (!On)
        {
            On = true;
            LeverActivation?.Invoke();
            if(animationLever != null) animationLever.SetBool("LeverUp", true);
        }
        base.OnInteraction();
    }
}
