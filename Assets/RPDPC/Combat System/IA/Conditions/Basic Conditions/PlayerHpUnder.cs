using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Player/PlayerHpUnder")]
public class PlayerHpUnder : ConditionBase
{
    [Range(0, 1)]
    public int hpPercentage;
    PlayerDamageable pd;
    public override void Init(GameObject parent, GameObject player)
    {
        pd = player.GetComponent<PlayerDamageable>();
    }

    public override bool isConditionFulfilled()
    {
        return pd.getCurrentHealth() / pd.getMaxHealth() < hpPercentage;
    }
    void OnValidate()
    {
        hpPercentage = Mathf.Clamp(hpPercentage, 0, 1);
    }
}
