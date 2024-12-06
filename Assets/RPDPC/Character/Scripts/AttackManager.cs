using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackManager : MonoBehaviour
{
    PlayerInputEventManager PIE;
    [SerializeField] float timeBetweenAttacks = 0.5f;
    [SerializeField] float attackDamage = 5f;

    [SerializeField] AttackCollider collision1;
    [SerializeField] AttackCollider collision2;
    [SerializeField] AttackCollider collision3;

    StateManager stateManager;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        PIE = GameManager.Instance.PlayerInputEventManager;

        collision1.SetCollisionState(false);
        collision1.GetComponent<MeshRenderer>().enabled = false;
        collision1.Init(true, 1, false, 0, KnockbackMode.MoveAwayFromAttacker, false);
        collision1.OnDamageableEnterTrigger += OnTriggerDetectDamageable;

        collision2.SetCollisionState(false);
        collision2.GetComponent<MeshRenderer>().enabled = false;
        collision2.Init(true, 1, false, 0, KnockbackMode.MoveAwayFromAttacker, false);
        collision2.OnDamageableEnterTrigger += OnTriggerDetectDamageable;

        collision3.SetCollisionState(false);
        collision3.GetComponent<MeshRenderer>().enabled = false;
        collision3.Init(true, 1, true, 1f, KnockbackMode.MoveAwayFromAttacker, false);
        collision3.OnDamageableEnterTrigger += OnTriggerDetectDamageable;

        PIE.PlayerInputAction.Player.Attack.performed += OnAttack;
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if (nextAttackPhase == nextAttackPhaseLate && isStateCompatible(stateManager.playerState))
        {
            collision1.hasAlreadyDealtDamage = false;
            collision2.hasAlreadyDealtDamage = false;
            collision3.hasAlreadyDealtDamage = false;
            stateManager.SetPlayerState(StateManager.States.attack, timeBetweenAttacks);
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

    IEnumerator AttackPhase1()
    {
        Debug.Log("Attack 1");
        collision1.GetComponent<MeshRenderer>().enabled = true;
        collision1.SetCollisionState(true);
        nextAttackPhase = attackPhaseEnum.Phase2;
        yield return new WaitForSeconds(timeBetweenAttacks);
        collision1.GetComponent<MeshRenderer>().enabled = false;
        collision1.SetCollisionState(false);
        nextAttackPhaseLate = attackPhaseEnum.Phase2;

        yield return new WaitForSeconds(timeBetweenAttacks); //Reset combo if no input
        if (nextAttackPhase == attackPhaseEnum.Phase2)
        {
            nextAttackPhase = attackPhaseEnum.Phase1;
            nextAttackPhaseLate = attackPhaseEnum.Phase1;
        }
    }
    IEnumerator AttackPhase2()
    {
        Debug.Log("Attack 2");
        collision1.GetComponent<MeshRenderer>().enabled = true;
        collision2.SetCollisionState(true);
        nextAttackPhase = attackPhaseEnum.Phase3;
        yield return new WaitForSeconds(timeBetweenAttacks);
        collision1.GetComponent<MeshRenderer>().enabled = false;
        collision2.SetCollisionState(false);
        nextAttackPhaseLate = attackPhaseEnum.Phase3;

        yield return new WaitForSeconds(timeBetweenAttacks); //Reset combo if no input
        if (nextAttackPhase == attackPhaseEnum.Phase3)
        {
            nextAttackPhase = attackPhaseEnum.Phase1;
            nextAttackPhaseLate = attackPhaseEnum.Phase1;
        }
    }
    IEnumerator AttackPhase3()
    {
        Debug.Log("Attack 3");
        collision1.GetComponent<MeshRenderer>().enabled = true;
        collision3.SetCollisionState(true);
        nextAttackPhase = attackPhaseEnum.Phase1;
        yield return new WaitForSeconds(timeBetweenAttacks);
        collision1.GetComponent<MeshRenderer>().enabled = false;
        collision3.SetCollisionState(false);
        nextAttackPhaseLate = attackPhaseEnum.Phase1;
    }

    void OnTriggerDetectDamageable(IDamageable damageable, GameObject collider)
    {
        damageable.takeDamage(attackDamage);
    }

    bool isStateCompatible(StateManager.States state)
    {
        return state != StateManager.States.dash && state != StateManager.States.stun && state != StateManager.States.talk;
    }
}
