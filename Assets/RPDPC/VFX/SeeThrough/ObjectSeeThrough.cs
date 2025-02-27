using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSeeThrough : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] LayerMask layer;
    [SerializeField] float scale = 6f;

    public bool IsVisible { get; set; } = true;

    private void Update()
    {
        if (!IsVisible)
        {
            this.transform.DOScale(0, 0.5f);
            return;
        }

        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, (this.transform.position - Camera.main.transform.position).normalized, out hit, (this.transform.position - Camera.main.transform.position).sqrMagnitude, layer))
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject == target)
            {
                this.transform.DOScale(0, 0.5f);
            }
            else
            {
                this.transform.DOScale(scale, 0.5f);
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(Camera.main.transform.position, (this.transform.position - Camera.main.transform.position));
    }
}
