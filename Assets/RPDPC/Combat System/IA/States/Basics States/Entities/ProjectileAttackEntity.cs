using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class ProjectileAttackEntity : StateEntityBase
{
    List<SOProjectileAttack.ProjectileAttackDetails> attacks;
    bool doAllAttacks;

    Dictionary<GameObject, SOProjectileAttack.ProjectileAttackDetails> currentAttacks = new Dictionary<GameObject, SOProjectileAttack.ProjectileAttackDetails>();
    bool finishedSpawnAllAttacks = false;
    public override void ExitState()
    {
        onActionFinished?.Invoke();
    }

    public override void Init(List<SOProjectileAttack.ProjectileAttackDetails> projectileAttacks, bool doAllAttacks)
    {
        this.attacks = projectileAttacks;
        this.doAllAttacks = doAllAttacks;
    }

    public override void OnEndState()
    {
        manager.shouldSearchStates = true;
        manager.StopCoroutine(SpawnAttack());
    }

    public override void OnEnterState()
    {
        manager.shouldSearchStates = false;
        finishedSpawnAllAttacks = false;
        manager.StartCoroutine(SpawnAttack());
    }

    public override void OnUpdate()
    {
    }

    IEnumerator SpawnAttack()
    {
        foreach (SOProjectileAttack.ProjectileAttackDetails attack in attacks)
        {
            foreach (SOProjectileAttack.ProjectileAttackPrefabDetails collider in attack.prefabProjectiles)
            {
                manager.StartCoroutine(SpawnCollisionNEW(collider, attack));
            }

            yield return new WaitForSeconds(attack.attackDuration);

            //if (!doAllAttacks)
            //{
            //    if (!isStateValid())
            //    {
            //        //ExitState();
            //        manager.shouldSearchStates = true;
            //        break;
            //    }
            //}
        }
        ExitState();
    }

    IEnumerator SpawnCollisionNEW(SOProjectileAttack.ProjectileAttackPrefabDetails papd, SOProjectileAttack.ProjectileAttackDetails ad)
    {
        yield return new WaitForSeconds(papd.delayBeforeSpawn);

        ProjectileSpawner ps = parent.GetComponentsInChildren<ProjectileSpawner>().ToList().Find(x => x.spawnerID == ad.spawnerID);

        GameObject projectile = MonoBehaviour.Instantiate(papd.prefab, ps.gameObject.transform);
        projectile.GetComponent<ProjectileManager>().Init(parent, player);
        projectile.layer = 9;
    }

    //IEnumerator SpawnCollision(SOProjectileAttack.ProjectileAttackColliderDetails detail, SOProjectileAttack.ProjectileAttackDetails ad)
    //{
    //    yield return new WaitForSeconds(detail.delayBeforeColliderSpawn);

    //    GameObject attackCollider = new GameObject("BotAttackCollider");
    //    attackCollider.layer = 9;
    //    attackCollider.transform.parent = parent.transform;
    //    currentAttacks.Add(attackCollider, ad);

    //    switch (detail.colliderShape)
    //    {
    //        case SOProjectileAttack.ProjectileColliderShape.Box:
    //            BoxCollider boxCollider = attackCollider.AddComponent<BoxCollider>();
    //            boxCollider.size = detail.BoxColliderDimension;
    //            boxCollider.isTrigger = true;
    //            break;

    //        case SOProjectileAttack.ProjectileColliderShape.Sphere:
    //            SphereCollider sphereCollider = attackCollider.AddComponent<SphereCollider>();
    //            sphereCollider.radius = detail.SphereAndCapsuleColliderRadius;
    //            sphereCollider.isTrigger = true;
    //            break;

    //        case SOProjectileAttack.ProjectileColliderShape.Capsule:
    //            CapsuleCollider capsuleCollider = attackCollider.AddComponent<CapsuleCollider>();
    //            capsuleCollider.radius = detail.SphereAndCapsuleColliderRadius;
    //            capsuleCollider.height = detail.CapsuleColliderHeight;
    //            capsuleCollider.isTrigger = true;
    //            break;
    //    }

    //    //Pour décembre : Visuel PlaceHolder
    //    GameObject mesh = new GameObject("mesh");
    //    mesh.transform.parent = attackCollider.transform;
    //    Material material = new Material(Shader.Find("FlatKit/Stylized Surface"));
    //    material.color = Color.red;
    //    mesh.AddComponent<MeshRenderer>().material = material;
    //    mesh.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Capsule.fbx");
    //    mesh.transform.localScale = new Vector3(0.1f, 0, 0.3f);

    //    AttackCollider ac = attackCollider.AddComponent<AttackCollider>();
    //    ac.Init(detail.DoesStun, detail.StunDuration, detail.DoesKnockback, detail.KnockForce, detail.KnockbackMode, true);
    //    ac.OnDamageableEnterTrigger += DealDamage;
    //    ac.OnEnterTrigger += DestroyCollision;

    //    attackCollider.transform.localPosition = detail.ColliderRelativePosition;
    //    attackCollider.transform.localRotation = detail.ColliderRelativeRotation;

    //    Vector3 vDistance = (attackCollider.transform.position - player.transform.position);
    //    vDistance = new Vector3(-vDistance.x, vDistance.y, -vDistance.z).normalized;

    //    yield return attackCollider.transform.DOMove(attackCollider.transform.position + new Vector3(vDistance.x * detail.distance, vDistance.y * -detail.distance, vDistance.z * detail.distance), detail.distance / detail.speed).SetEase(detail.animCurv).WaitForCompletion();

    //    currentAttacks.Remove(attackCollider);
    //    if(attackCollider != null)
    //    {
    //        MonoBehaviour.Destroy(attackCollider);
    //    }
    //}
}
