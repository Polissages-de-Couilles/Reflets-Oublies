using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunAndKnockbackManager : StunAndKnockbackManagerBase
{
    StateManager sm;

    private void Start()
    {
        sm = GetComponent<StateManager>();
    }

    public override void ApplyStun(float stunDuration)
    {
        sm.SetPlayerState(StateManager.States.stun, stunDuration);
    }
}
