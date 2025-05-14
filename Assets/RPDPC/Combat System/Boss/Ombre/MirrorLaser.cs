using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MirrorLaser : MonoBehaviour
{
    Action<MirrorLaser> PlayerTouched;
    Vector3 wantedSize;

    public void InitLaser(int firstMirrorID, int lastMirrorID, float duration, float durationBeforeSpawn, float size, MirrorsManager mm)
    {

        Material material = new Material(Shader.Find("Universal Render Pipeline/Simple Lit"));
        material.SetFloat("_Surface", 1.0f);
        material.SetFloat("_Blend", 0.0f);
        Color baseColor = Color.red;
        baseColor.a = 0.2f;
        material.SetColor("_BaseColor", baseColor);
        material.SetFloat("_ReceiveShadows", 0.0f);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        GetComponent<MeshRenderer>().material = material;

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
        wantedSize = new Vector3(size, Vector3.Distance(firstMirror.transform.position, lastMirror.transform.position)/2, 1);

        StartCoroutine(LaunchPreviewLaser(durationBeforeSpawn));
        StartCoroutine(LaunchLaser(duration, durationBeforeSpawn));
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

    IEnumerator LaunchLaser(float duration, float durationBeforeSpawn)
    {
        yield return new WaitForSeconds(durationBeforeSpawn);
        transform.localScale = wantedSize;
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.magenta);
        GetComponent<CapsuleCollider>().enabled = true;
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
