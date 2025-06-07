using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BossDeathManager : BotDeathManager
{
    [SerializeField] StateBase stateToPlayAtDeath;
    private bool actionInvoked = false;

    override protected IEnumerator CheckBotHealth(float damageTaken, float playerHealth)
    {
        if (playerHealth <= 0)
        {
            Debug.Log("The boss (" + gameObject + ") is dead.");

            if (stateToPlayAtDeath != null)
            {
                StateEntityBase seb = stateToPlayAtDeath.PrepareEntityInstance();
                seb.InitGlobalVariables(stateMachine, gameObject, GameManager.Instance.Player, stateToPlayAtDeath.conditions, stateToPlayAtDeath.priority, stateToPlayAtDeath.isHostileState, stateMachine.Animator, stateToPlayAtDeath.animationNames, stateToPlayAtDeath.isAttack);
                seb.onActionFinished += OnActionInvoked;
                stateMachine.forceState(seb);
                yield return new WaitUntil(() => actionInvoked);
                stateMachine.enabled = false;
            }

            float animationDuration = 0;
            if (stateMachine.Animator != null)
            {
                stateMachine.Animator.Play(DeathAnimName);
                animationDuration = stateMachine.Animator.runtimeAnimatorController.animationClips.ToList().Find(x => x.name == DeathAnimName).length;
            }
            else
            {
                animationDuration = 0.1f;
            }
            stateMachine.enabled = false;

            if (TryGetComponent<MoneyDrop>(out MoneyDrop money))
            {
                money.DropMonney();
            }

            OnBotDied?.Invoke();
            yield return new WaitForSeconds(animationDuration);

            Destroy(gameObject);
        }
    }
    private void OnActionInvoked()
    {
        actionInvoked = true;
    }
}
