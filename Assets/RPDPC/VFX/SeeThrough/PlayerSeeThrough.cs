using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSeeThrough : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] GameObject target;
    [SerializeField] LayerMask layer;

    private void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, (target.transform.position - cam.transform.position).normalized, out hit, Mathf.Infinity, layer))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Player")
            {
                target.transform.DOScale(0, 0.5f);
            }
            else
            {
                target.transform.DOScale(6, 0.5f);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(cam.transform.position, (target.transform.position - cam.transform.position).normalized * 1000f);
    }
}
