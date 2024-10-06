using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackManager : MonoBehaviour
{
    PlayerInputEvent PIE;
    [SerializeField] float timeBetweenAttacks = 0.5f;

    enum attackPhaseEnum
    {
        Phase1,
        Phase2,
        Phase3
    }
    attackPhaseEnum nextAttackPhaseLate = attackPhaseEnum.Phase1;
    attackPhaseEnum nextAttackPhase = attackPhaseEnum.Phase1;

    void Awake()
    {
        PIE = GameObject.Find("InputManager").GetComponent<PlayerInputEvent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PIE.PlayerInputAction.Player.Attack.performed += OnAttack;
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if (nextAttackPhase == nextAttackPhaseLate)
        {
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
        nextAttackPhase = attackPhaseEnum.Phase2;
        yield return new WaitForSeconds(timeBetweenAttacks);
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
        nextAttackPhase = attackPhaseEnum.Phase3;
        yield return new WaitForSeconds(timeBetweenAttacks);
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
        nextAttackPhase = attackPhaseEnum.Phase1;
        yield return new WaitForSeconds(timeBetweenAttacks);
        nextAttackPhaseLate = attackPhaseEnum.Phase1;
    }
}
