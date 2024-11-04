using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/States/Base/Attack")]
public class Attack : StateBase
{
    [SerializeField] List<AttackDetails> attacks;
    [Tooltip("True if we want the bot to continue to attack, even if the conditions are not met anymore")]
    [SerializeField] bool doAllAttacks;

    GameObject parent;
    public override void ExitState()
    {
    }

    public override void Init(StateMachineManager manager, GameObject parent, GameObject player)
    {
        this.parent = parent;
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
    }

    public override void OnUpdate()
    {
    }
}

[Serializable]
struct AttackDetails
{
    public List<AttackColliderDetails> colliders;
    public float attackDuration;
}

[Serializable]
struct AttackColliderDetails
{
    public float delayBeforeColliderSpawn;
    public ColliderShape colliderShape;
    public Vector3 ColliderRelativePosition;
    public Vector3 ColliderRelativeRotation;
    public Vector3 BoxColliderDimension;
    public float CapsuleColliderHeight;
    public float SphereAndCapsuleColliderRadius;
    public float ColliderDuration;
}

[Serializable]
enum ColliderShape
{
    Box,
    Sphere,
    Capsule
}
