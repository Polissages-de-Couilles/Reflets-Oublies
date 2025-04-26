using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/Teleport")]
public class Teleport : StateBase
{
    [SerializeField] TeleportMode teleportMode;
    [SerializeField] Vector3 SetPoint;
    [SerializeField] List<Vector3> RandomPointInZone;
    [SerializeField] Vector3 SymetricPoint;
    [SerializeField] bool HaveToSeePlayer;
    [SerializeField] bool IgnoreY = true;
    [SerializeField] bool SnapToNavMesh = true;
    public override StateEntityBase PrepareEntityInstance()
    {
        TeleportEntities teleportEntities = new TeleportEntities();
        teleportEntities.Init(teleportMode, SetPoint, RandomPointInZone, SymetricPoint, HaveToSeePlayer, IgnoreY, SnapToNavMesh);
        return teleportEntities;
    }
}

public enum TeleportMode
{
    SetPoint,
    RandomPointInZone,
    RandomPointInSpawnedZone,
    SymetricPoint
}