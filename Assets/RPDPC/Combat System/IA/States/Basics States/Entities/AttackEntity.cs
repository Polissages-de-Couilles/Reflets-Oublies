using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackEntity : StateEntityBase
{
    List<SOAttack.AttackDetails> attacks;
    bool doAllAttacks;
    Vector2 timeWithoutAttackAfter;

    Dictionary<GameObject, SOAttack.AttackDetails> currentAttacks = new Dictionary<GameObject, SOAttack.AttackDetails>();
    Dictionary<SOAttack.AttackDetails, bool> attackAlreadyDealtDamage = new Dictionary<SOAttack.AttackDetails, bool>();
    bool grabed = false;
    Transform grabTransform;
    float currentAttackTimer = 0f;
    public override void ExitState()
    {
        manager.StopPrioritizeAttack(UnityEngine.Random.Range(timeWithoutAttackAfter.x, timeWithoutAttackAfter.y));
        onActionFinished?.Invoke();
    }

    public override void Init(List<SOAttack.AttackDetails> attacks, bool doAllAttacks, Vector2 timeWithoutAttackAfter) 
    {
        this.attacks = attacks;
        this.doAllAttacks = doAllAttacks;
        this.timeWithoutAttackAfter = timeWithoutAttackAfter;
    }

    public override void OnEndState()
    {
        manager.StopCoroutine(SpawnAttack());
        manager.shouldSearchStates = true;

        foreach (GameObject go in currentAttacks.Keys)
        {
            go.GetComponent<AttackColliderManager>().DesactivateCollider();
        }

        currentAttacks.Clear();
        attackAlreadyDealtDamage.Clear();
    }

    public override void OnEnterState()
    {
        manager.shouldSearchStates = false;
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
        currentAttackTimer += Time.deltaTime;

    }

    IEnumerator SpawnAttack()
    {
        foreach (SOAttack.AttackDetails attack in attacks)
        {
            currentAttackTimer = 0f;

            if (animator != null)
            {
                animator.SetFloat("AttackSpeed", 1f);
                float animationDuration = animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == animationNames[attack.animationID]).length;
                manager.StartCoroutine(PlayAnimationSpeed(animationDuration, attack));
            }

            foreach (SOAttack.AttackColliderDetails collider in attack.colliders)
            {
                manager.StartCoroutine(SpawnCollision(collider, attack));
            }

            yield return new WaitForSeconds(attack.attackDuration);
            

            //if (!doAllAttacks)
            //{
            //    if (!isStateValid())
            //    {
            //        ExitState();
            //        break;
            //    }
            //}
        }

        ExitState();
    }

    IEnumerator PlayAnimationSpeed(float animDuration, SOAttack.AttackDetails ad)
    {
        float timer = 0f;
        animator.Play(animationNames[ad.animationID]);
        while (timer < animDuration)
        {
            animator.SetFloat("AttackSpeed", ad.animationSpeed.Evaluate(timer / animDuration));
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime * ad.animationSpeed.Evaluate(timer / animDuration);
        }
        animator.Play(animationNames[0]);
    }

    IEnumerator SpawnCollision(SOAttack.AttackColliderDetails detail, SOAttack.AttackDetails ad)
    {
        AttackColliderManager acm = parent.GetComponentsInChildren<AttackColliderManager>().ToList().Find(s => s.colliderID == detail.colliderID);
        AttackCollider ac; 

        yield return new WaitForSeconds(detail.delayBeforeColliderSpawn);

        ac = acm.CreateAttackCollider(detail.DoesStun, detail.StunDuration, detail.DoesKnockback, detail.KnockForce, detail.KnockbackMode, true, parent);
        ac.gameObject.layer = 9;
        ac.OnDamageableEnterTrigger += DealDamage;

        currentAttacks.Add(ac.gameObject, ad);

        acm.ActivateCollider();

        if(detail.VFX != null)
        {
            GameObject vfx = MonoBehaviour.Instantiate(detail.VFX, parent.transform);
            vfx.transform.localPosition = acm.gameObject.transform.localPosition;
        }

        //Pour décembre : Visuel PlaceHolder
        //attackCollider.AddComponent<MeshRenderer>();
        //attackCollider.AddComponent<MeshFilter>().mesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");

        yield return new WaitForSeconds(detail.ColliderDuration);

        ac.OnDamageableEnterTrigger -= DealDamage;
        acm.DesactivateCollider();

        //Debug.Log("TRY EXIT STATE : " + (currentAttacks.Count == 1) + " & " + finishedSpawnAllAttacks);
        //if (currentAttacks.Count == 1 && finishedSpawnAllAttacks)
        //{
        //    Debug.Log("EXIT STATE");
        //    ExitState();
        //}

        currentAttacks.Remove(ac.gameObject);
    }

    

    void DealDamage(IDamageable damageable, GameObject collider) 
    {
        if (!attackAlreadyDealtDamage[currentAttacks[collider]])
        {
            attackAlreadyDealtDamage[currentAttacks[collider]] = true;
            damageable.takeDamage(currentAttacks[collider].damage, parent);

            GrabSocketManager gsm = null;

            if (currentAttacks[collider].grabDetails.grabID > -1)
            {
                gsm = parent.GetComponentsInChildren<GrabSocketManager>().ToList().Find(s => s.grabID == currentAttacks[collider].grabDetails.grabID);
            }
            if (gsm != null && currentAttacks[collider].grabDetails.grabReleaseTime - currentAttackTimer > 0)
            {
                gsm.LaunchGrab(player, currentAttacks[collider].grabDetails, currentAttacks[collider].grabDetails.grabReleaseTime - currentAttackTimer);
            }
        }
    }
}
