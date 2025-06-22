using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/Teleport")]
public class Teleport : StateBase
{
    [SerializeField] TeleportMode teleportMode;
    [SerializeField] Vector3 SetPoint;
    [SerializeField] List<Vector3> RandomPointInZone;
    [SerializeField] float RandomPointInCircularZone;
    [SerializeField] Vector3 SymetricPoint;
    [SerializeField] float behindPlayerDistance;
    [SerializeField] bool HaveToSeePlayer;
    [SerializeField] bool IgnoreY = true;
    [SerializeField] bool SnapToNavMesh = true;
    [SerializeField] Vector2 timeWithoutAttackAfter;
    public override StateEntityBase PrepareEntityInstance()
    {
        TeleportEntities teleportEntities = new TeleportEntities();
        teleportEntities.Init(teleportMode, SetPoint, RandomPointInZone, RandomPointInCircularZone, SymetricPoint, behindPlayerDistance, HaveToSeePlayer, IgnoreY, SnapToNavMesh);
        return teleportEntities;
    }
}

public enum TeleportMode
{
    SetPoint,
    RandomPointInZone,
    RandomPointInCircularZone,
    RandomPointInSpawnedZone,
    SymetricPoint,
    BehindPlayer
}