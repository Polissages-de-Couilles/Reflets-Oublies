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
    int currentIndex = 0;

    GameObject parent;
    StateMachineManager manager;
    public override void ExitState()
    {
    }

    public override void Init(StateMachineManager manager, GameObject parent, GameObject player)
    {
        this.parent = parent;
        this.manager = manager;
    }

    public override void OnEndState()
    {
    }

    public override void OnEnterState()
    {
        currentIndex = 0;
    }

    public override void OnUpdate()
    {
    }

    IEnumerator SpawnAttack()
    {
        foreach (AttackDetails attack in attacks)
        {
            foreach(AttackColliderDetails collider in attack.colliders)
            {
                manager.StartCoroutine(SpawnCollision(collider));
            }

            yield return new WaitForSeconds(attack.attackDuration);

            if (!doAllAttacks)
            {
                break;
            }
        }
    }

    IEnumerator SpawnCollision(AttackColliderDetails detail)
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
    [Tooltip("Also depends of bot rotation. If you put 1 0 0 it will spawn at the x 1 * the bot current rotation. Which is logic")]
    public Vector3 ColliderRelativePosition; // Also depends of Bots rotation
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
