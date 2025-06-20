using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackManager : MonoBehaviour
{
    PlayerInputEventManager PIE;
    private AnimationManager animationManager;

    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float timeBetweenAttacks1;
    [SerializeField] float timeBetweenAttacks2;
    [SerializeField] float timeBetweenAttacks3;
    [SerializeField] float attackDamage;
    [SerializeField] float bigAttackDamage;

    [SerializeField] AttackCollider collision1;
    [SerializeField] AttackCollider collision2;
    [SerializeField] AttackCollider collision3;

    StateManager stateManager;
    bool doNextAttack = false;
    [SerializeField] private List<StateManager.States> states = new List<StateManager.States>();

    public bool canAttack;

    enum attackPhaseEnum
    {
        Phase1,
        Phase2,
        Phase3
    }
    attackPhaseEnum nextAttackPhaseLate = attackPhaseEnum.Phase1;
    attackPhaseEnum nextAttackPhase = attackPhaseEnum.Phase1;

    private void Awake()
    {
        stateManager = GetComponent<StateManager>();
        animationManager = GetComponent<AnimationManager>();
    }

    private void HitFrame(IDamageable damageable, GameObject @object)
    {
        if(@object.TryGetComponent(out GuardManager guardManager))
        {
            if(guardManager.isGuarding) return;
        }
        Time.timeScale = 0.1f;
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.15f, 0.025f).SetUpdate(UpdateType.Late).OnComplete(
            () => Time.timeScale = 1f);
    }

    private void HitFrame3(IDamageable damageable, GameObject @object)
    {
        if(@object.TryGetComponent(out GuardManager guardManager))
        {
            if(guardManager.isGuarding) return;
        }
        Time.timeScale = 0.1f;
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.15f, 0.05f).SetUpdate(UpdateType.Late).OnComplete(
            () => Time.timeScale = 1f);
    }

    void Start()
    {
        PIE = GameManager.Instance.PlayerInputEventManager;

        collision1.SetCollisionState(false);
        collision1.Init(true, 1, false, 0, KnockbackMode.MoveAwayFromAttacker, false, gameObject);
        collision1.OnDamageableEnterTrigger += OnTriggerDetectDamageable;

        collision2.SetCollisionState(false);
        collision2.Init(true, 1, false, 0, KnockbackMode.MoveAwayFromAttacker, false, gameObject);
        collision2.OnDamageableEnterTrigger += OnTriggerDetectDamageable;

        collision3.SetCollisionState(false);
        collision3.Init(true, 1, true, 1f, KnockbackMode.MoveAwayFromAttacker, false, gameObject);
        collision3.OnDamageableEnterTrigger += OnTriggerDetectDamageable;

        PIE.PlayerInputAction.Player.Attack.performed += OnAttack;

        collision1.OnDamageableEnterTrigger += HitFrame;
        collision2.OnDamageableEnterTrigger += HitFrame;
        collision3.OnDamageableEnterTrigger += HitFrame3;
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if (canAttack)
        {
            Debug.Log("Interraction Attack");
            if (nextAttackPhase != nextAttackPhaseLate)
            {
                doNextAttack = true;
            }

            if (isStateCompatible(stateManager.playerState))
            {
                if (nextAttackPhase == nextAttackPhaseLate)
                {
                    collision1.CharacterAlreadyAttacked.Clear();
                    collision2.CharacterAlreadyAttacked.Clear();
                    collision3.CharacterAlreadyAttacked.Clear();
                    if (nextAttackPhase == attackPhaseEnum.Phase1)
                    {
                        StartCoroutine(AttackPhase1());
                    }
                    else if (nextAttackPhase == attackPhaseEnum.Phase2)
                    {
                        StartCoroutine(AttackPhase2());
                    }
                    else
                    {
                        StartCoroutine(AttackPhase3());
                    }
                }
            }
        }
    }

    IEnumerator AttackPhase1()
    {
        //Debug.Log("Attack 1");
        GameManager.Instance.FirebaseManager.UpdateAnim("Attack1 Trigger");
        stateManager.SetPlayerState(StateManager.States.attack, timeBetweenAttacks1);
        animationManager.SetAttackState(1);
        //collision1.SetCollisionState(true);
        nextAttackPhase = attackPhaseEnum.Phase2;
        yield return new WaitForSeconds(timeBetweenAttacks1);
        collision1.SetCollisionState(false);
        nextAttackPhaseLate = attackPhaseEnum.Phase2;

        yield return null;

        if (doNextAttack)
        {
            doNextAttack = false;
            yield return AttackPhase2();
            yield break;
        }

        GameManager.Instance.FirebaseManager.UpdateAnim("None");

        yield return new WaitForSeconds(timeBetweenAttacks); //Reset combo if no input
        if (nextAttackPhase == attackPhaseEnum.Phase2)
        {
            animationManager.ResetAttack();
            nextAttackPhase = attackPhaseEnum.Phase1;
            nextAttackPhaseLate = attackPhaseEnum.Phase1;
        }
    }
    IEnumerator AttackPhase2()
    {
        //Debug.Log("Attack 2");
        GameManager.Instance.FirebaseManager.UpdateAnim("Attack2 Trigger");
        stateManager.SetPlayerState(StateManager.States.attack, timeBetweenAttacks2);
        animationManager.SetAttackState(2);
        //collision2.SetCollisionState(true);
        nextAttackPhase = attackPhaseEnum.Phase3;
        yield return new WaitForSeconds(timeBetweenAttacks2);
        collision2.SetCollisionState(false);
        nextAttackPhaseLate = attackPhaseEnum.Phase3;

        yield return null;
        if (doNextAttack)
        {
            doNextAttack = false;
            yield return AttackPhase3();
            yield break;
        }

        GameManager.Instance.FirebaseManager.UpdateAnim("None");

        yield return new WaitForSeconds(timeBetweenAttacks); //Reset combo if no input
        if (nextAttackPhase == attackPhaseEnum.Phase3)
        {
            animationManager.ResetAttack();
            nextAttackPhase = attackPhaseEnum.Phase1;
            nextAttackPhaseLate = attackPhaseEnum.Phase1;
        }
    }
    IEnumerator AttackPhase3()
    {
        //Debug.Log("Attack 3");
        GameManager.Instance.FirebaseManager.UpdateAnim("Attack3 Trigger");
        stateManager.SetPlayerState(StateManager.States.attack, timeBetweenAttacks3);
        animationManager.SetAttackState(3);
        //collision3.SetCollisionState(true);
        nextAttackPhase = attackPhaseEnum.Phase1;
        yield return new WaitForSeconds(timeBetweenAttacks3);
        collision3.SetCollisionState(false);
        nextAttackPhaseLate = attackPhaseEnum.Phase1;

        doNextAttack = false;
        animationManager.ResetAttack();
        GameManager.Instance.FirebaseManager.UpdateAnim("None");
    }

    void OnTriggerDetectDamageable(IDamageable damageable, GameObject collider)
    {
        if (collider == collision3.gameObject)
            damageable.takeDamage(bigAttackDamage, gameObject);
        else 
            damageable.takeDamage(attackDamage, gameObject);
    }

    bool isStateCompatible(StateManager.States state)
    {
        return states.Contains(state);
    }
}
