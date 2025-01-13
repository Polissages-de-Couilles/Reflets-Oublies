using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AttackEntity : StateEntityBase
{
    List<SOAttack.AttackDetails> attacks;
    bool doAllAttacks;

    Dictionary<GameObject, SOAttack.AttackDetails> currentAttacks = new Dictionary<GameObject, SOAttack.AttackDetails>();
    Dictionary<SOAttack.AttackDetails, bool> attackAlreadyDealtDamage = new Dictionary<SOAttack.AttackDetails, bool>();
    bool finishedSpawnAllAttacks = false;
    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void Init(List<SOAttack.AttackDetails> attacks, bool doAllAttacks) 
    {
        this.attacks = attacks;
        this.doAllAttacks = doAllAttacks;
    }

    public override void OnEndState()
    {
        manager.StopCoroutine(SpawnAttack());
        manager.shouldSearchStates = true;

        foreach (GameObject go in currentAttacks.Keys)
        {
            MonoBehaviour.Destroy(go);
        }

        currentAttacks.Clear();
        attackAlreadyDealtDamage.Clear();
        finishedSpawnAllAttacks = false;
    }

    public override void OnEnterState()
    {
        manager.shouldSearchStates = false;
        finishedSpawnAllAttacks = false;
        foreach (SOAttack.AttackDetails attack in attacks)
        {
            if(attackAlreadyDealtDamage.ContainsKey(attack))
            {
                attackAlreadyDealtDamage.Remove(attack);
            }
            attackAlreadyDealtDamage.Add(attack, false);
        }
        manager.StartCoroutine(SpawnAttack());
    }

    public override void OnUpdate()
    {
    }

    IEnumerator SpawnAttack()
    {
        int index = 0;
        foreach (SOAttack.AttackDetails attack in attacks)
        {
            animator.Play(animationNames[attack.animationID]);
            float animationDuration = animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == animationNames[attack.animationID]).length;

            foreach (SOAttack.AttackColliderDetails collider in attack.colliders)
            {
                manager.StartCoroutine(SpawnCollision(collider, attack));
            }

            if (index + 1 == attacks.Count)
            {
                finishedSpawnAllAttacks = true;
            }

            yield return new WaitForSeconds(animationDuration);

            animator.Play(animationNames[0]);

            yield return new WaitForSeconds(attack.attackDuration - animationDuration);
            

            //if (!doAllAttacks)
            //{
            //    if (!isStateValid())
            //    {
            //        ExitState();
            //        break;
            //    }
            //}

            index++;
        }

        ExitState();
    }

    IEnumerator SpawnCollision(SOAttack.AttackColliderDetails detail, SOAttack.AttackDetails ad)
    {
        yield return new WaitForSeconds(detail.delayBeforeColliderSpawn);

        if(detail.VFX != null)
        {
            GameObject vfx = MonoBehaviour.Instantiate(detail.VFX, parent.transform);
            vfx.transform.localPosition = detail.ColliderRelativePosition;
        }

        GameObject attackCollider = new GameObject("BotAttackCollider");
        attackCollider.layer = 9;
        attackCollider.transform.parent = parent.transform;
        currentAttacks.Add(attackCollider, ad);

        switch (detail.colliderShape)
        {
            case SOAttack.ColliderShape.Box:
                BoxCollider boxCollider = attackCollider.AddComponent<BoxCollider>();
                boxCollider.size = detail.BoxColliderDimension;
                boxCollider.isTrigger = true;
                break;

            case SOAttack.ColliderShape.Sphere:
                SphereCollider sphereCollider = attackCollider.AddComponent<SphereCollider>();
                sphereCollider.radius = detail.SphereAndCapsuleColliderRadius;
                sphereCollider.isTrigger = true;
                break;

            case SOAttack.ColliderShape.Capsule:
                CapsuleCollider capsuleCollider = attackCollider.AddComponent<CapsuleCollider>();
                capsuleCollider.radius = detail.SphereAndCapsuleColliderRadius;
                capsuleCollider.height = detail.CapsuleColliderHeight;
                capsuleCollider.isTrigger = true;
                break;
        }

        //Pour décembre : Visuel PlaceHolder
        attackCollider.AddComponent<MeshRenderer>();
        attackCollider.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");

        AttackCollider ac = attackCollider.AddComponent<AttackCollider>();
        ac.Init(detail.DoesStun, detail.StunDuration, detail.DoesKnockback, detail.KnockForce, detail.KnockbackMode, true);
        ac.OnDamageableEnterTrigger += DealDamage;

        attackCollider.transform.localPosition = detail.ColliderRelativePosition;
        attackCollider.transform.localRotation = detail.ColliderRelativeRotation;

        yield return new WaitForSeconds(detail.ColliderDuration);

        //Debug.Log("TRY EXIT STATE : " + (currentAttacks.Count == 1) + " & " + finishedSpawnAllAttacks);
        //if (currentAttacks.Count == 1 && finishedSpawnAllAttacks)
        //{
        //    Debug.Log("EXIT STATE");
        //    ExitState();
        //}

        currentAttacks.Remove(attackCollider);
        if (attackCollider != null)
        {
            MonoBehaviour.Destroy(attackCollider);
        }
    }

    void DealDamage(IDamageable damageable, GameObject collider) {
        if (!attackAlreadyDealtDamage[currentAttacks[collider]])
        {
            attackAlreadyDealtDamage[currentAttacks[collider]] = true;
            damageable.takeDamage(currentAttacks[collider].damage);
        }
    }
}
