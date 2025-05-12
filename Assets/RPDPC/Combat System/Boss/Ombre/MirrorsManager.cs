using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MirrorsManager : MonoBehaviour
{
     Dictionary<int, Mirror> mirrors = new Dictionary<int, Mirror>();
    [SerializeField] BossStartManager OmbreBSM;

    private void Start()
    {
        foreach (Mirror mirror in GetComponentsInChildren<Mirror>())
        {
            mirrors.Add(mirror.ID, mirror);
            mirror.gameObject.SetActive(false);
        }
        OmbreBSM.OnBossIsActivated += InitMirror;
    }

    void InitMirror()
    {
        foreach (Mirror mirror in mirrors.Values)
        {
            mirror.gameObject.SetActive(true);
        }
    }

    public Mirror GetMirror(int id) 
    {
        if (id == -1)
        {
            return GetRandomMirror();
        }
        return mirrors[id]; 
    }

    public virtual Mirror GetOppositeMirror(int id)
    {
        if(id == -1)
        {
            return GetRandomMirror();
        }
        return mirrors[(id + mirrors.Count / 2) % mirrors.Count];
    }

    public virtual Mirror GetOppositeMirror(Mirror mirror)
    {
        return GetOppositeMirror(mirror.ID);
    }

    public Mirror GetRandomMirror() 
    {
        System.Random rnd = new System.Random();
        return mirrors[rnd.Next(1, mirrors.Count)];
    }
}
