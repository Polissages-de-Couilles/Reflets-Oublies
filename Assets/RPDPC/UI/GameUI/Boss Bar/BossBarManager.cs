using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBarManager : MonoBehaviour
{
    [SerializeField] GameObject bar;
    [SerializeField] TextMeshProUGUI bossNameText;
    Slider slider;
    float maxHealth;

    void Start()
    {
        slider = bar.GetComponent<Slider>(); 
        bar.SetActive(false);
    }

    public void ActivateBar(GameObject boss, string bossName)
    {
        bar.SetActive(true);
        bossNameText.text = bossName;

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
        slider.transform.DOShakePosition(0.2f, 10);
        slider.value = currentHealth/maxHealth;
        if (currentHealth <= 0)
        {
            ResetBar();
        }
    }
}
