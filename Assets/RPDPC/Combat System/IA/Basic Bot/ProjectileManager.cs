using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public GameObject launcher;
    public GameObject target;

    public void Init(GameObject launcher, GameObject target)
    {
        this.launcher = launcher;
        this.target = target;
    }
}
