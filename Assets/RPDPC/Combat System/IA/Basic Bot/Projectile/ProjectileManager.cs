using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [HideInInspector] public GameObject launcher;
    [HideInInspector] public GameObject target;
    [HideInInspector] public ProjectileAttackDamageDetails damageDetail;

    public void Init(GameObject launcher, GameObject target, ProjectileAttackDamageDetails damageDetail)
    {
        this.launcher = launcher;
        this.target = target;
        this.damageDetail = damageDetail;
    }
}
