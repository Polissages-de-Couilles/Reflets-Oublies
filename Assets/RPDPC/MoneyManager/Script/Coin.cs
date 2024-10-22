using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int valueCoin;

    [SerializeField] private float speed = 5;

    private Rigidbody rigidBody;
    private bool coinGet = false;

    private void Awake()
    {
        StartCoroutine(FollowPlayer());
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == GameManager.Instance.Player.name)
        {
            GameManager.Instance.MoneyManager.ChangePlayerMonney(valueCoin);
            coinGet = true;
            Destroy(this.gameObject);
        }
    }

    private IEnumerator FollowPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        while (coinGet == false)
        {
            Vector3 direction = transform.position - GameManager.Instance.Player.transform.position;
            direction.Normalize();
            rigidBody.velocity = direction * -speed;
            yield return new WaitForEndOfFrame();
        }
    }
}
