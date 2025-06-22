using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TeleportEntities : StateEntityBase
{
    TeleportMode teleportMode;
    Vector3 SetPoint;
    List<Vector3> RandomPointInZone;
    float RandomPointInCircularZone;
    Vector3 SymetricPoint;
    float distanceWithPlayer;
    bool HaveToSeePlayer;
    bool IgnoreY;
    bool SnapToNavMesh;
    Vector2 TimeWithoutAttack;

    public override void Init(TeleportMode teleportMode, Vector3 SetPoint, List<Vector3> RandomPointInZone, float RandomPointInCircularZone, Vector3 SymetricPoint, float distanceWithPlayer,bool HaveToSeePlayer, bool IgnoreY, bool SnapToNavMesh, Vector2 timeWithoutAttack)
    { 
        this.teleportMode = teleportMode;
        this.SetPoint = SetPoint;
        this.RandomPointInZone = RandomPointInZone;
        this.RandomPointInCircularZone = RandomPointInCircularZone;
        this.SymetricPoint = SymetricPoint;
        this.distanceWithPlayer = distanceWithPlayer;
        this.HaveToSeePlayer = HaveToSeePlayer;
        this.IgnoreY = IgnoreY;
        this.SnapToNavMesh = SnapToNavMesh;
        this.TimeWithoutAttack = timeWithoutAttack;
    }

    public override void ExitState()
    {
        manager.StopPrioritizeAttack(UnityEngine.Random.Range(TimeWithoutAttack.x, TimeWithoutAttack.y));
        onActionFinished?.Invoke();
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
        manager.StartCoroutine(DoTeleportation());
    }

    public override void OnUpdate()
    {
    }

    void Teleport() 
    {
        Vector3 teleportDestination = Vector3.zero;

        switch (teleportMode)
        {
            case TeleportMode.SymetricPoint:
                teleportDestination = SymetricPoint - (parent.transform.position - SymetricPoint);
                Debug.Log("TELEPORT " + teleportDestination);
                break;
            case TeleportMode.SetPoint:
                teleportDestination = SetPoint;
                break;
            case TeleportMode.RandomPointInZone:
                teleportDestination = new Vector3(Random.Range(RandomPointInZone[0].x, RandomPointInZone[1].x), Random.Range(RandomPointInZone[0].y, RandomPointInZone[1].y), Random.Range(RandomPointInZone[0].z, RandomPointInZone[1].z));
                break;
            case TeleportMode.RandomPointInCircularZone:
                Vector2 randomPoint = Random.insideUnitCircle * RandomPointInCircularZone;
                teleportDestination = SymetricPoint + new Vector3(randomPoint.x, 0f, randomPoint.y);
                break;
            case TeleportMode.RandomPointInSpawnedZone: 
                FromSpawnerManager fsm = parent.GetComponent<FromSpawnerManager>();
                if (fsm != null)
                {
                    teleportDestination = new Vector3(Random.Range(fsm.spawnerCollider.bounds.max.x, fsm.spawnerCollider.bounds.min.x), Random.Range(fsm.spawnerCollider.bounds.max.y, fsm.spawnerCollider.bounds.min.y), Random.Range(fsm.spawnerCollider.bounds.max.z, fsm.spawnerCollider.bounds.min.z));
                }
                break;
            case TeleportMode.BehindPlayer:
                teleportDestination = player.transform.position - player.transform.forward * distanceWithPlayer;
                break;
        }

        if (IgnoreY)
        {
            teleportDestination.y = parent.transform.position.y;
        }

        if (SnapToNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(teleportDestination, out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                teleportDestination = hit.position;
            }
        }

        if (HaveToSeePlayer) 
        { 
            PlayerIsVisible piv = new PlayerIsVisible();
            piv.viewAngle = 360;
            piv.Init(parent, player, this);
            if (!piv.isConditionFulfilled())
            {
                Debug.Log("HAVEN'T SEEN PLAYER");
                return;
            }
        }

        parent.transform.position = teleportDestination;
        Quaternion LookAtRotation = Quaternion.LookRotation(player.transform.position - parent.transform.position, parent.transform.up);
        LookAtRotation = Quaternion.Euler(parent.transform.rotation.eulerAngles.x, LookAtRotation.eulerAngles.y, parent.transform.rotation.eulerAngles.z);
        parent.transform.rotation = LookAtRotation;
    }

    IEnumerator DoTeleportation()
    {
        if (animator != null && animationNames.Count != 0)
        {
            animator.Play(animationNames[0]);
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == animationNames[0]).length);
        }

            Teleport();

        if (animator != null && animationNames.Count != 0)
        {
            animator.Play(animationNames[1]);
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == animationNames[1]).length);
        }

        ExitState();
    }
}
