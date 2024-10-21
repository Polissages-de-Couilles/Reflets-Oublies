using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int valueCoin;

    private void Awake()
    {
        StartCoroutine(FollowPlayer());
    }

    private IEnumerator FollowPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        print("saucisse");
    }
}
