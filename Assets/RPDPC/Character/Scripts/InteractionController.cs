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
    private DashController dashController;

    public void Start()
    {
        PIE = GameManager.Instance.PlayerInputEventManager;
        PIE.PlayerInputAction.Player.Interaction.performed += OnInteraction;
        dashController = gameObject.GetComponent<DashController>();
    }

    private void FixedUpdate()
    {
        List<Interactible> interacibles = FindObjectsOfType<Interactible>().ToList();
        var atRange = interacibles.FindAll(x => x.IsAtRange && x.enabled);
        atRange = atRange.OrderBy(x => x.Distance - x.Priority).ToList();

        if (atRange.Count > 0) 
        { 
            currentInteraction = atRange[0];
            dashController.CanDash = false;
        }
        else
        {
            currentInteraction = null;
            dashController.CanDash = true;

        } 
        //Debug.Log(currentInteraction);

        foreach (var i in interacibles)
        {
            if (i.Equals(currentInteraction) || (atRange.Contains(i) && i.UIShowAnyway))
                i.SetUI(true, i.Equals(currentInteraction));
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
