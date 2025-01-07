using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDC;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    [SerializeField] private int valueCoin;
    [SerializeField] private AnimationCurve animCurv;

    private const float DISTANCE = 0.5f;

    private void Awake()
    {
        StartCoroutine(FollowPlayer());
    }
    private IEnumerator FollowPlayer()
    {
        yield return new WaitForSeconds(1.5f);

        //yield return transform.DOMoveInTargetLocalSpace(GameManager.Instance.Player.transform, Vector3.zero, DURATION).SetEase(animCurv).WaitForCompletion();

        float value = 0;
        while(Vector3.Distance(this.transform.position, GameManager.Instance.Player.transform.position) > DISTANCE && value < 1f)
        {
            var v = animCurv.Evaluate(value);
            transform.position = Vector3.Lerp(transform.position, GameManager.Instance.Player.transform.position, v);
            yield return new WaitForFixedUpdate();
            value += Time.fixedDeltaTime / 1.5f;
            Debug.Log(value);
        }

        GameManager.Instance.MoneyManager.ChangePlayerMonney(valueCoin);
        Destroy(this.gameObject);

        //ajouter pailette LOL
    }
}
