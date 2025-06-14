using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardienneParticleHolder : MonoBehaviour
{
    public GameObject normal;
    public GameObject wind;
    public GameObject strong;

    public void Switch(GardienneWind windForce)
    {
        switch (windForce) 
        { 
            case GardienneWind.Static:
                normal.SetActive(true);
                wind.SetActive(false);
                strong.SetActive(false);
                break;
            case GardienneWind.Strong:
                normal.SetActive(false);
                wind.SetActive(true);
                strong.SetActive(false);
                break;
            case GardienneWind.ReallyStrong:
                normal.SetActive(false);
                wind.SetActive(false);
                strong.SetActive(true);
                break;
        }
    }
}

public enum GardienneWind
{
    Static,
    Strong,
    ReallyStrong
}