using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactible
{
    public Action LeverActivation;
    public bool On = false;

    public override void OnInteraction()
    {
        if (!On)
        {
            On = true;
            LeverActivation?.Invoke();
        }
    }
}
