using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading;

public class SS : ProjectileBase
{
    [SerializeField] GameObject Spear;
    [SerializeField] float duration;
    [SerializeField] float durationBetweenEwenPosAndSS;
    [SerializeField] float delayBetweenSpears;
    Dictionary<float, Vector3> ewenPos = new Dictionary<float, Vector3>();
    float globaltimer;
    GameObject ewen;

    protected override void LaunchProjectile()
    {
        ewen = GameManager.Instance.Player;
        StartCoroutine(DoSS());
        globaltimer = 0;
    }

    private void Update()
    {
        globaltimer += Time.deltaTime;
        ewenPos.Add(globaltimer, ewen.transform.position);
        foreach (var i in ewenPos.Where(d => d.Key < (globaltimer - (durationBetweenEwenPosAndSS + durationBetweenEwenPosAndSS / 10))).ToList())
        {
            ewenPos.Remove(i.Key);
        }
    }

    IEnumerator DoSS()
    {
        yield return new WaitForSeconds(durationBetweenEwenPosAndSS);

        float timer = 0;
        while (timer < duration)
        {
            Vector3 temp = getClosestTransform(globaltimer - durationBetweenEwenPosAndSS);
            temp.y -= 0.7f;
            Vector2 randomPosDif = UnityEngine.Random.insideUnitCircle * 1f;
            temp = temp + new Vector3(randomPosDif.x, 0, randomPosDif.y);
            GameObject go = Instantiate(Spear, temp, Quaternion.identity);
            go.GetComponentInChildren<SpearOfSS>().projectileManager = manager;
            yield return new WaitForSeconds(delayBetweenSpears);
            timer += delayBetweenSpears;
        }
    }

    Vector3 getClosestTransform(float time)
    {
        if (ewenPos == null || ewenPos.Count == 0)
            throw new ArgumentException("Le dictionnaire est vide ou nul.");

        // Trouve la clé la plus proche
        float closestKey = ewenPos.Keys.Aggregate((x, y) =>
            Math.Abs(x - time) < Math.Abs(y - time) ? x : y);

        return ewenPos[closestKey];
    }
}
