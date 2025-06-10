using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DKProjectile : ProjectileBase
{
    [SerializeField] GameObject spear;
    [SerializeField] float spearSize;
    [SerializeField] float speed;
    [SerializeField] float numberOfLinePerSeconds;
    [SerializeField] float duration;

    protected override void LaunchProjectile()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator launchLines()
    {
        for (int i = 0; i < numberOfLinePerSeconds * duration; i++)
        {
            GameObject go = new GameObject("DKLine");
            go.AddComponent<DKLine>().Init(manager);

            yield return new WaitForSeconds(1/numberOfLinePerSeconds);
        }
    }
}
