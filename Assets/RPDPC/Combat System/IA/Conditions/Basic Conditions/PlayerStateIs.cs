using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Player/PlayerStateIs")]
public class PlayerStateIs : ConditionBase
{
    [SerializeField] StateManager.States state;
    StateManager stateManager;

    public override void Init(GameObject parent, GameObject player, StateEntityBase seb)
    {
        stateManager = player.GetComponent<StateManager>();
    }

    public override bool isConditionFulfilled()
    {
        return state == stateManager.playerState;
    }
}
