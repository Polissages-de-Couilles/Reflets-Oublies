using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputBindings : MonoBehaviour
{
    public PlayerInput playerInput;

    public Text InteractionText;

    private PlayerInputAction playerInputAction;
    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        InteractionText.text = playerInputAction.Player.Interaction.GetBindingDisplayString();
    }

    public void InteractionRebind()
    {
        ///0 = KeyBoard / 1 = Gamepad

        InteractionText.text = "Wait...";
        playerInputAction.Player.Disable();
        playerInputAction.Player.Interaction.PerformInteractiveRebinding(0)
            .OnComplete(callback => {
                callback.Dispose();
                playerInputAction.Player.Enable();
                InteractionText.text = playerInputAction.Player.Interaction.GetBindingDisplayString();
            })
            .Start();
    }
}
