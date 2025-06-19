using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardienneParticleHolder : MonoBehaviour
{
    public GameObject normal;
    public GameObject wind;
    public GameObject strong;
    bool forced;

    private void Start()
    {
        GetComponent<BossDeathManager>().OnBotDied += OnBossDied;
    }

    private void OnBossDied()
    {
        Switch(GardienneWind.Static, true);
    }

    public void Switch(GardienneWind windForce, bool force = false)
    {
        if (!forced)
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
            forced = force;
        }
    }

    public void Unlock()
    {
        forced = false;
    }
}

public enum GardienneWind
{
    Static,
    Strong,
    ReallyStrong
}