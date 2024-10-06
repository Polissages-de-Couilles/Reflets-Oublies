using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputBindings : MonoBehaviour
{
    public PlayerInput playerInput;
    public Text InteractionText;

    [SerializeField] private GameObject keyBoardScreen;
    [SerializeField] private GameObject gamepadScreen;

    private PlayerInputAction playerInputAction;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        InteractionText.text = playerInputAction.Player.Interaction.GetBindingDisplayString();

        Close();
    }

    public void ControllerScreen()
    {
        if (Gamepad.all.Count > 0) gamepadScreen.SetActive(true);
        else 
            keyBoardScreen.SetActive(true);
    }

    public void Close()
    {
        keyBoardScreen.gameObject.SetActive(false);
        gamepadScreen.SetActive(false);
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
