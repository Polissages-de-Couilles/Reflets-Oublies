using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputEvent : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInputAction playerInputAction;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
        playerInputAction.Player.Interaction.performed += Interaction;

        playerInputAction.Player.Disable();
        playerInputAction.Player.Interaction.PerformInteractiveRebinding()
            .OnComplete(callback => {
                print(callback);
                callback.Dispose();
                playerInputAction.Player.Enable();

            })
            .Start();
    }
    public void Interaction(InputAction.CallbackContext context)
    {
        print("Bien jouer d'avoir intéragit");
    }
}
