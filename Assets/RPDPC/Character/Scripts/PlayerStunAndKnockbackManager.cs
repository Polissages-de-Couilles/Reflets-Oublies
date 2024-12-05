using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

public class PlayerStunAndKnockbackManager : StunAndKnockbackManagerBase
{
    StateManager sm;
    List<StateManager.States> incompatibleStates = new List<StateManager.States> { StateManager.States.talk };

    private void Start()
    {
        sm = GetComponent<StateManager>();
    }

    new public void ApplyKnockback(float knockbackForce, KnockbackMode mode, GameObject attacker, GameObject attacked, Vector3 collisionPosWhenTouched)
    {
        if (!incompatibleStates.Contains(sm.playerState))
        {
            base.ApplyKnockback(knockbackForce, mode, attacker, attacked, collisionPosWhenTouched);
        }
    }

    public override void ApplyStun(float stunDuration)
    {
        if (!incompatibleStates.Contains(sm.playerState))
        {
            sm.SetPlayerState(StateManager.States.stun, stunDuration);
        }
    }
}
