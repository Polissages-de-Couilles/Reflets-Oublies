using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughChanger : MonoBehaviour
{
    [SerializeField] bool ActiveSeeThrough;
    [SerializeField] GameObject SeeThroughPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.Instance.Player.gameObject)
        {
            SeeThroughPlayer.SetActive(ActiveSeeThrough);
        }
    }
}
