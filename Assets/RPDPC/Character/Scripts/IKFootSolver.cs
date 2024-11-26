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
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    float footSpacing;
    float lerp;
    float distance;
    bool isMoving = false;
    MovementController movementController;

    Vector3 oldPosition, currentPosition, newPosition;

    public void Start()
    {
        movementController = GameManager.Instance.Player.GetComponent<MovementController>();
        currentPosition = newPosition = oldPosition = transform.position;
        footSpacing = transform.localPosition.x;
    }

    public void Update()
    {
        if (!isMoving)
        {
            transform.position = currentPosition;
        }

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down + (characterController.transform.forward * 0.4f));

        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {
            oldPosition = info.point;
            distance = Vector3.Distance(info.point, currentPosition);
            //Debug.Log(distance);

            if (distance > 0.8f && otherFoot.Distance() > 0.4f && !otherFoot.IsMoving())
            {
                newPosition = info.point;
                StartCoroutine(Move(newPosition));
            }
        }

        currentPosition = newPosition;
        //if (lerp < 1)
        //{
        //    Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
        //    tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

        //    currentPosition = tempPosition;
        //    lerp += Time.deltaTime * speed;
        //}
        //else
        //{
        //    oldPosition = newPosition;
        //}

        //Debug.Log(lerp);
    }

    IEnumerator Move(Vector3 end)
    {
        isMoving = true;
        yield return transform.DOMove(end, 0.1f / movementController.Speed).WaitForCompletion();
        isMoving = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(oldPosition, 0.1f);
    }

    public bool IsMoving() => isMoving;

    public float Distance() => distance;
}
