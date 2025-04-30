using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorLaser : MonoBehaviour
{
    Action<MirrorLaser> PlayerTouched;

    public void InitLaser(int firstMirrorID, int lastMirrorID, float duration, float durationBeforeSpawn, float size, MirrorsManager mm)
    {
        GetComponent<MeshRenderer>().enabled = false;
        CapsuleCollider cc = gameObject.AddComponent<CapsuleCollider>();
        cc.enabled = false;

        Mirror firstMirror = mm.GetMirror(firstMirrorID);
        Mirror lastMirror = mm.GetMirror(lastMirrorID);

        if(firstMirror == null || lastMirror == null)
        {
            Destroy(gameObject);
        }

        transform.position = firstMirror.transform.position + (lastMirror.transform.position - firstMirror.transform.position) / 2;
        transform.rotation = Quaternion.LookRotation((lastMirror.transform.position - firstMirror.transform.position).normalized, Vector3.up);
        transform.Rotate(new Vector3(90, 0, 0));
        transform.localScale = new Vector3(size, Vector3.Distance(firstMirror.transform.position, lastMirror.transform.position)/2, 1);

        StartCoroutine(LaunchLaser(duration, durationBeforeSpawn));
    }

    IEnumerator LaunchLaser(float duration, float durationBeforeSpawn)
    {
        yield return new WaitForSeconds(durationBeforeSpawn);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
