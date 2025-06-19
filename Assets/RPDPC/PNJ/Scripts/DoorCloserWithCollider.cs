using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloserWithCollider : MonoBehaviour
{
    [SerializeField] Animator Animator;
    [SerializeField] bool onlyOnce;
    bool hasClosed;
    private void OnTriggerEnter(Collider other)
    {
        if (!onlyOnce || hasClosed == false)
        {
            if (other.gameObject == GameManager.Instance.Player || other.gameObject.transform.IsChildOf(GameManager.Instance.Player.transform))
            {
                hasClosed = true;
                Animator.SetTrigger("Ferme");
            }
        }
    }
}
