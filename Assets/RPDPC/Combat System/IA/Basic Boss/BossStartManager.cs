using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossStartManager : MonoBehaviour
{
    [SerializeField] List<WallsInfo> wallsInfos;
    [SerializeField] List<WallsInfo> detectorsInfos;
    [SerializeField] string bossName;
    public Action OnBossIsActivated;
    List<BossPlayerDetector> detectors = new List<BossPlayerDetector>();
    List<GameObject> walls = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BotDeathManager>().OnBotDied += OnBotDied;

        SpawnDetectors();
    }

    public void SpawnDetectors()
    {
        foreach (WallsInfo d in detectorsInfos)
        {
            GameObject dG = new GameObject("BossPlayerDetector");
            BoxCollider bc = dG.AddComponent<BoxCollider>();
            bc.isTrigger = true;
            BossPlayerDetector bpd = dG.AddComponent<BossPlayerDetector>();
            dG.transform.position = d.absPos;
            dG.transform.localScale = d.size;
            bpd.PlayerDetected += OnPlayerDetected;
            detectors.Add(bpd);
        }
    }

    public void OnPlayerDetected()
    {
        Debug.Log("ACTIVATE BOSS");

        foreach (BossPlayerDetector bpd in detectors) 
        {
            bpd.PlayerDetected -= OnPlayerDetected;
            Destroy(bpd.gameObject);
        }

        foreach (WallsInfo w in wallsInfos)
        {
            GameObject wG = new GameObject("BossWall");
            BoxCollider bc = wG.AddComponent<BoxCollider>();
            wG.transform.position = w.absPos;
            wG.transform.localScale = w.size;
            walls.Add(wG);
        }

        ActivateBoss();
    }

    private void ActivateBoss()
    {
        OnBossIsActivated?.Invoke();
        GameManager.Instance.UIManager.GetComponent<BossBarManager>().ActivateBar(gameObject, bossName);
        GetComponent<StateMachineManager>().enabled = true;
    }

    public void OnBotDied()
    {
        foreach (GameObject wG in walls)
        {
            Destroy(wG);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (WallsInfo w in wallsInfos)
        {
            Gizmos.DrawLine(w.absPos - w.size/2, w.absPos + new Vector3(w.size.x/2, -w.size.y/2, -w.size.z/2));
            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, w.size.y / 2, -w.size.z / 2), w.absPos + new Vector3(w.size.x/2, -w.size.y/2, w.size.z/2));
            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, -w.size.z / 2), w.absPos + new Vector3(w.size.x/2, w.size.y/2, w.size.z/2));
            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, w.size.z / 2), w.absPos + new Vector3(w.size.x/2, w.size.y/2, -w.size.z/2));

            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, w.size.y / 2, w.size.z / 2), w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, -w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(-w.size.x / 2, w.size.y / 2, w.size.z / 2), w.absPos - new Vector3(-w.size.x / 2, -w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(-w.size.x / 2, w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(-w.size.x / 2, -w.size.y / 2, -w.size.z / 2));

            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(w.size.x / 2, w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(-w.size.x / 2, w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(-w.size.x / 2, w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(-w.size.x / 2, -w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(-w.size.x / 2, -w.size.y / 2, w.size.z / 2));
        }

        Gizmos.color = Color.green;

        foreach (WallsInfo w in detectorsInfos)
        {
            Gizmos.DrawLine(w.absPos - w.size / 2, w.absPos + new Vector3(w.size.x / 2, -w.size.y / 2, -w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, w.size.y / 2, -w.size.z / 2), w.absPos + new Vector3(w.size.x / 2, -w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, -w.size.z / 2), w.absPos + new Vector3(w.size.x / 2, w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, w.size.z / 2), w.absPos + new Vector3(w.size.x / 2, w.size.y / 2, -w.size.z / 2));

            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, w.size.y / 2, w.size.z / 2), w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, -w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(-w.size.x / 2, w.size.y / 2, w.size.z / 2), w.absPos - new Vector3(-w.size.x / 2, -w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(-w.size.x / 2, w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(-w.size.x / 2, -w.size.y / 2, -w.size.z / 2));

            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(w.size.x / 2, w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(-w.size.x / 2, w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(-w.size.x / 2, w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(w.size.x / 2, -w.size.y / 2, w.size.z / 2));
            Gizmos.DrawLine(w.absPos - new Vector3(-w.size.x / 2, -w.size.y / 2, -w.size.z / 2), w.absPos - new Vector3(-w.size.x / 2, -w.size.y / 2, w.size.z / 2));
        }
    }
}

[Serializable]
struct WallsInfo
{
    public Vector3 absPos;
    public Vector3 size;
}