using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class MoveToME : StagingEvent
{
    public static Dictionary<NavMeshAgent, Action> _allMovementInProcess = new Dictionary<NavMeshAgent, Action>();

    [SerializeField] NavMeshAgent objectToMove;
    [SerializeField] Perso character = Perso.None;
    [SerializeField] float speed;
    [SerializeField] float stoppingDistance;
    [SerializeField] string animationName;

    float realStoppingDistance;
    float oldSpeed;
    bool navMeshStateBefore;
    CharacterController ewen;
    Coroutine coroutine;
    Action OnStop;

    public override void PlayEvent()
    {
        base.PlayEvent();

        if(objectToMove == null && character != Perso.None && GameManager.Instance.StoryManager._perso[character].TryGetComponent(out NavMeshAgent comp))
        {
            objectToMove = comp;
        }

        if (objectToMove == null)
        {
            DebugError("Invalid Agent");
            OnEventFinished?.Invoke();
            return;
        }

        if(animationName != string.Empty)
        {
            var animator = objectToMove.GetComponentInChildren<Animator>();
            if(animator != null)
            {
                animator.Play(animationName);
            }
        }

        OnStop += Stop;
        OnEventFinished += Stop;

        if (_allMovementInProcess.ContainsKey(objectToMove))
        {
            _allMovementInProcess[objectToMove]?.Invoke();
            _allMovementInProcess.Remove(objectToMove);
        }
        _allMovementInProcess.Add(objectToMove, OnStop);

        navMeshStateBefore = objectToMove.enabled;
        if(!navMeshStateBefore) objectToMove.enabled = true;
        realStoppingDistance = stoppingDistance;

        if (objectToMove.CompareTag("Player"))
        {
            ewen = objectToMove.GetComponent<CharacterController>();
            ewen.enabled = false;
            realStoppingDistance += 0.5f;
        }

        NavMeshPath navMeshPath = new NavMeshPath();
        NavMeshHit myNavHit;
        if(NavMesh.SamplePosition(transform.position, out myNavHit, 1000, -1))
        {
            if(objectToMove.CalculatePath(myNavHit.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                oldSpeed = objectToMove.speed;
                objectToMove.speed = speed;
                objectToMove.SetPath(navMeshPath);
                coroutine = StartCoroutine(CheckIfDestinationReached());
            }
            else
            {
                DebugError("Destination is unreachable for this agent");
                OnEventFinished?.Invoke();
            }
        }
        else
        {
            DebugError("Destination is unreachable for this agent");
            OnEventFinished?.Invoke();
        }

    }

    private void Stop()
    {
        if (coroutine == null) return;
        StopCoroutine(coroutine);
        coroutine = null;

        if (!pathComplete())
        {
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(transform.position, out myNavHit, 100, -1))
            {
                objectToMove.transform.position = myNavHit.position;
            }
        }

        objectToMove.speed = oldSpeed;
        objectToMove.enabled = navMeshStateBefore;
        if (ewen != null)
        {
            ewen.enabled = true;
            ewen = null;
        }

        var animator = objectToMove.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Reset");
        }

        OnEventFinished -= Stop;
        OnStop -= Stop;
    }

    IEnumerator CheckIfDestinationReached()
    {
        while (!pathComplete())
        {
            yield return null;
            //Debug.Log(this.gameObject.name + " Distance from Destination : " + Vector3.Distance(IgnoreYAxis(objectToMove.destination), IgnoreYAxis(objectToMove.transform.position)) + " to " + realStoppingDistance + " = " + pathComplete());
        }
        objectToMove.speed = oldSpeed;
        objectToMove.enabled = navMeshStateBefore;
        if (ewen != null)
        {
            ewen.enabled = true;
            ewen = null;
        }
        OnEventFinished?.Invoke();
    }

    protected bool pathComplete()
    {
        if (Vector3.Distance(IgnoreYAxis(objectToMove.destination), IgnoreYAxis(objectToMove.transform.position)) <= realStoppingDistance)
        {
            if (!objectToMove.hasPath || objectToMove.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }


    Vector3 IgnoreYAxis(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
}
