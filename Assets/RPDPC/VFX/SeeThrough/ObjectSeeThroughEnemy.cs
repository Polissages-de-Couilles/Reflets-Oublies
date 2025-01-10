using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSeeThroughEnemy : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] StateMachineManager stateMachineManager;
    [SerializeField] LayerMask layer;
    [SerializeField] float scale = 6f;

    public bool IsVisible => stateMachineManager.getCurrentState().isHostileState;

    private void Update()
    {
        if (!IsVisible)
        {
            this.transform.DOScale(0, 0.5f);
            return;
        }

        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, (this.transform.position - Camera.main.transform.position).normalized, out hit, Mathf.Infinity, layer))
        {
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
        Gizmos.DrawRay(Camera.main.transform.position, (this.transform.position - Camera.main.transform.position).normalized * 1000f);
    }
}
