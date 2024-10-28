using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Own/OwnHpUnder")]
public class OwnHpUnder : ConditionBase
{
    public int hpThreshold;
    IDamageable id;
    public override void Init(GameObject parent, GameObject player)
    {
        if (parent.GetComponent<IDamageable>() != null)
            id = parent.GetComponent<IDamageable>();
    }

    public override bool isConditionFulfilled()
    {
        return id.getCurrentHealth() < hpThreshold;
    }
}
