using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultUiControllerInput : MonoBehaviour
{
    protected EventSystem UINavigation;

    private void Awake()
    {
        UINavigation = EventSystem.current;
    }

    private void OnControllerChange(PlayerInputEventManager.ControllerType type)
    {
        if(type == PlayerInputEventManager.ControllerType.Gamepad)
        {
            if (UINavigation.currentSelectedGameObject == null || !UINavigation.currentSelectedGameObject.activeSelf) UINavigation.SetSelectedGameObject(this.gameObject);
        }
        else
        {
            UINavigation.SetSelectedGameObject(null);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Enable());
    }

    IEnumerator Enable()
    {
        yield return null;
        GameManager.Instance.PlayerInputEventManager.PlayerInputAction.UI.Enable();
        GameManager.Instance.PlayerInputEventManager.PlayerInputAction.Player.Disable();
        PlayerInputEventManager.OnNewController += OnControllerChange;
        if (PlayerInputEventManager.currentController == PlayerInputEventManager.ControllerType.Gamepad)
        {
            if (UINavigation.currentSelectedGameObject == null || !UINavigation.currentSelectedGameObject.activeSelf) UINavigation.SetSelectedGameObject(this.gameObject);
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerInputEventManager.PlayerInputAction.UI.Disable();
        GameManager.Instance.PlayerInputEventManager.PlayerInputAction.Player.Enable();
        UINavigation.SetSelectedGameObject(null);
        PlayerInputEventManager.OnNewController -= OnControllerChange;
    }
}
