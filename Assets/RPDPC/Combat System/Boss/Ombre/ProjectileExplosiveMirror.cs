using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ProjectileExplosiveMirror : ProjectileBase
{
    [SerializeField] Vector3 centerPos;
    [SerializeField] float spawnRadius;
    [SerializeField] float delayBeforeExplosion;
    [SerializeField] GameObject enterexitSFX;
    [SerializeField] float groundLevel;
    [SerializeField] Mesh DestroyedMesh;
    [SerializeField] GameObject ExplosionParticle;
    bool isDestroyed = false;

    [SerializeField] AudioClip _sfx;
    float _sfxVolume = 0.15f;

    protected override void LaunchProjectile()
    {
        Debug.Log("LAUNCH MIRROR");

        //var texture = new RenderTexture(new RenderTextureDescriptor(256, 256, RenderTextureFormat.ARGB32));
        //GetComponentInChildren<Camera>().targetTexture = texture;
        //GetComponentInChildren<MeshRenderer>().materials[1].SetTexture("_BaseMap", texture);


        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        transform.position = centerPos + new Vector3(randomPoint.x, 0f, randomPoint.y); // for XZ plane
        Quaternion LookAtRotation = Quaternion.LookRotation(GameManager.Instance.Player.transform.position - transform.position, transform.up);
        LookAtRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, LookAtRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = LookAtRotation;
        

        gameObject.layer = 9;
        attackCollider.Init(manager.damageDetail.doesStun, manager.damageDetail.stunDuration, manager.damageDetail.doesKnockback, manager.damageDetail.knockbackForce, KnockbackMode.MoveAwayFromAttackCollision, true, manager.launcher);
        attackCollider.OnDamageableEnterTrigger += TriggerEnter;
        StartCoroutine(EnterTransition());
        StartCoroutine(manageMagicCircle());
        StartCoroutine(DelayExplosion());
    }

    IEnumerator EnterTransition()
    {
        GameObject trans = Instantiate(enterexitSFX, new Vector3(transform.position.x, groundLevel, transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)));
        yield return transform.DOMove(transform.position + new Vector3(0, 4.3f, 0), 1).WaitForCompletion();
        //Destroy(trans);
    }

    IEnumerator manageMagicCircle()
    {
        List<ParticleSystemRenderer> part = GetComponentsInChildren<ParticleSystemRenderer>().ToList();
        float timer = 0;
        while (timer <= delayBeforeExplosion)
        {
            foreach (ParticleSystemRenderer part2 in part)
            {
                part2.material.SetFloat("_RevealAngle", 360 - (360 * (timer / delayBeforeExplosion)));
            }

            timer += Time.deltaTime;
            yield return null;
        }

        foreach (ParticleSystemRenderer part2 in part)
        {
            Destroy(part2.gameObject);
        }
    }

    IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(delayBeforeExplosion);
        GetComponent<Collider>().enabled = true;
        isDestroyed = true;
        GetComponentInChildren<MeshFilter>().mesh = DestroyedMesh;
        Instantiate(ExplosionParticle, transform);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider>().enabled = false;
        GameObject trans = Instantiate(enterexitSFX, new Vector3(transform.position.x, groundLevel, transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)));
        var sfx = Instantiate(GameManager.Instance.AudioManager.SfxPrefab);
        sfx.clip = _sfx;
        sfx.volume = _sfxVolume;
        sfx.Play();
        Destroy(sfx, _sfx.length);
        yield return transform.DOMove(transform.position - new Vector3(0, 4.3f, 0), 1.5f).WaitForCompletion();
        //Destroy(trans);
        Destroy(gameObject);
    }

    void TriggerEnter(IDamageable attacked, GameObject attacker)
    {
        attacked.takeDamage(manager.damageDetail.damage, gameObject);
    }
}
