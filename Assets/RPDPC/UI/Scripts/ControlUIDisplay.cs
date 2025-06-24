using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUIDisplay : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Sprite _controller;
    [SerializeField] Sprite _keyboard;

    public void OnEnable()
    {
        var controllerType = PlayerInputEventManager.currentController;
        DisplayImage(controllerType);
        PlayerInputEventManager.OnNewController += DisplayImage;
    }

    public void OnDisable()
    {
        PlayerInputEventManager.OnNewController -= DisplayImage;
    }

    public void OnDestroy()
    {
        PlayerInputEventManager.OnNewController -= DisplayImage;
    }

    void DisplayImage(PlayerInputEventManager.ControllerType type)
    {
        var img = type == PlayerInputEventManager.ControllerType.Keyboard ? _keyboard : _controller;
        if(img != null) 
        { 
            _image.sprite = img;
            _image.enabled = true;
        } 
        else _image.enabled = false;
    }
}
