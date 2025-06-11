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
        PlayerInputEventManager.OnNewController += OnControllerChange;
        if (PlayerInputEventManager.currentController == PlayerInputEventManager.ControllerType.Gamepad)
        {
            if (UINavigation.currentSelectedGameObject == null || !UINavigation.currentSelectedGameObject.activeSelf) UINavigation.SetSelectedGameObject(this.gameObject);
        }
    }

    private void OnDisable()
    {
        UINavigation.SetSelectedGameObject(null);
        PlayerInputEventManager.OnNewController -= OnControllerChange;
    }
}
