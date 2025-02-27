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
    private Dictionary<int, IEnumerator> _invicibleList = new();
    public Action<float, float> OnDamageTaken { get; set; }

    List<StateManager.States> incompatibleStates = new List<StateManager.States> { StateManager.States.talk };

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
        }
    }

    public void BecameInvicible(float time)
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
        var coroutine = InvicibleFrame(time, id);
        _invicibleList.Add(id, coroutine);
        StartCoroutine(coroutine);
    }
    IEnumerator InvicibleFrame(float time, int id)
    {
        yield return null;
        isInvicible = true;
        characterController.detectCollisions = false;
        yield return new WaitForSeconds(time);
        _invicibleList.Remove(id);
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
    }

    public void heal(float heal)
    {
        currentHealth += heal;
        if (maxHealth < currentHealth) currentHealth = maxHealth;
    }

    //IEnumerator testDamage() { yield return new WaitForSeconds(2); takeDamage(maxHealth); }
}
