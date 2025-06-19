using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardienneANimationVFX : MonoBehaviour
{
    [SerializeField] GameObject vfx;
    public void PlayVFX(int nb)
    {
        Instantiate(vfx, transform.parent.transform);
    }
}
