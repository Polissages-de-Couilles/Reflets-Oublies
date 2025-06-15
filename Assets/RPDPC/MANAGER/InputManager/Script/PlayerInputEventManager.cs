using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputEventManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInputAction PlayerInputAction => playerInputAction;
    private PlayerInputAction playerInputAction;

    private InputActionMap map;

    public enum ControllerType
    {
        Gamepad,
        Keyboard,
    }
    public static ControllerType currentController = ControllerType.Gamepad;
    public static Action<ControllerType> OnNewController;

    public void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
    }

    private void Start()
    {
        //map = GameManager.Instance.InputAction.FindActionMap("Player");
        //map.actionTriggered += CheckController;
        currentController = ControllerType.Gamepad;
    }

    public void Update()
    {
        //Debug.Log(playerInput.currentControlScheme);
        ChangeCurrentController((ControllerType)Enum.Parse(typeof(ControllerType), playerInput.currentControlScheme));
    }

    private void ChangeCurrentController(ControllerType controllerType)
    {
        if (controllerType == currentController) return;

        currentController = controllerType;
        OnNewController?.Invoke(currentController);
    }

    //private void CheckController(InputAction.CallbackContext context)
    //{
    //    //Debug.Log(context.control.device.displayName);
    //    var name = Enum.GetNames(typeof(ControllerType)).ToList().Find(x => x.Contains(context.control.device.displayName.Replace(" ", string.Empty)));
    //    var newController = (ControllerType)Enum.Parse(typeof(ControllerType), name);
    //    if (newController != currentController)
    //    {
    //        OnNewController?.Invoke(newController);
    //    }
    //    currentController = newController;
    //}
}
