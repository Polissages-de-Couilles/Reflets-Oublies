using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputEventManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInputAction PlayerInputAction => playerInputAction;
    private PlayerInputAction playerInputAction;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
        playerInputAction.Player.Interaction.performed += Interaction;
    }
    public void Interaction(InputAction.CallbackContext context)
    {
        //print("Bien jouer d'avoir intéragit");
    } 
}
