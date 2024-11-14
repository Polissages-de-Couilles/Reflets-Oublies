using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputBindings : MonoBehaviour
{
    public TextMeshProUGUI InteractionText;

    [SerializeField] private GameObject keyBoardScreen;
    [SerializeField] private GameObject gamepadScreen;

    [SerializeField] InputActionReference inputAction;
    private PlayerInput playerInput;
    private PlayerInputAction playerInputAction;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        InteractionText.text = inputAction.action.GetBindingDisplayString();

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
        inputAction.action.PerformInteractiveRebinding(0)
            .OnComplete(callback => {
                callback.Dispose();
                playerInputAction.Player.Enable();
                InteractionText.text = inputAction.action.GetBindingDisplayString();
            })
            .Start();

        
    }
}
