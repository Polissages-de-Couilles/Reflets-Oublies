using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunAndKnockbackManager : StunAndKnockbackManagerBase
{
    protected CharacterController characterController;

    StateManager sm;
    List<StateManager.States> incompatibleStates = new List<StateManager.States> { StateManager.States.talk };

    private void Start()
    {
        sm = GetComponent<StateManager>();
    }

    public override void ApplyKnockback(float knockbackForce, KnockbackMode mode, GameObject attacker, GameObject attacked, Vector3 collisionPosWhenTouched)
    {
        if (!incompatibleStates.Contains(sm.playerState))
        {
            characterController = GetComponent<CharacterController>();
            Vector3 finalPos = new Vector3();
            switch (mode)
            {
                case KnockbackMode.MoveAwayFromAttackCollision:
                    finalPos = attacked.transform.position + (attacked.transform.position - collisionPosWhenTouched).normalized * knockbackForce;
                    break;
                case KnockbackMode.MoveAwayFromAttacker:
                    finalPos = attacked.transform.position + (attacked.transform.position - attacker.transform.position).normalized * knockbackForce;
                    break;
            }
            Debug.Log("Knockback from : " + attacked.transform.position + " to " + finalPos);
            StartCoroutine(ApplyKnockbackEnum(finalPos, attacked.transform.position));
        }
    }

    public override void ApplyStun(float stunDuration)
    {
        if (!incompatibleStates.Contains(sm.playerState))
        {
            sm.SetPlayerState(StateManager.States.stun, stunDuration);
        }
    }

    protected override IEnumerator ApplyKnockbackEnum(Vector3 finalPos, Vector3 attackedPos)
    {
        float time = 0;
        while (time < knockbackDuration)
        {
            Vector3 knockback = Vector3.Lerp(Vector3.zero, finalPos - attackedPos, time / knockbackDuration);
            knockback += gravity;
            characterController.Move(knockback);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
