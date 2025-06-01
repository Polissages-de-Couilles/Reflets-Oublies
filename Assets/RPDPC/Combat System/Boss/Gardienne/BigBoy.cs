using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class BigBoy : ProjectileBase
{
    [SerializeField] Vector3 randomCenter;
    [SerializeField] float randomRadius;
    [SerializeField] float durationOfFall;
    [SerializeField] float size;
    [SerializeField] Vector3 desiredRotation;
    [SerializeField] GameObject visualizerFX;
    [SerializeField] float cameraShakeMaxIntensity;

    protected override void LaunchProjectile()
    {
        attackCollider.Init(manager.damageDetail.doesStun, manager.damageDetail.stunDuration, manager.damageDetail.doesKnockback, manager.damageDetail.knockbackForce, KnockbackMode.MoveAwayFromAttackCollision, true, manager.launcher);
        attackCollider.OnDamageableEnterTrigger += TriggerEnter;

        Vector2 random = Random.insideUnitCircle * randomRadius;
        Vector3 randomPos = randomCenter + new Vector3(random.x, 0, random.y);
        transform.position = randomPos;
        transform.rotation = Quaternion.Euler(desiredRotation.x, desiredRotation.y, desiredRotation.z);

        transform.position += -transform.up * size * 1.5f;

        //GetComponentInChildren<MeshFilter>().gameObject.transform.localScale = new Vector3(size, size, size);
        transform.localScale = new Vector3(size, size, size);

        visualizerFX = Instantiate(visualizerFX);
        visualizerFX.transform.position = randomPos + new Vector3(0, 0.15f, 0);
        var main = visualizerFX.GetComponent<ParticleSystem>().main;
        main.startLifetime = durationOfFall + 1;
        main.startSize = size / 3;
        ParticleSystem[] allParticles = visualizerFX.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in allParticles)
        {
            if (ps.gameObject != visualizerFX) // Ensure it's not the parent
            {
                var main2 = ps.main;
                main2.startLifetime = durationOfFall + 1;
                main2.startSize = size / 5f;
                //ps.Play();
            }
        }
        visualizerFX.GetComponent<ParticleSystem>().Play();
        float touchingDuration = (Vector3.Distance(transform.position, randomPos) / Vector3.Distance(transform.position, randomPos + transform.up * size / 1.5f)) * durationOfFall;
        Debug.Log("TOUCHING " + touchingDuration / durationOfFall);

        StartCoroutine(DoCameraShake(touchingDuration / 2));
        transform.DOMove(randomPos + transform.up * size / 1.5f, durationOfFall).SetEase(Ease.Linear);
    }

    void TriggerEnter(IDamageable attacked, GameObject attacker)
    {
        attacked.takeDamage(manager.damageDetail.damage, gameObject);
    }

    IEnumerator DoCameraShake(float durationBeforeMaxAmplitude)
    {
        CinemachineBasicMultiChannelPerlin noise = GameManager.Instance.CamManager.VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        float timer = 0;
        while (timer <= durationOfFall)
        {
            noise.m_AmplitudeGain = Mathf.Clamp(timer / durationBeforeMaxAmplitude, 0, 1) * cameraShakeMaxIntensity;
            yield return null;
            timer += Time.deltaTime;
        }

        noise.m_AmplitudeGain = 0.0f;
        Destroy(gameObject);
    }
}
