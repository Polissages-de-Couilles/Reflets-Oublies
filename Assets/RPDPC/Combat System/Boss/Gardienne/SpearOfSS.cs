using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpearOfSS : MonoBehaviour
{
    [SerializeField] float randomDegree = 15;
    [SerializeField] Vector2 randomSize = new Vector2(2, 4);
    [SerializeField] Vector2 randomDelay = new Vector2(1, 2);
    float damage;

    // Start is called before the first frame update
    void Start()
    {
        float randSize = Random.Range(randomSize.x, randomSize.y);

        GetComponent<AttackCollider>().Init(true, 0.5f, true, 2, KnockbackMode.MoveAwayFromAttackCollision, true, gameObject);
        transform.SetLocalPositionAndRotation(new Vector3(0, -randSize / 2, 0), Quaternion.Euler(Random.Range(-randomDegree, randomDegree), 0, Random.Range(-randomDegree, randomDegree)));
        //CapsuleCollider cc = GetComponent<CapsuleCollider>();
        //cc.height = randSize;
        transform.localScale = new Vector3(1, randSize, 1);
        Vector3 dif = (transform.localPosition + transform.up * (randSize / 2)) - transform.localPosition;
        transform.localPosition += new Vector3(-dif.x, 0, -dif.z);

        StartCoroutine(releaseSpear(randSize));
    }

    IEnumerator releaseSpear(float randSize)
    {
        yield return new WaitForSeconds(Random.Range(randomDelay.x, randomDelay.y));

        transform.DOMove(transform.position + transform.up * (randSize - 0.2f), 0.2f);
    }
}
