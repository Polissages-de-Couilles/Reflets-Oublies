using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Player/PlayerHpAbove")]
public class PlayerHpAbove : ConditionBase
{
    public float hpPercentage;
    PlayerDamageable pd;
    public override void Init(GameObject parent, GameObject player)
    {
        pd = player.GetComponent<PlayerDamageable>();
    }

    public override bool isConditionFulfilled()
    {
        return pd.getCurrentHealth() / pd.getMaxHealth() > hpPercentage;
    }
    void OnValidate()
    {
        hpPercentage = Mathf.Clamp(hpPercentage, 0, 1);
    }
}
