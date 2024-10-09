using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    private PlayerInputEventManager PIE;
    private Interactible currentInteraction;

    public void Start()
    {
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Interaction.performed += OnInteraction;
    }

    private void FixedUpdate()
    {
        List<Interactible> interacibles = FindObjectsOfType<Interactible>().ToList();
        var atRange = interacibles.FindAll(x => x.IsAtRange);
        atRange = atRange.OrderBy(x => x.Distance - x.Priority).ToList();

        if(atRange.Count > 0) currentInteraction = atRange[0];
        else currentInteraction = null;
        Debug.Log(currentInteraction);

        foreach (var i in interacibles)
        {
            if (i.Equals(currentInteraction))
                i.SetUI(true);
            else
                i.SetUI(false);
        }
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        if (currentInteraction == null) return;
        currentInteraction.OnInteraction();
    }
}
