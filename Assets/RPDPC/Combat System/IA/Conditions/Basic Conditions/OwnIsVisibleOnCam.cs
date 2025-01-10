using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/IA/Conditions/Base/Own/OwnIsVisibleOnCam")]
public class OwnIsVisibleOnCam : ConditionBase
{
    GameObject parent;
    public override void Init(GameObject parent, GameObject player)
    {
        this.parent = parent;
    }

    public override bool isConditionFulfilled()
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(parent.transform.position);
        return (screenPos.x <= 1 && screenPos.x >= 0) && (screenPos.y <= 1 && screenPos.y >= 0);
    }
}
