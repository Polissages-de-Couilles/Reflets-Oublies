using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBarManager : MonoBehaviour
{
    [SerializeField] GameObject bar;
    Slider slider;
    float maxHealth;

    void Start()
    {
        slider = bar.GetComponent<Slider>(); 
        bar.SetActive(false);
    }

    public void ActivateBar(GameObject boss)
    {
        bar.SetActive(true);

        IDamageable damageable = boss.GetComponent<IDamageable>();
        damageable.OnDamageTaken += OnDamageTaken;
        maxHealth = damageable.getMaxHealth();
    }

    void ResetBar()
    {
        slider.value = 1;
        bar.SetActive(false); 
    }

    void OnDamageTaken(float damage, float currentHealth)
    {
        slider.value = currentHealth/maxHealth;
        if (currentHealth <= 0)
        {
            ResetBar();
        }
    }
}
