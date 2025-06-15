using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MirrorsManager : MonoBehaviour
{
    Dictionary<int, Mirror> mirrors = new Dictionary<int, Mirror>();
    [SerializeField] List<Mirror> mirrorList = new List<Mirror>();
    [SerializeField] BossStartManager OmbreBSM;

    private void Start()
    {
        Debug.Log("OMBRE ");
        foreach (Mirror mirror in mirrorList)
        {
            mirrors.Add(mirror.ID, mirror);
            mirror.gameObject.SetActive(false);
            Debug.Log("OMBRE " + mirror.ID);
        }
        OmbreBSM.OnBossIsActivated += InitMirror;
    }

    void InitMirror()
    {
        MemoryManager memoryManager = GameManager.Instance.MemoryManager;
        if (memoryManager.AllMemory.Count == 6)
        {
            int i = 1;
            foreach (MemorySO mem in memoryManager.AllMemory)
            {
                mirrors.Values.ToList()[i - 1].gameObject.SetActive(true);
                mirrors.Values.ToList()[GetOppositeMirror(i).ID - 1].gameObject.SetActive(true);

                if (mem._isTaken)
                {
                    mirrors.Values.ToList()[i - 1].SetBroken();
                    mirrors.Values.ToList()[GetOppositeMirror(i).ID - 1].SetBroken();
                }
                i++;
            }
        }
        else // FOR TEST PURPOSES
        {
            for (int i = 1; i < 7; i++)
            {
                mirrors.Values.ToList()[i-1].gameObject.SetActive(true);
                Debug.Log("OPPOSITE MIRROR " + i + GetOppositeMirror(i).ID);
                mirrors.Values.ToList()[GetOppositeMirror(i).ID-1].gameObject.SetActive(true);

                //System.Random rand = new System.Random();
                //if (rand.Next(2) == 0)
                //{
                //    mirrors.Values.ToList()[i - 1].SetBroken();
                //    mirrors.Values.ToList()[GetOppositeMirror(i).ID - 1].SetBroken();
                //}
            }
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
        if (id == mirrors.Count / 2) 
        {
            return mirrors[id * 2];
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
