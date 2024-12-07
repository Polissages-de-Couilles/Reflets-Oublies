using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.XInput;

public class UIPanel : MonoBehaviour
{
    [SerializeField] protected GameObject firstSelected;
    protected EventSystem UINavigation;

    public enum ControllerType
    {
        WirelessController,
        Keyboard,
        Mouse
    }
    private static ControllerType currentController = ControllerType.WirelessController;
    private Action<ControllerType> OnNewController;

    private InputActionMap map;

    private void Awake()
    {
        UINavigation = EventSystem.current;
    }

    private void Start()
    {
        map = GameManager.Instance.InputAction.FindActionMap("UI");
        map.actionTriggered += CheckController;
    }

    protected virtual void OnEnable()
    {
        OnNewController += OnControllerChange;
        Debug.Log(name + " is Enable");
        OnControllerChange(currentController);
    }

    protected virtual void OnDisable()
    {
        OnNewController -= OnControllerChange;
        UINavigation.SetSelectedGameObject(null);
    }

    private void OnControllerChange(ControllerType type)
    {
        if (!this.gameObject.activeInHierarchy) return;

        switch (type)
        {
            case ControllerType.WirelessController:
                if(UINavigation.currentSelectedGameObject == null)
                {
                    UINavigation.SetSelectedGameObject(firstSelected);
                }
                break;

            case ControllerType.Keyboard:
                if (UINavigation.currentSelectedGameObject == null)
                {
                    UINavigation.SetSelectedGameObject(firstSelected);
                }
                break;

            case ControllerType.Mouse:
                if(UINavigation.currentSelectedGameObject != null)
                {
                    UINavigation.SetSelectedGameObject(null);
                }
                break;

            default:
                break;
        }

        Debug.Log(name + " | " + type);
    }

    private void CheckController(InputAction.CallbackContext context)
    {
        //Debug.Log(context.control.device.displayName);
        var name = Enum.GetNames(typeof(ControllerType)).ToList().Find(x => x.Contains(context.control.device.displayName.Replace(" ", string.Empty)));
        var newController = (ControllerType)Enum.Parse(typeof(ControllerType), name);
        if (newController != currentController)
        {
            OnNewController?.Invoke(newController);
        }
        currentController = newController;
    }
}
