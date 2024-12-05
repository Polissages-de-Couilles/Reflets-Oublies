using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] CharacterController characterController;
    [SerializeField] IKFootSolver otherFoot = default;
    [SerializeField] Transform body = default;
    [SerializeField] float speed = 4f;
    [SerializeField] float stepDistance = 0.8f;
    float footSpacing;
    float distance;
    float lerp;
    MovementController movementController;

    Vector3 oldPosition, currentPosition, newPosition;

    public void Start()
    {
        footSpacing = transform.localPosition.x;
        movementController = GameManager.Instance.Player.GetComponent<MovementController>();
        currentPosition = newPosition = oldPosition = transform.position;
        lerp = 1;
    }

    public void FixedUpdate()
    {
        transform.position = currentPosition;

        Ray ray = new (body.position + (body.right * footSpacing), Vector3.down + (characterController.transform.forward * (stepDistance / 1.75f) * (movementController.DistanceTravelInOneFrame <= 0 ? 0 : Mathf.Clamp01(movementController.Velocity.magnitude))));

        Debug.Log(movementController.DistanceTravelInOneFrame.ToString("0.00"));
        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {
            distance = Vector3.Distance(info.point, currentPosition);

            if (Mathf.Clamp01(movementController.Velocity.magnitude) <= 0 || movementController.DistanceTravelInOneFrame <= 0)
            {
                //Debug.Log(name + " Distance : " + distance);
                if (distance > (stepDistance / 10f))
                {
                    //Debug.Log(new Vector3(body.position.x, 0, body.transform.position.z));
                    newPosition = info.point;
                    //Debug.Log(name + " Pos : " + newPosition);
                    if(lerp >= 1) lerp = 0;
                }
            }
            else
            {
                if (distance > (stepDistance / 1.25f) && otherFoot.Distance() > (stepDistance / 2f) && !otherFoot.IsMoving())
                {
                    newPosition = info.point;
                    if (lerp >= 1) lerp = 0;
                }
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);

            currentPosition = tempPosition;
            lerp += Time.fixedDeltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
        }

        //transform.localPosition = new Vector3(footSpacing, transform.localPosition.y, transform.localPosition.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(newPosition, 0.1f);
    }

    public bool IsMoving() => lerp < 1 || (distance > (stepDistance / 2.5f) && lerp > 0 && lerp < 1);

    public float Distance() => distance;
}
