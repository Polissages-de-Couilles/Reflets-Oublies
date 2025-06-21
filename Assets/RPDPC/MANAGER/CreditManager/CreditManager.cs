using DG.Tweening;
using PDC.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditManager : MonoBehaviour
{
    [SerializeField] Transform _creditHolder;
    [SerializeField] RectTransform _listHolder;
    [SerializeField] Image _fade;
    [SerializeField] float _speed = 100;

    private PlayerInputAction playerInputAction;

    public void Awake()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.UI.Disable();
        playerInputAction.Player.Enable();
    }

    private void ExitCredit(InputAction.CallbackContext context)
    {
        playerInputAction.Player.Pause.performed -= ExitCredit;
        SceneManager.LoadScene(0);
    }

    public IEnumerator Start()
    {
        yield return new WaitUntil(() => LocalizationManager.IsLocaReady);
        yield return null;
        RebuildLayout(_creditHolder);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_creditHolder as RectTransform);
        yield return _fade.DOColor(new Color(_fade.color.r, _fade.color.g, _fade.color.b, 0f), 3f).WaitForCompletion();

        playerInputAction.Player.Pause.performed += ExitCredit;

        while(_creditHolder.localPosition.y < ((_creditHolder as RectTransform).sizeDelta.y))
        {
            yield return null;
            Debug.Log((_creditHolder as RectTransform).sizeDelta.y + " | " + (_creditHolder as RectTransform).localPosition.y + " | " + (_creditHolder as RectTransform).position.y);
            var speed = _speed;
            if(playerInputAction.Player.Validate.IsPressed()) speed *= 10;
            _creditHolder.position += Vector3.up * Time.deltaTime * speed;
        }
    }

    void RebuildLayout(Transform parent)
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetChild(i) as RectTransform);
            if(parent.GetChild(i).childCount > 0) RebuildLayout(parent.GetChild(i));
        }
    }
}
