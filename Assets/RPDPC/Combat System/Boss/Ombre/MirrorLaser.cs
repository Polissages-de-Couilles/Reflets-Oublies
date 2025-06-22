using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MirrorLaser : MonoBehaviour
{
    Action<MirrorLaser> PlayerTouched;
    Vector3 wantedSize;
    GameObject fx;

    [SerializeField] AudioClip _sfx;
    float _sfxVolume = 0.15f;

    public void InitLaser(int firstMirrorID, int lastMirrorID, float duration, float durationBeforeSpawn, float size, MirrorsManager mm, GameObject LaserFX, Material previewMat)
    {

        Mirror firstMirror = mm.GetMirror(firstMirrorID);
        Mirror lastMirror = mm.GetMirror(lastMirrorID);

        fx = LaserFX;

        GetComponent<MeshRenderer>().material = previewMat;

        CapsuleCollider cc = gameObject.AddComponent<CapsuleCollider>();
        cc.enabled = false;

        transform.position = firstMirror.transform.position + (lastMirror.transform.position - firstMirror.transform.position) / 2;
        transform.rotation = Quaternion.LookRotation((lastMirror.transform.position - firstMirror.transform.position).normalized, Vector3.up);
        transform.Rotate(new Vector3(90, 0, 0));
        wantedSize = new Vector3(size, Vector3.Distance(firstMirror.transform.position, lastMirror.transform.position)/2, 1);

        StartCoroutine(LaunchPreviewLaser(durationBeforeSpawn));
        StartCoroutine(LaunchLaser(duration, durationBeforeSpawn, firstMirror, lastMirror));
    }

    IEnumerator LaunchPreviewLaser(float durationBeforeSpawn)
    {
        float timer = durationBeforeSpawn;
        while (timer > 0)
        {
            Vector3 tempSize = wantedSize * (timer / durationBeforeSpawn);
            transform.localScale = new Vector3(tempSize.x, wantedSize.y, tempSize.z);
            timer -= Time.deltaTime;
            yield return null;  
        }
    }

    IEnumerator LaunchLaser(float duration, float durationBeforeSpawn, Mirror firstMirror, Mirror lastMirror)
    {
        yield return new WaitForSeconds(durationBeforeSpawn);
        //var sfx = Instantiate(GameManager.Instance.AudioManager.SfxPrefab);
        //sfx.clip = _sfx;
        //sfx.volume = _sfxVolume;
        //sfx.Play();
        //Destroy(sfx, _sfx.length);
        transform.localScale = wantedSize;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = false;
        fx = Instantiate(fx);
        fx.GetComponent<LineRenderer>().startWidth = wantedSize.x;
        fx.GetComponent<LineRenderer>().endWidth = wantedSize.x;
        fx.transform.position = firstMirror.transform.position;
        fx.transform.rotation = Quaternion.LookRotation((lastMirror.transform.position - firstMirror.transform.position).normalized, Vector3.up);
        yield return new WaitForSeconds(duration);
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(fx);
        Destroy(gameObject);
    }
}
