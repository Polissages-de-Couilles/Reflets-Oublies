using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCollider : MonoBehaviour
{
    [SerializeField] ZoneManager.ZoneName _zone;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.ZoneManager.OnZoneChange(GameManager.Instance.ZoneManager.GetZone(_zone));
        }
    }
}
