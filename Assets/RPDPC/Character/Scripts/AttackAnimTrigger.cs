using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimTrigger : MonoBehaviour
{
    [SerializeField] AttackCollider collision1;
    [SerializeField] AttackCollider collision2;
    [SerializeField] AttackCollider collision3;

    [SerializeField] Transform vfxAttack3Pos;
    [SerializeField] GameObject vfxAttack3;

    [SerializeField] AudioClip sfxAttack1 = null;
    [SerializeField] AudioClip sfxAttack3 = null;
    [SerializeField] AudioSource sfxSource;

    public void PlayAttack(int id)
    {
        AttackCollider collider = null;
        switch (id)
        {
            case 1: 
                collider = collision1;
                sfxSource.pitch = UnityEngine.Random.Range(0.75f, 1.25f);
                sfxSource.PlayOneShot(sfxAttack1);
                break;
            case 2: 
                collider = collision2;
                sfxSource.pitch = UnityEngine.Random.Range(0.75f, 1.25f);
                sfxSource.PlayOneShot(sfxAttack1);
                break;
            case 3: 
                collider = collision3;
                sfxSource.pitch = 1;
                sfxSource.PlayOneShot(sfxAttack3);
                break;
            default:
                break;
        }
        if (collider == null) return;
        collider.SetCollisionState(true);
    }

    public void StopAttack(int id)
    {
        AttackCollider collider = null;
        switch (id)
        {
            case 1: collider = collision1; break;
            case 2: collider = collision2; break;
            case 3: collider = collision3; break;
            default:
                break;
        }
        if (collider == null) return;
        collider.SetCollisionState(false);
    }

    public void PlayVfx()
    {
        Instantiate(vfxAttack3, vfxAttack3Pos.position, Quaternion.identity);
    }
}
