using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DKLine : MonoBehaviour
{
    ProjectileManager pm;

    public void Init(ProjectileManager pm)
    {
        this.pm = pm;
        LaunchLine();
    }

    void LaunchLine()
    {

    }
}
