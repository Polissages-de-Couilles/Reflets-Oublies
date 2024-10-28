using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Player/PlayerHpEqual")]
public class PlayerHpEqual : ConditionBase
{
    public int hpThreshold;
    PlayerDamageable pd;
    public override void Init(GameObject parent, GameObject player)
    {
        pd = player.GetComponent<PlayerDamageable>();
    }

    public override bool isConditionFulfilled()
    {
        return pd.currentHealth == hpThreshold;
    }
}
