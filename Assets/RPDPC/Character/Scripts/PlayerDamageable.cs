using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    private CharacterController characterController;
    public float maxHealth = 100f;
    private float currentHealth;
    private float defence = 1f;
    StateManager sm;
    [SerializeField] float invicibleTime;
    public bool IsInvicible => isInvicible;
    private bool isInvicible = false;
    private bool isBlink = false;
    private Dictionary<int, IEnumerator> _invicibleList = new();
    public Action<float, float> OnDamageTaken { get; set; }

    List<StateManager.States> incompatibleStates = new List<StateManager.States> { StateManager.States.talk };

    [SerializeField] SkinnedMeshRenderer[] _renderer;
    [SerializeField] MeshRenderer[] _meshrenderer;

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public void ChangeDefence(float _defenceChange)
    {
        defence = defence * (1 + _defenceChange);
    }

    public void SetMaxHealth(float value)
    {
        var pourcentageHealth = currentHealth / maxHealth;
        maxHealth = value;
        currentHealth = pourcentageHealth * value;
        GameManager.Instance.UIManager.SetHealthMax(value);
    }

    public void takeDamage(float damage, GameObject attacker)
    {
        if(isInvicible) return;
        if (!incompatibleStates.Contains(sm.playerState))
        {
            currentHealth -= damage / defence;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
            }
            Debug.Log("Player took damage. Their health is now at " + currentHealth);
            OnDamageTaken?.Invoke(damage, currentHealth);
            BecameInvicible(invicibleTime);
            GameManager.Instance.CamManager.ShakeCamera(((damage / defence) / maxHealth) * 20, 0.25f);
            if(((damage / defence) / maxHealth) >= 0.15f)
            {
                Time.timeScale = 0.1f;
                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.15f, 0.1f).SetUpdate(UpdateType.Late).OnComplete(
                    () =>
                    DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, ((damage / defence) / maxHealth) * 1.5f).SetUpdate(UpdateType.Late)
                    );
            }
        }
    }

    public void BecameInvicible(float time, bool blink = true)
    {
        System.Random rng = new System.Random();
        int id = rng.Next(0, 9);

        for(int i = 0; i < 1000000; i++)
        {
            id = int.Parse($"{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}{rng.Next(0, 9)}");
            Debug.Log(id);
            if(id != 0 && !_invicibleList.ContainsKey(id))
            {
                break;
            }
            else if(i >= 999999)
            {
                Debug.LogError("Max id Reach");
            }
        }
        var coroutine = InvicibleFrame(time, id, blink);
        _invicibleList.Add(id, coroutine);
        StartCoroutine(coroutine);
    }
    IEnumerator InvicibleFrame(float time, int id, bool blink)
    {
        yield return null;
        if (blink) isBlink = true;
        isInvicible = true;
        characterController.detectCollisions = false;
        yield return new WaitForSeconds(time);
        _invicibleList.Remove(id);
        if (blink) isBlink = false;
        if(_invicibleList.Count <= 0)
        {
            characterController.detectCollisions = true;
            isInvicible = false;
        }
    }
    void Awake()
    {
        currentHealth = maxHealth;
        //StartCoroutine(testDamage());
        sm = GetComponent<StateManager>();
        characterController = GetComponent<CharacterController>();
        StartCoroutine(BlinkIfInvisible());
    }

    IEnumerator BlinkIfInvisible()
    {
        //List<Material> materials = new List<Material>();
        //foreach (var mat in _mesh.materials)
        //{
        //    if (!materials.Contains(mat))
        //    {
        //        materials.Add(mat);
        //    }
        //}

        while (true) 
        {
            while (isInvicible && isBlink)
            {
                foreach (var rend in _renderer)
                {
                    rend.enabled = false;
                }
                foreach (var rend in _meshrenderer)
                {
                    rend.enabled = false;
                }
                yield return new WaitForSeconds(0.05f);

                foreach (var rend in _renderer)
                {
                    rend.enabled = true;
                }
                foreach (var rend in _meshrenderer)
                {
                    rend.enabled = true;
                }
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }

    public void heal(float heal)
    {
        Debug.Log("HEAL " + heal);
        currentHealth += heal;
        if (maxHealth < currentHealth) currentHealth = maxHealth;
        OnDamageTaken?.Invoke(-heal, currentHealth);
        //VFX Heal
    }

    //IEnumerator testDamage() { yield return new WaitForSeconds(2); takeDamage(maxHealth); }
}
