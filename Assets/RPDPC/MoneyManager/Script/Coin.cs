using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    [SerializeField] private int valueCoin;
    [SerializeField] private AnimationCurve animCurv;

    private const float DURATION = 0.5f;

    private void Awake()
    {
        StartCoroutine(FollowPlayer());
    }
    private IEnumerator FollowPlayer()
    {
        yield return new WaitForSeconds(1f);

        yield return transform.DOMoveInTargetLocalSpace(GameManager.Instance.Player.transform, Vector3.zero, DURATION).SetEase(animCurv).WaitForCompletion();

        GameManager.Instance.MoneyManager.ChangePlayerMonney(valueCoin);
        Destroy(this.gameObject);

        //ajouter pailette LOL
    }
}
